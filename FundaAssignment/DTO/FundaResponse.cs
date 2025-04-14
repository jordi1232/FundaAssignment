using FundaAssignment.Models;

namespace FundaAssignment.DTO
{
    /// <summary>
    /// Represents the data we are interested in from the Funda API
    /// </summary>
    public class FundaResponse
    {
        /// <summary>
        /// Array of Huisentry in the response.
        /// </summary>
        public HuisEntry[] Objects { get; set; } = Array.Empty<HuisEntry>();
        /// <summary>
        /// Represents the total amount of objects available for the used ZoekOpdracht.
        /// Used for the ProgressBar and to know when we can stop sending more requests.
        /// </summary>
        public int TotaalAantalObjecten { get; set; }
    }
}
