using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DoctorAppointment.Migrations
{
    /// <inheritdoc />
    public partial class newBd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "doctorMasters",
                columns: table => new
                {
                    Did = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChargeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    SRate = table.Column<double>(type: "float", nullable: false),
                    AvailableDate = table.Column<DateOnly>(type: "date", nullable: false),
                    FromTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ToTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doctorMasters", x => x.Did);
                });

            migrationBuilder.CreateTable(
                name: "appointments",
                columns: table => new
                {
                    AId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Fromtime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Totime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Did = table.Column<int>(type: "int", nullable: false),
                    TotalCharge = table.Column<int>(type: "int", nullable: false),
                    GST = table.Column<double>(type: "float", nullable: false),
                    ServiceCharge = table.Column<double>(type: "float", nullable: false),
                    TotalAmount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointments", x => x.AId);
                    table.ForeignKey(
                        name: "FK_appointments_doctorMasters_Did",
                        column: x => x.Did,
                        principalTable: "doctorMasters",
                        principalColumn: "Did",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "doctorMasters",
                columns: new[] { "Did", "AvailableDate", "ChargeType", "FromTime", "Name", "Rate", "SRate", "ToTime" },
                values: new object[,]
                {
                    { 1, new DateOnly(2025, 1, 10), "Hourly", new TimeSpan(0, 5, 0, 0, 0), "Mr Patel", 200.0, 400.0, new TimeSpan(0, 6, 0, 0, 0) },
                    { 2, new DateOnly(2025, 1, 10), "Fix", new TimeSpan(0, 10, 0, 0, 0), "Mr Shah", 500.0, 100.0, new TimeSpan(0, 15, 0, 0, 0) },
                    { 3, new DateOnly(2025, 1, 10), "Hourly", new TimeSpan(0, 10, 0, 0, 0), "Mr Sharma", 300.0, 300.0, new TimeSpan(0, 15, 0, 0, 0) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_appointments_Did",
                table: "appointments",
                column: "Did");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointments");

            migrationBuilder.DropTable(
                name: "doctorMasters");
        }
    }
}
