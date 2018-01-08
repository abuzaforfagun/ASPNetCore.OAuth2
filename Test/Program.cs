using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri authorizationServerTokenIssuerUri = new Uri("http://localhost:57738/connect/token");
            string clientId = "ClientIdThatCanOnlyRead";
            string clientSecret = "secret1";
            string scope = "scope.readaccess";

            //access token request
            string rawJwtToken = RequestTokenToAuthorizationServer(
                 authorizationServerTokenIssuerUri,
                 clientId,
                 scope,
                 clientSecret)
                .GetAwaiter()
                .GetResult();
            using(HttpClient client = new HttpClient())
            {

            }
            Console.WriteLine(rawJwtToken);
            Console.Read();
        }

        private static async Task<string> RequestTokenToAuthorizationServer(Uri uriAuthorizationServer, string clientId, string scope, string clientSecret)
        {
            HttpResponseMessage responseMessage;
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage tokenRequest = new HttpRequestMessage(HttpMethod.Post, uriAuthorizationServer);
                HttpContent httpContent = new FormUrlEncodedContent(
                    new[]
                    {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("scope", scope),
                    new KeyValuePair<string, string>("client_secret", clientSecret)
                    });
                tokenRequest.Content = httpContent;
                responseMessage = await client.SendAsync(tokenRequest);
            }
            return await responseMessage.Content.ReadAsStringAsync();
        }
    }
}
