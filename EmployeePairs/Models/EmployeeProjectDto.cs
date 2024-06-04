namespace EmployeePairs.Models;

public record EmployeeProjectDto
{
    public int EmpId { get; set; }

    public int ProjectId { get; set; }

    public string? DateFrom { get; set; }

    public string? DateTo { get; set; }
}
