using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Xml.Linq;
using U2UConsult.Framework.Cryptography;
using U2UConsult.Framework.Identity;
using U2UConsult.IdentityHub.AccountProvider;
using U2UConsult.IdentityHub.Contracts.AccountProviders;

namespace U2UConsult.DemoAccountProvider
{
    /// <summary>
    /// Class thata manages the Account Provider instances (configuration) and has some general settings.
    /// </summary>
    /// <seealso cref="U2UConsult.IdentityHub.AccountProvider.AccountProviderManager{U2UConsult.DemoAccountProvider.CustomAccountProviderConfiguration, U2UConsult.DemoAccountProvider.CustomAccountProvider}" />
    internal sealed class CustomAccountProviderManager : AccountProviderManager<CustomAccountProviderConfiguration, CustomAccountProvider>
    {
        /// <summary>
        /// The issuer name.
        /// </summary>
        private const string IssuerNamePrefix = "uri:customaccountprovider:";

        /// <summary>
        /// The account provider default image URL
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public override Uri AccountProviderDefaultImageUrl
        {
            get
            {
                return new Uri("https://www.theidentityhub.com/content/images/u2uconsult.jpg");
            }
        }

        /// <summary>
        /// The account provider display name
        /// </summary>
        public override string AccountProviderDisplayName
        {
            get
            {
                return "CustomAccountProvider";
            }
        }

        /// <summary>
        /// The account provider image configurable.
        /// </summary>
        public override bool AccountProviderImageConfigurable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// The account provider is unique
        /// </summary>
        /// <remarks>
        /// If unique only one instance will be allowed per Tenant.
        /// </remarks>
        public override bool AccountProviderIsUnique
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// The account provider protocol
        /// </summary>
        /// <remarks>
        /// MUST be AccountProviderProtocol.Custom.
        /// </remarks>
        public override AccountProviderProtocol AccountProviderProtocol
        {
            get
            {
                return AccountProviderProtocol.Custom;
            }
        }

        /// <summary>
        /// The account provider type identifier
        /// </summary>
        /// <remarks>
        /// MUST be globally unique.
        /// </remarks>
        public override Guid AccountProviderTypeId
        {
            get
            {
                return CustomAccountProviderManagerFactory.CustomAccountProviderTypeId;
            }
        }

        /// <summary>
        /// The display name configurable
        /// </summary>
        /// <remarks>
        /// When true the display name can be modified in the administration UI.
        /// </remarks>
        public override bool DisplayNameConfigurable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the account provider configuration.
        /// </summary>
        /// <param name="storedConfiguration">The stored configuration.</param>
        /// <returns>
        /// The configuration.
        /// </returns>
        /// <remarks>
        /// Use this method to modify the configuration coming from the database, before it is used.
        /// </remarks>
        public override CustomAccountProviderConfiguration GetAccountProviderConfiguration(CustomAccountProviderConfiguration storedConfiguration)
        {
            return base.GetAccountProviderConfiguration(storedConfiguration);
        }

        /// <summary>
        /// Gets the provided claim types.
        /// </summary>
        /// <param name="accountProvider">The account provider.</param>
        /// <returns>
        /// The type claims that can be returned by the account provider.
        /// </returns>
        public override string[] GetProvidedClaimTypes(IAccountProvider accountProvider)
        {
            return base.GetProvidedClaimTypes(accountProvider);
        }

        /// <summary>
        /// Updates the account provider claims tenant URL segment.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="currentUrlSegment">The current URL segment.</param>
        /// <param name="newUrlSegment">The new URL segment.</param>
        /// <returns>
        /// The updated claims
        /// </returns>
        /// <remarks>
        /// When the Tenant url segment changes, this method can be used to modify the claims of accounts if that would be required.
        /// </remarks>
        public override XElement UpdateAccountProviderClaimsTenantUrlSegment(XElement claims, string currentUrlSegment, string newUrlSegment)
        {
            return claims;
        }

