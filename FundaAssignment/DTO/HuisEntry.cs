namespace FundaAssignment.Models
{
    public class HuisEntry
    {
        /// <summary>
        /// Address of the property. Not used, but can be used for instances where uniqueness between properties is needed
        /// </summary>
        public string Adres { get; set; } = string.Empty;
        /// <summary>
        /// Name of the Makelaar associated with the property
        /// </summary>
        public string MakelaarNaam { get; set; } = string.Empty;
    }
}
