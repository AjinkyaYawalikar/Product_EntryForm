using Product_Entry_Demo.Library.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Product_Entry_Demo.Library.BAL
{
    public class BalProduct_Entry
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsIGSTApplicable { get; set; }
        public string Purchase_Date { get; set; }
        public string Expiry_Date { get; set; }
        public string Color { get; set; }
        public List<BalProduct_Entry> baproduct_lst { get; set; }

        DalProduct_Entry dal_product = new DalProduct_Entry();
        public bool InsertProduct_Entry(BalProduct_Entry product)
        {
            return dal_product.InsertProduct_Entry(product);
        }
        public List<BalProduct_Entry> Select_ProductEntry_ById(int id)
        {
            baproduct_lst = dal_product.Select_ProductEntry_ById(id).ToList();
            return baproduct_lst;
        }
       
    }
}