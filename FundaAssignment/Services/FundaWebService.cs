using System.Net.Http;
using FundaAssignment.DTO;
using FundaAssignment.Helpers;
using Newtonsoft.Json;

namespace FundaAssignment.Services
{
    /// <summary>
    /// Service used to perform the web requests to the Funda API
    /// </summary>
    public class FundaWebService
    {
        private HttpClient _client = new HttpClient();

        private string[] _searchParams = Array.Empty<string>();
        private int _retries = 0;

        //Maximum the API ever returns is 25
        private const int PAGE_SIZE = 25;
        //Arbitrary amount. Could be moved to a config file.
        private const int MAX_RETRIES = 3;

        /// <summary>
        /// Total amount of items we can get for the specified search parameters.
        /// Gets set after the first request has been sent.
        /// </summary>
        public int TotalEntries { get; private set; }
        /// <summary>
        /// Keeps track of how many entries we have received.
        /// </summary>
        public int ReturnedEntries { get; private set; }
        /// <summary>
        /// Page we are currently requesting.
        /// </summary>
        public int CurrentPage { get; private set; } = 1;

        public FundaWebService() { }

        public void Init(string[] searchParams)
        {
            TotalEntries = 0;
            ReturnedEntries = 0;
            CurrentPage = 1;

            _retries = 0;

            _searchParams = searchParams;
        }

        /// <summary>
        /// Fetches the next array of makelaars from the API.
        /// If a known exception occurs (e.g. too many requests per minute) will retry 3 times.
        /// If the max retries is exceeded or an unknown error occurs, the web service will cease its requests.
        /// </summary>
        /// <returns>
        ///     Returns a tuple with the following data:
        ///         bool: Indicates whether there is more data to be fetched.
        ///         string[]: List of makelaar names
        /// </returns>
        public async Task<Tuple<bool, string[]>> GetNextMakelaars()
        {
            var queryParameters = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("type", "koop"),
                new KeyValuePair<string, string>("zo", $"/{string.Join("/", _searchParams)}/"),
                new KeyValuePair<string, string>("pagesize", $"{PAGE_SIZE}"),
                new KeyValuePair<string, string>("page", $"{CurrentPage++}")
            };

            var uri = FundaURIHelper.UriWithSearch(queryParameters);
            string responseJson = string.Empty;
            try
            {
                responseJson = await _client.GetStringAsync(uri);
            }
            catch (HttpRequestException)
            {
                LoggingService.Instance.Log("Something went wrong with the request. This could be due to sending too many requests. Attempting again in a few seconds...");
                if (_retries < MAX_RETRIES)
                {
                    _retries++;
                    return new Tuple<bool, string[]>(true, Array.Empty<string>());
                }
                else
                {
                    LoggingService.Instance.Log("Maximum retries attempted. Stopping process.");
                    return new Tuple<bool, string[]>(false, Array.Empty<string>());
                }
            }
            catch (Exception)
            {
                LoggingService.Instance.Log("An unknown error occurred. Please try again later.");
                return new Tuple<bool, string[]>(false, Array.Empty<string>());
            }
            var response = JsonConvert.DeserializeObject<FundaResponse>(responseJson);

            var makelaars = response?.Objects?.Select(x => x.MakelaarNaam).ToArray();

            if (response != null)
            {
                TotalEntries = response.TotaalAantalObjecten;
                ReturnedEntries += response.Objects.Length;
            }

            return new Tuple<bool, string[]>(ReturnedEntries < response?.TotaalAantalObjecten, makelaars ?? Array.Empty<string>());
        }
    }
}
