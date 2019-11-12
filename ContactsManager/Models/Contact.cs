using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsManager.Models
{
    public class Contact
    {
        [JsonProperty("vid")]
        public string Id { get; set; }

        [JsonProperty("properties")]
        public ContactProperties Properties { get; set; }

        //[JsonProperty("firstname")]
        //public string FirstName { get; set; }
        //[JsonProperty("lastname")]
        //public string LastName { get; set; }
        //[JsonProperty("associatedcompanyid")]
        //public int CompanyId { get; set; }
        //[JsonProperty("lifecyclestage")]
        //public string LifecycleStage { get; set; }

    }

    public class ContactProperties
    {
        [JsonProperty("firstname")]
        public ContactProperty FirstName { get; set; }

        [JsonProperty("lastname")]
        public ContactProperty LastName { get; set; }

        [JsonProperty("lifecyclestage")]
        public ContactProperty LifecycleStage { get; set; }

        [JsonProperty("associatedcompanyid", NullValueHandling = NullValueHandling.Ignore)]
        public ContactProperty AssociatedCompanyid { get; set; }

        [JsonProperty("lastmodifieddate")]
        public ContactProperty LastModifiedDate { get; set; }
    }

    public  class ContactProperty
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class ContactsList
    {
        public List<Contact> Contacts { get; set; }
    }
}
