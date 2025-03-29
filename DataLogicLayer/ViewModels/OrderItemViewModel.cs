namespace DataLogicLayer.ViewModels;

public class OrderItemViewModel
{
    public long ItemId { get; set; }
    public string ItemName { get; set; } = null!;
    public decimal? Rate { get; set; }
    public int Quantity { get; set; }
    public decimal? TotalAmount { get; set; }
    public List<ModifierItemViewModel> AddOns { get; set; } = new List<ModifierItemViewModel>();
}
