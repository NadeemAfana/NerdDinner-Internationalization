using System.Reflection;
using System.Web.Mvc;

namespace NerdDinner.Controllers
{
    public class ResourceController : BaseController
    {
        public FileStreamResult OpenIdSelector()
        {
            //<link rel="Stylesheet" type="text/css" href="${ Page.ClientScript.GetWebResourceUrl(typeof(DotNetOpenAuth.OpenId.RelyingParty.OpenIdSelector), "DotNetOpenAuth.OpenId.RelyingParty.OpenIdSelector.css") }" />
            var assembly = typeof(DotNetOpenAuth.OpenId.RelyingParty.OpenIdSelector).Assembly;

            var resource = assembly.GetManifestResourceStream("DotNetOpenAuth.OpenId.RelyingParty.OpenIdSelector.css");

            return new FileStreamResult(resource, "text/css");
        }

        public FileStreamResult OpenIdAjaxTextBox()
        {
            //<link rel="Stylesheet" type="text/css" href="${ Page.ClientScript.GetWebResourceUrl(typeof(DotNetOpenAuth.OpenId.RelyingParty.OpenIdSelector), "DotNetOpenAuth.OpenId.RelyingParty.OpenIdAjaxTextBox.css") }" />
            var assembly = typeof(DotNetOpenAuth.OpenId.RelyingParty.OpenIdSelector).Assembly;

            var resource = assembly.GetManifestResourceStream("DotNetOpenAuth.OpenId.RelyingParty.OpenIdAjaxTextBox.css");

            return new FileStreamResult(resource, "text/css");
        }

        public FileStreamResult GetWebResourceUrl(string assemblyName, string typeName, string resourceName)
        {
            var assembly = Assembly.Load(assemblyName);

            var resource = assembly.GetManifestResourceStream(resourceName);

            return new FileStreamResult(resource, GetKnownContentType(resourceName));
        }

        private string GetKnownContentType(string resourceName)
        {
            switch (System.IO.Path.GetExtension(resourceName))
            {
                case ".css":
                    return "text/css";
                case ".gif":
                    return "image/gif";
                case ".png":
                    return "image/png";
                case ".js":
                    return "application/x-javascript";
                default:
                    return "text/plain";
            }
        }
    }
}
