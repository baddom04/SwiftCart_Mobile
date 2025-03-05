using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ShoppingList.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum HouseholdRelationship
    {
        [EnumMember(Value = "nonMember")]
        NonMember,
        [EnumMember(Value = "member")]
        Member,
        [EnumMember(Value = "owner")]
        Owner,
        [EnumMember(Value = "applied")]
        Applied
    }
}
