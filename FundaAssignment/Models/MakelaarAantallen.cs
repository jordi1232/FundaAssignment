namespace FundaAssignment.Models
{
    /// <summary>
    /// This object is used to keep track of how often we have encountered a specific Makelaar.
    /// </summary>
    public class MakelaarAantallen
    {
        /// <summary>
        /// Name of the makelaar
        /// </summary>
        public string MakelaarNaam { get; set; } = string.Empty;
        /// <summary>
        /// Amount of times we have encountered this Makelaar
        /// </summary>
        public int Aantal { get; set; } = 1;

        public MakelaarAantallen(string naam)
        {
            MakelaarNaam = naam;
        }
    }
}
