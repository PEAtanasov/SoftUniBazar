using SoftUniBazar.Data;
using System.ComponentModel.DataAnnotations;

namespace SoftUniBazar.Models
{
    public class AdFormModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(Constants.AdNameMaxLength,MinimumLength =Constants.AdNameMinLength)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(Constants.AdDescriptionMaxLength, MinimumLength = Constants.AdDescriptionMinLength)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public ICollection<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}
