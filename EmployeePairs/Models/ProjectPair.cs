namespace EmployeePairs.Models;

public record ProjectPair
{
    public int EmployeeOneId { get; set; }

    public int EmployeeTwoId { get; set; }

    public int ProjectId { get; set; }

    public int DaysWorked { get; set; }
}
