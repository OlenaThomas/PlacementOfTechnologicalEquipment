using System.ComponentModel.DataAnnotations;

namespace placementOfTechnologicalEquipment.Models
{
    public class ProductionRoom
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter production room code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Enter production room name")]
        public string Name { get; set; }
        [Range(100, 1000, ErrorMessage = "Area must be between 100 and 1000")]
        [Required(ErrorMessage = "Enter area")]
        public int Yardage { get; set; }
    }
}

