using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ContactsManager.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ContactsManager.Controllers
{
    public class HomeController : Controller
    {
        IOptions<HubspotSettings> settings;

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
            DateTime eDate = Convert.ToDateTime(endDate);

            if (eDate < sDate)
                return BadRequest();

            long startDateUnix = (Int64)sDate.Subtract((new DateTime(1970, 1, 1))).TotalMilliseconds;
            long endDateUnix = (Int64)eDate.Subtract((new DateTime(1970, 1, 1))).TotalMilliseconds;

            GenerateContactListForGivenDateRange(startDateUnix, endDateUnix);
            return View();
        }

        private void GenerateContactListForGivenDateRange(double startDate, double endDate)
        {
            using (WebClient wc = new WebClient())
            {
                string contactsApiUrl = string.Format(settings.Value.ContactsApiUrl,
                                                    settings.Value.HapiKey);
                string contactsJson = wc.DownloadString(contactsApiUrl);
                var contacts = JsonConvert.DeserializeObject<ContactsList>(contactsJson);

                string companiesApiUrl = string.Format(settings.Value.CompaniesApiUrl,
                                                    settings.Value.HapiKey);
                string companyJson = wc.DownloadString(companiesApiUrl);
                var companies = JsonConvert.DeserializeObject<CompanyList>(companyJson);

                string tmp = "breakPoint";
            }
        }
    }
}