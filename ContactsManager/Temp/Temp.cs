using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsManager.Temp
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Welcome
    {
        [JsonProperty("contacts")]
        public List<Contact> Contacts { get; set; }

        [JsonProperty("has-more")]
        public bool HasMore { get; set; }

        [JsonProperty("vid-offset")]
        public long VidOffset { get; set; }
    }

    public partial class Contact
    {
        [JsonProperty("addedAt")]
        public long AddedAt { get; set; }

        [JsonProperty("vid")]
        public long Vid { get; set; }

        [JsonProperty("canonical-vid")]
        public long CanonicalVid { get; set; }

        [JsonProperty("merged-vids")]
        public List<object> MergedVids { get; set; }

        [JsonProperty("portal-id")]
        public long PortalId { get; set; }

        [JsonProperty("is-contact")]
        public bool IsContact { get; set; }

        [JsonProperty("profile-token")]
        public string ProfileToken { get; set; }

        [JsonProperty("profile-url")]
        public Uri ProfileUrl { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("form-submissions")]
        public List<object> FormSubmissions { get; set; }

        [JsonProperty("identity-profiles")]
        public List<IdentityProfile> IdentityProfiles { get; set; }

        [JsonProperty("merge-audits")]
        public List<object> MergeAudits { get; set; }
    }

    public partial class IdentityProfile
    {
        [JsonProperty("vid")]
        public long Vid { get; set; }

        [JsonProperty("saved-at-timestamp")]
        public long SavedAtTimestamp { get; set; }

        [JsonProperty("deleted-changed-timestamp")]
        public long DeletedChangedTimestamp { get; set; }

        [JsonProperty("identities")]
        public List<Identity> Identities { get; set; }
    }

    public partial class Identity
    {
        [JsonProperty("type")]
        public TypeEnum Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("is-primary", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsPrimary { get; set; }
    }

    public partial class Properties
    {
        [JsonProperty("firstname")]
        public Associatedcompanyid Firstname { get; set; }

        [JsonProperty("lastmodifieddate")]
        public Associatedcompanyid Lastmodifieddate { get; set; }

        [JsonProperty("lifecyclestage")]
        public Associatedcompanyid Lifecyclestage { get; set; }

        [JsonProperty("lastname")]
        public Associatedcompanyid Lastname { get; set; }

        [JsonProperty("associatedcompanyid", NullValueHandling = NullValueHandling.Ignore)]
        public Associatedcompanyid Associatedcompanyid { get; set; }
    }

    public partial class Associatedcompanyid
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public enum TypeEnum { Email, LeadGuid };

    public partial class Welcome
    {
        public static Welcome FromJson(string json) => JsonConvert.DeserializeObject<Welcome>(json, Temp.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Welcome self) => JsonConvert.SerializeObject(self, Temp.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                TypeEnumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "EMAIL":
                    return TypeEnum.Email;
                case "LEAD_GUID":
                    return TypeEnum.LeadGuid;
            }
            throw new Exception("Cannot unmarshal type TypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeEnum)untypedValue;
            switch (value)
            {
                case TypeEnum.Email:
                    serializer.Serialize(writer, "EMAIL");
                    return;
                case TypeEnum.LeadGuid:
                    serializer.Serialize(writer, "LEAD_GUID");
                    return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }
}
