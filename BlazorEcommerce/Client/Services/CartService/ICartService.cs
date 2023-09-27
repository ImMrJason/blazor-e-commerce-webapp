using Blazored.LocalStorage;

namespace BlazorEcommerce.Client.Services.CartService
{
    public interface ICartService
    {
        event Action OnChange;
        Task AddToCart(CartItem cartItem);
        Task<List<CartItem>> GetCartItems();
        Task<List<CartProductResponse>> GetCartProducts();
        Task RemoveProductFromCart(int productId, int productTypeId);
    }

    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;

        public CartService(ILocalStorageService localStorage, HttpClient http)
        {
            _localStorage = localStorage;
            _http = http;
        }

        public event Action OnChange;

        public async Task AddToCart(CartItem cartItem)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

            if (cart == null)
            {
                // create cart if it doesn't exist yet
                cart = new List<CartItem>();
            }

            cart.Add(cartItem);

            // set the cart in local storage
            await _localStorage.SetItemAsync("cart", cart);

            OnChange.Invoke();
        }

        public async Task<List<CartItem>> GetCartItems()
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

            if (cart == null)
            {
                // create cart if it doesn't exist yet
                cart = new List<CartItem>();
            }

            return cart;
        }

        public async Task<List<CartProductResponse>> GetCartProducts()
        {
            // get "cart" from local storage
            var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");

            // use _http to get the products from the server (using post)
            var response = await _http.PostAsJsonAsync("api/cart/products", cartItems);

            var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();

            return cartProducts.Data;
        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                return;
            }

            var cartItem = cart.Find(x => x.ProductId == productId && x.ProductTypeId == productTypeId);

            if (cartItem != null)
            {
                cart.Remove(cartItem);
                await _localStorage.SetItemAsync("cart", cart);
                OnChange.Invoke();
            }
        }
    }
}
