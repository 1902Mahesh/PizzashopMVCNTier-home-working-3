using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;

namespace DataLogicLayer.Interfaces;

public interface IOrderRepository
{
    public Task<(List<OrderListViewModel> orders, int totalRecords)> GetOrdersAsync(int pageNo = 1, int pageSize = 3, string search = "",string columnName="", string sortOrder="",string toDate="",string fromDate="",string status="",string time="");
    public Task<List<OrderListViewModel>> GetOrderListForExportAsync(string search = "",string status="",string time="");
    public Task<List<OrderStatus>> GetOrderStatusAsync();
    public Task<OrderDetailsViewModel> GetOrderDetailsAsync(long id);
}
