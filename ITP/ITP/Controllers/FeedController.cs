using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITP.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using MailChimp.Net.Models;

namespace ITP.Controllers
{
    public class FeedController : Controller
    {

        SqlCommand command;
        private static String connectionstring = "workstation id=Project.mssql.somee.com;packet size=4096;user id=donkavi2_SQLLogin_1;pwd=12345678;data source=Project.mssql.somee.com;persist security info=False;initial catalog=Project";
        SqlConnection connection = new SqlConnection(connectionstring);
        SqlDataReader reader;
        List<Newfeed> TestList = new List<Newfeed>();
        Newfeed test = null;

        private readonly AppDbContext _db;
        public FeedController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult FeedIn()
        {
            var displaydata = _db.Feedback.ToList();
            return View(displaydata);
        }
        public IActionResult CreateFeed()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> CreateFeed(Newfeed nec,string St_01, string St_02, string Rate)
        {
            int r1, r2, r3;
            r1 = int.Parse(St_01);
            r2 = int.Parse(St_02);
            r3 = int.Parse(Rate);

            nec.St_01 = r1;
            nec.St_02 = r2;
            nec.Rate = r3;

            if (ModelState.IsValid)
            {
                _db.Add(nec);
                await _db.SaveChangesAsync();
                return RedirectToAction("CreateFeed");
            }
            return View(nec);
        }
        public async Task<IActionResult> EditFeed(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("FeedIn");
            }
            var getFeeddetails = await _db.Feedback.FindAsync(id);
            return View(getFeeddetails);
        }
        [HttpPost]

        public async Task<IActionResult> EditFeed(Newfeed nc)
        {
            if (ModelState.IsValid)
            {
                _db.Update(nc);
                await _db.SaveChangesAsync();
                return RedirectToAction("FeedIn");
            }
            return View(nc);
        }

        public async Task<IActionResult> DetailsFeed(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("FeedIn");
            }
            var getFeeddetails = await _db.Feedback.FindAsync(id);
            return View(getFeeddetails);
        }

        public async Task<IActionResult> DeleteFeed(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("FeedIn");
            }
            var getFeeddetails = await _db.Feedback.FindAsync(id);
            return View(getFeeddetails);
        }
        [HttpPost]

        public async Task<IActionResult> DeleteFeed(int id)
        {

            var getFeeddetails = await _db.Feedback.FindAsync(id);
            _db.Feedback.Remove(getFeeddetails);
            await _db.SaveChangesAsync();

            return RedirectToAction("FeedIn");
        }



        public IActionResult FeedbackPartial()
        {
            command = new SqlCommand("SELECT * FROM Feedback", connection);
            reader = command.ExecuteReader();
            connection.Open();
            int fcount = reader.Cast<object>().Count();
            reader.Close();

            ViewBag.feed = fcount;

            return PartialView("Owner/FeedDash");
        }
 



        public ActionResult Feed1()
        {        
            command = new SqlCommand("SELECT FeedID,FirstName,LastName,FeedDes FROM Feedback",connection);

            Link();
            connection.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                test = new Newfeed();
                test.FeedID = int.Parse(reader["FeedID"].ToString());
                test.FirstName= reader["FirstName"].ToString();
                test.LastName = reader["LastName"].ToString();
                test.FeedDes = reader["FeedDes"].ToString();
                

                TestList.Add(test);
            }

            ViewBag.ob = TestList;
            return View("CreateFeed");
        }
        public static void Link()
        {
          connectionstring = "workstation id=Project.mssql.somee.com;packet size=4096;user id=donkavi2_SQLLogin_1;pwd=12345678;data source=Project.mssql.somee.com;persist security info=False;initial catalog=Project";

    }


}
}
