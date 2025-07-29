
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorAppointment.Models
{
    public class Appointment
    {
        [Key]
        public int AId { get; set; }
        [Required]
        public string Pname { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public TimeSpan Fromtime { get; set; }
        [Required]
        public TimeSpan Totime { get; set; }
        [Required]
        public int Did { get; set; }

        [ForeignKey("Did")]
        [ValidateNever]
        public DoctorMaster DoctorMaster { get; set; }
        [Required]
        public int TotalCharge {  get; set; }
        [Required]
        public Double GST {  get; set; }
        [Required]
        public Double ServiceCharge { get; set; }
        [Required]
        public Double TotalAmount { get; set; }

    }
}
