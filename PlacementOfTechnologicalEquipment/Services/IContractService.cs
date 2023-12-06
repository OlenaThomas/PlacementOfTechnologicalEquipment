using placementOfTechnologicalEquipment.Models;
using PlacementOfTechnologicalEquipment.Controllers;
using PlacementOfTechnologicalEquipment.Models;

namespace PlacementOfTechnologicalEquipment.Services
{
    public interface IContractService
    {
        Task<IEnumerable<ContractResponse>> GetListOfContracts();
        int GetOccupiedSpaceOfRoom(List<TechnologicalMachine> technologicalMachines,
            List<Contract> ListOfContractsForThisRoom);
        Task<int> CheckSpaceToPlaceMachines(AddContractRequest request);
        Task<Contract> GetNewContract(AddContractRequest request);
        Task<Dictionary<string, string>> ValidateRequest(AddContractRequest request);
        int GetSpaceToPlaceMachines(int occupiedSpaceOfRoom, int productionRoomYardage,
            int requestAmountOfEquipment, int technologicalMachinesYardage);
        Task<bool> IsValidRequestNumber(string requestNumber);
        Task<bool> IsValidRequestClientName(string requestClientName);
        Task<bool> IsValidRequestTechnologicalMachineCode(string requestTechnologicalMachineCode);
        Task<bool> IsValidRequestProductionRoomCode(string requestProductionRoomCode);
    }
}
