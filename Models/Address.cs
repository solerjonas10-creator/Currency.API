using Currency.API.Models.DTOs;

namespace Currency.API.Models
{
    public class Address : AddressDTO
    {
        public int  Id { get; set; }

        //Propiedades heredadas de AddressDTO
        //public required int UserId { get; set; }
        //public required string Street { get; set; }
        //public required string City { get; set; }
        //public required string Country { get; set; }
        //public required string ZipCode { get; set; }
    }
}
