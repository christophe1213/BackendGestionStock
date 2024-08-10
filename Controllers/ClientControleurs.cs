using Microsoft.AspNetCore.Mvc;
using Services.client;
using API.Models.client;
namespace Client.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    public ClientController()
    {
    }

    // GET all action
    [HttpGet]
    public ActionResult<List<ClientStock>> GetAll() =>
    ClientService.GetAll();
    // GET by Id action
    [HttpGet("{id}")]
    public ActionResult<ClientStock> Get(string id)
    {
        // ClientStock clientUpdate = ClientService.Get(id);

        // if(clientUpdate == null)
        //     return NotFound();
        var index = ClientService.Clients.FindIndex(client => client.idclient == id);
        if (index == -1)
        {
            return NotFound(new {message="Erreur 404 client id="+id+" non trouvée"}); 
        }
        else return ClientService.Clients[index];

        // return clientUpdate;
    }

    // POST action
   [HttpPost]
    //ilaina rehefa post methode axios
    public IActionResult Create(ClientStock c)
    {
        var index = ClientService.Clients.FindIndex(client => client.idclient == c.idclient);
        if(index!=-1)return BadRequest(new {message="dupplication de clé primaire"});
        ClientService.Create(c);
        return CreatedAtAction(nameof(GetAll), new { id = c.idclient }, c);
    }
    //ilaina rehefa put methode axios
    [HttpPut("{id}")]
        public IActionResult Update(string id, ClientStock c)
        {
            if (id != c.idclient)
            {
                return BadRequest();
            }

            var existingClient = ClientService.GetAll().FirstOrDefault(client => client.idclient == id);
            if (existingClient == null)
            {
                return NotFound();
            }

            ClientService.Update(c);

            return NoContent();
        }
    // PUT action

    // DELETE action
//ilaina rehefa delete methode axios
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var existingClient = ClientService.GetAll().FirstOrDefault(client => client.idclient == id);
            if (existingClient == null)
            {
                return NotFound();
            }

            ClientService.Delete(id);

            return NoContent();
        }

    
}