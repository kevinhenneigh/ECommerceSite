using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceSite.Data;
using ECommerceSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSite.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductContext _context;

        public ProductController(ProductContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a view that lists all products
        /// </summary>
        public IActionResult Index()
        {
            // Get all products from database
            List<Product> products = _context.Products.ToList();

            // Send list of products to view to be displayed
            return View(products);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Product p)
        {
            if (ModelState.IsValid)
            {
                // Add to Db
                _context.Products.Add(p);
                _context.SaveChanges();

                TempData["Message"] = $"{p.Title} was added sucessfully";

                // redirect back to catalouge page
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
