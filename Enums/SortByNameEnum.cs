using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PDP.University.Examine.Project.Web.API.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortByNameEnum
    {
        [EnumMember(Value = "name")]
        Name
    }
}
