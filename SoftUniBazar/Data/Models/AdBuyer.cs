using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftUniBazar.Data.Models
{
    public class AdBuyer
    {
        /// <summary>
        /// Buyer identifier
        /// </summary>
        [Required]
        [Comment("Buyer identifier")]
        [ForeignKey(nameof(Buyer))]
        public string BuyerId { get; set; } = string.Empty;

        [Required]
        public IdentityUser Buyer { get; set; } = null!;

        /// <summary>
        /// Ad identifier
        /// </summary>
        [Required]
        [Comment("Ad identifier")]
        [ForeignKey(nameof(Ad))]
        public int AdId { get; set; }

        [Required]
        public Ad Ad { get; set; } = null!;
    }
}
