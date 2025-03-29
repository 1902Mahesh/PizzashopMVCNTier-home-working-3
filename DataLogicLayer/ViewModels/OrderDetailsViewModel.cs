namespace DataLogicLayer.ViewModels;

public class OrderDetailsViewModel
{
    public long OrderId { get; set; }
    public string OrderStatus { get; set; }
    public string PaymentMethod { get; set; } = null!;
    public string InvoiceNo { get; set; }
    public string? PaidOn { get; set; }
    public string? PlacedOn { get; set; }
    public string? ModifiedOn { get; set; }
    public string? OrderDuration { get; set; }
    public int Members { get; set; }
    public Decimal? SubTotal { get; set; }
    public CustomerViewModel? CustomerDetails { get; set; }
    public List<string> TableDetails { get; set; } = new List<string>();
    public string SectionName { get; set; } = null!;
    public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
    public List<TaxListViewModel> Taxes { get; set; } = new List<TaxListViewModel>();
}
