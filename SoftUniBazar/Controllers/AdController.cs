using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftUniBazar.Data;
using SoftUniBazar.Models;
using System.Security.Claims;

namespace SoftUniBazar.Controllers
{
    [Authorize]
    public class AdController : Controller
    {
        private readonly BazarDbContext data;

        public AdController(BazarDbContext context)
        {
            this.data=context;
        }
        public async Task<IActionResult> All()
        {
            var ads = await data.Ads
                .AsNoTracking()
                .Select(a=> new AdViewModel()
                {
                    Id = a.Id,
                    Name=a.Name,
                    ImageUrl=a.ImageUrl,
                    CreatedOn = a.CreatedOn.ToString(Constants.DateFormat),
                    Description=a.Description,
                    Category=a.Category.Name,
                    Price=a.Price,
                    Owner=a.Owner.UserName
                })
                .ToListAsync();

            return View(ads);
        }

        public async Task<IActionResult> Add()
        {
            var model = new AdFormModel()
            {
                Categories = await GetCategoriesAsync()
            };

            return View(model);
        }

        private async Task<ICollection<CategoryViewModel>> GetCategoriesAsync()
        {
            var categories = await data.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return categories;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
