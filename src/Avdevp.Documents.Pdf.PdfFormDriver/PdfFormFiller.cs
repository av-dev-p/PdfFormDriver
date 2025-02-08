using iTextSharp.text.pdf;
using Avdevp.Common.Documents.Models.Pdf;

namespace Avdevp.Common.Documents;

public static class PdfFormFiller
{

    public static byte[] FillForm(byte[] pdfBytes, PdfFields pdfFields, List<string>? fieldsNotToFlat = null, float textSize = 9.0f)
    {
        Dictionary<string, string> formFields = pdfFields.GetPdfFields();

        using var pdfReader = new PdfReader(pdfBytes);
        using var memoryStream = new MemoryStream();

        using (var pdfStamper = new PdfStamper(pdfReader, memoryStream))
        {
            pdfStamper.Writer.PdfVersion = PdfWriter.VERSION_1_7;
            pdfStamper.Writer.SetPdfVersion(PdfWriter.PdfVersion17);
            var acroFieldsDictionary = pdfStamper.AcroFields.Fields;
            bool activeFlattening = false;
            foreach (var acroField in acroFieldsDictionary)
            {
                if (formFields.TryGetValue(acroField.Key, out string? formFieldValue))
                {
                    if (!activeFlattening)
                    {
                        activeFlattening = true;
                        pdfStamper.FormFlattening = true;
                    }

                    if (formFieldValue == "True")
                    {
                        pdfStamper.AcroFields.SetField(acroField.Key, "Yes");
                    }
                    else if (formFieldValue == "False")
                    {
                        pdfStamper.AcroFields.SetField(acroField.Key, "Off");
                    }
                    else
                    {
                        pdfStamper.AcroFields.SetFieldProperty(acroField.Key, "textsize", textSize, null);
                        pdfStamper.AcroFields.SetField(acroField.Key, formFieldValue);
                    }
                }
                if (fieldsNotToFlat == null || !fieldsNotToFlat.Contains(acroField.Key))
                {
                    pdfStamper.PartialFormFlattening(acroField.Key);
                }
            }
        }
        return memoryStream.ToArray();
    }

    public static byte[] AddSignatureFieldsToPdf(byte[] pdfBytes, List<PdfSignatureField> signatureFields)
    {
        using PdfReader pdfReader = new(pdfBytes);
        using MemoryStream memoryStream = new();
        using (PdfStamper pdfStamper = new(pdfReader, memoryStream))
        {
            foreach (PdfSignatureField field in signatureFields)
            {
                pdfStamper.AddSignature(field.Name, field.Page, field.X, field.Y, field.X + field.Width, field.Y + field.Height);
            }
        }
        return memoryStream.ToArray();
    }

    public static byte[] MergeDocuments(byte[] pdf1, byte[] pdf2)
    {
        using PdfReader pdfReader1 = new(pdf1);
        using PdfReader pdfReader2 = new(pdf2);
        using MemoryStream memoryStream = new();
        PdfCopyFields pdfCopyFields = new(memoryStream);
        pdfCopyFields.Writer.PdfVersion = PdfWriter.VERSION_1_7;
        pdfCopyFields.Writer.SetPdfVersion(PdfWriter.PdfVersion17);
        pdfCopyFields.AddDocument(pdfReader1);
        pdfCopyFields.AddDocument(pdfReader2);
        pdfCopyFields.Close();
        return memoryStream.ToArray();
    }
}
