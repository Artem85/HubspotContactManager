using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ContactsManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

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

            var contacts = GetContactsListForGivenDateRange(startDateUnix, endDateUnix);

            return GenerateExcelFile(contacts);

            //return View(contacts);
        }

        private List<ContactsDataItem> GetContactsListForGivenDateRange (long startDateUnix, long endDateUnix)
        {
            return ContactsData.GetContactsData(settings)
                    .Where(t => {
                        long unixDate = Int64.Parse(t.LastModifiedDate);
                        return startDateUnix <= unixDate && unixDate <= endDateUnix;
                    }).ToList();
        }

        public IActionResult GenerateExcelFile(List<ContactsDataItem> contacts)
        {
            var memoryStream = new MemoryStream();
            string fileName = "report.xlsx";

            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Contacts");

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(ContactsDataItem));

                IRow row = excelSheet.CreateRow(0);
                for (int i = 0; i < properties.Count; i++)
                {
                    row.CreateCell(i).SetCellValue(properties[i].Name);
                }

                for (int i = 0; i < contacts.Count; i++)
                {
                    row = excelSheet.CreateRow(i + 1);
                    for (int j = 0; j < properties.Count; j++)
                    {
                        row.CreateCell(j).SetCellValue(properties[j].GetValue(contacts[i]).ToString());
                    }
                }
                workbook.Write(fileStream);
            }

            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                stream.CopyTo(memoryStream);
            }
            memoryStream.Position = 0;

            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

    }
}