namespace DataLogicLayer.ViewModels;

public class CustomerListViewModel
{
    public List<CustomerViewModel>? CustomerList { get; set; }
    public PaginationViewModel? Page { get; set; }
}
