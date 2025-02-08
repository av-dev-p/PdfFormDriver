namespace Avdevp.Common.Documents.Attributes.Pdf;

[AttributeUsage(AttributeTargets.Property)]
public class PdfFieldAttribute(string fieldName) : Attribute
{
    public string FieldName => fieldName;
}
