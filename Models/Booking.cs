using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternalResourceBookingSystem.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ResourceId { get; set; }
        [ForeignKey("ResourceId")]
        public Resource? Resource { get; set; }
        [Required]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        [Required]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }
        [Required]
        public string BookedBy { get; set; }
        [Required]
        public string purpose { get; set; }
    }
}
