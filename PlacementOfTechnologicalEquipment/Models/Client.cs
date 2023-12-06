using System.ComponentModel.DataAnnotations;

namespace placementOfTechnologicalEquipment.Models
{
    public class Client
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter customer name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter your address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Enter identification code")]
        public string IdentificationСode { get; set; }
    }
}
