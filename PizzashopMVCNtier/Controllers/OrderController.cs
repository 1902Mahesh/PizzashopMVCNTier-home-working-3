using System.Globalization;
using System.Threading.Tasks;
using BusinessLogicLayer.Interfaces;
using DataLogicLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace PizzashopMVCNtier.Controllers;

public class OrderController : Controller
{
    private readonly IOrderService _orderService;


    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;

    }
    public async Task<IActionResult> Index()
    {
        ViewData["ActiveLink"] = "Order";
        OrderIndexViewModel model = new OrderIndexViewModel();
        model.orderStatus = await _orderService.GetOrderStatus();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetOrderList(int pageNo = 1, int pageSize = 3, string search = "", string columnName = "", string sortOrder = "", string toDate = "", string fromDate = "", string status = "", string time = "")
    {
        return PartialView("_orderListPartial", await _orderService.GetOrderList(pageNo, pageSize, search, columnName, sortOrder, toDate, fromDate, status, time));
    }

    public async Task<IActionResult> ExportOrdersToExcel(string search = "", string status = "", string time = "")
    {
        byte[] fileData = await _orderService.GetOrderListForExport(search, status, time);
        return File(fileData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Orders.xlsx");
    }

    public async Task<IActionResult> OrderDetail(long id)
    {
        ViewData["ActiveLink"] = "Order";
        OrderDetailsViewModel model = await _orderService.GetOrderDetails(id);
        return View(model);
    }
    public async Task<IActionResult> GenerateInvoice(long id)
    {
        byte[] fileBytes = await _orderService.ExportToPdf(id);
        return File(fileBytes, "application/pdf", "Order.pdf");
    }

}
