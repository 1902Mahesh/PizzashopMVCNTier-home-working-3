using System.Globalization;
using DataLogicLayer.Interfaces;
using DataLogicLayer.Models;
using DataLogicLayer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DataLogicLayer.Implementations;

public class OrderRepository : IOrderRepository
{
    private readonly PizzaShopDbContext _context;

    public OrderRepository(PizzaShopDbContext context)
    {
        _context = context;

    }


    public async Task<List<OrderListViewModel>> GetOrderListForExportAsync(string search = "", string status = "", string time = "")
    {
        var query = from ordertable in _context.Orders
                    join customer in _context.Customers on ordertable.Customerid equals customer.CustomerId
                    join orderstatus in _context.OrderStatuses on ordertable.Orderstatus equals orderstatus.Id
                    join payment in _context.Payments on ordertable.Id equals payment.Orderid
                    join paymentMethod in _context.Paymentmethods on payment.Methods equals paymentMethod.Id
                    join customerRating in _context.Customerreviews on ordertable.Id equals customerRating.Orderid
                    orderby ordertable.Id
                    select new OrderListViewModel
                    {
                        OrderNo = ordertable.Id,
                        OrderDate = DateOnly.FromDateTime(ordertable.Orderdate),
                        CustomerName = customer.Name,
                        Status = orderstatus.Name,
                        PaymentMode = paymentMethod.Name,
                        AvgRating = customerRating.AverageRating ?? 0,
                        TotalAmount = ordertable.Subtotal
                    };

        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            query = query.Where(u => u.OrderNo.ToString().Contains(search) || u.CustomerName.ToLower().Contains(search));
        }

        if (!string.IsNullOrEmpty(status) && !status.Equals("All Status"))
        {
            query = query.Where(u => u.Status.Contains(status));
        }

        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        switch (time.ToLower())
        {
            case "last 7 days":
                query = query.Where(o => o.OrderDate >= today.AddDays(-7));
                break;
            case "last 30 days":
                query = query.Where(o => o.OrderDate >= today.AddDays(-30));
                break;
            case "current month":
                query = query.Where(o => o.OrderDate.Month == today.Month && o.OrderDate.Year == today.Year);
                break;
        }

