using Microsoft.AspNetCore.Mvc;
using placementOfTechnologicalEquipment.Models;
using PlacementOfTechnologicalEquipment.Models;

namespace PlacementOfTechnologicalEquipment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private ApplicationContext db;
        public ClientController(ApplicationContext context)
        {
            db = context;
        }

        [HttpPost]
        public async Task<ActionResult<Client>> Post(Client client)
        {
            db.Clients.Add(client);
            await db.SaveChangesAsync();
            return Ok(client);
        }
    }
}
