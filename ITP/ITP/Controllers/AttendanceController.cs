using ITP.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP.Controllers
{
    public class AttendanceController : Controller
    {
        private AppDbContext _context;

        public AttendanceController(AppDbContext context)
        {
            _context = context;
        }

        // Route for employee attendence marking page
        [HttpGet("/Index")]
        public ActionResult Index(string searching)
        {
            // For the serch value
            if (searching == null)
            {
                DateTime dateTime = DateTime.Today;
                searching = dateTime.ToString("yyyy-MM-dd");
            }

            return View( _context.AttendeesTable.Where(e => e.WorkDate == searching).ToList());
        }
        
        // Route for employees view page
        [HttpGet("/Employees")]
        public IActionResult Employees()
        {
            return View(_context.Employee.ToList());
        }

        // Route for employees salary view page
        [HttpGet("/Salary")]
        public ActionResult Salary(string searching)
        {
            // For the serch value
            if (searching == null)
            {
                DateTime dateTime = DateTime.Today;
                searching = dateTime.ToString("yyyy-MM");
            }

            // Get data from 2 tables and grouped for specific date with salary details
            var salaryDetails = (from a in _context.Employee
                                 join b in _context.AttendeesTable on a.Empname equals b.EmpName
                                 where (b.WorkDate.Contains(searching))
                                 group b by new { a.Empid, b.EmpName, a.EmpNIC, a.EmpSalary } into salary
                                 select new SalaryModel()
                                 {
                                     EmpId = salary.Key.Empid,
                                     EmpName = salary.Key.EmpName,
                                     EmpNIC = salary.Key.EmpNIC,
                                     BasicSalary = salary.Key.EmpSalary,
                                     OverTime = salary.Sum(s => s.OverTime)
                                 });

            return View(salaryDetails);
        }

        // Route for view employee monthly attendence
        [HttpGet("/MonthlyAttendence")]
        public ActionResult MonthlyAttendence(string searching)
        {
            // For the search values
            if (searching == null)
            {
                DateTime dateTime = DateTime.Today;
                searching = dateTime.ToString("yyyy-MM");
            }

            // Get data from 2 tables and grouped for specific date with attendance
            var empMonthlyAttendenceDetails = (from a in _context.Employee
                                 join b in _context.AttendeesTable on a.Empname equals b.EmpName
                                 where (b.WorkDate.Contains(searching))
                                 group b by new { a.Empid, b.EmpName, a.EmpNIC, a.EmpSalary } into salary
                                 select new MonthlyAttendenceModel()
                                 {
                                     EmpId = salary.Key.Empid,
                                     EmpName = salary.Key.EmpName,
                                     DayCount = salary.Count(d => d.Availability == "Present"),
                                     OverTime = salary.Sum(s => s.OverTime)
                                 });

            return View(empMonthlyAttendenceDetails);
        }

        //Get create method
        public IActionResult Create()
        {
            return View();
        }

        // Route for add employee attendence
        [HttpPost]
        public async Task<IActionResult> Create(string empName, string workDay, string attendTime, string isAvailabity)
        {
            if (ModelState.IsValid)
            {
                // Check fields are empty
                if (empName != null)
                {
                    if (workDay != null && attendTime != null)
                    {
                        // Check employee is exist in employee table
                        if (EmployeeIsExistInEmployeeTable(empName))
                        {
                            // Validate entered day future date or not
                            DateTime dateTime = DateTime.Today;
                            if (DateTime.Parse(dateTime.ToString("yyyy-MM-dd")) < DateTime.Parse(workDay))
                            {
                                ViewBag.Message = String.Format("Please check the date. You entered future date (" + workDay + ")!");
                            }
                            else
                            {
                                // Check employee attended, absent or not
                                if (EmployeeIsAttended(empName, workDay))
                                {
                                    if (!EmployeeIsAbsent(empName, workDay))
                                    {
                                        if (EmployeeWorkedIsOver(empName, workDay))
                                        {
                                            // Employee off time update in database
                                            TimeSpan ot;
                                            var attendees = _context.AttendeesTable.SingleOrDefault(e => e.EmpName == empName && e.WorkDate == workDay);
                                            var workedHours = (TimeSpan.Parse(attendTime) - TimeSpan.Parse(attendees.WorkStartTime));

                                            if (workedHours < TimeSpan.Parse("00:01"))
                                            {
                                                ViewBag.Message = String.Format("Please check work end time!");
                                            }
                                            else
                                            {
                                                if (workedHours < TimeSpan.Parse("08:00"))
                                                {
                                                    ot = TimeSpan.Parse("00:00");
                                                }
                                                else
                                                {
                                                    ot = workedHours - TimeSpan.Parse("08:00");
                                                }

                                                attendees.WorkEndTime = attendTime;
                                                attendees.OverTime = Int32.Parse(ot.ToString("hh"));

                                                await _context.SaveChangesAsync();
                                                return RedirectToAction(nameof(Index));
                                            }
                                        }
                                        else
                                        {
                                            string msg = empName + " is already worked for " + workDay + " this day!";
                                            ViewBag.Message = String.Format(msg);
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.Message = String.Format(empName + " absent for (" + workDay + ") this day. If you want to chenge please update!");
                                    }
                                }
                                else
                                {
                                    // Employee present or not insert to the database
                                    if (isAvailabity != null)
                                    {
                                        AttendeesModel attendeesModel = new AttendeesModel();
                                        attendeesModel.EmpName = empName;
                                        attendeesModel.WorkDate = workDay;
                                        attendeesModel.WorkStartTime = "";
                                        attendeesModel.WorkEndTime = "";
                                        attendeesModel.Availability = "Absent";
                                        attendeesModel.OverTime = 0;

                                        _context.Add(attendeesModel);
                                        await _context.SaveChangesAsync();
                                        return RedirectToAction(nameof(Index));

                                    }
                                    else
                                    {
                                        AttendeesModel attendeesModel = new AttendeesModel();
                                        attendeesModel.EmpName = empName;
                                        attendeesModel.WorkDate = workDay;
                                        attendeesModel.WorkStartTime = attendTime;
                                        attendeesModel.WorkEndTime = "";
                                        attendeesModel.Availability = "Present";
                                        attendeesModel.OverTime = 0;
                                        _context.Add(attendeesModel);
                                    }

                                    await _context.SaveChangesAsync();
                                    return RedirectToAction(nameof(Index));
                                }
                            }
                        }
                        else
                        {
                            ViewBag.Message = String.Format(empName+" employee not exist in employee table!");
                        }
                    }
                    else
                    {
                        ViewBag.Message = String.Format("Please select date and time!");
                    }
                }
                else
                {
                    ViewBag.Message = String.Format("Please type employe name!");
                }

            }
            return View();
        }

        // Route view employeee attendence details for edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var getattendancedetails = await _context.AttendeesTable.FindAsync(id);
            return View(getattendancedetails);
        }

        // Route action for update employee attendence details
        [HttpPost]
        public async Task<IActionResult> Edit(AttendeesModel nc)
        {
            if (ModelState.IsValid)
            {
                // Validate fields
                if (nc.WorkDate != null)
                {
                    // Validate entered day future date or not
                    DateTime dateTime = DateTime.Today;
                    if (DateTime.Parse(dateTime.ToString("yyyy-MM-dd")) < DateTime.Parse(nc.WorkDate))
                    {
                        ViewBag.Message = String.Format("Please check the date. You entered future date (" + nc.WorkDate + ")!");
                    }
                    else
                    {

                        if (nc.Availability.ToLower() == "absent")
                        {
                            nc.WorkStartTime = "";
                            nc.WorkEndTime = "";
                            nc.Availability = "Absent";
                            nc.OverTime = 0;

                            _context.Update(nc);
                            await _context.SaveChangesAsync();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            if (nc.WorkStartTime != null)
                            {
                                if (nc.WorkEndTime != null)
                                {
                                    var workedHours = (TimeSpan.Parse(nc.WorkEndTime) - TimeSpan.Parse(nc.WorkStartTime));
                                    if (workedHours < TimeSpan.Parse("00:01"))
                                    {
                                        ViewBag.Message = String.Format("Please check work end time!");
                                    }
                                    else
                                    {
                                        TimeSpan ot;
                                        if (workedHours < TimeSpan.Parse("08:00"))
                                        {
                                            ot = TimeSpan.Parse("00:00");
                                        }
                                        else
                                        {
                                            ot = workedHours - TimeSpan.Parse("08:00");
                                        }
                                        nc.OverTime = Int32.Parse(ot.ToString("hh"));
                                    }

                                }

                                _context.Update(nc);
                                await _context.SaveChangesAsync();
                                return RedirectToAction("Index");

                            }
                            else
                            {
                                ViewBag.Message = String.Format("Please select work start time or absent tik!");
                            }
                        }
                    }

                }
                else
                {
                    ViewBag.Message = String.Format("Please select worked date!");
                }
            }
            return View(nc);

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var getattendancedetails = await _context.AttendeesTable.FindAsync(id);
            return View(getattendancedetails);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var getprdctdetails = await _context.AttendeesTable.FindAsync(id);
            return View(getprdctdetails);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var getattendancedetails = await _context.AttendeesTable.FindAsync(id);
            _context.AttendeesTable.Remove(getattendancedetails);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Method for check is attend to that date
        private bool EmployeeIsAttended(string Empname, string EDate)
        {
            return _context.AttendeesTable.Any(x => x.EmpName == Empname.ToString() && x.WorkDate == EDate.ToString());
        }

        // Method for user is off for the day
        private bool EmployeeWorkedIsOver(string Empname, string EDate)
        {
            return _context.AttendeesTable.Any(x => x.EmpName == Empname.ToString() && x.WorkDate == EDate.ToString() && x.WorkEndTime == "");
        }

        // Method for check employee is absent
        private bool EmployeeIsAbsent(string Empname, string EDate)
        {
            return _context.AttendeesTable.Any(x => x.EmpName == Empname.ToString() && x.WorkDate == EDate.ToString() && x.Availability == "Absent");
        }

        // Method for check user is absent
        private bool EmployeeIsExistInEmployeeTable(string Empname)
        {
            return _context.Employee.Any(x => x.Empname == Empname.ToString());
        }
    }
}
