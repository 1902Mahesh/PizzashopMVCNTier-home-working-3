using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DataLogicLayer.Implementations;

public class CustomerRepository : ICustomerRepository
{
    private readonly PizzaShopDbContext _context;


    public CustomerRepository(PizzaShopDbContext context)
    {
        _context = context;

    }
    public async Task<(List<CustomerViewModel> customers, int totalRecords)> GetCustomerAsync(int pageNo = 1, int pageSize = 3, string search = "", string columnName = "", string sortOrder = "", string toDate = "", string fromDate = "", string time = "")
    {
        IQueryable<CustomerViewModel> query = _context.Customers
                                        .Where(c => !c.Isdeleted)
                                        .Include(c => c.Orders)
                                        .Select(c => new CustomerViewModel
                                        {
                                            CustomerId = c.CustomerId,
                                            Customername = c.Name,
                                            Phone = c.Phone,
                                            Email = c.Email,
                                            Date = DateOnly.FromDateTime(c.CreatedAt),
                                            TotalOrders = c.Orders.Where(o => o.Customerid == c.CustomerId).Select(o => o.Customerid).Count(),
                                        });
        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            query = query.Where(c => c.CustomerId.ToString().Contains(search)
                        || c.Customername.ToLower().Contains(search)
                         || c.Email.ToLower().Contains(search));
        }
        switch (columnName.ToLower())
        {
            case "name":
                query = (sortOrder == "asc") ? query.OrderBy(u => u.Customername) : query.OrderByDescending(u => u.Customername);
                break;
            case "date":
                query = (sortOrder == "asc") ? query.OrderBy(u => u.Date) : query.OrderByDescending(u => u.Date);
                break;
            case "totalorders":
                query = (sortOrder == "asc") ? query.OrderBy(u => u.TotalOrders) : query.OrderByDescending(u => u.TotalOrders);
                break;
            default:
                query = query.OrderBy(u => u.CustomerId);
                break;
        }
        if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            switch (time.ToLower())
            {
                case "last 7 days":
                    query = query.Where(o => o.Date >= today.AddDays(-7));
                    break;
                case "last 30 days":
                    query = query.Where(o => o.Date >= today.AddDays(-30));
                    break;
                case "current month":
                    query = query.Where(o => o.Date.Month == today.Month && o.Date.Year == today.Year);
                    break;
            }
        }
        else{
            if (!string.IsNullOrEmpty(fromDate))
            {
                DateOnly FromDate = DateOnly.Parse(fromDate);
                query = query.Where(o => o.Date >= FromDate);

            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateOnly ToDate = DateOnly.Parse(toDate);
                query = query.Where(o => o.Date <= ToDate);
            }
        }

        int totalRecords = await query.CountAsync();
        List<CustomerViewModel> customers = await query
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

        return (customers, totalRecords);
    }

    public async Task<List<CustomerViewModel>> GetCustomerListForExportAsync(string search = "", string time = "", string fromDate = "", string toDate = "")
    {
        IQueryable<CustomerViewModel> query = _context.Customers
                                        .Where(c => !c.Isdeleted)
                                        .Include(c => c.Orders)
                                        .Select(c => new CustomerViewModel
                                        {
                                            CustomerId = c.CustomerId,
                                            Customername = c.Name,
                                            Phone = c.Phone,
                                            Email = c.Email,
                                            Date = DateOnly.FromDateTime(c.CreatedAt),
                                            TotalOrders = c.Orders.Where(o => o.Customerid == c.CustomerId).Select(o => o.Customerid).Count(),
                                        });
        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            query = query.Where(c => c.CustomerId.ToString().Contains(search)
                        || c.Customername.ToLower().Contains(search)
                         || c.Email.ToLower().Contains(search));
        }

        if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            switch (time.ToLower())
            {
                case "last 7 days":
                    query = query.Where(o => o.Date >= today.AddDays(-7));
                    break;
                case "last 30 days":
                    query = query.Where(o => o.Date >= today.AddDays(-30));
                    break;
                case "current month":
                    query = query.Where(o => o.Date.Month == today.Month && o.Date.Year == today.Year);
                    break;
            }
        }
        else{
            if (!string.IsNullOrEmpty(fromDate))
            {
                DateOnly FromDate = DateOnly.Parse(fromDate);
                query = query.Where(o => o.Date >= FromDate);

            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateOnly ToDate = DateOnly.Parse(toDate);
                query = query.Where(o => o.Date <= ToDate);
            }
        }

        return await query.ToListAsync();
    }

}
