using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Threading.Tasks;

namespace ECommerce514.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartRepository _cartRepository;
        private readonly ApplicationDbContext _context;
        private readonly IOrderRepository _orderRepository;

        public CartController(UserManager<ApplicationUser> userManager, ICartRepository cartRepository, ApplicationDbContext context, IOrderRepository orderRepository)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            _context = context;
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> AddToCart(int productId, int count)
        {
            var user = await _userManager.GetUserAsync(User);

            if(user is not null)
            {
                var product = _context.Products.Find(productId);

                if(product.Quantity >= count && count > 0)
                {
                    var productInCart = await _cartRepository.GetOneAsync(e => e.ApplicationUserId == user.Id && e.ProductId == productId);

                    if(productInCart is not null)
                    {
                        productInCart.Count += count;
                    }
                    else
                    {
                        await _cartRepository.CreateAsync(new()
                        {
                            ApplicationUserId = user.Id,
                            ProductId = productId,
                            Count = count
                        });
                    }

                    await _cartRepository.CommitAsync();

                    TempData["success-notification"] = "Add to Cart successfully";
                    return RedirectToAction("Index", "Home");
                }

                TempData["error-notification"] = "overs count";
                return RedirectToAction("Details", "Home", new { area = "Customer", id = productId });
            }

            return NotFound();
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if(user is not null)
            {
                var carts = await _cartRepository.GetAsync(e=>e.ApplicationUserId == user.Id, includes: [e => e.Product]);

                var totalPrice = carts.Sum(e => e.Product.Price * e.Count);
                ViewBag.totalPrice = totalPrice;

                return View(carts);
            }

            return NotFound();
        }

        public async Task<IActionResult> IncrementCount(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is not null)
            {
                var productInCart = await _cartRepository.GetOneAsync(e => e.ApplicationUserId == user.Id && e.ProductId == productId);
                if(productInCart is not null)
                {
                    productInCart.Count++;
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
                return NotFound();
            }
            return NotFound();

        }

        public async Task<IActionResult> DecrementCount(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is not null)
            {
                var productInCart = await _cartRepository.GetOneAsync(e => e.ApplicationUserId == user.Id && e.ProductId == productId);
                if (productInCart is not null)
                {
                    if(productInCart.Count > 1)
                    {
                        productInCart.Count--;
                        _context.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
                return NotFound();
            }
            return NotFound();

        }

        public async Task<IActionResult> Delete(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is not null)
            {
                var productInCart = await _cartRepository.GetOneAsync(e => e.ApplicationUserId == user.Id && e.ProductId == productId);
                if (productInCart is not null)
                {
                    await _cartRepository.DeleteAsync(productInCart);
                    await _cartRepository.CommitAsync();

                    return RedirectToAction("Index");
                }
                return NotFound();
            }
            return NotFound();

        }

        public async Task<IActionResult> Pay()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is not null)
            {
                var carts = await _cartRepository.GetAsync(e => e.ApplicationUserId == user.Id, includes: [e=>e.Product]);

                if (carts is not null)
                {

                    await _orderRepository.CreateAsync(new()
                    {
                        ApplicationUserId = user.Id,
                        Date = DateTime.UtcNow,
                        OrderStatus = OrderStatus.pending,
                        PaymentMethod = PaymentMethod.Visa,
                        TotalPrice = carts.Sum(e => e.Product.Price * e.Count)
                    });

                    var order = (await _orderRepository.GetAsync(e => e.ApplicationUserId == user.Id)).OrderBy(e => e.Id).LastOrDefault();

                    if (order is null)
                        return BadRequest();

                    var options = new SessionCreateOptions
                    {
                        PaymentMethodTypes = new List<string> { "card" },
                        LineItems = new List<SessionLineItemOptions>(),
                        Mode = "payment",
                        SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Success?orderId={order.Id}",
                        CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Cancel",
                    };


                    foreach (var item in carts)
                    {
                        options.LineItems.Add(new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency = "egp",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = item.Product.Name,
                                    Description = item.Product.Description,
                                },
                                UnitAmount = (long)item.Product.Price * 100,
                            },
                            Quantity = item.Count,
                        });
                    }


                    var service = new SessionService();
                    var session = service.Create(options);
                    order.SessionId = session.Id;
                    await _orderRepository.CommitAsync();
                    return Redirect(session.Url);

                }
                return NotFound();
            }
            return NotFound();

            
        }

    }
}
