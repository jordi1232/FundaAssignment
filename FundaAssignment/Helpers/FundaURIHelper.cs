using System.Collections.Specialized;
using System.Web;

namespace FundaAssignment.Helpers
{
    public static class FundaURIHelper
    {
        //Example: http://partnerapi.funda.nl/feeds/Aanbod.svc/[Key]/?type=koop&zo=/amsterdam/tuin/&page=1&pagesize=25

        //Below should be replaced with a call to retrieve the key from some secret vault.
        //Currently, this approach allows for minimal setup time
        private const string KEY = "<REPLACE_WITH_KEY>";
        private const string BASE_URI = "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/";

        private static Uri UriWithKey()
        {
            var baseUri = new Uri(BASE_URI);
            return new Uri(baseUri, KEY);
        }

        /// <summary>
        /// Builds a URI using the base address, key, and input parameters.
        /// </summary>
        /// <param name="parameters">KeyValuePairs used to build the query string</param>
        /// <returns></returns>
        public static Uri UriWithSearch(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var uri = UriWithKey();

            NameValueCollection queryParams = HttpUtility.ParseQueryString(string.Empty);

            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                queryParams.Add(parameter.Key, parameter.Value);
            }

            return new Uri(uri, $"?{queryParams}");
        }
    }
}
