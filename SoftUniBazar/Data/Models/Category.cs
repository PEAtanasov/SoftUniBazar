using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SoftUniBazar.Data.Models
{
    public class Category
    {
        /// <summary>
        /// Category identifier
        /// </summary>
        [Required]
        [Comment("Category identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Category name
        /// </summary>
        [Required]
        [MaxLength(Constants.CategoryNameMaxLength)]
        [Comment("Category name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Collection of ads in certain catecory
        /// </summary>
        [Comment("Collection of ads in certain catecory")]
        public ICollection<Ad> Ads { get; set; } = new List<Ad>();

    }
}
