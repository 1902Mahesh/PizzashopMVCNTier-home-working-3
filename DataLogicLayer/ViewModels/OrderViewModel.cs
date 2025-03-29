namespace DataLogicLayer.ViewModels;

public class OrderViewModel
{
    public List<OrderListViewModel>? OrderList { get; set; } = new List<OrderListViewModel>();
    public PaginationViewModel? Page { get; set; }
}
