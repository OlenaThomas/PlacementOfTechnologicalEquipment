using Microsoft.EntityFrameworkCore;
using placementOfTechnologicalEquipment.Models;
using PlacementOfTechnologicalEquipment.Controllers;
using PlacementOfTechnologicalEquipment.Models;

namespace PlacementOfTechnologicalEquipment.Services
{
    public class ContractService: IContractService
    {
        private ApplicationContext db;
        public ContractService(ApplicationContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<ContractResponse>> GetListOfContracts()
        {
            var contracts = await db.Contracts.ToListAsync();
            var productionRooms = await db.ProductionRooms.ToListAsync();
            var technologicalMachines = await db.TechnologicalMachines.ToListAsync();
            var clients = await db.Clients.ToListAsync();

            var resultListContracts = from p in contracts
                                      join c in productionRooms on p.ProductionRoomId equals c.Id
                                      join d in technologicalMachines on p.TechnologicalMachineId equals d.Id
                                      join l in clients on p.ClientId equals l.Id
                                      select new ContractResponse
                                      {
                                          contractNumber = p.Number,
                                          AmountOfEquipment = p.AmountOfEquipment,
                                          ClientName = l.Name,
                                          TechnologicalMachineId = d.Name,
                                          ProductionRoomName = c.Name
                                      };

            return resultListContracts;
        }

        public int GetOccupiedSpaceOfRoom(List<TechnologicalMachine> technologicalMachines,
            List<Contract> ListOfContractsForThisRoom)
        {
            var AmountOfEquipmentWithYardage = from p in ListOfContractsForThisRoom
                    join c in technologicalMachines on p.TechnologicalMachineId equals c.Id
                    select new { p.AmountOfEquipment, c.Yardage };

            int sum = 0;
            foreach (var item in AmountOfEquipmentWithYardage)
            {
                sum += item.AmountOfEquipment * item.Yardage;
            }

            return sum;
        }

        public async Task<int> CheckSpaceToPlaceMachines(AddContractRequest request)
        {
            var productionRoom = await db.ProductionRooms.FirstOrDefaultAsync(x => x.Code.Equals(request.ProductionRoomCode));
            var ListOfContractsForThisRoom = await db.Contracts
                .Where(p => p.ProductionRoomId == productionRoom.Id).ToListAsync();

            List<TechnologicalMachine> technologicalMachines;
            int occupiedSpaceOfRoom = 0;

            if (ListOfContractsForThisRoom.Any())
            {
                technologicalMachines = await db.TechnologicalMachines.ToListAsync();
                occupiedSpaceOfRoom = GetOccupiedSpaceOfRoom(technologicalMachines, ListOfContractsForThisRoom);
            }

            var technologicalMachineToPlace = await db.TechnologicalMachines
                .FirstOrDefaultAsync(x => x.Code.Equals(request.TechnologicalMachineCode));

            var PlaceAfterPlacingEquipment = GetSpaceToPlaceMachines(occupiedSpaceOfRoom,
                productionRoom.Yardage, request.AmountOfEquipment, technologicalMachineToPlace.Yardage);

            return PlaceAfterPlacingEquipment;
        }

        public async Task<Contract> GetNewContract(AddContractRequest request)
        {
            var technologicalMachineToPlace = await db.TechnologicalMachines
               .FirstOrDefaultAsync(x => x.Code.Equals(request.TechnologicalMachineCode));
            var client = await db.Clients.FirstOrDefaultAsync(x => x.Name.Equals(request.ClientName));
            var productionRoom = await db.ProductionRooms.FirstOrDefaultAsync(x => x.Code.Equals(request.ProductionRoomCode));

            if(technologicalMachineToPlace != null && client != null && productionRoom != null)
            {
                var contract = new Contract
                {
                    Number = request.Number,
                    AmountOfEquipment = request.AmountOfEquipment,
                    ClientId = client.Id,
                    TechnologicalMachineId = technologicalMachineToPlace.Id,
                    ProductionRoomId = productionRoom.Id
                };

                return contract;
            }

            return new Contract();
        }
        
        public async Task<Dictionary<string,string>> ValidateRequest(AddContractRequest request)
        {
            //TODO Implement FluentValidation
            var resultValidationRequest = new Dictionary<string,string>();

            if (!await IsValidRequestNumber(request.Number))
            {
                resultValidationRequest.Add("Number",
                    $"Contract number {request.Number} exists.Enter a different contract number.");
            }

            if (!await IsValidRequestClientName(request.ClientName))
            {
                resultValidationRequest.Add("ClientName",
                    $"Client {request.ClientName} does not exist. Add a customer to the Clients table.");
            }

            if (!await IsValidRequestTechnologicalMachineCode(request.TechnologicalMachineCode))
            {
                resultValidationRequest.Add("TechnologicalMachineCode",
                    $"TechnologicalMachineCode {request.TechnologicalMachineCode} does not exist." +
                    " Add a machine to the TechnologicalMachines table.");
            }

            if (!await IsValidRequestProductionRoomCode(request.ProductionRoomCode))
            {
                resultValidationRequest.Add("ProductionRoomCode",
                    $"ProductionRoomCode {request.ProductionRoomCode} does not exist." +
                    " Add a production room to the ProductionRoom table.");
            }

            return resultValidationRequest;
        }

        public int GetSpaceToPlaceMachines(int occupiedSpaceOfRoom, int productionRoomYardage,
            int requestAmountOfEquipment, int technologicalMachinesYardage) => 
            productionRoomYardage - occupiedSpaceOfRoom - requestAmountOfEquipment * technologicalMachinesYardage;

        public async Task<bool> IsValidRequestNumber(string requestNumber) =>
             await db.Contracts.FirstOrDefaultAsync(x => x.Number.Equals(requestNumber)) == null;

        public async Task<bool> IsValidRequestClientName(string requestClientName) =>
             await db.Clients.FirstOrDefaultAsync(x => x.Name.Equals(requestClientName)) != null;

        public async Task<bool> IsValidRequestTechnologicalMachineCode(string requestTechnologicalMachineCode) => 
            await db.TechnologicalMachines
            .FirstOrDefaultAsync(x => x.Code.Equals(requestTechnologicalMachineCode)) != null;

        public async Task<bool> IsValidRequestProductionRoomCode(string requestProductionRoomCode) =>
             await db.ProductionRooms.FirstOrDefaultAsync(x => x.Code.Equals(requestProductionRoomCode)) != null;

    }
}
