using Currency.API.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Currency.API.Models
{
    public class User : UserDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //Propiedades heredadas de UserDTO
        //public required string Name { get; set; }
        //public required string Email { get; set; }
        //public required string Password { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
