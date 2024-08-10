using Microsoft.AspNetCore.Mvc;
using Services.suviStock;
using API.Models.FicheStock;

namespace Entrer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FicheStockController : ControllerBase
    {
        public FicheStockController() { }

        [HttpGet]
        public ActionResult<List<SuiviStock>> GetAll() => ServicesFicheStock.GetAll();
    }
} 