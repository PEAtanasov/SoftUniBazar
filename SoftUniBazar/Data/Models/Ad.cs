using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftUniBazar.Data.Models
{
    public class Ad
    {
        /// <summary>
        /// Ad identifier
        /// </summary>
        [Key]
        [Comment("Ad identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Ad name
        /// </summary>
        [Required]
        [MaxLength(Constants.AdNameMaxLength)]
        [Comment("Ad name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Ad description
        /// </summary>
        [Required]
        [MaxLength(Constants.AdDescriptionMaxLength)]
        [Comment("Ad Description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Ad price
        /// </summary>
        [Required]
        [Comment("Ad price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Ad owiner identifier
        /// </summary>
        [Required]
        [Comment("Ad owiner identifier")]
        public string OwnerId { get; set; } = string.Empty;

        /// <summary>
        /// Ad owner
        /// </summary>
        [Required]
        [Comment("Ad owner")]
        public IdentityUser Owner { get; set; } = null!;

        /// <summary>
        /// Ad image url
        /// </summary>
        [Required]
        [Comment("Ad image url")]
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Date ad was created
        /// </summary>
        [Required]
        [Comment("Date ad was created")]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Category identifier
        /// </summary>
        [Required]
        [ForeignKey(nameof(Category))]
        [Comment("Category identifier")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Ad category
        /// </summary>
        [Required]
        [Comment("Ad category")]
        public Category Category { get; set; } = null!;

        /// <summary>
        /// Ad buyers
        /// </summary>
        public ICollection<AdBuyer> AdBuyers { get; set; } = new List<AdBuyer>();
    }
}
