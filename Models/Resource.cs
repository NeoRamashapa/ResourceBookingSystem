using System.ComponentModel.DataAnnotations;

namespace InternalResourceBookingSystem.Models
{
    public class Resource
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name cannot be empty")]
        public string? Name { get; set; } 
        public string? Description { get; set; }
        public string? Location { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be at least 1")]
        public int? Capacity { get; set; }
        public bool IsAvailable { get; set; }

        public ICollection<Booking>? Bookings { get; set; }

    }
}
