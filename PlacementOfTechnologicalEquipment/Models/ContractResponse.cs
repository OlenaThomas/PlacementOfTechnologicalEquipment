namespace PlacementOfTechnologicalEquipment.Models
{
    public class ContractResponse
    {
        public string contractNumber { get; set; }
        public int AmountOfEquipment { get; set; }
        public string ClientName { get; set; }
        public string TechnologicalMachineId { get; set; }
        public string ProductionRoomName { get; set; }
    }
}
