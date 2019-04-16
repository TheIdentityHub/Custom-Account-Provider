using System;
using U2UConsult.IdentityHub.Contracts.AccountProviders;
using U2UConsult.IdentityHub.Web.Services;

namespace U2UConsult.DemoAccountProvider
{
    /// <summary>
    /// Account provider manager factory for Custom provider.
    /// </summary>
    [AccountProviderManagerFactory]
    internal sealed class CustomAccountProviderManagerFactory : IAccountProviderManagerFactory
    {
        /// <summary>
        /// The custom account provider logoff URL.
        /// For Custom Account Providers that use external site provider the url of the external site.
        /// </summary>
        internal static readonly string CustomAccountProviderLogOffUrl = Configuration.HubUrl + "/{0}/CustomAccountProvider/SignOut";

        /// <summary>
        /// The custom account provider logon URL.
        /// For Custom Account Providers that use external site provider the url of the external site.
        /// </summary>
        internal static readonly string CustomAccountProviderLogonUrl = Configuration.HubUrl + "/{0}/CustomAccountProvider/SignIn";

        /// <summary>
        /// The account provider type identifier.
        /// </summary>
        /// <remarks>
        /// Change this value and use a new GUID.
        /// </remarks>
        internal static readonly Guid CustomAccountProviderTypeId = new Guid("{254B3FC0-913D-41E4-80E7-FD1A4F664448}");

        /// <summary>
        /// Gets the account provider type identifier.
        /// </summary>
        /// <value>
        /// The account provider type identifier.
        /// </value>
        public Guid AccountProviderTypeId
        {
            get { return CustomAccountProviderTypeId; }
        }

        /// <summary>
        /// Gets the account provider manager.
        /// </summary>
        /// <returns>
        /// A <see cref="IAccountProviderManager" />
        /// </returns>
        public IAccountProviderManager GetAccountProviderManager()
        {
            return new CustomAccountProviderManager();
        }
    }
}