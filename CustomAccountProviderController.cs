using System;
using System.Diagnostics;
using System.Web.Mvc;
using U2UConsult.IdentityHub.AccountProvider;
using U2UConsult.IdentityHub.Web.Controllers;

namespace U2UConsult.DemoAccountProvider
{
    /// <summary>
    /// Deploy the assembly in Customizations/Extensions/AccountProviders folder.
    /// Deploy the view (cshtml) in Customizations/Views/[ControllerName]/ (Customizations/Views/CustomAccountProvider/)
    /// </summary>
    /// <seealso cref="U2UConsult.IdentityHub.Web.Controllers.BaseController" />
    [RequireHttps]
    public sealed class CustomAccountProviderController : BaseController
    {
        /// <summary>
        /// Returns the SignIn view.
        /// </summary>
        /// <returns>
        /// The SignIn view.
        /// </returns>
        [HttpGet]
        public ActionResult SignIn(string returnUrl, string state)
        {
            if (string.IsNullOrEmpty(returnUrl) || string.IsNullOrEmpty(state))
            {
                return Redirect(Configuration.HubUrl + "/" + Tenant);
            }

            var accountProvider = (GetAccountProvider() as CustomAccountProvider) ?? GetLinkAccountAccountProvider() as CustomAccountProvider;

            int tenantId = accountProvider.AccountProviderManager.GetAccountProviderTenant(accountProvider.AccountProviderId).TenantId;
            var customParameters = CustomAccountProviderManager.GetCustomPassedParameters(tenantId);

            return CustomView(new CustomAccountProviderSignInViewModel() { ReturnUrl = returnUrl, State = state, CustomParameter = customParameters != null ? customParameters[0] : string.Empty });
        }

        /// <summary>
        /// When a user signs in.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("SignIn")]
        [ValidateAntiForgeryToken]
        public ActionResult SignInPost(CustomAccountProviderSignInViewModel model)
        {
            Debug.Assert(model != null);

            if (!ModelState.IsValid)
            {
                return CustomView(model);
            }

            var returnUrl = new Uri(model.ReturnUrl);

            if (!CustomAccountProviderManager.ValidateReturnUrl(returnUrl))
            {
                return new HttpUnauthorizedResult();
            }

            // Do validation
            // If the user is in the process of linking his account to another account there is the account provider can be retrieved with GetLinkAccountAccountProvider())
            var accountProvider = (GetAccountProvider() as CustomAccountProvider) ?? GetLinkAccountAccountProvider() as CustomAccountProvider;

            if (accountProvider == null)
            {
                return new HttpUnauthorizedResult();
            }

            CookieManager.SetCustomAccountProviderValueInCookie("124578895613", new Framework.Cryptography.Identifier(accountProvider.AccountProviderId));

            return new RedirectResult(returnUrl.AbsoluteUri);
        }
    }
}