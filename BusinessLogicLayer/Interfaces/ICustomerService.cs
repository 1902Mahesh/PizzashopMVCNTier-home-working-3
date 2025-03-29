using DataLogicLayer.ViewModels;

namespace BusinessLogicLayer.Interfaces;

public interface ICustomerService
{
    public Task<CustomerListViewModel> GetCustomerList(int pageNo, int pageSize, string search,string columnName, string sortOrder,string toDate, string fromDate, string time);
    public Task<byte[]> GetCustomerListForExport(string search, string time, string fromDate, string toDate);
}
