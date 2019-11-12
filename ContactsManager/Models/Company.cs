using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsManager.Models
{
    public class Company
    {
        [JsonProperty("companyId")]
        public string Id { get; set; }

        [JsonProperty("properties")]
        public CompanyProperties Properties { get; set; }

        //public string Name { get; set; }
        //public string Website { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }
        //public string Zip { get; set; }
        //public string Phone { get; set; }

    }

    public class CompanyProperties
    {
        [JsonProperty("name")]
        public ContactProperty Name { get; set; }

        [JsonProperty("website")]
        public ContactProperty Website { get; set; }

        [JsonProperty("city")]
        public ContactProperty City { get; set; }

        [JsonProperty("state")]
        public ContactProperty State { get; set; }

        [JsonProperty("zip")]
        public ContactProperty Zip { get; set; }

        [JsonProperty("phone")]
        public ContactProperty Phone { get; set; }
    }

    public class CompanyProperty
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class CompanyList
    {
        public List<Company> Companies { get; set; }
    }
}
