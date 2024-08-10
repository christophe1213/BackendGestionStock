using Microsoft.AspNetCore.Mvc;
using Services.client;
using API.Models.client;
using API.Models.Produit;
using Services.Produit;
namespace Produit.Controllers;

[ApiController]
[Route("[controller]")]
public class ProduitController : ControllerBase
{
    public ProduitController()
    {
    }
    [HttpGet]
    public ActionResult<List<ProduitStock>> GetAll() =>
    ProduitService.GetAll();

    //récuper qu'un seule element
    [HttpGet("{id}")]
    public ActionResult<ProduitStock> Get(string id)
    {
    
        var index = ProduitService.Produits.FindIndex(produit => produit.Codepro == id);
        if (index != -1)
        {
            return ProduitService.Produits[index];
        }else{
            return NotFound(new { message= "erreur 404, produit id="+id+" non trouvé"});
        }

    }
    [HttpPost]
    //ilaina rehefa post methode axios
    public IActionResult Create(ProduitStock p)
    {
        var index = ProduitService.Produits.FindIndex(produit => produit.Codepro == p.Codepro);
        if (index!=-1) return BadRequest(new { message= "Dupplication de clé primaire "});
        ProduitService.Create(p);
        return CreatedAtAction(nameof(GetAll), new { id = p.Codepro }, p);
    }

    [HttpPut("{id}")]
    public IActionResult Update(string id, ProduitStock p)
    {
        if (id != p.Codepro)
        {
            return BadRequest();
        }

        var existingClient = ProduitService.GetAll().FirstOrDefault(produit => produit.Codepro == id);
        if (existingClient == null)
        {
            return NotFound();
        }

            ProduitService.Update(p);

            return NoContent();
    }
    //ilaina rehefa delete methode axios
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
     {
        var existingClient = ProduitService.GetAll().FirstOrDefault(produit => produit.Codepro == id);
            if (existingClient == null)
            {
                return NotFound();
            }

            ProduitService.Delete(id);

            return NoContent();
        }
    
}