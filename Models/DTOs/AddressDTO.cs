namespace Currency.API.Models.DTOs
{
    public class AddressDTO
    {
        public required int UserId { get; set; }
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public required string ZipCode { get; set; }
    }
}
