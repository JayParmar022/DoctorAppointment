using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Numerics;
using DoctorAppointment.Data;
using DoctorAppointment.Models;
using DoctorAppointment.Models.ViewModel;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {

            return View();

        }
        [HttpPost]
        public IActionResult Index(AppointmentVM appointmentVM)
        {
            if (ModelState.IsValid)
            {
                var check = _db.appointments.Where(x => x.Did == appointmentVM.Appointment.Did &&
                            x.Date == appointmentVM.Appointment.Date).ToList();
                var f = 0;
                foreach (var a in check)
                {
                    if ((a.Fromtime <= appointmentVM.Appointment.Fromtime && a.Totime > appointmentVM.Appointment.Fromtime) ||
                            (a.Fromtime < appointmentVM.Appointment.Totime && a.Totime >= appointmentVM.Appointment.Totime))
                    {
                        f = 1;
                    }
                }


                if (f == 1)
                {
                    TempData["MessageError"] = "This Date And Time is Already Book!!";
                    return RedirectToAction("Index");
                }


                Appointment appointment = new Appointment
                {
                    Pname = appointmentVM.Appointment.Pname,
                    Email = appointmentVM.Appointment.Email,
                    Date = appointmentVM.Appointment.Date,
                    Fromtime = appointmentVM.Appointment.Fromtime,
                    Totime = appointmentVM.Appointment.Totime,
                    Did = appointmentVM.Appointment.Did,
                    TotalCharge = appointmentVM.Appointment.TotalCharge,
                    GST = appointmentVM.Appointment.GST,
                    ServiceCharge = appointmentVM.Appointment.ServiceCharge,
                    TotalAmount = appointmentVM.Appointment.TotalAmount
                };
                _db.appointments.Add(appointment);
                _db.SaveChanges();

                TempData["Message"] = "Appointment booked";
                return RedirectToAction("Index");
            }

            return View(appointmentVM);
        }

        public IActionResult GetAvailableDoctors(string fromTime, string toTime)
        {
            try
            {
                var from = TimeSpan.ParseExact(fromTime, @"hh\:mm", CultureInfo.InvariantCulture);
                var to = TimeSpan.ParseExact(toTime, @"hh\:mm", CultureInfo.InvariantCulture);


                if (from < to)
                {
                    var availableDoctors = _db.doctorMasters
                    .Where(d => d.FromTime <= from && d.ToTime >= to)
                    .ToList();



                    if (availableDoctors.Any())
                    {
                        return Json(availableDoctors.Select(d => new { d.Did, d.Name }));
                    }
                    else
                    {
                        var doctor = _db.doctorMasters
                                .AsEnumerable() // Fetches data in memory before applying calculations
                                .OrderBy(d => (from - d.FromTime).Duration())
                                .FirstOrDefault();

                        var FromtimeDb = doctor?.FromTime;
                        var ToTimeDb = doctor?.ToTime;

                        DateTime ConvertFromTime = DateTime.Today.Add(FromtimeDb.GetValueOrDefault());
                        DateTime ConvertToTime = DateTime.Today.Add(ToTimeDb.GetValueOrDefault());

                        string nearestFromTime = ConvertFromTime.ToString("hh:mm tt");
                        string nearestToTime = ConvertToTime.ToString("hh:mm tt");

                        return Json(new { doctor?.Did, doctor?.Name, FromTime = nearestFromTime, ToTime = nearestToTime });
                    }
                }
                else
                {
                    return Json(new { error = "Enter valid time" });
                }


            }
            catch (Exception e)
            {
                return Json(new { error = e });
            }
        }
        public IActionResult GetDoctorCharge(int doctorId, DateOnly appointmentDate, string fromTime, string toTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // or return View(), as per your use case
            }
            try
            {
                var from = TimeSpan.ParseExact(fromTime, @"hh\:mm", CultureInfo.InvariantCulture);
                var to = TimeSpan.ParseExact(toTime, @"hh\:mm", CultureInfo.InvariantCulture);

                var id = doctorId;

                var availableDoctors = _db.doctorMasters.FirstOrDefault(x => x.Did == id);
                if (availableDoctors == null)
                {
                    return Json(new { error = "Doctor not found." });
                }
                bool sunday = appointmentDate.DayOfWeek == DayOfWeek.Sunday;
                var calculatedCharge = 0.0;
                if (sunday)
                {
                    if (availableDoctors.ChargeType == "Hourly")
                    {
                        var hours = (to - from).TotalHours;
                        calculatedCharge += hours * availableDoctors.SRate;
                    }
                    else if (availableDoctors.ChargeType == "Fix")
                    {
                        calculatedCharge += availableDoctors.SRate;
                    }
                }
                else
                {
                    if (availableDoctors.ChargeType == "Hourly")
                    {
                        var hours = (to - from).TotalHours;
                        calculatedCharge += hours * availableDoctors.Rate;
                    }
                    else if (availableDoctors.ChargeType == "Fix")
                    {
                        calculatedCharge += availableDoctors.Rate;
                    }
                }
                return Json(calculatedCharge);
            }
            catch (Exception)
            {
                return Json(new { error = "An error occurred while fetching available doctors." });
            }

        }

       


       
    }
}
