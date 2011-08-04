using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NerdDinner.Controllers {

    [HandleErrorWithELMAH]
    public class HomeController : BaseController
    {
        private const string XrdsType = "application/xrds+xml";

        public ActionResult Index() {
            // Enables RP Discovery, which avoids warnings from OpenID providers like Yahoo during login.
            // Some Providers ask for a specific accept-type, which we can optimize for here.
            if (Request != null && Request.AcceptTypes != null && Array.IndexOf(Request.AcceptTypes, XrdsType) >= 0) {
                return View("Xrds");
            }
            // Other Providers don't say they're performing RP discovery, so always include an HTTP header to help them.
            if (Response != null)
                Response.AppendHeader("X-XRDS-Location", Url.Action("Xrds"));

            return View();
        }

        public ActionResult About() {
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

      
        /// <summary>
        /// Provides an RP discovery document to OpenID 2.0 Providers that are performing a security check on this RP.
        /// </summary>
        public ActionResult Xrds() {
            return View("Xrds");
        }
    }
}
