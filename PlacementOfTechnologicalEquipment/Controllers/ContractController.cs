using Microsoft.AspNetCore.Mvc;
using placementOfTechnologicalEquipment.Models;
using PlacementOfTechnologicalEquipment.Models;
using PlacementOfTechnologicalEquipment.Services;
using Contract = placementOfTechnologicalEquipment.Models.Contract;

namespace PlacementOfTechnologicalEquipment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractService contractService;

        private ApplicationContext db;
        public ContractController(ApplicationContext context, IContractService contractService)
        {
            db = context;
            this.contractService = contractService;

            if (!db.Clients.Any())
            {
                db.Clients.Add(new Client { Name = "Trust Technology", Address = "Kiev, Sirenevaya st., 15 office 3", IdentificationСode = "0058974" });
                db.Clients.Add(new Client { Name = "MASTERPRODUCT", Address = "Kiev, Zelenaya st., 33", IdentificationСode = "8896574" });
                db.Clients.Add(new Client { Name = "COMBINEZONE", Address = "Lviv, Chestnut street, 1", IdentificationСode = "5896330" });
                db.Clients.Add(new Client { Name = "ZoneKey", Address = "Dnipro, st. Shirokaya, 88", IdentificationСode = "8563247" });
                db.SaveChanges();
            }

            if (!db.TechnologicalMachines.Any())
            {
                db.TechnologicalMachines.Add(new TechnologicalMachine { Code = "gth523", Name = "machine2560", Yardage = 20 });
                db.TechnologicalMachines.Add(new TechnologicalMachine { Code = "ytuj02j", Name = "machine2565", Yardage = 15 });
                db.TechnologicalMachines.Add(new TechnologicalMachine { Code = "Rtg859", Name = "machine2588", Yardage = 10 });
                db.TechnologicalMachines.Add(new TechnologicalMachine { Code = "Wpo5yt", Name = "machine3348", Yardage = 18 });
                db.SaveChanges();
            }

            if (!db.ProductionRooms.Any())
            {
                db.ProductionRooms.Add(new ProductionRoom { Code = "KS15", Name = "main", Yardage = 100 });
                db.ProductionRooms.Add(new ProductionRoom { Code = "KD20", Name = "Kiev20", Yardage = 2500 });
                db.ProductionRooms.Add(new ProductionRoom { Code = "DD63", Name = "Dnipro63", Yardage = 1600 });
                db.SaveChanges();
            }

            if (!db.Contracts.Any())
            {
                db.Contracts.Add(new Contract { Number = "TST202302011", AmountOfEquipment = 2, ClientId = 1, TechnologicalMachineId = 1, ProductionRoomId = 1 });
                db.Contracts.Add(new Contract { Number = "TST202302022", AmountOfEquipment = 2, ClientId = 2, TechnologicalMachineId = 2, ProductionRoomId = 2 });
                db.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<IEnumerable<ContractResponse>> Get() =>
            await contractService.GetListOfContracts();
        

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AddContractRequest request)
        {
            var resultValidationRequest = await contractService.ValidateRequest(request);

            foreach (var item in resultValidationRequest)
            {
                ModelState.AddModelError(item.Key, item.Value);
            }

            if (ModelState.Count > 0)
            {
                return BadRequest(ModelState);
            }

            var PlaceAfterPlacingEquipment = await contractService.CheckSpaceToPlaceMachines(request);

            if (PlaceAfterPlacingEquipment < 0)
            {
                ModelState.AddModelError("Yardage",
                    "There is not enough space in the room to accommodate the equipment." +
                    $" Lack {PlaceAfterPlacingEquipment*(-1)} square meters to accommodate equipment.");
                return BadRequest(ModelState);
            }

            var contract = await contractService.GetNewContract(request);

            if(contract == null)
            {
                ModelState.AddModelError("Request",
                    "Something went wrong, please try again.");
                return BadRequest(ModelState);
            }

            db.Contracts.Add(contract);
            db.SaveChanges();
            return Ok();
        }
    }
}
