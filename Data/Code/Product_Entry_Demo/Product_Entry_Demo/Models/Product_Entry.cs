using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Product_Entry_Demo.Models
{
    public class Product_Entry
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsIGSTApplicable { get; set; }
        public string Purchase_Date { get; set; }
        public string Expiry_Date { get; set; }
        public string Color { get; set; }
    }
}