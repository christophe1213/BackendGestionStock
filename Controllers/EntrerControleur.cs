using Microsoft.AspNetCore.Mvc;
using Services.Entre;
using API.Models.Produit;
using Services.Produit;
using  API.Models.EntrerStock;
namespace Entrer.Controllers;

[ApiController]
[Route("[controller]")]
public class EntrerController : ControllerBase
{
    public EntrerController()
    {
    }

    // GET all action
    [HttpGet]
    public ActionResult<List<EntreStock>> GetAll() =>
    EntreService.GetAll();

    // GET by Id action
    //récuper qu'un seule element
    [HttpGet("{id}")]
    public ActionResult<EntreStock> Get(int id)
    {
        var index = EntreService.ListEntre.FindIndex(entrer => entrer.IdEntrer == id);
        if (index != -1)
        {
             return EntreService.ListEntre[index];
        }else{
            return NotFound();
        }
        
       
    }
    // POST action
    [HttpPost]
    //ilaina rehefa post methode axios
    public IActionResult Create(EntreStock es)
    {
        //Verification de clé primaire
        var i = EntreService.ListEntre.FindIndex(entrer => entrer.IdEntrer == es.IdEntrer);
        if (i!=-1)
        {                
            return BadRequest(new {message="duplication de clé primaire"});
        }else{
            //Mise à jour de produit
            var index = ProduitService.Produits.FindIndex(produit => produit.Codepro == es.Codepro);
            //Mise à jour du produit
            ProduitService.Produits[index].Qte_produit+=es.Quantite;
            EntreService.Create(es);
            return CreatedAtAction(nameof(GetAll), new { id = es.IdEntrer }, es);
        }
    }
    // // PUT actionrer
    // //ilaina rehefa put methode axios
    [HttpPut("{id}")]
    public IActionResult Update(int id, EntreStock es)
    {
        if (id != es.IdEntrer)
        {
           return BadRequest(new {message = "erreur de misse à jour"});
        }
        var existingClient = EntreService.GetAll().FirstOrDefault(entrer => entrer.IdEntrer == id);
        if (existingClient == null)
        {
            return NotFound();
        }
        
        var index = ProduitService.Produits.FindIndex(produit => produit.Codepro == es.Codepro);
        var indexEnt = EntreService.ListEntre.FindIndex(entrer =>entrer.IdEntrer == id);
        ProduitService.Produits[index].Qte_produit-=EntreService.ListEntre[indexEnt].Quantite-es.Quantite;
        EntreService.Update(es);

        return NoContent();
    }

    // // DELETE action
    // //ilaina rehefa delete methode axios
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existingClient = EntreService.GetAll().FirstOrDefault(entrer => entrer.IdEntrer == id);
        if (existingClient == null)
        {
            return NotFound();
        }
        var idEnt = EntreService.ListEntre.FindIndex(entrer => entrer.IdEntrer == id);
        var indexProduit = ProduitService.Produits.FindIndex(produit => produit.Codepro ==EntreService.ListEntre[idEnt].Codepro);

        if(ProduitService.Produits[indexProduit].Qte_produit-EntreService.ListEntre[idEnt].Quantite<0)
                return   BadRequest(new { message= "Vous ne pouvez pas supprimer ce entrer car elle est déja été commandé par un client \npour le supprimer il faut supprimer le commande de ce produit"});
        ProduitService.Produits[indexProduit].Qte_produit-=EntreService.ListEntre[idEnt].Quantite;
        EntreService.Delete(id);
        return NoContent();
    }
  

  
}