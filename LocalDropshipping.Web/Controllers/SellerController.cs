using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Extensions;
using LocalDropshipping.Web.Models;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace LocalDropshipping.Web.Controllers
{
    public class SellerController : Controller
    {
        private readonly IAccountService service;
        private readonly IProductsService productsService;

        public SellerController(IAccountService service, IProductsService productsService)
        {
            this.service = service;
            this.productsService = productsService;
        }
        #region Seller Register and Login
        public IActionResult Register()
        {
            return View();
        }
        //Seller Account login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isLoggedin = await service.LoginAsync(model.Email, model.Password);

                if (isLoggedin)
                {
                    // Redirect to the desired page after successful login
                    return RedirectToAction("ShopPage", "ShopPage");
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(SignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isSucceeded = await service.RegisterAsync(model.Email, model.Password, string.Join(" ", model.FirstName, model.LastName));
                if (!isSucceeded)
                {
                    ModelState.AddModelError("", "Unknow error occured");
                    return View(model);
                }

                ModelState.Clear();
                return RedirectToAction("Account", "Login");
            }

            return View(model);
        }
        #endregion

        #region Shop
        public IActionResult Shop()
        {
            ViewBag.products = productsService.GetAll();
			var cart = HttpContext.Session.Get<List<OrderItem>>("cart");
			ViewBag.total = TotalCost();
			ViewBag.totalCost = ViewBag.total + ViewBag.shipping;
			return View(cart);
        }
        [HttpPost]
        public JsonResult AddToCart(string id)
        {
            try
            {
                Product productItem = productsService.GetById(Convert.ToInt32(id == string.Empty ? 0 : id));
                var cart = HttpContext.Session.Get<List<OrderItem>>("cart");
                int quantity = 1;
                if (cart == null) //no item in the cart
                {
                    cart = new List<OrderItem>();
                    cart.Add(new OrderItem
                    {
                        ProductId = productItem.ProductId,
                        Name = productItem.Name,
						Image = productItem.ImageLink,
						Quantity = quantity,
                        Price = productItem.Price,
                        SubTotal = quantity * productItem.Price
                    });
                }
                else
                {
                    int index = cart.FindIndex(w => w.ProductId == (Convert.ToInt32(id)));
                    if (index != -1) //if item already in the 
                    {
                        cart[index].Quantity++; //increment by 1
                        cart[index].SubTotal = cart[index].Quantity * cart[index].Price;
                    }
                    else
                    {
                        cart.Add(new OrderItem
                        {
                            ProductId = productItem.ProductId,
                            Name = productItem.Name,
							Image = productItem.ImageLink,
							Quantity = quantity,
                            Price = productItem.Price,
                            SubTotal = quantity * productItem.Price
                        });
                    }
                }
                HttpContext.Session.Set<List<OrderItem>>("cart", cart);
                return Json(data: new { Success = true, Counter = cart.Count, Cart = cart });
            }
            catch (Exception ex)
            {
                return Json(data: new { Error = ex.ToString() });
            }
        }

        public JsonResult Minus(string id)
        {
            
            try
            {
                Product productItem = productsService.GetById(Convert.ToInt32(id == string.Empty ? 0 : id));
                var cart = HttpContext.Session.Get<List<OrderItem>>("cart");

				if (cart!= null && cart.Any())
				{
					int index = cart.FindIndex(w => w.ProductId == (Convert.ToInt32(id == string.Empty ? 0 : id)));
					if (index == -1)
					{
						return Json(data: new { error = false, Counter = cart.Count, Cart = cart });
					}
					else
					{
						if (cart[index].Quantity == 1) //last item of a product
						{
							cart.RemoveAt(index); //remove it
						}
						else
						{
							cart[index].Quantity--; //reduce by 1
						}
						HttpContext.Session.Set<List<OrderItem>>("cart", cart);
						return Json(data: new { Success = true, Counter = cart.Count, Cart = cart });
					}
					
				}
                else
                {
					return Json(data: new { error = false});
				}
				
			}
			catch (Exception ex)
			{
				return Json(data: new { Error = ex.ToString() });
			}
		}
        public PartialViewResult GetCartItems()
        {
            var cart = HttpContext.Session.Get<List<OrderItem>>("cart");
            ViewBag.total = TotalCost();
            return PartialView("_cartItem", cart);
        }

        #endregion

        #region SellerDashboard
        public IActionResult SellerDashboard()
        {
            return View();
        }
        #endregion

        #region Productleftthumbnail
        public IActionResult Productleftthumbnail()
        {

            return View();

        }
        #endregion

        #region Cart
        public IActionResult Cart()
        {
			var cart = HttpContext.Session.Get<List<OrderItem>>("cart");
			ViewBag.total = TotalCost();
			ViewBag.shipping = 250;
			ViewBag.totalCost = ViewBag.total + ViewBag.shipping;
			return View(cart);
		}

        public IActionResult Remove(int id)
        {
            var cart = HttpContext.Session.Get<List<OrderItem>>("cart");
            int index = cart.FindIndex(w => w.ProductId == id);
            cart.RemoveAt(index);
            HttpContext.Session.Set<List<OrderItem>>("cart", cart);
            return RedirectToAction("Cart");
        }

        #endregion

        #region Checkout
        public IActionResult Checkout()
        {
			var cart = HttpContext.Session.Get<List<OrderItem>>("cart");
			ViewBag.total =TotalCost();
            ViewBag.shipping = 250;
            ViewBag.totalCost = ViewBag.total + ViewBag.shipping;
            return View(cart);
        }
        #endregion

        private decimal TotalCost()
        {
            try
            {
				decimal totalRecord;
				var cart = HttpContext.Session.Get<List<OrderItem>>("cart");
				if (cart != null)
				{
					totalRecord = (decimal)cart.Sum(s => s.Quantity * s.Price);
				}
				else
				{
					cart = new List<OrderItem>();
					totalRecord = 0;
				}

				return totalRecord;
			}
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
		}

    }
}
