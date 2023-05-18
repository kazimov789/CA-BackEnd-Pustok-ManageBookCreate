using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using P328Pustok.DAL;
using P328Pustok.Models;
using P328Pustok.ViewModels;

namespace P328Pustok.Controllers
{
    public class BookController : Controller
    {
        private readonly PustokContext _context;

        public BookController(PustokContext context)
        {
            _context = context;
        }
        public IActionResult GetBookDetail(int id)
        {
            Book book = _context.Books
                .Include(x => x.Author)
                .Include(x => x.BookImages)
                .FirstOrDefault(x => x.Id == id);

            if (book == null) return StatusCode(404);

            //return Json(new { book = book });
            return PartialView("_BookModalPartial", book);
        }

        public IActionResult AddBasket(int id)
        {
            List<BasketViewModel> cookieItems = new List<BasketViewModel>();

            BasketViewModel cookieItem;
            var basketStr = Request.Cookies["basket"];
            if (basketStr != null)
            {
                cookieItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketStr);

                cookieItem = cookieItems.FirstOrDefault(x => x.Id == id);

                if (cookieItem != null)
                    cookieItem.Count++;
                else
                {
                    cookieItem = new BasketViewModel { Id = id, Count = 1 };
                    cookieItems.Add(cookieItem);
                }
            }
            else
            {
                cookieItem = new BasketViewModel { Id = id, Count = 1 };
                cookieItems.Add(cookieItem);
            }

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(cookieItems));

            BasketListViewModel bv = new BasketListViewModel();
            foreach (var ci in cookieItems)
            {
                BasketItemViewModel bi = new BasketItemViewModel
                {
                    Count = ci.Count,
                    Book = _context.Books.Include(x => x.BookImages).FirstOrDefault(x => x.Id == ci.Id)
                };
                bv.BasketItems.Add(bi);
                bv.TotalPrice += (bi.Book.DiscountPercent > 0 ? (bi.Book.SalePrice * (100 - bi.Book.DiscountPercent) / 100) : bi.Book.SalePrice) * bi.Count;
            }

            return PartialView("_BasketPartialView", bv);
        }

        public IActionResult RemoveBasket(int id)
        {
            var basketStr = Request.Cookies["basket"];
            if (basketStr == null)
                return StatusCode(404);

            List<BasketViewModel> cookieItems = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketStr);

            BasketViewModel  item = cookieItems.FirstOrDefault(x => x.Id == id);

            if (item == null)
                return StatusCode(404);

            if (item.Count > 1)
                item.Count--;
            else
                cookieItems.Remove(item);

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(cookieItems));

            BasketListViewModel bv = new BasketListViewModel();
            foreach (var ci in cookieItems)
            {
                BasketItemViewModel bi = new BasketItemViewModel
                {
                    Count = ci.Count,
                    Book = _context.Books.Include(x => x.BookImages).FirstOrDefault(x => x.Id == ci.Id)
                };
                bv.BasketItems.Add(bi);
                bv.TotalPrice += (bi.Book.DiscountPercent > 0 ? (bi.Book.SalePrice * (100 - bi.Book.DiscountPercent) / 100) : bi.Book.SalePrice) * bi.Count;
            }


            return PartialView("_BasketPartialView", bv);
        }

        public IActionResult Showbasket()
        {
            var basket = new List<BasketViewModel>();
            var basketStr = Request.Cookies["basket"];
            if (basketStr != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketStr);
            }

            return Json(new { basket });
        }
    }
}
