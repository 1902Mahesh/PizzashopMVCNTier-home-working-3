using DataLogicLayer.Models;

namespace DataLogicLayer.ViewModels;

public class OrderIndexViewModel
{
    public List<OrderStatus> orderStatus { get; set; } = new List<OrderStatus>();
} 
