namespace Avdevp.Common.Documents.Models.Pdf;

public class PdfSignatureField
{
    public string Name { get; set; } = string.Empty;
    public int Page {get; set;}
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; } = 150;
    public float Height { get; set; } = 20;
    public string Reason {get; set;} = string.Empty;
    public string Location {get; set;} = string.Empty;
}
