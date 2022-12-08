using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshop.Data;
using Eshop.Models;
using Microsoft.AspNetCore.Http;

namespace Eshop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly EshopContext _context;

        public ProductsController(EshopContext context)
        {
            _context = context;
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.username = HttpContext.Session.GetString("username");
            }
           
            ViewBag.idProduct = id;
            var account = _context.Accounts.FirstOrDefault(a => a.Username == HttpContext.Session.GetString("username"));
            if (account == null)
            {
                ViewBag.idAccount = null;
            }
            else { 
                ViewBag.idAccount = account.Id;
            }
            ViewBag.AccountComment = _context.Comment.Include(c=>c.Account).Where(a=>a.Account.Id  == a.AccountId).Where(a=> a.ProductId == id);
            return View(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
