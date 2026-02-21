using Currency.API.Models.DTOs;

namespace Currency.API.Models
{
    public class User : UserDTO
    {
        public int Id { get; set; }

        //Propiedades heredadas de UserDTO
        //public required string Name { get; set; }
        //public required string Email { get; set; }
        //public required string Password { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
