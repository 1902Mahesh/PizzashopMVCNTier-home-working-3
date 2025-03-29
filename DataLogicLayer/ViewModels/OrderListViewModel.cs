using DataLogicLayer.Models;

namespace DataLogicLayer.ViewModels;

public class OrderListViewModel
{
    public long OrderNo { get; set; }
    public DateOnly OrderDate { get; set; }
    public string? CustomerName { get; set; }
    public string Status { get; set; }
    public string PaymentMode { get; set; }
    public decimal AvgRating { get; set; }
    public decimal? TotalAmount { get; set; }
}
