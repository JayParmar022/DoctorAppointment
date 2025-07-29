using DoctorAppointment.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointment.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<DoctorMaster> doctorMasters { get; set; }
        public DbSet<Appointment> appointments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DoctorMaster>().HasData(
                new DoctorMaster
                {
                    Did = 1,
                    Name = "Mr Patel",
                    ChargeType = "Hourly",
                    Rate = 200,
                    SRate = 400,
                    AvailableDate = DateOnly.Parse("2025-01-10"),
                    FromTime = TimeSpan.Parse("05:00:00"),
                    ToTime = TimeSpan.Parse("06:00:00")
                },
                new DoctorMaster
                {
                    Did = 2,
                    Name = "Mr Shah",
                    ChargeType = "Fix",
                    Rate = 500,
                    SRate = 100,
                    AvailableDate = DateOnly.Parse("2025-01-10"),
                    FromTime = TimeSpan.Parse("10:00:00"),
                    ToTime = TimeSpan.Parse("15:00:00")
                },
                new DoctorMaster
                {
                    Did = 3,
                    Name = "Mr Sharma",
                    ChargeType = "Hourly",
                    Rate = 300,
                    SRate = 300,
                    AvailableDate = DateOnly.Parse("2025-01-10"),
                    FromTime = TimeSpan.Parse("10:00:00"),
                    ToTime = TimeSpan.Parse("15:00:00")
                }
            );
        }
    }
}