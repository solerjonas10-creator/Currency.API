namespace Currency.API.Models.DTOs
{
    public class AddressDTO
    {
        public required string Street { get; set; } = "Calle";
        public required string City { get; set; } = "Asunción";
        public required string Country { get; set; } = "Paraguay";
        public required string ZipCode { get; set; } = "";
    }
}
