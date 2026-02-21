using Currency.API.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Currency.API.Models
{
    public class Address : AddressDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  Id { get; set; }

        //Propiedades heredadas de AddressDTO
        //public required int UserId { get; set; }
        //public required string Street { get; set; }
        //public required string City { get; set; }
        //public required string Country { get; set; }
        //public required string ZipCode { get; set; }
    }
}
