using System;
using System.Linq;
using ContactsManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ContactsManager.Controllers
{
    public class HomeController : Controller
    {
        readonly IOptions<HubspotSettings> settings;

        public HomeController(IOptions<HubspotSettings> settings)
        {
            this.settings = settings;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Contacts")]
        public IActionResult Contacts(string startDate, string endDate)
        {
            DateTime sDate = Convert.ToDateTime(startDate);
            //We need to add 1 day to end date because of Unix DateTime stamp assume that date is a begin of the day (12:00 AM)
            DateTime eDate = Convert.ToDateTime(endDate).AddDays(1);

            if (eDate < sDate)
                return BadRequest();

            long startDateUnix = (long)sDate.Subtract((new DateTime(1970, 1, 1))).TotalMilliseconds;
            long endDateUnix = (long)eDate.Subtract((new DateTime(1970, 1, 1))).TotalMilliseconds;

            var contacts = ContactsData.GetContactsData(settings)
                    .Where(t => { long unixDate = Int64.Parse(t.LastModifiedDate);
                                    return startDateUnix <= unixDate && unixDate <= endDateUnix; });

            return View(contacts);
        }

    }
}