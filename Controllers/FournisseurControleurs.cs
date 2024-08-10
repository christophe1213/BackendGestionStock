using Microsoft.AspNetCore.Mvc;
using API.Models.Fournisseur;
using Services.Fournisseur;
namespace Fournisseur.Controllers;

[ApiController]
[Route("[controller]")]
public class FournisseurController : ControllerBase
{
    public FournisseurController()
    {
    }

    // GET all action
    [HttpGet]
    public ActionResult<List<FournisseurStock>> GetAll() =>
    FournisseurService.GetAll();


    // GET by Id action
   [HttpGet("{id}")]
    public ActionResult<FournisseurStock> Get(string id)
    {
        // var existingClient = FournisseurService.GetAll().FirstOrDefault(fournisseur => fournisseur.NumImm == id);
        // if (existingClient == null)
        // {
        //     return NotFound();
        // }
        // var fournisseur = FournisseurService.Get(id);

        // if(fournisseur == null)
        //     return NotFound();

        // return fournisseur;
        var index = FournisseurService.Fournisseurs.FindIndex(fournisseur => fournisseur.NumImm == id);
        if (index == -1)
        {
            return NotFound(new {message="erreur 404, fournisseur id="+id+" non trouvée"});
        }
        return FournisseurService.Fournisseurs[index];
    }

    // POST action

  [HttpPost]
    public IActionResult Create(FournisseurStock f)
    {
        
        var index = FournisseurService.Fournisseurs.FindIndex(fournisseur => fournisseur.NumImm == f.NumImm);
        if (index != -1)
        {
            return BadRequest(new {message="duplication de clé primaire"});
        }
        FournisseurService.Create(f);
        return CreatedAtAction(nameof(Get), new { id = f.NumImm }, f);            
        // This code will save the pizza and return a result
    }
    // PUT action
   [HttpPut("{id}")]
    public IActionResult Update(string id, FournisseurStock f)
    {
        if (id != f.NumImm)
        {
             return BadRequest();
        }

        var existingClient = FournisseurService.GetAll().FirstOrDefault(fournisseur => fournisseur.NumImm == id);
        if (existingClient == null)
        {
            return NotFound();
        }

        FournisseurService.Update(f);

        return NoContent();
    }
    // DELETE action 
 
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var existingClient = FournisseurService.GetAll().FirstOrDefault(fournisseur => fournisseur.NumImm == id);
        if (existingClient == null)
        {
            return NotFound();
        }

        FournisseurService.Delete(id);

        return NoContent();
    }    
}