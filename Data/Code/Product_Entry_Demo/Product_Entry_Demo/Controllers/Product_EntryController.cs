using Product_Entry_Demo.Library.BAL;
using Product_Entry_Demo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Product_Entry_Demo.Controllers
{
    public class Product_EntryController : Controller
    {
        // GET: Product_Entry
        public ActionResult Add_Product_Entry()
        {
            ViewBag.ColorList = ListData(typeof(Product_Entry_Demo.Library.Constants.ColorList));
            return View();
        }
       
        [HttpPost]
        public ActionResult Add_Product_Entry(Product_Entry product)
        {
            BalProduct_Entry balProduct_Entry = new BalProduct_Entry();
            Product_Entry productEntry = new Product_Entry();
                try
                {
                    bool IsSave = false;
                balProduct_Entry.ID = product.ID;
                balProduct_Entry.Name = product.Name;
                balProduct_Entry.Price = product.Price;
                balProduct_Entry.Quantity = product.Quantity;
                balProduct_Entry.IsIGSTApplicable = product.IsIGSTApplicable;
                balProduct_Entry.Purchase_Date = product.Purchase_Date;
                balProduct_Entry.Expiry_Date = product.Expiry_Date;
                balProduct_Entry.Color = product.Color;


                balProduct_Entry.baproduct_lst = balProduct_Entry.Select_ProductEntry_ById(0).Where(m=>m.Name==product.Name).ToList();
                    if (balProduct_Entry.baproduct_lst.Count() == 0)
                    {
                        
                            if (product.ID == 0)
                            {

                                IsSave = balProduct_Entry.InsertProduct_Entry(balProduct_Entry);
                                TempData["UsersaveUpdate"] = "Product Entry Saved Successfully.";

                            }
                        
                    }
                    else
                    {                        
                        TempData["UserSaveUpdate"] = "Name Allredy Exist.";
                    }
                    
                }
                catch (Exception ex)
                {
                }
                 return RedirectToAction("GetAllProduct_Entry");
            
        }

        public ActionResult GetAllProduct_Entry()
        {
                BalProduct_Entry balProduct = new BalProduct_Entry();               
                    ViewBag.ProductDetails= balProduct.Select_ProductEntry_ById(0).ToList();
                    ViewBag.UserTitle = "Other User";
                return View();
            
        }
        public ActionResult AllProduct_Entry()
        {
            BalProduct_Entry balProduct = new BalProduct_Entry();

            balProduct.baproduct_lst = balProduct.Select_ProductEntry_ById(0).ToList();
            var Selectdatalst = from s in balProduct.baproduct_lst select new { s.ID, s.Name, s.Price, s.Quantity, s.IsIGSTApplicable, s.Purchase_Date, s.Expiry_Date,s.Color };
            return Json(new { data = Selectdatalst }, JsonRequestBehavior.AllowGet);
        }

        public List<string> ListData(Type type)
        {
            Type InLinenumType = type;
            var _InlinenumValues = InLinenumType.GetEnumValues();
            List<string> _LstValue = new List<string>();
            foreach (var value in _InlinenumValues)
            {
                MemberInfo memberInfo = InLinenumType.GetMember(value.ToString()).First();
                var descriptionAttribute = memberInfo.GetCustomAttribute<DescriptionAttribute>();
                string val = descriptionAttribute.Description.ToString();
                _LstValue.Add(val);
            }
            return _LstValue;
        }
        
    }
}