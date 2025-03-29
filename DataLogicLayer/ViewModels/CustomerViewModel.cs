namespace DataLogicLayer.ViewModels;

public class CustomerViewModel
{
    public long? CustomerId { get; set; }
    public string Customername { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateOnly Date { get; set; }
    public int TotalOrders { get; set; }
}
