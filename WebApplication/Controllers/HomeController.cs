using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string year = Service.GetActiveYear();
            if (year != null) //If reading is active
            {
                Session["Year"] = year;
                return RedirectToAction("Login", "Home");
            }
            else //if reading is not active
            {
                return View();
            }
        }

        public ActionResult Login()
        {
            ViewBag.Year = Session["Year"];
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM login)
        {
            ViewBag.Year = Session["Year"];
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            if (Service.AuthenticateLogin(login.GaugeID, login.Password)) //Checks login information
            {
                Reading r = Service.GetGaugeReadingOnYear(login.GaugeID, (string)Session["Year"]);

                if (r != null) //if gauge has already been read
                {
                    ViewBag.LoginError = "Der er allerede modtaget en aflæsning fra denne måler.";
                    return View(login);
                }

                Session["GaugeID"] = login.GaugeID;
                return RedirectToAction("Reading", "Home");
            }
            else
            {
                ViewBag.LoginError = "ID eller password er forkert.";
                return View(login);
            }
        }

        public ActionResult Reading()
        {
            if (Session["GaugeID"] != null) //Can only access view if logged in
            {
                ViewBag.Year = Session["Year"];
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public ActionResult Reading(ReadingVM reading)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Year = Session["Year"];
                return View(reading);
            }

            Session["Reading"] = reading;
            return RedirectToAction("Receipt", "Home");
        }

        public ActionResult Receipt()
        {
            if (Session["GaugeID"] != null) //Can only access view if logged in
            {
                int gaugeID = (int)Session["GaugeID"];
                int lastYear = Int32.Parse((string)Session["Year"]) - 1;
                Reading lastReading = Service.GetGaugeReadingOnYear(gaugeID, lastYear + "");

                if (lastReading != null) //if there was a reading last year
                {
                    ViewBag.lastKilo = lastReading.KilowattHours;
                    ViewBag.lastCubic = lastReading.CubicMeters;
                    ViewBag.lastHour = lastReading.Hours;
                }

                ReadingVM reading = (ReadingVM)Session["Reading"];
                ViewBag.kilo = reading.kWh;
                ViewBag.cubic = reading.CubicMeters;
                ViewBag.hour = reading.Hours;

                if (!Service.CheckCoolingOnReading(reading.kWh, reading.CubicMeters)) //Checks if cooling is too low
                {
                    ViewBag.CoolingAlert = "Advarsel! Afkøling er for lille!";
                }
                ViewBag.year = Session["Year"];
                ViewBag.lastYear = lastYear;

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        //Confirm button clicked
        public ActionResult Confirm()
        {
            if (Session["GaugeID"] != null) //Can only access if logged in
            {
                ReadingVM r = (ReadingVM)Session["Reading"];
                string year = (string)Session["Year"];
                int gaugeID = (int)Session["GaugeID"];

                Service.CreateReading(r.kWh, r.CubicMeters, r.Hours, year, gaugeID);
                return RedirectToAction("End", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult End()
        {
            if (Session["GaugeID"] != null) //Can only access view if logged in
            {
                Session["GaugeID"] = null; //Logs out
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
    }
}