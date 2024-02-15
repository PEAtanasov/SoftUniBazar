using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftUniBazar.Data;
using SoftUniBazar.Data.Models;
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

        [HttpGet]
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

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AdFormModel()
            {
                Categories = await GetCategoriesAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AdFormModel model)
        {
            if (User==null)
            {
                Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategoriesAsync();
                return View(model);
            }

            var entity = new Ad() 
            {
                Name = model.Name,
                ImageUrl = model.ImageUrl,
                Description = model.Description,
                CreatedOn = DateTime.Now,
                CategoryId = model.CategoryId,
                OwnerId = GetUserId(),
                Price = model.Price,  
            };

            await data.AddAsync(entity);
            await data.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var adToEdit = await data.Ads.FindAsync(id);

            if (adToEdit == null)
            {
                return BadRequest();
            }

            if (adToEdit.OwnerId != GetUserId())
            {
                return Unauthorized();
            }

            var model = new AdFormModel()
            {
                Id = id,
                Name = adToEdit.Name,
                Description = adToEdit.Description,
                ImageUrl = adToEdit.ImageUrl,
                Price= adToEdit.Price,
                CategoryId=adToEdit.CategoryId,
                Categories=await GetCategoriesAsync(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdFormModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var adToEdit = await data.Ads.FindAsync(model.Id);
            if (adToEdit == null) 
            {
                return BadRequest();
            }

            if (!(adToEdit.OwnerId == GetUserId())) 
            { 
                return BadRequest(); 
            }
            
            adToEdit.Name = model.Name;
            adToEdit.Description = model.Description;
            adToEdit.ImageUrl = model.ImageUrl;
            adToEdit.Price = model.Price;
            adToEdit.CategoryId=model.CategoryId;

            data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            var ad = await data.Ads
                .Where(a=>a.Id==id)
                .Include(a=>a.AdBuyers)
                .FirstOrDefaultAsync();

            if (ad==null)
            {
                return BadRequest();
            }

            if (ad.OwnerId == GetUserId())
            {
                return Unauthorized();
            }

            if (ad.AdBuyers.Any(a=>a.BuyerId==GetUserId()))
            {
                return RedirectToAction(nameof(All));
            }

            ad.AdBuyers.Add(new AdBuyer()
            {
                BuyerId = GetUserId(),
                AdId=id
            });

            await data.SaveChangesAsync();

            return RedirectToAction(nameof(Cart));
            
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var adToRemove = await data.Ads
                .Where(a=>a.Id==id)
                .Include(a=>a.AdBuyers)
                .FirstOrDefaultAsync();

            if (adToRemove ==null)
            {
                return BadRequest();
            }

            if (!adToRemove.AdBuyers.Any(a=>a.BuyerId == GetUserId()))
            {
                return Unauthorized();
            }

            var buyerToRemove = adToRemove.AdBuyers.FirstOrDefault(a=>a.BuyerId==GetUserId());

            if (buyerToRemove != null)
            {
                data.AdsBuyers.Remove(buyerToRemove);
                await data.SaveChangesAsync();
            }

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            var ads = await data.AdsBuyers
                .AsNoTracking()
                .Where(ab => ab.BuyerId == GetUserId())
                .Select(ab => new AdViewModel()
                {
                    Id=ab.AdId,
                    Name = ab.Ad.Name,
                    ImageUrl = ab.Ad.ImageUrl,
                    CreatedOn = ab.Ad.CreatedOn.ToString(Constants.DateFormat),
                    Description = ab.Ad.Description,
                    Price = ab.Ad.Price,
                    Owner = ab.Ad.Owner.UserName
                })
                .ToListAsync();
                

            return View(ads);
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

