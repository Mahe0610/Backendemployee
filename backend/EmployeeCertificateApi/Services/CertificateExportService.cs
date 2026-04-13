using ClosedXML.Excel;
using EmployeeCertificateApi.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ZXing;
using ZXing.Common;

namespace EmployeeCertificateApi.Services;

public interface ICertificateExportService
{
    byte[] ExportToExcel(Employee employee);
    byte[] ExportToPdf(Employee employee, string logoPath);
    string BuildDocumentCode(Employee employee);
}

public class CertificateExportService : ICertificateExportService
{
    public byte[] ExportToExcel(Employee employee)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Certificate");

        worksheet.Cell("A1").Value = "Company Certificate";
        worksheet.Range("A1:C1").Merge().Style.Font.SetBold().Font.SetFontSize(18);

        worksheet.Cell("A3").Value = "Document Code";
        worksheet.Cell("B3").Value = BuildDocumentCode(employee);
        worksheet.Cell("A4").Value = "Employee Name";
        worksheet.Cell("B4").Value = employee.EmployeeName;
        worksheet.Cell("A5").Value = "Age";
        worksheet.Cell("B5").Value = employee.Age;
        worksheet.Cell("A6").Value = "Date of Birth";
        worksheet.Cell("B6").Value = employee.DateOfBirth.ToString("yyyy-MM-dd");
        worksheet.Cell("A7").Value = "Email";
        worksheet.Cell("B7").Value = employee.Email;
        worksheet.Cell("A8").Value = "Role";
        worksheet.Cell("B8").Value = employee.Role.ToString();
        worksheet.Cell("A9").Value = "Salary";
        worksheet.Cell("B9").Value = employee.Salary?.ToString("F2") ?? "N/A";

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public byte[] ExportToPdf(Employee employee, string logoPath)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        var code = BuildDocumentCode(employee);
        var barcode = BuildBarcode(code);

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);

                page.Header().Row(row =>
                {
                    if (File.Exists(logoPath))
                    {
                        row.ConstantItem(110).Height(60).Image(logoPath);
                    }

                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignCenter().Text("Employee Certificate").Bold().FontSize(20);
                        col.Item().AlignCenter().Text($"Document Code: {code}").FontSize(10);
                    });
                });

                page.Content().PaddingVertical(20).Column(col =>
                {
                    col.Spacing(10);
                    col.Item().Text($"Employee Name: {employee.EmployeeName}");
                    col.Item().Text($"Age: {employee.Age}");
                    col.Item().Text($"Date Of Birth: {employee.DateOfBirth:yyyy-MM-dd}");
                    col.Item().Text($"Email: {employee.Email}");
                    col.Item().Text($"Role: {employee.Role}");
                    col.Item().Text($"Salary: {(employee.Salary.HasValue ? employee.Salary.Value.ToString("C") : "N/A")}");
                });

                page.Footer().AlignCenter().Column(col =>
                {
                    col.Item().Image(barcode).FitWidth();
                    col.Item().Text("Scan barcode to validate certificate details").FontSize(9);
                });
            });
        }).GeneratePdf();
    }

    public string BuildDocumentCode(Employee employee)
        => $"CERT-{employee.Id:D5}-{employee.CreatedAtUtc:yyyyMMdd}";

    private static byte[] BuildBarcode(string content)
    {
        var writer = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.CODE_128,
            Options = new EncodingOptions
            {
                Height = 80,
                Width = 320,
                Margin = 2
            }
        };

        var pixels = writer.Write(content);
        using var bitmap = new System.Drawing.Bitmap(pixels.Width, pixels.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
        var data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixels.Width, pixels.Height),
            System.Drawing.Imaging.ImageLockMode.WriteOnly,
            System.Drawing.Imaging.PixelFormat.Format32bppRgb);

        try
        {
            System.Runtime.InteropServices.Marshal.Copy(pixels.Pixels, 0, data.Scan0, pixels.Pixels.Length);
        }
        finally
        {
            bitmap.UnlockBits(data);
        }

        using var ms = new MemoryStream();
        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        return ms.ToArray();
    }
}
