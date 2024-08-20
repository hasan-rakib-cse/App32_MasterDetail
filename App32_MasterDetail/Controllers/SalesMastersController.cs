using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App32_MasterDetail.Data;
using App32_MasterDetail.Models;
using System.Security.Cryptography;

namespace App32_MasterDetail.Controllers
{
    public class SalesMastersController : Controller
    {
        private readonly AppDBContext _context;

        public SalesMastersController(AppDBContext context)
        {
            _context = context;
        }


        #region ......:::::: CUSTOM CODE BLOCK ::::::...................

        public IActionResult SalesInvoice()
        {
            var prod = _context.Product.ToList();
            return View(prod);
        }

        [HttpPost]
        public JsonResult SaveInvoice(string Customer)
        {
            if(string.IsNullOrEmpty(Customer))
            {
                return Json(new { flag = "0", msg = "Customer info is empty" });
            }

            var extcart = HttpContext.Session.GetObject<List<Product>>("myinvoice");
            if (extcart == null)
            {
                return Json(new { flag = "0", msg = "Atleast one Line item must be there."});
            }

            SalesMaster m = new SalesMaster();
            m.Customer = Customer;
            m.InvoiceNo = getNo();
            m.InvoiceDate = DateTime.Now;
            m.InvoiceStatus = 1;

            _context.SalesMaster.Add(m);
            _context.SaveChanges();

            foreach (var item in extcart)
            {
                SalesDetail sd = new SalesDetail();
                sd.SalesMasterId = m.Id;
                sd.ProductId = item.Id;
                sd.Price = item.Price;
                sd.Quantity = item.Quantity;

                _context.SalesDetail.Add(sd); 
            }
            _context.SaveChanges();

            HttpContext.Session.SetObject<List<Product>>("myinvoice", null);

            return Json(new { flag = "1", msg = "Invoice is Saved Successfully." });
        }

        [HttpPost]
        public JsonResult getPrice(int id)
        {
            var prod = _context.Product.FirstOrDefault(x=> x.Id == id);
            if(prod == null){
                return Json(new { flag = "0", msg = "Sorry ! Product does not exist." });
            }

            return Json(new { flag = "1", msg = "Product exist.", price=prod.Price });
        }

        [HttpPost]
        public JsonResult AddToCart(int pid, double qty)
        {
            var extcart = HttpContext.Session.GetObject<List<Product>>("myinvoice");
            if (extcart == null)
            {
                var prod = _context.Product.FirstOrDefault(x => x.Id == pid);
                if (prod != null)
                {
                    prod.Quantity = qty;
                    prod.SubTotal = prod.Price * prod.Quantity;

                    List<Product> plist = new List<Product>();
                    plist.Add(prod);
                    HttpContext.Session.SetObject<List<Product>>("myinvoice", plist);
                    return Json(new { flag = "1", msg = "Product is added in empty cart" });
                }
                else
                {
                    return Json(new { flag = "0", msg = "Product is invalid" });
                }
            }
            else
            {
                var prod1 = _context.Product.FirstOrDefault(x => x.Id == pid);
                if (prod1 != null)
                {
                    var prod2 = extcart.FirstOrDefault(x => x.Id == pid);
                    if (prod2 == null)
                    {
                        prod1.Quantity = qty;
                        prod1.SubTotal = prod1.Price * prod1.Quantity;

                        extcart.Add(prod1);
                        HttpContext.Session.SetObject<List<Product>>("myinvoice", extcart);
                        return Json(new { flag = "1", msg = "Product is added in existing cart" });
                    }
                    else
                    {
                        //prod2.Quantity += 1;
                        prod2.Quantity = qty;
                        HttpContext.Session.SetObject<List<Product>>("myinvoice", extcart);
                        return Json(new { flag = "1", msg = "Product is updated in existing cart" });
                    }
                }
                else
                {
                    return Json(new { flag = "0", msg = "Product is invalid" });
                }
            }
        }

        [HttpPost]
        public JsonResult RemoveItem(int pid)
        {
            var extcart = HttpContext.Session.GetObject<List<Product>>("myinvoice");
            if (extcart != null)
            {
                var prod = extcart.FirstOrDefault(x => x.Id == pid);
                if (prod != null)
                {
                    extcart.Remove(prod);
                    HttpContext.Session.SetObject<List<Product>>("myinvoice", extcart);
                    return Json(new { flag = "1", msg = "Product is removed successfully." });
                }
                else
                {
                    return Json(new { flag = "0", msg = "Product is invalid" });
                }
            }
            else
            {
                return Json(new { flag = "0", msg = "Product is invalid" });
            }
        }

        [HttpPost]
        public JsonResult ShowLineItem()
        {
            var extcart = HttpContext.Session.GetObject<List<Product>>("myinvoice");
            if (extcart != null)
            {
                return Json(new { flag = "1", msg = "Line item exists.", data = extcart });
            }
            else
            {
                return Json(new { flag = "0", msg = "No Line Item exist" });
            }
        }

        private string getNo()
        {
            int count_row = _context.SalesMaster.Count(); 
            if(count_row > 0)
            {
                count_row++; 
            }
            else
            {
                count_row = 1;
            }
            return "TRX" + count_row.ToString().PadLeft(7, 'X');
        }

        #endregion
    }
}
