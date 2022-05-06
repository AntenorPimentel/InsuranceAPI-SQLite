using System.Net.Http;
using System.Threading.Tasks;

namespace Insurance.Data
{
    public class APIClientBase
    {
        protected readonly HttpClient _httpClient;

        public APIClientBase() { }

        public APIClientBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAsync(string url)
        {
            using var httpResponse = await _httpClient.GetAsync(url).ConfigureAwait(false);

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                throw new HttpRequestException(await BuildHttpException(httpResponse));

            return await httpResponse.Content.ReadAsStringAsync();
        }

        private static async Task<string> BuildHttpException(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();
            var code = response.StatusCode.GetHashCode().ToString();
            return $"API thrown an exception. Response code: {code}, Response body: {body}.";
        }
    }
}