using System.Globalization;
using System.Linq;
using System.Web;
using U2UConsult.Framework;
using U2UConsult.Framework.Cryptography;

namespace U2UConsult.DemoAccountProvider
{
    /// <summary>
    /// To manage all cookies.
    /// </summary>
    internal static class CookieManager
    {
        /// <summary>
        /// The custom account provider cookie name.
        /// </summary>
        private const string CustomAccountProviderCookieName = "CustomAccountProviderCookie";

        /// <summary>
        /// The  key name.
        /// </summary>
        private const string CustomAccountProviderKeyName = "CustomAccountProviderCookieKey";

        /// <summary>
        /// Gets the session secret from the cookie.
        /// </summary>
        /// <param name="accountProviderId">The account provider identifier.</param>
        /// <returns>
        /// The session secret.
        /// </returns>
        internal static string GetCustomAccountProviderValueFromCookie(Identifier accountProviderId)
        {
            if (HttpContext.Current != null)
            {
                HttpRequest request = HttpContext.Current.Request;

                if (request.Cookies.AllKeys.Contains(CustomAccountProviderCookieName))
                {
                    HttpCookie cookie = request.Cookies[CustomAccountProviderCookieName];

                    var value = cookie.Values[CustomAccountProviderKeyName + accountProviderId.ObfuscatedValue.ToString(CultureInfo.InvariantCulture)];

                    if (string.IsNullOrEmpty(value))
                    {
                        return null;
                    }

                    return value.Base64UrlToBase64();
                }
            }

            return null;
        }

        /// <summary>
        /// Removes the custom account provider value from cookie.
        /// </summary>
        /// <param name="accountProviderId">The account provider identifier.</param>
        internal static void RemoveCustomAccountProviderValueFromCookie(Identifier accountProviderId)
        {
            if (HttpContext.Current != null)
            {
                CookieManager.SetCustomAccountProviderValueInCookie(null, accountProviderId);
            }
        }

        /// <param name="sessionSecret">The session secret.</param>
        /// <param name="accountProviderId">The account provider identifier.</param>
        internal static void SetCustomAccountProviderValueInCookie(string value, Identifier accountProviderId)
        {
            if (HttpContext.Current != null)
            {
                HttpResponse response = HttpContext.Current.Response;
                HttpRequest request = HttpContext.Current.Request;

                HttpCookie cookie;

                if (request.Cookies.AllKeys.Contains(CustomAccountProviderCookieName))
                {
                    cookie = request.Cookies[CustomAccountProviderCookieName];
                    cookie.HttpOnly = true;
                    cookie.Secure = true;
                }
                else
                {
                    cookie = new HttpCookie(CustomAccountProviderCookieName)
                    {
                        HttpOnly = true,
                        Secure = true
                    };
                }

                CookieManager.SetCustomAccountProviderValueInCookie(cookie, value, accountProviderId);

                response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// Sets the session secret in cookie.
        /// </summary>
        /// <param name="cookie">The cookie.</param>
        /// <param name="sessionSecret">The session secret.</param>
        /// <param name="accountProviderId">The account provider identifier.</param>
        private static void SetCustomAccountProviderValueInCookie(HttpCookie cookie, string value, Identifier accountProviderId)
        {
            cookie.Values[CustomAccountProviderKeyName + accountProviderId.ObfuscatedValue.ToString(CultureInfo.InvariantCulture)] = string.IsNullOrEmpty(value) ? value : value.Base64ToBase64Url();
        }
    }
}