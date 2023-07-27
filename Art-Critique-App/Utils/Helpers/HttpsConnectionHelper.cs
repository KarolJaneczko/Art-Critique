using System.Net.Security;

namespace Art_Critique.Utils.Helpers {
    public class HttpsConnectionHelper {
        #region Properties
        public int SslPort { get; }
        public string DevServerRootUrl { get; }
        private readonly Lazy<HttpClient> LazyHttpClient;
        public HttpClient HttpClient => LazyHttpClient.Value;
        public static string DevServerName =>
#if WINDOWS
                "localhost";
#elif ANDROID
                "10.0.2.2";
#else
                throw new PlatformNotSupportedException("Only Windows and Android currently supported.");
#endif
        #endregion

        #region Constructor
        public HttpsConnectionHelper(int sslPort) {
            SslPort = sslPort;
            DevServerRootUrl = FormattableString.Invariant($"https://{DevServerName}:{SslPort}");
            LazyHttpClient = new Lazy<HttpClient>(() => new HttpClient(GetPlatformMessageHandler()));
        }
        #endregion

        #region Methods
        public static HttpMessageHandler GetPlatformMessageHandler() {
#if WINDOWS
                    return null;
#elif ANDROID
            return new CustomAndroidMessageHandler {
                ServerCertificateCustomValidationCallback = (_, cert, __, errors) => {
                    if (cert?.Issuer.Equals("CN=localhost") == true)
                        return true;
                    return errors == SslPolicyErrors.None;
                }
            };
#else
            throw new PlatformNotSupportedException("Only Windows and Android currently supported.");
#endif
        }
        #endregion

        #region Classes
#if ANDROID
        internal sealed class CustomAndroidMessageHandler : Xamarin.Android.Net.AndroidMessageHandler {
            protected override Javax.Net.Ssl.IHostnameVerifier GetSSLHostnameVerifier(Javax.Net.Ssl.HttpsURLConnection connection)
                => new CustomHostnameVerifier();

            private sealed class CustomHostnameVerifier : Java.Lang.Object, Javax.Net.Ssl.IHostnameVerifier {
                public bool Verify(string hostname, Javax.Net.Ssl.ISSLSession session) {
                    return
                        Javax.Net.Ssl.HttpsURLConnection.DefaultHostnameVerifier.Verify(hostname, session)
                        || (hostname == "10.0.2.2" && session.PeerPrincipal?.Name == "CN=localhost");
                }
            }
        }
#endif
        #endregion
    }
}