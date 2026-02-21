using Currency.API.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Currency.API.Models
{
    public class Currency : CurrencyDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //propiedades heredadas de CurrencyDTO
        //public required string Name { get; set; }
        //public required string Code { get; set; }
        //public required decimal RateToBase { get; set; }
        
    }
}
