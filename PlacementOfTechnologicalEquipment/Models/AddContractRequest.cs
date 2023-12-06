using System.ComponentModel.DataAnnotations;

namespace PlacementOfTechnologicalEquipment.Controllers
{
    public class AddContractRequest
    {
        [Required(ErrorMessage = "Enter contract number")]
        public string Number { get; set; }
        [Range(1, 100, ErrorMessage = "The number of equipment must be between 1 and 100")]
        [Required(ErrorMessage = "Enter number of equipment")]
        public int AmountOfEquipment { get; set; }
        [Required(ErrorMessage = "Enter customer name")]
        public string ClientName { get; set; }
        [Required(ErrorMessage = "Enter equipment code")]
        public string TechnologicalMachineCode { get; set; }
        [Required(ErrorMessage = "Enter room code")]
        public string ProductionRoomCode { get; set; }
    }
}
