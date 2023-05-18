namespace P328Pustok.ViewModels
{
    public class BasketListViewModel
    {
        public List<BasketItemViewModel> BasketItems { get; set; } = new List<BasketItemViewModel>();
        public decimal TotalPrice { get; set; }
    }
}
