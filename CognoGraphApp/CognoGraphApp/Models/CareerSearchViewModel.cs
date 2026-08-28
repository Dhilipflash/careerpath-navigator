using System.ComponentModel.DataAnnotations;

namespace CognoGraphApp.Models;

public class CareerSearchViewModel
{
    [Required(ErrorMessage = "Please select your current role.")]
    public string? CurrentRole { get; set; }

    [Required(ErrorMessage = "Please select your target role.")]
    public string? TargetRole { get; set; }

    public List<string> AvailableRoles { get; set; } = new();

    public CareerPathResult? Result { get; set; }

    public string? ErrorMessage { get; set; }
}