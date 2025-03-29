using DataLogicLayer.ViewModels;

namespace DataLogicLayer.Interfaces;

public interface ICustomerRepository
{
    public Task<(List<CustomerViewModel> customers, int totalRecords)> GetCustomerAsync(int pageNo = 1, int pageSize = 3, string search = "",string columnName="", string sortOrder="",string toDate = "", string fromDate = "",  string time = "");
    public Task<List<CustomerViewModel>> GetCustomerListForExportAsync(string search = "", string time = "", string fromDate = "", string toDate = "");

}
