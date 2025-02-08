using System.Reflection;
using Avdevp.Common.Documents.Attributes.Pdf;

namespace Avdevp.Common.Documents.Models.Pdf;

public abstract class PdfFields
{

    public Dictionary<string, string> GetPdfFields()
    {
        var fields = new Dictionary<string, string>();
        var properties = GetType().GetProperties();
        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttribute<PdfFieldAttribute>();
            if (attribute != null)
            {
                fields.Add(attribute.FieldName, property.GetValue(this)?.ToString() ?? string.Empty);
            }
        }
        return fields;
    }

}
