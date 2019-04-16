using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using U2UConsult.Framework;
using U2UConsult.Framework.Cryptography;
using U2UConsult.IdentityHub.AccountProvider;
using U2UConsult.IdentityHub.Contracts.AccountProviders;

namespace U2UConsult.DemoAccountProvider
{
    /// <summary>
    /// A custom Account Provider implementation.
    /// </summary>
    /// <remarks>
    /// Inheriting from IQueryableAccountProvider is also supported.
    /// IQueryableAccountProvider support retrieving the claims of a user when the user is not "online" (not logging in).
    /// </remarks>
    /// <seealso cref="U2UConsult.IdentityHub.Contracts.AccountProviders.IAccountProvider" />
    /// public sealed class CustomAccountProvider : IQueryableAccountProvider
    public sealed class CustomAccountProvider : IAccountProvider
    {
        /// <summary>
        /// Gets or sets the account provider identifier.
        /// </summary>
        /// <value>
        /// The account provider identifier.
        /// </value>
        public int AccountProviderId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the account provider manager.
        /// </summary>
        /// <value>
        /// The account provider manager.
        /// </value>
        public IAccountProviderManager AccountProviderManager
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the account provider type identifier.
        /// </summary>
        /// <value>
        /// The account provider type identifier.
        /// </value>
        /// <remarks>
        /// Must be a global unique value.
        /// </remarks>
        public Guid AccountProviderTypeId
        {
            get { return CustomAccountProviderManagerFactory.CustomAccountProviderTypeId; }
        }

        /// <summary>
        /// Gets a value indicating whether to always show display name.
        /// </summary>
        /// <value>
        /// <c>true</c> if to always show display name; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Applies to the page the user sees to choose an Account Provider to log on.
        /// </remarks>
        public bool AlwaysShowDisplayName
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets the configuration settings.
        /// </summary>
        /// <value>
        /// The configuration settings.
        /// </value>
        public AccountProviderConfiguration ConfigurationSettings
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        /// <remarks>
        /// Applies to the page the user sees to choose an Account Provider to log on.
        /// </remarks>
        public string DisplayName
        {
            get { return ConfigurationSettings.DisplayName; }
        }

        /// <summary>
        /// Gets a value indicating whether the information in the claims can be overwritten by the identity.
        /// </summary>
        /// <value>
        /// <c>true</c> if claim information is overwritable; otherwise, <c>false</c>.
        /// </value>
        public bool IdentityCanEditInformation
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the name of the issuer.
        /// </summary>
        /// <value>
        /// The name of the issuer.
        /// </value>
        /// <remarks>
        /// Used to set the Original Issuer for the claims that are the result of logging on with this Account Provider.
        /// </remarks>
        public string IssuerName
        {
            get { return CustomAccountProviderManager.GetIssuerName(new Identifier(AccountProviderId).ObfuscatedValue.ToString(CultureInfo.InvariantCulture)); }
        }

        /// <summary>
        /// Gets a value indicating whether accounts from this account provider can be linked with other accounts.
        /// </summary>
        /// <value>
        /// <c>true</c> if accounts from this account provider can be linked with other accounts; otherwise, <c>false</c>.
        /// </value>
        public bool LinkWithEnabled
        {
            get { return ConfigurationSettings.LinkWithEnabled; }
        }

        /// <summary>
        /// Gets a value indicating whether accounts from this account provider can be used to log on.
        /// </summary>
        /// <value>
        /// <c>true</c> if accounts from this account provider can be used to log on; otherwise, <c>false</c>.
        /// </value>
        public bool LogOnEnabled
        {
            get { return ConfigurationSettings.LogOnEnabled; }
        }

