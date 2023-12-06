using System.ComponentModel.DataAnnotations;

namespace placementOfTechnologicalEquipment.Models
{
    public class TechnologicalMachine
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter equipment code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Enter the name of the equipment")]
        public string Name { get; set; }
        [Range(1, 100, ErrorMessage = "Area must be between 1 and 100")]
        [Required(ErrorMessage = "Enter area")]
        public int Yardage { get; set; }
    }
}
