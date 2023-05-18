using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using P328Pustok.DAL;
using P328Pustok.Models;
using P328Pustok.ViewModels;

namespace P328Pustok.Services
{
    public class LayoutService
    {
        private readonly PustokContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LayoutService(PustokContext context,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings.ToDictionary(x => x.Key, x => x.Value);
        }

        public List<Genre> GetGenres()
        {
            return _context.Genres.ToList();
        }

        public BasketListViewModel GetBasket()
        {
            var bv = new BasketListViewModel();
            var basketJson = _httpContextAccessor.HttpContext.Request.Cookies["basket"];

            if (basketJson != null)
            {
                var cookieItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketJson);

                foreach (var ci in cookieItems)
                {
                    BasketItemViewModel bi = new BasketItemViewModel
                    {
                        Count = ci.Count,
                        Book = _context.Books.FirstOrDefault(x => x.Id == ci.Id)
                    };
                    bv.BasketItems.Add(bi);
                    bv.TotalPrice += (bi.Book.DiscountPercent > 0 ? (bi.Book.SalePrice * (100 - bi.Book.DiscountPercent) / 100) : bi.Book.SalePrice) * bi.Count;
                }
            }

            return bv;
        }
    }
}
