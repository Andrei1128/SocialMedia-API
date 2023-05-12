using System.ComponentModel.DataAnnotations;

namespace FirstProject_API.Models
{
    public class VillaNumberUpdatedDTO
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; }
        public string SpecialDetails { get; set; }
    }
}
