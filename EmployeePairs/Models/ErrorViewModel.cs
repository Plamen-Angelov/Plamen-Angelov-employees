namespace EmployeePairs.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public string? Details { get; set; }

    public bool ShowDetails => !string.IsNullOrEmpty(Details);
}