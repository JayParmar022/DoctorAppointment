using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoctorAppointment.Models.ViewModel
{
    public class AppointmentVM
    {
        public Appointment Appointment { get; set; }
        [ValidateNever]
        public List<SelectListItem> DoctorList { get; set; }
    }
}