        /// <summary>
        /// Updates the account provider configuration tenant URL segment.
        /// </summary>
        /// <param name="configurationXml">The configuration XML.</param>
        /// <param name="currentUrlSegment">The current URL segment.</param>
        /// <param name="newUrlSegment">The new URL segment.</param>
        /// <returns>
        /// The updated configuration.
        /// </returns>
        /// <remarks>
        /// When the Tenant url segment changes, this method can be used to modify the Account Provider configuration, if that would be required.
        /// </remarks>
        public override XElement UpdateAccountProviderConfigurationTenantUrlSegment(XElement configurationXml, string currentUrlSegment, string newUrlSegment)
        {
            return configurationXml;
        }

        /// <summary>
        /// Validates the create account provider configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns><see cref="System.Array"/> of <see cref="System.String"/> containing validation errors.</returns>
        /// <remarks>
        /// This method can be used to validate and modify the configuration before it is stored in the database.
        /// This method is called when a new Account Provider instance is created.
        /// </remarks>
        public override string[] ValidateCreateAccountProviderConfiguration(CustomAccountProviderConfiguration configuration, string tenantUrlSegment)
        {
            return base.ValidateCreateAccountProviderConfiguration(configuration, tenantUrlSegment);
        }

        /// <summary>
        /// Validates the delete account provider configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns><see cref="System.Array"/> of <see cref="System.String"/> containing validation errors.</returns>
        /// <remarks>
        /// This method can be used to validate the deletion (or perform custom clean up) of an Account Povider instance before the deletion is executed in the database.
        /// This method is called when an Account Provider instance is being deleted.
        /// </remarks>
        public override string[] ValidateDeleteAccountProviderConfiguration(CustomAccountProviderConfiguration configuration)
        {
            return base.ValidateDeleteAccountProviderConfiguration(configuration);
        }

        /// <summary>
        /// Validates the update account provider configuration.
        /// </summary>
        /// <param name="oldConfiguration">The old configuration.</param>
        /// <param name="newConfiguration">The new configuration.</param>
        /// <returns><see cref="System.Array"/> of <see cref="System.String"/> containing validation errors.</returns>
        /// This method can be used to validate and modify the configuration before it is stored in the database.
        /// This method is called when an Account Provider instance is updated.
        /// </remarks>
        public override string[] ValidateUpdateAccountProviderConfiguration(CustomAccountProviderConfiguration oldConfiguration, CustomAccountProviderConfiguration newConfiguration)
        {
            return base.ValidateUpdateAccountProviderConfiguration(oldConfiguration, newConfiguration);
        }

        /// <summary>
        /// Creates the user claim set.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The claims</returns>
        /// <remarks>
        /// Modify this method to retrieve and return the claims from the custom location.
        /// </remarks>
        internal static ICollection<Claim> CreateUserClaimSet(CustomAccountProviderConfiguration configuration)
        {
            var accountProviderId = new Identifier(configuration.Id);
            var userId = CookieManager.GetCustomAccountProviderValueFromCookie(accountProviderId);

            var claims = new List<Claim>(5);

            var issuer = GetIssuerName(accountProviderId.ObfuscatedValue.ToString(CultureInfo.InvariantCulture));

            claims.Add(CreateAccountProviderClaim(U2UConsultClaimTypes.EmailAddress, "user@mail.com", issuer));
            claims.Add(CreateAccountProviderClaim(U2UConsultClaimTypes.PrivatePersonalIdentifier, userId, issuer));
            claims.Add(CreateAccountProviderClaim(U2UConsultClaimTypes.Name, userId, issuer));
            claims.Add(CreateAccountProviderClaim(U2UConsultClaimTypes.GivenName, "UserGivenName", issuer));
            claims.Add(CreateAccountProviderClaim(U2UConsultClaimTypes.Surname, "UserSurname", issuer));

            return claims;
        }

        /// <summary>
        /// Gets the name of the issuer.
        /// </summary>
        /// <param name="accountProviderId">The account provider identifier.</param>
        /// <returns>
        /// The name of the issuer.
        /// </returns>
        internal static string GetIssuerName(string accountProviderId)
        {
            return IssuerNamePrefix + accountProviderId;
        }
    }
}