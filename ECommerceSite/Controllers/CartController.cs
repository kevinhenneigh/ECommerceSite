using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSite.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Add(int id) // Id of the product to add
        {
            // Get product from the database

            // Add product to the cart

            // Redirect to previous page
            return View();
        }

        public IActionResult Summary()
        {
            //Display all products in the  shopping cart cookie
            return View();       
        }
    }
}