        /// <summary>
        /// Gets the provider image.
        /// </summary>
        /// <value>
        /// The provider image.
        /// </value>
        /// <remarks>
        /// Applies to the page the user sees to choose an Account Provider to log on.
        /// </remarks>
        public Uri ProviderImageUrl
        {
            get
            {
                if (AccountProviderManager.AccountProviderImageConfigurable && !string.IsNullOrEmpty(ConfigurationSettings.AccountProviderImageUrl))
                {
                    return new Uri(ConfigurationSettings.AccountProviderImageUrl, UriKind.Absolute);
                }
                else
                {
                    return AccountProviderManager.AccountProviderDefaultImageUrl;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the account provider supports friends.
        /// </summary>
        /// <value>
        /// <c>true</c> if the account provider supports friends; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Applies to IQueryableAccountProvider
        /// </remarks>
        public bool SupportsFriends
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the provider supports sign out.
        /// </summary>
        /// <value>
        /// <c>true</c> if the provider supports sign out; otherwise, <c>false</c>.
        /// </value>
        public bool SupportsSignOut
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the list of claims.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        /// A list of claims for the user.
        /// </returns>
        /// <exception cref="ArgumentNullException">state is <c>null</c> or request is <c>null</c></exception>
        /// <exception cref="UnauthorizedAccessException">
        /// </exception>
        public Task<ICollection<Claim>> GetListOfClaimsAsync(HttpRequestBase request, string state)
        {
            if (string.IsNullOrEmpty(state))
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var tenantId = AccountProviderManager.GetAccountProviderTenant(this.AccountProviderId).TenantId;
            var customPassedParameters = CustomAccountProviderManager.GetCustomPassedParameters(tenantId);

            return Task.FromResult(CustomAccountProviderManager.CreateUserClaimSet(ConfigurationSettings as CustomAccountProviderConfiguration));
        }

        /// <summary>
        /// Gets the list of claims asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="privatePersonalIdentifier">The private personal identifier.</param>
        /// <returns></returns>
        /// <remarks>
        /// Applies to IQueryableAccountProvider
        /// Return the claims of the user even if the user is not "online".
        /// </remarks>
        //public Task<ICollection<Claim>> GetListOfClaimsAsync(Claim name, Claim privatePersonalIdentifier)
        //{
        //}

        /// <summary>
        /// Gets the list of friends asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="privatePersonalIdentifier">The private personal identifier.</param>
        /// <returns></returns>
        /// <remarks>
        /// Applies to IQueryableAccountProvider.
        /// Return the PPID of the Friends of the user identified by name/privatePersonalIdentifier parameters.
        /// Must match values set by claims.Add(CreateAccountProviderClaim(U2UConsultClaimTypes.PrivatePersonalIdentifier, userId, issuer)) for the Friends.
        /// </remarks>
        //public Task<Claim[]> GetListOfFriendsAsync(Claim name, Claim privatePersonalIdentifier)
        //{
        //}

        /// <summary>
        /// Gets the profile URL.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="privatePersonalIdentifier">The private personal identifier.</param>
        /// <returns>
        /// The url to the profile page of the account.
        /// </returns>
        /// <remarks>
        /// An external page related to this Account Provider that show profile info.
        /// </remarks>
        public Uri GetProfileUrl(Claim name, Claim privatePersonalIdentifier)
        {
            return null;
        }

        /// <summary>
        /// Gets the sign in URL.
        /// </summary>
        /// <param name="returnUrl">The return url.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        /// The sign in url.
        /// </returns>
        public Task<Uri> GetSignInUrlAsync(Uri returnUrl, string state)
        {
            if (returnUrl == null)
            {
                throw new ArgumentNullException(nameof(returnUrl));
            }

            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (CustomAccountProviderManager.ValidateReturnUrl(returnUrl))
            {
                var tenant = CustomAccountProviderManager.GetTenantUrlSegment(HttpContext.Current.Request);

                return Task.FromResult(
                    new Uri(
                        string.Format(
                        CultureInfo.InvariantCulture,
                        string.Format(CultureInfo.InvariantCulture, CustomAccountProviderManagerFactory.CustomAccountProviderLogonUrl, tenant) + "?returnUrl={0}&state={1}",
                        returnUrl.UrlEncode(),
                        state.Base64ToBase64Url())));
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(returnUrl), "Invalid returnUrl");
            }
        }

        /// <summary>
        /// Gets the sign out URL.
        /// </summary>
        /// <param name="name">The name claim of the user.</param>
        /// <param name="privatePersonalIdentifier">The private personal identifier of the user.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>
        /// The sign out url.
        /// </returns>
        public Uri GetSignOutUrl(Claim name, Claim privatePersonalIdentifier, Uri returnUrl)
        {
            var tenant = CustomAccountProviderManager.GetTenantUrlSegment(HttpContext.Current.Request);

            return new Uri(string.Format(CultureInfo.InvariantCulture, CustomAccountProviderManagerFactory.CustomAccountProviderLogOffUrl, tenant, returnUrl.UrlEncode()));
        }

        /// <summary>
        /// Processes the log on.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>
        /// The returnUrl.
        /// </returns>
        public Task<ProcessLogonResult> ProcessLogOnAsync(HttpContextBase httpContext)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Processes the completion of the sign in.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="state">The state.</param>
        public void ProcessSignInComplete(HttpRequestBase httpRequest, string state)
        {
        }

        /// <summary>
        /// Processes the sign out.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        public void ProcessSignOut(HttpContextBase httpContext)
        {
            CookieManager.RemoveCustomAccountProviderValueFromCookie(new Framework.Cryptography.Identifier(AccountProviderId));
        }
    }
}