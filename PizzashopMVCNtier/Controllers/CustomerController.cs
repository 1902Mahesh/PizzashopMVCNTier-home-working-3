using BusinessLogicLayer.Interfaces;
using DataLogicLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace PizzashopMVCNtier.Controllers;

public class CustomerController : Controller
{
    private readonly ICustomerService _customerService;


    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;

    }
    public IActionResult Index()
    {
        ViewData["ActiveLink"] = "Customers";
        CustomerViewModel model = new CustomerViewModel();
        return View(model);
    }

    public async Task<IActionResult> GetCustomerList(int pageNo = 1, int pageSize = 3, string search = "", string columnName = "", string sortOrder = "",string toDate = "", string fromDate = "", string time = "")
    {
        return PartialView("_customerList", await _customerService.GetCustomerList(pageNo, pageSize, search, columnName, sortOrder,toDate, fromDate, time));
    }

    public async Task<IActionResult> ExportCustomerToExcel(string search = "", string time = "", string fromDate = "", string toDate = "")
    {
        byte[] fileData = await _customerService.GetCustomerListForExport(search, time, fromDate, toDate);
        return File(fileData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Customers.xlsx");
    }
}
