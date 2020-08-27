using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceSite.Data;
using ECommerceSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ECommerceSite.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductContext _context;
        private readonly IHttpContextAccessor _httpcontext;
        public CartController(ProductContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpcontext = httpContext;
        }

        /// <summary>
        /// Adds a product to the shopping cart
        /// </summary>
        /// <param name="id">The id of the product to add</param>
        /// <returns></returns>
        public async Task<IActionResult> Add(int id) // Id of the product to add
        {
            
            Product product = await ProductDb.GetProductAsync(_context, id);

            const string CartCookie = "CartCookie";

            // Get existing cart items
             string existingItems = _httpcontext.HttpContext.Request.Cookies[CartCookie];
            List<Product> cartProducts = new List<Product>();
            if (existingItems != null)
            {
                cartProducts = JsonConvert.DeserializeObject<List<Product>>(existingItems); 
            }

            // Add current product to existing cart
            cartProducts.Add(product);

            // Add product to the cart cookie
            string data = JsonConvert.SerializeObject(cartProducts);

            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.Now.AddYears(1),
                Secure = true,
                IsEssential = true
            };

            _httpcontext.HttpContext.Response.Cookies.Append(CartCookie, data, options);

            // Redirect to previous page
            return RedirectToAction("Index", "Product");
        }

        public IActionResult Summary()
        {
            //Display all products in the  shopping cart cookie
            string cookieData = _httpcontext.HttpContext.Request.Cookies["CartCookies"];

            List<Product> cartProducts = JsonConvert.DeserializeObject<List<Product>>(cookieData);
            return View();       
        }
    }
}
