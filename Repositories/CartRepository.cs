using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SC.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public CartRepository(ApplicationDbContext db,
            IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager ) 
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        //24101
        public async Task<int> AddItem(int bookId, int qty)
        {
            string userId = GetUserId();
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                
                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("user not logged in");
                }
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _db.ShoppingCarts.Add(cart);
                }
                _db.SaveChanges();
                var cartItem = _db.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.Id
                && a.BookId == bookId);
                if(cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var book = _db.Books.Find(bookId);
                    cartItem = new CartDetail
                    {
                        BookId = bookId,
                        ShoppingCartId = cart.Id,
                        Quantity=qty,
                        UnitPrice = book.Price
                    };
                    _db.CartDetails.Add(cartItem);
                }
                _db.SaveChanges();
                transaction.Commit();
            }
            catch(Exception e)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<int> RemoveItem(int bookId)
        {
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("user not logged in");
                }
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    throw new InvalidOperationException("Invalid cart");
                }
                _db.SaveChanges();
                var cartItem = _db.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.Id
                && a.BookId == bookId);

                if (cartItem is null) throw new InvalidOperationException("No items in cart");
                else if(cartItem.Quantity==1)
                {
                    _db.CartDetails.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity = cartItem.Quantity - 1;
                }


                _db.SaveChanges();
            }
            catch (Exception e)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null) throw new InvalidOperationException("Invalid user Id");
            //connect 4 tables
            var shoppingCart = await _db.ShoppingCarts
                .Include(a => a.CartDetails)
                .ThenInclude(a => a.Book)
                .ThenInclude(a => a.Stock)
                .Include(a => a.CartDetails)
                .ThenInclude(a => a.Book)
                .ThenInclude(a => a.Genre)
                .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;
        }

        public async Task<int> GetCartItemCount(string userId="")
        {
            if(string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _db.ShoppingCarts
                              join CartDetail in _db.CartDetails
                              on cart.Id equals CartDetail.ShoppingCartId
                              where cart.UserId==userId
                              select new { CartDetail.Id }).ToListAsync();
            return data.Count;
        }

        public async Task<ShoppingCart> GetCart(string userId) 
            {
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        public async Task<bool> DoCheckout(CheckOutModel model)
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId)) throw new UnauthorizedAccessException("user not logged in");
                var cart = await GetCart(userId);
                if (cart is null) throw new InvalidOperationException("Invalid cart");
                var cartDetail = _db.CartDetails.Where(a => a.ShoppingCartId == cart.Id).ToList();
                if (cartDetail.Count == 0) throw new InvalidOperationException("No items in cart");
                var pendingRecord = _db.OrderStatuses.FirstOrDefault(s => s.StatusName == "Pending");
                if (pendingRecord is null) throw new InvalidOperationException("Order does not have Pending status");
                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    Name=model.Name,
                    Email=model.Email,
                    MobileNumber=model.MobileNumber,
                    PaymentMethod=model.PaymentMethod,
                    Address=model.Address,
                    Ispaid=false,
                    OrderStatusId = pendingRecord.Id,
                };
                _db.Orders.Add(order);
                _db.SaveChanges();
                foreach(var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        BookId = item.BookId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _db.OrderDetails.Add(orderDetail);
                    
                var stock = await _db.Stocks.FirstOrDefaultAsync(a => a.BookId == item.BookId);
                if (stock == null)
                    {
                        throw new InvalidOperationException("Stock is null");
                    }
                if (item.Quantity > stock.Quantity)
                    {
                        throw new InvalidOperationException($"Only {stock.Quantity} items available");
                    }
                    //decrease the quantity after checkout
                    stock.Quantity -= item.Quantity;
                }
                //_db.SaveChanges();

                



                //Remove details after checkout
                _db.CartDetails.RemoveRange(cartDetail);
                _db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception ex) { return false; }
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(principal);
            return userId;
        }


    }

}
