using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;

namespace Framework.Serialization.Event;

/// <summary>
/// https://www.mking.net/blog/working-with-private-setters-in-json-net
/// </summary>
public class PrivateSetterContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var jsonProperty = base.CreateProperty(member, memberSerialization);
        if (jsonProperty.Writable)
            return jsonProperty;

        if (member is PropertyInfo propertyInfo)
        {
            var setter = propertyInfo.GetSetMethod(true);
            jsonProperty.Writable = setter != null;
        }

        return jsonProperty;
    }
}
