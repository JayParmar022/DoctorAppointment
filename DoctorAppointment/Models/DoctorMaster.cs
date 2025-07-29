using System.ComponentModel.DataAnnotations;

namespace DoctorAppointment.Models
{
    public class DoctorMaster
    {
        [Key]
        public int Did { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ChargeType { get; set; }
        [Required]
        public Double Rate { get; set; }
        [Required]
        public Double SRate { get; set; }
        [Required]
        public DateOnly AvailableDate { get; set; }
        [Required]
        public TimeSpan FromTime { get; set; }
        [Required]
        public TimeSpan ToTime { get; set; }
    }
}
