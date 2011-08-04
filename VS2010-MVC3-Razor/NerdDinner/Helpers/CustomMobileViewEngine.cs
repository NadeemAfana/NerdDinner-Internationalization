using System;
using System.Web.Mvc;

namespace NerdDinner.Helpers
{
    public class CustomMobileViewEngine : IViewEngine
    {
        public IViewEngine BaseViewEngine { get; private set; }
        public Func<ControllerContext, bool> IsTheRightDevice { get; private set; }
        public string PathToSearch { get; private set; }

        public CustomMobileViewEngine(Func<ControllerContext, bool> isTheRightDevice, string pathToSearch, IViewEngine baseViewEngine)
        {
            BaseViewEngine = baseViewEngine;
            IsTheRightDevice = isTheRightDevice;
            PathToSearch = pathToSearch;
        }

        public ViewEngineResult FindPartialView(ControllerContext context, string viewName, bool useCache)
        {
            if (IsTheRightDevice(context))
            {
                return BaseViewEngine.FindPartialView(context, PathToSearch + "/" + viewName, useCache);
            }
            return new ViewEngineResult(new string[] { }); //we found nothing and we pretend we looked nowhere
        }

        public ViewEngineResult FindView(ControllerContext context, string viewName, string masterName, bool useCache)
        {
            if (IsTheRightDevice(context))
            {
                return BaseViewEngine.FindView(context, PathToSearch + "/" + viewName, masterName, useCache);
            }
            return new ViewEngineResult(new string[] { }); //we found nothing and we pretend we looked nowhere
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            throw new NotImplementedException();
        }
    }

    public static class MobileHelpers
    {
        public static bool UserAgentContains(this ControllerContext c, string agentToFind)
        {
            return (c.HttpContext.Request.UserAgent.IndexOf(agentToFind, StringComparison.OrdinalIgnoreCase) > 0);
        }

        public static bool IsMobileDevice(this ControllerContext c)
        {
            return c.HttpContext.Request.Browser.IsMobileDevice;
        }

        public static void AddMobile<T>(this ViewEngineCollection ves, Func<ControllerContext, bool> isTheRightDevice, string pathToSearch)
            where T : IViewEngine, new()
        {
            ves.Add(new CustomMobileViewEngine(isTheRightDevice, pathToSearch, new T()));
        }

        public static void AddMobile<T>(this ViewEngineCollection ves, string userAgentSubstring, string pathToSearch)
            where T : IViewEngine, new()
        {
            ves.Add(new CustomMobileViewEngine(c => c.UserAgentContains(userAgentSubstring), pathToSearch, new T()));
        }

        public static void AddIPhone<T>(this ViewEngineCollection ves) //specific example helper
            where T : IViewEngine, new()
        {
            ves.Add(new CustomMobileViewEngine(c => c.UserAgentContains("iPhone"), "Mobile/iPhone", new T()));
        }

        public static void AddGenericMobile<T>(this ViewEngineCollection ves)
            where T : IViewEngine, new()
        {
            ves.Add(new CustomMobileViewEngine(c => c.IsMobileDevice(), "Mobile", new T()));
        }
    }
}