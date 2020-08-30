using System.ComponentModel.DataAnnotations;

namespace Product.API.Resources
{
    public class SaveItemResource
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}