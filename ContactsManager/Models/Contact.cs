using Newtonsoft.Json;
using System.Collections.Generic;

namespace ContactsManager.Models
{
    public class Contact
    {
        [JsonProperty("vid")]
        public string Id { get; set; }

        [JsonProperty("properties")]
        public ContactProperties Properties { get; set; }

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
