using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using NerdDinner.Helpers;
using NerdDinner.Models;
using Resources;

namespace NerdDinner.Controllers {

	  [HandleErrorWithELMAH]
    public class AccountController : BaseController
    {

        // This constructor is used by the MVC framework to instantiate the controller using
        // the default forms authentication and membership providers.

        public AccountController()
            : this(null, null) {
        }

        // This constructor is not used by the MVC framework but is instead provided for ease
        // of unit testing this type. See the comments at the end of this file for more
        // information.
        public AccountController(IFormsAuthentication formsAuth, IMembershipService service) {
            FormsAuth = formsAuth ?? new FormsAuthenticationService();
            MembershipService = service ?? new AccountMembershipService();
        }

        public IFormsAuthentication FormsAuth {
            get;
            private set;
        }

        public IMembershipService MembershipService {
            get;
            private set;
        }

        public ActionResult LogOn() {

            return View();
        }

        [HttpPost]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ValidateLogOn(model.UserName, model.Password))
                {
                    // Make sure we have the username with the right capitalization
                    // since we do case sensitive checks for OpenID Claimed Identifiers later.
                    string userName = MembershipService.GetCanonicalUsername(model.UserName);

                    FormsAuth.SignIn(userName, model.RememberMe);

                    // Make sure we only follow relative returnUrl parameters to protect against having an open redirector
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.Resources.UsernamePasswordIncorrect);
                }
            }
            return View(model);
        }

        public ActionResult LogOff() {

            FormsAuth.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register() {

            ViewBag.PasswordLength = MembershipService.MinPasswordLength;

            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.PasswordLength = MembershipService.MinPasswordLength;

                if (ValidateRegistration(model.UserName, model.Email, model.Password, model.ConfirmPassword))
                {
                    // Attempt to register the user
                    MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        FormsAuth.SignIn(model.UserName, false /* createPersistentCookie */);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", ErrorCodeToString(createStatus));
                    }
                }
            }
            return View(model);
        }

        [Authorize]
        public ActionResult ChangePassword() {

            ViewBag.PasswordLength = MembershipService.MinPasswordLength;

            return View();
        }

        [Authorize]
        [HttpPost]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Exceptions result in password not being changed.")]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword) {

            ViewBag.PasswordLength = MembershipService.MinPasswordLength;

            if (!ValidateChangePassword(currentPassword, newPassword, confirmPassword)) {
                return View();
            }

            try {
                if (MembershipService.ChangePassword(User.Identity.Name, currentPassword, newPassword)) {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else {
                    ModelState.AddModelError("_FORM", Resources.Resources.CurrentPasswordInvalid);
                    return View();
                }
            }
            catch {
                ModelState.AddModelError("_FORM", Resources.Resources.CurrentPasswordInvalid);
                return View();
            }
        }

        public ActionResult ChangePasswordSuccess() {

            return View();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity is WindowsIdentity) {
                throw new InvalidOperationException(Resources.Resources.WindowsAuthenticationNotSupported);
            }
        }

        #region Validation Methods

        private bool ValidateChangePassword(string currentPassword, string newPassword, string confirmPassword) {
            if (String.IsNullOrEmpty(currentPassword)) {
                ModelState.AddModelError("currentPassword", Resources.Resources.YouMustSpecifyCurrentPassword);
            }
            if (newPassword == null || newPassword.Length < MembershipService.MinPasswordLength) {
                ModelState.AddModelError("newPassword",
                    String.Format(CultureInfo.CurrentCulture,
                         Resources.Resources.YouMustSpecifyLongerPassword,
                         MembershipService.MinPasswordLength));
            }

            if (!String.Equals(newPassword, confirmPassword, StringComparison.Ordinal)) {
                ModelState.AddModelError("_FORM", Resources.Resources.NewPasswordAndConfirmationMismatch);
            }

            return ModelState.IsValid;
        }

        private bool ValidateLogOn(string userName, string password) {
            if (String.IsNullOrEmpty(userName)) {
                ModelState.AddModelError("username", Resources.Resources.YouMustSpecifyUsername);
            }
            if (String.IsNullOrEmpty(password)) {
                ModelState.AddModelError("password", Resources.Resources.YouMustSpecifyPassword);
            }
            if (!MembershipService.ValidateUser(userName, password)) {
                ModelState.AddModelError("_FORM", Resources.Resources.UsernamePasswordIncorrect);
            }

            return ModelState.IsValid;
        }

        private bool ValidateRegistration(string userName, string email, string password, string confirmPassword) {
            if (String.IsNullOrEmpty(userName)) {
                ModelState.AddModelError("username", Resources.Resources.YouMustSpecifyUsername);
            }
            if (String.IsNullOrEmpty(email)) {
                ModelState.AddModelError("email", Resources.Resources.YouMustSpecifyEmailAddress);
            }
            if (password == null || password.Length < MembershipService.MinPasswordLength) {
                ModelState.AddModelError("password",
                    String.Format(CultureInfo.CurrentCulture,
                          Resources.Resources.YouMustSpecifyLongerPassword,
                         MembershipService.MinPasswordLength));
            }
            if (!String.Equals(password, confirmPassword, StringComparison.Ordinal)) {
                ModelState.AddModelError("_FORM", Resources.Resources.NewPasswordAndConfirmationMismatch);
            }
            return ModelState.IsValid;
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus) {
            // See http://msdn.microsoft.com/en-us/library/system.web.security.membershipcreatestatus.aspx for
            // a full list of status codes.
            switch (createStatus) {
                case MembershipCreateStatus.DuplicateUserName:
                    return Resources.Resources.UsernameAlreadyExists;

                case MembershipCreateStatus.DuplicateEmail:
                    return Resources.Resources.EnterDifferentEmail;

                case MembershipCreateStatus.InvalidPassword:
                    return Resources.Resources.InvalidPassword;

                case MembershipCreateStatus.InvalidEmail:
                    return Resources.Resources.InvalidEmail;

                case MembershipCreateStatus.InvalidAnswer:
                    return Resources.Resources.InvalidAnswer;

                case MembershipCreateStatus.InvalidQuestion:
                    return Resources.Resources.InvalidQuestion;

                case MembershipCreateStatus.InvalidUserName:
                    return Resources.Resources.InvalidUsername;

                case MembershipCreateStatus.ProviderError:
                    return Resources.Resources.ProviderError;

                case MembershipCreateStatus.UserRejected:
                    return Resources.Resources.UserRejected;

                default:
                    return Resources.Resources.UnknownError;
            }
        }
        #endregion
    }

    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    public interface IFormsAuthentication {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthentication {
        public void SignIn(string userName, bool createPersistentCookie) {
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                1, //version
                userName, // user name
                DateTime.Now,             //creation
                DateTime.Now.AddMinutes(30), //Expiration
                createPersistentCookie, //Persistent
                userName); //since Classic logins don't have a "Friendly Name".  OpenID logins are handled in the AuthController.

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
        }
        public void SignOut() {
            FormsAuthentication.SignOut();
        }
    }

    public interface IMembershipService {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        string GetCanonicalUsername(string userName);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }

    public class AccountMembershipService : IMembershipService {
        private MembershipProvider _provider;

        public AccountMembershipService()
            : this(null) {
        }

        public AccountMembershipService(MembershipProvider provider) {
            _provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength {
            get {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password) {
            return _provider.ValidateUser(userName, password);
        }

        public string GetCanonicalUsername(string userName)
        {
            var user = _provider.GetUser(userName, true);
            if (user != null)
            {
               return user.UserName;
            }

            return null;
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email) {
            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword) {
            MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
            return currentUser.ChangePassword(oldPassword, newPassword);
        }
    }
}
