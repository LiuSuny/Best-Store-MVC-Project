using BestStoreMVC.Models;
using BestStoreMVC.Services;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BestStoreMVC.Controllers
{
    /// <summary>
    /// Store controller for our client side to order products 
    /// </summary>
    public class StoreController : Controller
    {
        //TODO: need to use repository and Dto and Automappers later on 
        private readonly ApplicationDbContext context; //inject our db 

        private readonly int pageSize = 8; //how many pages to be display

        public StoreController(ApplicationDbContext context)
        {
            this.context = context; //injected
        }

        public IActionResult Index(int pageIndex, string? search, string? brand, string? category, string? sort)
        {
            IQueryable<Product> query = context.Products;

            // search functionality
            if (search != null && search.Length > 0)
            {
                query = query.Where(p => p.Name.Contains(search));
            }


            // filter functionality
            if (brand != null && brand.Length > 0)
            {
                query = query.Where(p => p.Brand.Contains(brand));
            }

            if (category != null && category.Length > 0)
            {
                query = query.Where(p => p.Category.Contains(category));
            }

            // sort functionality
            if (sort == "price_asc")
            {
                query = query.OrderBy(p => p.Price);
            }
            else if (sort == "price_desc")
            {
                query = query.OrderByDescending(p => p.Price);
            }
            else
            {
                // newest products first
                query = query.OrderByDescending(p => p.Id);
            }

            // pagination functionality
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            decimal count = query.Count();
            int totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
          

            var products = query.ToList();

            //var products =context.products.OrderByDescending(p=>p.Id).ToList();  
            ViewBag.Products = products;
            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalPages = totalPages;

            var storeSearchModel = new StoreSearchModel()
            {
                Search = search,
                Brand = brand,
                Category = category,
                Sort = sort
            };


            return View(storeSearchModel);
        }

        //Store products detail
        public IActionResult Details(int id)
        {
            var product = context.Products.Find(id); //finding products detail using id

            if (product == null)
            {
                return RedirectToAction("Index", "Store"); //redirect if the product is null
            }

            return View(product); //show products detail
        }
    }
}
