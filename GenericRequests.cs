using Newtonsoft.Json;
using System.Diagnostics;

namespace bvtcx_api {


    public class GenericRequests {
        private static readonly HttpClient client = new HttpClient();
        private readonly string baseUrl;
        private readonly string apiKey;

        public GenericRequests(string url, string apiKey) {
            this.baseUrl = $"https://{url}/"; // Construct the base URL here
            this.apiKey = apiKey;  // Store the API key for use in requests
        }

        public async Task<T> List<T>(string path) {
            var requestUrl = $"{baseUrl}{path}"; // Use the constructed base URL here
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("ApiKey", apiKey); // Add the API key to the request headers

            Stopwatch stopwatch = Stopwatch.StartNew();

            try {
                var response = await client.SendAsync(request).ConfigureAwait(false);
                stopwatch.Stop(); // Stopping stopwatch after getting response

                Console.ForegroundColor = response.IsSuccessStatusCode ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
                Console.Write($"{(int)response.StatusCode} {response.StatusCode}");
                Console.ForegroundColor = response.IsSuccessStatusCode ? ConsoleColor.Green : ConsoleColor.Red;
                Console.Write($" ({stopwatch.ElapsedMilliseconds} ms) ");
                Console.ResetColor();

                if (!response.IsSuccessStatusCode) {
                    response.Dispose();
                    return default(T);
                }

                var strResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var obj = JsonConvert.DeserializeObject<T>(strResponse);
                if (obj == null) throw new Exception("Unable to parse server response.");
                return obj;
            } catch (HttpRequestException e) {
                // Handle HTTP request-specific exceptions
                Console.WriteLine($"HttpRequestException: {e.Message}");
                return default(T);
            } catch (JsonException e) {
                // Handle JSON parsing errors
                Console.WriteLine($"JSON Parsing error: {e.Message}");
                return default(T);
            } catch (Exception e) {
                // General error handling
                Console.WriteLine($"An error occurred: {e.Message}");
                return default(T);
            } finally {
                request.Dispose();
            }
        }


        public async Task<T> GetObject<T>(string path) {
            var requestUrl = $"{baseUrl}{path}";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("ApiKey", apiKey);

            Stopwatch stopwatch = Stopwatch.StartNew();

            try {
                var response = await client.SendAsync(request).ConfigureAwait(false);
                stopwatch.Stop();

                Console.ForegroundColor = response.IsSuccessStatusCode ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
                Console.Write($"{(int)response.StatusCode} {response.StatusCode}");
                Console.ForegroundColor = response.IsSuccessStatusCode ? ConsoleColor.Green : ConsoleColor.Red;
                Console.Write($" ({stopwatch.ElapsedMilliseconds} ms) ");
                Console.ResetColor();

                if (!response.IsSuccessStatusCode) {
                    response.Dispose();
                    return default(T);
                }

                var strResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var obj = JsonConvert.DeserializeObject<T>(strResponse);
                if (obj == null) throw new Exception("Unable to parse server response.");
                return obj;
            } catch (HttpRequestException e) {
                Console.WriteLine($"HttpRequestException: {e.Message}");
                return default(T);
            } catch (JsonException e) {
                Console.WriteLine($"JSON Parsing error: {e.Message}");
                return default(T);
            } catch (Exception e) {
                Console.WriteLine($"An error occurred: {e.Message}");
                return default(T);
            } finally {
                request.Dispose();
            }
        }

    }

}
