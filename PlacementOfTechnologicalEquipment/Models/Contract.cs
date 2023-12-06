namespace placementOfTechnologicalEquipment.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int AmountOfEquipment { get; set; }
        public int ClientId { get; set; }
        public int TechnologicalMachineId { get; set; }
        public int ProductionRoomId { get; set; }
    }
}
