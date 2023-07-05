using Amado.Data;
using Amado.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Amado.Controllers
{
    public class Admin : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public Admin(AppDbContext context , IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        // Get Index page
        // @params: void
        public IActionResult Index()
        {
            var user = _context.User.Find(SharedVariables._UserID);
            if (user == null || !user.IsAdmin)
                return RedirectToAction("Login", "Home");

            List<Amado.Models.Item> items = _context.Item.ToList();
            List<Amado.Models.Image> images = _context.Image.ToList();
            ViewBag.ITEMS = items;
            ViewBag.IMAGES= images;
            return View();
        }

        // Delete Product(Item) 
        // HttpDelete To Index Page
        // @params: id => ItemID =>int
        [HttpDelete]
        public IActionResult Index(int id)
        {
             Amado.Models.Item item = _context.Item.Find(id);
            _context.Item.Remove(item);
            _context.SaveChanges();
            List< Amado.Models.Image >images = _context.Image.Where(im => im.ItemID == id).ToList();
            foreach(var im in images)
            {
                _context.Image.Remove(im);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Admin");
        }

        // Get AddProduct page
        // @params: void
        public IActionResult AddProduct()
        {
                return View();
        }

        // Add Product(Item) 
        // HttpPost To AddProduct Page
        // @params: item => Item , imageFile => IFormFile
        [HttpPost]
        public async Task<IActionResult> AddProduct(Item item,IFormFile imageFile)
        {
            // Save image to the server 
            string directoryToSave = Path.Combine(_environment.WebRootPath, "assets");
            if (imageFile != null && imageFile.Length > 0)
            {
                string filePath = Path.Combine(directoryToSave, imageFile.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
            }
            else
            {
                return View();
            }
            // Add product to database to allow retreiving its auto generated ID
            _context.Item.Add(item);
            _context.SaveChanges();

            // Retreive the product match with the item sent from the form 
            List<Models.Item> Newitem = _context.Item.Where(it => it.Name == item.Name && it.Price == item.Price).ToList();

            // Create new ImageModel and connect it to the item 
            Models.Image newImage = new Models.Image();
            newImage.ItemID = Newitem[0].ItemID;
            newImage.ImageURL = Path.Combine("/assets", imageFile.FileName).ToString();

            //Add new Image to the database
            _context.Image.Add(newImage);
            _context.SaveChanges();

            // return to the main page 
            return RedirectToAction("Index");
        }


        // Get UpdateProduct page
        // @params: id => ItemID => int
        public IActionResult UpdateProduct(int id)
        {
            Amado.Models.Item? item = _context.Item.Find(id);
            List<Amado.Models.Image> images = _context.Image.ToList();
            foreach (var im in images)
            {
                if(im.ItemID == item.ItemID)
                {
                    ViewBag.IMAGE = im;
                    break;
                }
            }  
            ViewBag.ITEM = item;
            
            return View();
        }


        // Update Product(Item) 
        // HttpPost To UpdateProduct Page
        // @params: item => Item
        [HttpPost]
        public IActionResult UpdateProduct(Item item)
        {
            _context.Item.Update(item);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // Get Orders page
        // @params: void
        public IActionResult Orders()
        {
            List<Amado.Models.Order> orders = _context.Order.ToList();
            ViewBag.ORDERS = orders;
            return View();
        }

        // Get OrderDetails page
        // @params: id => OrderID => int
        public IActionResult OrderDetails(int id)
        {
            Amado.Models.Order? order = _context.Order.Find(id);
            ViewBag.ORDER = order;
            Amado.Models.User? user = _context.User.Find(order.UserID);
            ViewBag.USER = user;
            Amado.Models.Cart? cart = _context.Cart.Find(user.CartID);
            ViewBag.CART = cart;
            List< Amado.Models.Order_Item> cartItems = _context.Order_Item.Where(cid => cid.OrderID == id).ToList();
            ViewBag.CARTITEMS = cartItems;
            return View();
        }

        // Delete Product(Item) 
        // HttpDelete To OrderDelete 
        // @params: id => OrderID => int
        [HttpDelete]
        public IActionResult OrderDelete(int id)
        {
            List<Amado.Models.Order_Item> order_Items = _context.Order_Item.Where(ordIt => ordIt.OrderID == id).ToList();
            foreach (var ordIt in order_Items)
            {
                _context.Order_Item.Remove(ordIt);
            }
            Amado.Models.Order? order = _context.Order.Find(id);
            _context.Order.Remove(order);
            _context.SaveChanges();
            return RedirectToAction("Orders", "Admin");
        }

    }
}
