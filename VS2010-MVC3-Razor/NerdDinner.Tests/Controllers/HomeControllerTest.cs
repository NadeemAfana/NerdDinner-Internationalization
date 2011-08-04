using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NerdDinner.Controllers;
using NerdDinner.Tests.Mocks;

namespace NerdDinner.Tests.Controllers {
    [TestClass]
    public class HomeControllerTest {
        [TestMethod]
        public void Index() {
			// Arrange
			HttpContextBase httpContext = MvcMockHelpers.FakeHttpContext();
			HomeController controller = new HomeController();
			RequestContext requestContext = new RequestContext(httpContext, new RouteData());

			controller.ControllerContext = new ControllerContext(requestContext, controller);
			controller.Url = new UrlHelper(requestContext);

			// Act
			ViewResult result = controller.Index() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About() {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
