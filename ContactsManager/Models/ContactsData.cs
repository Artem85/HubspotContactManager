using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ContactsManager.Models
{
    public class ContactsDataItem
    {
        public string ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LifecycleStage { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Website { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string LastModifiedDate { get; set; }
    }

    public class ContactsData: List<ContactsDataItem>
    {
        static IOptions<HubspotSettings> hubspotSettings;
        private static ContactsData contactsData;
        
        public static ContactsData GetContactsData (IOptions<HubspotSettings> settings)
        {
            if (contactsData == null)
            {
                hubspotSettings = settings;
                contactsData = new ContactsData();
                GenerateContactList();
            }

            return contactsData;
        }


        private static void GenerateContactList()
        {
            using (WebClient wc = new WebClient())
            {
                string contactsApiUrl = string.Format(hubspotSettings.Value.ContactsApiUrl,
                                                    hubspotSettings.Value.HapiKey);
                string contactsJson = wc.DownloadString(contactsApiUrl);
                var contacts = JsonConvert.DeserializeObject<ContactsList>(contactsJson);

                string companiesApiUrl = string.Format(hubspotSettings.Value.CompaniesApiUrl,
                                                    hubspotSettings.Value.HapiKey);
                string companyJson = wc.DownloadString(companiesApiUrl);
                var companies = JsonConvert.DeserializeObject<CompanyList>(companyJson);

                var linkedData = from contact in contacts.Contacts
                                 join company in companies.Companies
                                 on ((contact.Properties.AssociatedCompanyid == null) ? "" : contact.Properties.AssociatedCompanyid.Value) equals company.CompanyId
                                 select new
                                 {
                                     contact.Id,
                                     contact.Properties.FirstName,
                                     contact.Properties.LastName,
                                     contact.Properties.LifecycleStage,
                                     company.CompanyId,
                                     company.Properties.Name,
                                     company.Properties.Website,
                                     company.Properties.City,
                                     company.Properties.State,
                                     company.Properties.Zip,
                                     company.Properties.Phone,
                                     contact.Properties.LastModifiedDate
                                 };

                foreach (var contact in linkedData)
                {
                    ContactsDataItem item = new ContactsDataItem();

                    item.ContactId = contact.Id;
                    item.FirstName = contact.FirstName.Value;
                    item.LastName = contact.LastName.Value;
                    item.LifecycleStage = contact.LifecycleStage.Value;
                    item.CompanyId = contact.CompanyId;
                    item.CompanyName = contact.Name.Value;
                    item.Website = contact.Website.Value;
                    item.City = contact.City.Value;
                    item.State = contact.State.Value;
                    item.Zip = contact.Zip.Value;
                    item.Phone = contact.Phone.Value;
                    item.LastModifiedDate = contact.LastModifiedDate.Value;

                    contactsData.Add(item);
                }
            }
        }
    }
}
