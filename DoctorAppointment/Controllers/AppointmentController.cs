using Microsoft.AspNetCore.Mvc;
using DoctorAppointment.Models;
using DoctorAppointment.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using DoctorAppointment.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoctorAppointment.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AppointmentController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult GetAppointment()
        {
            var appointmentList = _db.appointments.Include(a=>a.DoctorMaster).ToList();

            return View(appointmentList);
        }
       
        public IActionResult Index()
        {
            return View();
        }
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var appointments = _db.appointments
                .Include(a => a.DoctorMaster)
                .Select(a => new {
                    aId = a.AId,
                    pname = a.Pname,
                    email = a.Email,
                    doctorMaster = new { name = a.DoctorMaster.Name }, 
                    date = a.Date,
                    fromtime = a.Fromtime,
                    totime = a.Totime,
                    totalCharge = a.TotalCharge,
                    gst = a.GST,
                    serviceCharge = a.ServiceCharge,
                    totalAmount = a.TotalAmount
                }).ToList();
            Console.WriteLine($"Appointments Found: {appointments.Count}");
            return Json(new { data = appointments });
        }

        #endregion
    }
}
