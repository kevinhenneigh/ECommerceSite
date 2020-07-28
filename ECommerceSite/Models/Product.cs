using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceSite.Models
{
    /// <summary>
    /// A salable product
    /// </summary>
    public class Product
    {
        [Key] // make Primary Key in database
        public int ProductId { get; set; } 

        /// <summary>
        /// The consumer facing name of the product
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The retsil price in US currency
        /// </summary>
        public double Price { get; set; }
        
        /// <summary>
        /// Category product falls under
        /// Ex. Electronics
        /// </summary>
        public string Category { get; set; }
    }
}
