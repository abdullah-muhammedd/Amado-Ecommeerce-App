using Amado.Data;
using Amado.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Amado.Controllers
{
    public static class SharedVariables
    {
        public static bool _LoginStatus { get; set; } = false;
        public static int _UserID { get; set; } = 0;
    }
    public class Home : Controller
    {
        private readonly AppDbContext _context;

        public Home(AppDbContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            ViewBag.LoginStatus = SharedVariables._LoginStatus;
            ViewBag.UserID = SharedVariables._UserID;
        }

        // Get Index page
        // @params: void
        public IActionResult Index()
        {
            ViewBag.LINK = "Index.css?v=@DateTime.Now.Ticks";
            ViewBag.TITLE = "Home";
            ViewBag.ITEMS = _context.Item.ToList();
            ViewBag.IMAGES = _context.Image.ToList();
            return View();
        }

        // Get Shop page
        // @params: void
        public IActionResult Shop()
        {
            ViewBag.LINK = "Shop.css?v=@DateTime.Now.Ticks";
            ViewBag.TITLE = "Shop";
            ViewBag.ITEMS = _context.Item.ToList();
            ViewBag.IMAGES = _context.Image.ToList();
            return View();
        }

        //CalcPrice
        // Helper Function to Calculate Total Price in the Cart
        // @params: cart , cartItems , itemsDetailed
        public void CalcPrice(Cart cart, List<Cart_Item> cartItems, List<Item> itemsDetailed)
        {
            foreach (var cart_item in cartItems)
            {
                Item item = _context.Item.Find(cart_item.ItemID);
                itemsDetailed.Add(item);
            }


            //Update the total price of the cart
            double tp = 0;
            foreach (var cart_item in cartItems)
            {
                foreach (var item in itemsDetailed)
                {
                    if (cart_item.ItemID == item.ItemID)
                    {
                        tp += cart_item.Quantity * item.Price;
                    }
                }
            }
            if (cart.TotalPrice != tp)
            {
                cart.TotalPrice = tp;
                _context.SaveChanges();
            }
        }

        // Get Cart page
        // @params: void
        public IActionResult Cart()
        {
            if (!ViewBag.LoginStatus)
                return RedirectToAction("Login", "Home");

            Cart cart = _context.Cart.Find(_context.User.Find(ViewBag.UserID).CartID);
            List<Cart_Item> cartItems = _context.Cart_Item.Where(cid => cid.CartID == cart.CartID).ToList();
            List<Item> itemsDetailed = new List<Item>();
            CalcPrice(cart, cartItems, itemsDetailed);

            ViewBag.IMAGES = _context.Image.ToList();
            ViewBag.ITEMSDETAILED = itemsDetailed;
            ViewBag.CART = cart;
            ViewBag.LINK = "Cart.css";
            ViewBag.TITLE = "Cart";
            return View(cartItems);
        }

        // Get Checkout page
        // @params: void
        [HttpGet]
        public IActionResult Checkout()
        {
            if (!ViewBag.LoginStatus)
                return RedirectToAction("Login", "Home");

            ViewBag.LINK = "Checkout.css";
            ViewBag.TITLE = "Checkout";

            Cart cart = _context.Cart.Find(_context.User.Find(ViewBag.UserID).CartID);
            List<Cart_Item> cartItems = _context.Cart_Item.Where(cid => cid.CartID == cart.CartID).ToList();
            List<Item> itemsDetailed = new List<Item>();
            CalcPrice(cart, cartItems, itemsDetailed);

            var user = _context.User.Find(ViewBag.UserID);
            ViewBag.TotalPrice = cart.TotalPrice;
            return View(user);
        }

        // Checkout
        // HttpPost To Checkout Page
        // @params: user => User , comment => string
        [HttpPost]
        public IActionResult Checkout(User user, string comment)
        {
            Cart cart = _context.Cart.Find(_context.User.Find(ViewBag.UserID).CartID);
            List<Cart_Item> cartItems = _context.Cart_Item.Where(cid => cid.CartID == cart.CartID).ToList();
            // Create a new  order 
            var NewOrder = new Order()
            {
                UserID = ViewBag.UserID,
                Comment = comment,
                TotalPrice = cart.TotalPrice
            };
            _context.Order.Add(NewOrder);
            _context.SaveChanges();

            // Add Cart_Items => Order_Items
            int orderId = NewOrder.OrderID;
            foreach (var cart_item in cartItems)
            {
                Item item = _context.Item.Find(cart_item.ItemID);
                Order_Item newOrderItem = new Order_Item() {
                    OrderID = orderId,
                    ItemID = cart_item.ItemID,
                    Quantity = cart_item.Quantity,
                    Name = item.Name,
                    Price = item.Price * cart_item.Quantity,
                };
                _context.Order_Item.Add(newOrderItem);
                _context.SaveChanges();
            }  
            foreach (var cart_item in cartItems)
            {
                _context.Cart_Item.Remove(cart_item);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        // Get Product page
        // @params: id => ItemID => int
        public IActionResult Product(int id)
        {
            ViewBag.ITEM = _context.Item.Find(id);
            ViewBag.IMAGES = _context.Image.ToList();
            ViewBag.OtherItems = _context.Item.ToList();
            ViewBag.LINK = "Product.css?v=@DateTime.Now.Ticks";
            ViewBag.TITLE = ViewBag.ITEM.Name;
            return View();
        }

        // AddToCart
        // HttpPost To AddToCart 
        // @params: QueryItemID => ItemID =>int , QueryQuantity => cart_Item.Quantity =>int
        [HttpPost]
        public IActionResult AddToCart(int QueryItemID, int QueryQuantity = 1)
        {
            if (!ViewBag.LoginStatus)
                return RedirectToAction("Login", "Home");
            
            Item item = _context.Item.Find(QueryItemID);
            if (item.Quantity < QueryQuantity)
                return RedirectToAction("Cart", "Home");

            int QueryCartID = _context.User.Find(ViewBag.UserID).CartID;

            var QueryCartItem = _context.Cart_Item.FirstOrDefault(ci => ci.CartID == QueryCartID && ci.ItemID == QueryItemID);

            if (QueryCartItem == null)
            {
                QueryCartItem = new Cart_Item()
                {
                    CartID = QueryCartID,
                    ItemID = QueryItemID,
                    Quantity = QueryQuantity,
                };
                _context.Cart_Item.Add(QueryCartItem);

            }
            else
            {
                QueryCartItem.Quantity += QueryQuantity;

            }
            item.Quantity = item.Quantity - QueryQuantity;
            _context.SaveChanges();
            return RedirectToAction("Cart", "Home");
        }

        // RemoveFromCart
        // HttpPost To RemoveFromCart 
        // @params: QueryItemID => ItemID =>int , QueryQuantity => cart_Item.Quantity =>int
        [HttpPost]
        public IActionResult RemoveFromCart(int QueryItemID, int QueryQuantity = 1)
        {
            if (!ViewBag.LoginStatus)
                return RedirectToAction("Login", "Home");

            int QueryCartID = _context.User.Find(ViewBag.UserID).CartID;

            var QueryCartItem = _context.Cart_Item.FirstOrDefault(ci => ci.CartID == QueryCartID && ci.ItemID == QueryItemID);
            if (QueryCartItem.Quantity <= 1)
            {
                _context.Cart_Item.Remove(QueryCartItem);
            }
            else
            {
                QueryCartItem.Quantity -= QueryQuantity;
            }
            Item item = _context.Item.Find(QueryItemID);
            item.Quantity = item.Quantity + QueryQuantity;
            _context.SaveChanges();
            return RedirectToAction("Cart", "Home");
        }

        // Get SignUp page
        // @params: void
        [HttpGet]
        public IActionResult SignUp()
        {
            if (ViewBag.LoginStatus)
                return RedirectToAction("Index", "Home");
            ViewBag.LINK = "signUp.css?v=@DateTime.Now.Ticks";
            ViewBag.TITLE = "Sign Up";
            return View();
        }

        // SignUp
        // HttpPost To SignUp Page
        // @params: user => User
        [HttpPost]
        public IActionResult SignUp(User user)
        {
            if (ModelState.IsValid)
            {
                user.IsAdmin = false;

                Cart cart = new Cart();
                _context.Cart.Add(cart);
                _context.SaveChanges();

                user.CartID = cart.CartID;
                _context.User.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Login", "Home");
            }

            return View(user);
        }

        // Get Login page
        // @params: void
        [HttpGet]
        public IActionResult Login()
        {
            if (ViewBag.LoginStatus)
                return RedirectToAction("Index", "Home");

            ViewBag.LINK = "Login.css?v=@DateTime.Now.Ticks";
            ViewBag.TITLE = "Login";
            return View();

        }

        // Login
        // HttpPost To Login Page
        // @params: user => User 
        [HttpPost]
        public IActionResult Login(User user)
        {

            var Admin = _context.User.FirstOrDefault(a => a.IsAdmin == true && a.Email == user.Email && a.Password == user.Password);
            if (Admin != null)
            {
                ViewBag.LoginStatus = SharedVariables._LoginStatus = true;
                ViewBag.UserID = SharedVariables._UserID = Admin.UserID;
                return RedirectToAction("Index", "Admin");
            }

            var QueryUser = _context.User.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
            if (QueryUser != null)
            {
                ViewBag.LoginStatus = SharedVariables._LoginStatus = true;
                ViewBag.UserID = SharedVariables._UserID = QueryUser.UserID;
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username or password");            
            return View(user);
        }

        // Get Login page
        // @params: void
        public IActionResult Logout()
        {
            ViewBag.LoginStatus = SharedVariables._LoginStatus = false;
            ViewBag.UserID = SharedVariables._UserID = 0;
            return RedirectToAction("Login", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}