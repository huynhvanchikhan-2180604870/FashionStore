using FashionStore.Data;
using FashionStore.HelperClass;
using FashionStore.Models;
using FashionStore.ModelsViews;
using FashionStore.Services;
using FashionStore.ShoppingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Controllers
{
    [Authorize]
    public class ShoppingController : Controller
    {
        private readonly FashionStoreDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVnPayService _vnPayService;
        private readonly PaypalClient _paypalClient;
        public ShoppingController(FashionStoreDbContext context, UserManager<ApplicationUser> userManager, IVnPayService vnPayService, PaypalClient paypalClient)
        {
            _context = context;
            _userManager = userManager;
            _vnPayService = vnPayService;
            _paypalClient = paypalClient;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            if (cart.Items.Count == 0)
            {
                return View("EmptyCart");
            }
            
            return View(cart);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(string id, int quantity, int size)
        {
            var product = await _context.Products
                .Include(p => p.ProductDetails)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(x => x.ProductID == id);
            var images = await _context.ProductImages
                .Include(p => p.Product)
                .Where(x => x.ProductId == id).ToListAsync();
            var productdetails = await _context.ProductDetails
                .Include(x => x.Size)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.ProductID == id && x.SizeID == size);
            if (productdetails == null)
            {
                return NotFound();
            }
            var cart = new CartItem()
            {
                ProductId = id,
                Product = product,
                Quantity = quantity,
                Price = product.Price * quantity,
                SizeID = size,
                Size = productdetails.Size,
                Images = images
            };
            var shoppingcart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            shoppingcart.AddItem(cart);
            HttpContext.Session.SetObjectAsJson("Cart", shoppingcart);
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> RemoveItem(string id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductID == id);


            if (product == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            cart.RemoveItem(product.ProductID);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            ViewBag.PaypalClientId = _paypalClient.ClientId;
            return View(new MVOrder() {ShoppingCart = cart, Order = new Order()});
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Checkout(Order order, string payment = "COD")
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null)
            {
                return RedirectToAction("Index");
            }
            if (payment == "VNPAY")
            {
                var vnPayModel = new VnPaymentRequest()
                {
                    Amount = (double)cart.Items.Sum(p => p.Price),
                    CreatedDate = DateTime.Now,
                    Description = $"{user.FullName}{user.PhoneNumber}",
                    FullName = user.FullName,
                    OrderId = new Random().Next(1000, 10000)
                };
                return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
            }

            order.UserID = user.Id;
            order.OrderDate = DateTime.UtcNow;
            order.Status = false;
            order.PaymentType = PaymentType.COD;
            order.Status = false;
            order.Details = cart.Items.Select(i => new OrderDetail
            {
                ProductID = i.ProductId,
                Quantity = (int)i.Quantity,
                Price = (decimal)i.Price,
            }).ToList();

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("Cart");
            return View("OrderCompleted");
        }
        #region paypal
        [HttpPost("/Shopping/create-paypal-order")]
        public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            var Total = cart.Items.Sum(o => o.Price).ToString();
            var currency = "USD";
            var reference = "DH" + DateTime.Now.Ticks.ToString();
            try
            {
                var response = await _paypalClient.CreateOrder(Total, currency, reference);

                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }
        [HttpPost("/Shopping/capture-paypal-order")]
        public async Task<IActionResult> CapturePaypalOrder(string orderId, CancellationToken cancellationToken)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            try
            {
                var response = await _paypalClient.CaptureOrder(orderId);
                var order = new Order
                {
                    UserID = _userManager.GetUserId(User),
                    OrderDate = DateTime.Now,
                    PaymentType = PaymentType.PAYPAL,
                    IsPayment = true,
                    Status = true
                };
                order.Details = cart.Items.Select(i => new OrderDetail
                {
                    ProductID = i.ProductId,
                    Quantity = (int)i.Quantity,
                    Price = (decimal) i.Price,
                }).ToList();
                foreach (var item in cart.Items)
                {
                    var product = await _context.Products
                        .Include(p => p.ProductDetails)
                        .FirstOrDefaultAsync(x => x.ProductID == item.ProductId);
                    product.QuantityOnHand -= (int)item.Quantity;
                    var prodt = product.ProductDetails.FirstOrDefault(x => x.SizeID == item.SizeID);
                    prodt.Quantity -= (int)item.Quantity;
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                }
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("Cart");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }
        #endregion

        public IActionResult OrderCompleted()
        {
            return View();
        }

        [Authorize]
        public IActionResult PaymentFail()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = "Lỗi thanh toán VNPAY:{ response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }

            var user = await _userManager.GetUserAsync(User);
            try
            {
                var order = new Order()
                {
                    UserID = user.Id,
                    OrderDate = DateTime.Now,
                    PaymentType = PaymentType.VNPAY,
                    Status = false
                };
                order.Details = cart.Items.Select(i => new OrderDetail
                {
                    ProductID = i.ProductId,
                    Quantity = (int) i.Quantity,
                    Price = (decimal)i.Price,
                }).ToList();

                foreach(var item in cart.Items)
                {
                    var product = await _context.Products
                        .Include(p => p.ProductDetails)
                        .FirstOrDefaultAsync(x => x.ProductID == item.ProductId);
                    product.QuantityOnHand -= (int) item.Quantity;
                    var prodt = product.ProductDetails.FirstOrDefault(x => x.SizeID == item.SizeID);
                    prodt.Quantity -= (int) item.Quantity;
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                }
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("Cart");

            }
            catch (Exception ex)
            {

            }
            TempData["Message"] = $"Thanh toán VNPAY thành công";
            return RedirectToAction("OrderCompleted");
        }
    }
}
