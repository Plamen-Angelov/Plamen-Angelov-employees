using EmployeePairs.Models;

namespace EmployeePairs.Services;

public class EmployeePairService
{
    public List<ProjectPair> GetLongestPairedEmployees(List<EmployeeProjectDto> records)
    {
        var adjustedDateRecords = records
            .Select(x => new EmployeeProject
            {
                EmpId = x.EmpId,
                ProjectId = x.ProjectId,
                DateFrom = AdjustDate(x.DateFrom),
                DateTo = AdjustDate(x.DateTo)
            })
            .ToList();

        var projectPairs = new List<ProjectPair>();

        foreach (var group in adjustedDateRecords.GroupBy(x => x.ProjectId))
        {
            var projects = group.ToList();

            for (int i = 0; i < projects.Count() - 1; i++)
            {
                for (int j = i + 1; j < projects.Count(); j++)
                {
                    var employeeOne = projects[i];
                    var employeeTwo = projects[j];
                    var daysWorkedTogether = GetDaysWorkedTogether(employeeOne.DateFrom, employeeOne.DateTo, employeeTwo.DateFrom, employeeTwo.DateTo);

                    if (daysWorkedTogether is 0)
                        continue;

                    projectPairs.Add(
                        new ProjectPair
                        {
                            EmployeeOneId = employeeOne.EmpId,
                            EmployeeTwoId = employeeTwo.EmpId,
                            ProjectId = employeeOne.ProjectId,
                            DaysWorked = daysWorkedTogether
                        });
                }
            }
        }

        var employeePair = projectPairs
            .GroupBy(x => new { x.EmployeeOneId, x.EmployeeTwoId })
            .OrderByDescending(x => x.Sum(x => x.DaysWorked))
            .Select(x => x.Key)
            .FirstOrDefault();

        return projectPairs
            .Where(x => x.EmployeeOneId == employeePair.EmployeeOneId && x.EmployeeTwoId == employeePair.EmployeeTwoId)
            .ToList();
    }

    private DateTime AdjustDate(string? date)
    {
        if (date == "NULL")
            return DateTime.UtcNow.Date;

        var isAdjusted = DateTime.TryParse(date, out DateTime adjustedDate);

        if (isAdjusted is false)
        {
            throw new ArgumentException("Date is not valid");
        }

        return adjustedDate.Date;
    }

    private int GetDaysWorkedTogether(DateTime employeeOneDateFrom, DateTime employeeOneDateTo, DateTime employeeTwoDateFrom, DateTime employeeTwoDateTo)
    {
        var startDate = employeeOneDateFrom >= employeeTwoDateFrom
            ? employeeOneDateFrom
            : employeeTwoDateFrom;

        var endDate = employeeOneDateTo <= employeeTwoDateTo
            ? employeeOneDateTo
            : employeeTwoDateTo;

        return startDate >= endDate
            ? 0
            : (endDate - startDate).Days;
    }

}
