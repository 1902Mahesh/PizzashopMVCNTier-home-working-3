using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;

namespace BusinessLogicLayer.Interfaces;

public interface IOrderService
{
    public Task<OrderViewModel> GetOrderList(int pageNo, int pageSize, string search,string columnName, string sortOrder,string toDate,string fromDate,string status,string time);
    public Task<byte[]> GetOrderListForExport(string search,string status,string time);
    public Task<List<OrderStatus>> GetOrderStatus();
    public Task<OrderDetailsViewModel> GetOrderDetails(long id);
    public Task<byte[]> ExportToPdf(long id);
    
}
