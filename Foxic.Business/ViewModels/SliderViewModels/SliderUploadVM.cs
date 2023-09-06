using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Foxic.Buisness.ViewModels.SliderViewModels;

public class SliderUploadVM
{
    public int Id { get; set; }

    [Required, MaxLength(40), MinLength(5)]
    public string SliderName { get; set; } = null!;

    [Required, MaxLength(90)]
    public string SliderAbout { get; set; } = null!;


    public IFormFile? Image { get; set; }

    public string? SliderImage { get; set; }
}


