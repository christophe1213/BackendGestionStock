using Microsoft.AspNetCore.Mvc;
using API.Models.Commande;
using API.Models.Produit;
using Services.Produit;
using Services.Commande;
namespace Commande.Controllers;

[ApiController]
[Route("[controller]")]
public class CommandeController : ControllerBase
{
    public CommandeController()
    {
    }

    // GET all action
    [HttpGet]
    public ActionResult<List<CommandeStock>> GetAll() =>
    CommandeService.GetAll();

    // GET by Id action
    //récuper qu'un seule element
    [HttpGet("{id}")]
    public ActionResult<CommandeStock> Get(int id)
    {
        // Chercher l'index
        var index = CommandeService.Commandes.FindIndex(commande => commande.Idcommande == id);
        if (index != -1)
        {
            return CommandeService.Commandes[index]; 
        }else{
            return NotFound();
        }
    }
    // POST action
    [HttpPost]
    //ilaina rehefa post methode axios
    public IActionResult Create(CommandeStock cs)
    {
       
        var i = CommandeService.Commandes.FindIndex(commande => commande.Idcommande == cs.Idcommande);
        if(i!=-1)return BadRequest(new { message="dupplication de clé primaire" });
        var index = ProduitService.Produits.FindIndex(produit => produit.Codepro == cs.Codepro);
        //Verifier si la quantité de produit commandé par le client est trop grand pour le produit
        if(ProduitService.Produits[index].Qte_produit-cs.Quantite<0)
        {
            return BadRequest(new { message = "Nombre de prouduit en  stock insuiffissant pour effectuer ce commande"});
        
        }else{
            ProduitService.Produits[index].Qte_produit-=cs.Quantite;
            CommandeService.Create(cs);
            return CreatedAtAction(nameof(GetAll), new { id = cs.Idcommande }, cs);
        }

    }
    // PUT action
    //ilaina rehefa put methode axios
    [HttpPut("{id}")]
    public IActionResult Update(int id, CommandeStock cs)
    {
        if (id != cs.Idcommande)
        {
           return BadRequest();
        }
        var existingClient = CommandeService.GetAll().FirstOrDefault(commande => commande.Idcommande == id);
        if (existingClient == null)
            {
                return NotFound();
            }
        
        var index = ProduitService.Produits.FindIndex(produit => produit.Codepro == cs.Codepro);
        var indexCommande = CommandeService.Commandes.FindIndex(commande => commande.Idcommande == id);
        ProduitService.Produits[index].Qte_produit+=CommandeService.Commandes[indexCommande].Quantite-cs.Quantite;
        CommandeService.Update(cs);

            return NoContent();
        }

    // DELETE action
    //ilaina rehefa delete methode axios
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existingClient = CommandeService.GetAll().FirstOrDefault(commande => commande.Idcommande == id);
            if (existingClient == null)
            {
                return NotFound();
            }
            var indexCommande = CommandeService.Commandes.FindIndex(commande => commande.Idcommande == id);
            var indexProduit = ProduitService.Produits.FindIndex(produit => produit.Codepro == CommandeService.Commandes[indexCommande].Codepro);

            ProduitService.Produits[indexProduit].Qte_produit+=CommandeService.Commandes[indexCommande].Quantite;
            CommandeService.Delete(id);
          
            return NoContent();
        }
  
}