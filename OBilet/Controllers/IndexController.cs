using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OBilet.Services;
using RestSharp;

namespace OBilet.Controllers;

public class IndexController : Controller
{
    private readonly RestClient _restClient;

    public IndexController(RestClient restClient)
    {
        _restClient = restClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        BusLocationsService busLocationsService =
            new BusLocationsService(new Logger<BusLocationsService>(new LoggerFactory()), _restClient);

        var busLocationsList = await busLocationsService.GetBusLocations();

        //ViewBag.BusLocations = new SelectList(busLocationsList.Data!, "Id", "Name");

        ViewBag.BusLocations = busLocationsList.Data!.Select(x => new SelectListItem()
        {
            Text = x.Name,
            Value = x.Id.ToString()
        });
        return View(busLocationsList.Data!);
    }
}