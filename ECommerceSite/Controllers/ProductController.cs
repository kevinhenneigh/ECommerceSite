using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceSite.Data;
using ECommerceSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        /// Displays a view that lists a page of products
        /// </summary>
        public async Task<IActionResult> Index(int? id)
        {
            int pageNum = id ?? 1;
            const int PageSize = 3;


            // Get all products from database
            //  List<Product> products = await _context.Products.ToListAsync();
            List<Product> products = await (from product in _context.Products
                                            orderby product.Title ascending
                                            select product)
                                           .Skip(PageSize * (pageNum - 1)) //Skip must be first
                                           .Take(pageNum)
                                           .ToListAsync();

            // Send list of products to view to be displayed
            return View(products);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            if (ModelState.IsValid)
            {
                // Add to Db
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"{product.Title} was added sucessfully";

                // redirect back to catalouge page
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Get product with corresponding id
            Product product = await _context.Products
                                      .Where(prod => prod.ProductId == id)
                                      .SingleAsync();
                


            // Pass product to view
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                ViewData["Message"] = "Product updated succesfully!";
            }
            return View(product);
        }
    }
}