        List<OrderListViewModel> model = await query.ToListAsync();
        return model;
    }


    public async Task<(List<OrderListViewModel> orders, int totalRecords)> GetOrdersAsync(int pageNo = 1, int pageSize = 3, string search = "", string columnName = "", string sortOrder = "", string toDate = "", string fromDate = "", string status = "", string time = "")
    {
        var query = from ordertable in _context.Orders
                    join customer in _context.Customers on ordertable.Customerid equals customer.CustomerId
                    join orderstatus in _context.OrderStatuses on ordertable.Orderstatus equals orderstatus.Id
                    join payment in _context.Payments on ordertable.Id equals payment.Orderid
                    join paymentMethod in _context.Paymentmethods on payment.Methods equals paymentMethod.Id
                    join customerRating in _context.Customerreviews on ordertable.Id equals customerRating.Orderid
                    orderby ordertable.Id
                    select new OrderListViewModel
                    {
                        OrderNo = ordertable.Id,
                        OrderDate = DateOnly.FromDateTime(ordertable.Orderdate),
                        CustomerName = customer.Name,
                        Status = orderstatus.Name,
                        PaymentMode = paymentMethod.Name,
                        AvgRating = customerRating.AverageRating ?? 0,
                        TotalAmount = ordertable.Subtotal
                    };


        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            query = query.Where(u => u.OrderNo.ToString().Contains(search) || u.CustomerName.ToLower().Contains(search));
        }

        if (!string.IsNullOrEmpty(status) && !status.Equals("All Status"))
        {
            query = query.Where(u => u.Status.Contains(status));
        }

        if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            switch (time.ToLower())
            {
                case "last 7 days":
                    query = query.Where(o => o.OrderDate >= today.AddDays(-7));
                    break;
                case "last 30 days":
                    query = query.Where(o => o.OrderDate >= today.AddDays(-30));
                    break;
                case "current month":
                    query = query.Where(o => o.OrderDate.Month == today.Month && o.OrderDate.Year == today.Year);
                    break;
            }
        }

        if (!string.IsNullOrEmpty(fromDate))
        {
            DateOnly FromDate = DateOnly.Parse(fromDate);
            query = query.Where(o => o.OrderDate >= FromDate);

        }
        if (!string.IsNullOrEmpty(toDate))
        {
            DateOnly ToDate = DateOnly.Parse(toDate);
            query = query.Where(o => o.OrderDate <= ToDate);
        }

        switch (columnName.ToLower())
        {
            case "order":
                query = (sortOrder == "asc") ? query.OrderBy(u => u.OrderNo) : query.OrderByDescending(u => u.OrderNo);
                break;
            case "date":
                query = (sortOrder == "asc") ? query.OrderBy(u => u.OrderDate) : query.OrderByDescending(u => u.OrderDate);
                break;
            case "customer":
                query = (sortOrder == "asc") ? query.OrderBy(u => u.CustomerName) : query.OrderByDescending(u => u.CustomerName);
                break;
            case "totalamount":
                query = (sortOrder == "asc") ? query.OrderBy(u => u.TotalAmount) : query.OrderByDescending(u => u.TotalAmount);
                break;
            default:
                query = query.OrderBy(u => u.OrderNo);
                break;
        }

        int totalRecords = await query.CountAsync();
        List<OrderListViewModel> orders = await query
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

        return (orders, totalRecords);
    }

    public async Task<List<OrderStatus>> GetOrderStatusAsync()
    {
        return await _context.OrderStatuses.ToListAsync();
    }

    public async Task<OrderDetailsViewModel> GetOrderDetailsAsync(long id)
    {
        OrderDetailsViewModel? model = await _context.Orders
                                    .Include(o => o.OrderstatusNavigation)
                                    .Include(o => o.Customer)
                                    .Include(o => o.Ordertablemappings)
                                    .ThenInclude(otm => otm.Table)
                                    .Include(o => o.Payments)
                                    .ThenInclude(p => p.MethodsNavigation)
                                    .Where(o => o.Id == id)
                                    .Select(o => new OrderDetailsViewModel
                                    {
                                        OrderId = id,
                                        OrderStatus = o.OrderstatusNavigation.Name,
                                        InvoiceNo = o.Invoices.Where(i => i.Orderid == id).Select(i => i.InvoiceNo).First(),
                                        PaidOn = o.Payments.Where(p => p.Orderid == id).Select(p => p.Paymentdate).First().ToString() ?? "",
                                        PlacedOn = o.CreatedAt.ToString(),
                                        ModifiedOn = o.UpdatedAt.ToString() ?? "",
                                        SubTotal = o.Subtotal,
                                        PaymentMethod = o.Payments.Where(p => p.Orderid == o.Id).Select(p => p.MethodsNavigation.Name).First(),
                                        OrderDuration = (o.Payments.Where(p => p.Orderid == o.Id).Select(p => p.Paymentdate).First() - o.CreatedAt).ToString(),
                                        Members = (int)o.TotalPersons,
                                        CustomerDetails = new CustomerViewModel
                                        {
                                            CustomerId = o.Customer.CustomerId,
                                            Customername = o.Customer.Name,
                                            Phone = o.Customer.Phone,
                                            Email = o.Customer.Email,
                                        },
                                        TableDetails = o.Ordertablemappings.Where(ot => ot.Orderid == id).Select(ot => ot.Table.Name).ToList(),
                                        SectionName = o.Ordertablemappings.Where(ot => ot.Orderid == id).Select(ot => ot.Table.Section.Name).First(),
                                        Items = o.Orderdetails.Where(od => od.Orderid == id).Select(od => new OrderItemViewModel
                                        {
                                            ItemId = o.Id,
                                            ItemName = od.Item.Name,
                                            Rate = od.Price,
                                            Quantity = od.Quantity,
                                            TotalAmount = od.Price * od.Quantity,
                                            AddOns = od.Orderitemsmodifiers.Where(otm => otm.OrderItemId == od.Id).Select(otm => new ModifierItemViewModel
                                            {
                                                Name = otm.ModifierItem.Name,
                                                Rate = otm.Rate,
                                                Quantity = otm.Quantity,
                                            }).ToList(),
                                        }).ToList(),
                                        Taxes = _context.Taxes.Where(t => !t.Isdeleted).OrderBy(t => t.Id).Select(t => new TaxListViewModel
                                        {
                                            TaxName = t.Name,
                                            TaxType = t.Taxtype,
                                            TaxValue = t.Amount,
                                        }).ToList(),
                                    }).FirstOrDefaultAsync();

        return model;
    }

}
