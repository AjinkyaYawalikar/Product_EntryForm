using MySql.Data.MySqlClient;
using Product_Entry_Demo.Library.BAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Product_Entry_Demo.Library.DAL
{
    public class DalProduct_Entry:DalBase
    {
        public bool InsertProduct_Entry(BalProduct_Entry product)
        {
            try
            {
                AddParameter("@Name", MySqlDbType.VarChar, product.Name);
                AddParameter("@Price", MySqlDbType.Decimal, product.Price);
                AddParameter("@Quantity", MySqlDbType.Int32, product.Quantity);
                AddParameter("@IsIGSTApplicable", MySqlDbType.Bit, product.IsIGSTApplicable);
                AddParameter("@Purchase_Date", MySqlDbType.VarChar, product.Purchase_Date);
                AddParameter("@Expiry_Date", MySqlDbType.VarChar, product.Expiry_Date);
                AddParameter("@Color", MySqlDbType.VarChar, product.Color);

                ExecuteScalar("Sp_Insert_Product_Entry");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                _dataConnection.Close();
            }
        }
        public List<BalProduct_Entry> Select_ProductEntry_ById(int id)
        {
            //if 0 then select all else by id
            try
            {
                List<BalProduct_Entry> ProductEntryList = new List<BalProduct_Entry>();
                AddParameter("@Id", MySqlDbType.Int32, id);
                DataSet ds = ExecuteForData("Sp_Select_Product_Entry");
                foreach (DataRow users in ds.Tables[0].Rows)
                {
                    BalProduct_Entry productDetail = new BalProduct_Entry();
                    productDetail.ID = Convert.ToInt32(users["ID"]);
                    productDetail.Name = Convert.ToString(users["Name"]);
                    productDetail.Price = Convert.ToDecimal(users["Price"]);
                    productDetail.Quantity = Convert.ToInt32(users["Quantity"]);
                    productDetail.IsIGSTApplicable = Convert.ToBoolean(users["IsIGSTApplicable"]);
                    productDetail.Purchase_Date = Convert.ToString(users["Purchase_Date"]);
                    productDetail.Expiry_Date = Convert.ToString(users["Expiry_Date"]);
                    productDetail.Color = Convert.ToString(users["Color"]);
                   
                    ProductEntryList.Add(productDetail);
                }
                return ProductEntryList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally { _dataConnection.Close(); }
        }

    }
}