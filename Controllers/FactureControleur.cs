using Microsoft.AspNetCore.Mvc;
using Services.Facture;
using API.Models.Facture;

namespace Facture.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FactureController : ControllerBase
    {
        public FactureController()
        {
        }

        // GET all action
       [HttpGet]
        public ActionResult<List<FactureClient>> GetAll() =>
        FactureService.GetAll(); // Utilisez la liste statique Factures pour obtenir toutes les factures
    }
}
