using System.ComponentModel.DataAnnotations;

namespace Foxic.Buisness.ViewModels.AreasViewModels.CategoryVMs;

public class CategoryListVM
{
    [Required, MaxLength(40), MinLength(5)]
    public string CategoryName { get; set; }
}
