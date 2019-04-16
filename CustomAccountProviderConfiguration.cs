using U2UConsult.IdentityHub.AccountProvider;
using U2UConsult.IdentityHub.Contracts.AccountProviders;

namespace U2UConsult.DemoAccountProvider
{
    /// <summary>
    /// Specific configuration settings for the Account Provider.
    /// </summary>
    /// <remarks>
    /// You can add any serializable configuration parameters.
    /// </remarks>
    /// <seealso cref="U2UConsult.IdentityHub.AccountProvider.AccountProviderConfiguration" />
    public sealed class CustomAccountProviderConfiguration : AccountProviderConfiguration
    {
        /// <summary>
        /// Gets the account provider protocol.
        /// </summary>
        /// <value>
        /// The account provider protocol.
        /// </value>
        public override AccountProviderProtocol AccountProviderProtocol
        {
            get
            {
                return AccountProviderProtocol.Custom;
            }
        }
    }
}