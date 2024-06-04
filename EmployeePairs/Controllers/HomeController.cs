using CsvHelper;
using CsvHelper.Configuration;
using EmployeePairs.Models;
using EmployeePairs.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace EmployeePairs.Controllers;

public class HomeController : Controller
{
    private readonly EmployeePairService _employeePairService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(EmployeePairService employeePairService, ILogger<HomeController> logger)
    {
        _employeePairService = employeePairService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View("UploadCsv");
    }

    [HttpGet]
    public IActionResult GetLongestPairedEmployees()
    {
        return View("UploadCsv");
    }

    [HttpPost]
    public IActionResult GetLongestPairedEmployees(IFormFile csvFile)
    {
        if (csvFile == null || csvFile.Length == 0)
        {
            ModelState.AddModelError("File", "Please upload a CSV file.");
            return View("UploadCsv");
        }

        var records = new List<EmployeeProjectDto>();
        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false
        };

        using (var reader = new StreamReader(csvFile.OpenReadStream()))
        using (var csvReader = new CsvReader(reader, csvConfiguration))
        {
            csvReader.Read();
            records = csvReader.GetRecords<EmployeeProjectDto>().ToList();
        }

        var employeePair = _employeePairService.GetLongestPairedEmployees(records);

        return View(employeePair);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
