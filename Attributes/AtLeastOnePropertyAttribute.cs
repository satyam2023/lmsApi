using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace lmsApi.Attributes;

public class AtLeastOnePropertyAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
            return false;

        var properties = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var property in properties)
        {
            var propertyValue = property.GetValue(value);
            

            if (propertyValue != null)
            {
                if (propertyValue is string stringValue && !string.IsNullOrWhiteSpace(stringValue))
                    return true;
                else if (!(propertyValue is string) && !propertyValue.Equals(GetDefaultValue(property.PropertyType)))
                    return true;
            }
        }
        
        return false;
    }

    private static object? GetDefaultValue(Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }

    public override string FormatErrorMessage(string name)
    {
        return "At least one field must be provided for update.";
    }
}
