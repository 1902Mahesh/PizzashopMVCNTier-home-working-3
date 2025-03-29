using DataLogicLayer.Models;

namespace DataLogicLayer.ViewModels;

public class ModifierItemViewModel
{
    public long? ModifierGroupId { get; set; }
    public long ModifierItemId { get; set; }
    public string Name { get; set; } = null!;
    public decimal? Rate { get; set; }
    public string Unit { get; set; } = null!;
    public int Quantity { get; set; }
}
