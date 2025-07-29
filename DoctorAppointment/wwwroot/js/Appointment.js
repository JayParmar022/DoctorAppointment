

document.addEventListener("DOMContentLoaded", function () {
    loadDataTable();  // Load DataTable
    loadPieChart();   // Load Pie Chart
    loadDateChart();
});

// ✅ Load DataTable
function loadDataTable() {
    if ($.fn.DataTable.isDataTable("#tblData")) {
        $('#tblData').DataTable().destroy(); // Destroy old instance before reloading
    }

    $('#tblData').DataTable({
        "ajax": {
            url: 'https://localhost:7092/Appointment/GetAll',
            type: "GET",
            datatype: "json"
        },
        "columns": [
            { data: 'aId', "width": "10%", title: "ID" },
            { data: 'pname', "width": "15%", title: "Patient Name" },
            { data: 'email', "width": "15%", title: "Email" },
            { data: 'doctorMaster.name', "width": "15%", title: "Doctor Name" },
            { data: 'date', "width": "10%", title: "Appointment Date" },
            { data: 'fromtime', "width": "10%", title: "From Time" },
            { data: 'totime', "width": "10%", title: "To Time" },
            { data: 'totalCharge', "width": "10%", title: "Total Charge" },
            { data: 'gst', "width": "10%", title: "GST" },
            { data: 'serviceCharge', "width": "10%", title: "Service Charge" },
            { data: 'totalAmount', "width": "10%", title: "Total Amount" },
            {
                data: 'aId',
                "render": function (data) {
                    return `
                        <div class="btn-group" role="group"> 
                            <a href="/Appointment/Details/${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square">Edit</i>
                            </a>
                            <a onClick=Delete('/Appointment/Delete/${data}') class="btn btn-danger mx-2">
                                <i class="bi bi-trash3-fill"></i> Delete
                            </a>
                        </div>`;
                },
                "width": "20%",
                title: "Action"
            }
        ],
        "language": {
            "emptyTable": "No appointments found."
        },
        "width": "100%"
    });
}

// ✅ Load Pie Chart (Doctor-wise Appointments)
let chart;  // ✅ Global variable to track the chart instance

function loadPieChart() {
    fetch('https://localhost:7092/Appointment/GetAll')
        .then(response => response.json())
        .then(apiResponse => {
            console.log("API Response:", apiResponse);

            let data = apiResponse.data || [];

            console.log("Extracted Data:", data);

            let doctorCount = {};

            data.forEach(appointment => {
                let doctorName = appointment.doctorMaster?.name || "Unknown Doctor";
                doctorCount[doctorName] = (doctorCount[doctorName] || 0) + 1;
            });

            console.log("Doctor Count:", doctorCount);

            let labels = Object.keys(doctorCount);
            let values = Object.values(doctorCount);

            if (labels.length === 0) {
                console.error("No data available for Pie Chart");
                return;
            }

            // ✅ Destroy existing chart before creating a new one
            if (chart) {
                chart.destroy();
            }

            let options = {
                series: values,
                chart: { type: "pie", height: 350 },
                labels: labels,
                responsive: [{
                    breakpoint: 480,
                    options: {
                        chart: { width: 300 },
                        legend: { position: "bottom" }
                    }
                }]
            };

            chart = new ApexCharts(document.querySelector("#pieChart"), options);
            chart.render();
            console.log("Pie Chart Rendered!");
        })
        .catch(error => console.error("Error fetching data:", error));
}


let dateChart;  // Global variable to store the chart instance

function loadDateChart() {
    fetch('https://localhost:7092/Appointment/GetAll')
        .then(response => response.json())
        .then(apiResponse => {
            console.log("API Response:", apiResponse);

            let data = apiResponse.data || [];

            console.log("Extracted Data:", data);

            let dateCount = {};

            data.forEach(appointment => {
                let appointmentDate = appointment.date || "Unknown Date";
                dateCount[appointmentDate] = (dateCount[appointmentDate] || 0) + 1;
            });

            console.log("Date Count:", dateCount);

            let dates = Object.keys(dateCount);
            let values = Object.values(dateCount);

            if (dates.length === 0) {
                console.error("No data available for Date Chart");
                return;
            }

            // ✅ Destroy existing chart before creating a new one
            if (dateChart) {
                dateChart.destroy();
            }

            let options = {
                series: [{
                    name: "Appointments",
                    data: values
                }],
                chart: { type: "bar", height: 350 },
                xaxis: { categories: dates, title: { text: "Appointment Date" } },
                yaxis: { title: { text: "Number of Appointments" } },
                colors: ["#007bff"],
                plotOptions: {
                    bar: { horizontal: false, columnWidth: "55%" }
                },
                dataLabels: { enabled: false },
                title: {
                    text: "Appointments by Date",
                    align: "center"
                }
            };

            dateChart = new ApexCharts(document.querySelector("#dateChart"), options);
            dateChart.render();
            console.log("Date Chart Rendered!");
        })
        .catch(error => console.error("Error fetching data:", error));
}


