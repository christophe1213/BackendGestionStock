using Microsoft.AspNetCore.Mvc;
using Services.NombreTotal;
using API.Models.NombreTotal;

namespace NombreTotal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NombreTotalController : ControllerBase
    {
        // Action GET all
        [HttpGet]
        public ActionResult<List<NombreStock>> GetAll() =>
            NombreTotalService.GetAll(); // Utilisez la liste des stocks totaux
    }
}

