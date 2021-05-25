using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using Dapper;
using System.IO;

namespace AutoPrint
{
    public class PrintingJobs
    {
        public Int64 Id { get; set; }
        public string Identifier { get; set; }
        public string Code { get; set; }
        public string PatientName { get; set; }
        public string Result { get; set; }
        public string DOB { get; set; }
        public DateTime DateTime { get; set; }
        public Guid CenterId { get; set; }
        public string QRCode { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            using (var connection = new SqlConnection("Data Source=champdatatek.database.windows.net;Initial Catalog=covid-db;User ID=kernal;Password=adminP@ssw0rd;MultipleActiveResultSets=True"))
            {
                while (true)
                {
                    try
                    {
                        string text = File.ReadAllText("center.txt");
                        var query = $"Select * from printingjobs where centerId='{text}'";
                        var prints = connection.Query<PrintingJobs>(query);
                        foreach (var print in prints)
                        {
                            try
                            {
                                //PdfDocument doc = new PdfDocument();
                                //PdfPageBase page = doc.Pages.Add();
                                //PdfFont font = new PdfFont(PdfFontFamily.TimesRoman, 12f);
                                //PdfSolidBrush brush = new PdfSolidBrush(Color.Black);
                                //PdfStringFormat leftAlignment = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                                //page.Canvas.DrawString($"Identifier           : {print.Identifier}", font, brush, 0, 10, leftAlignment);
                                //page.Canvas.DrawString($"Code                 : {print.Code}", font, brush, 0, 30, leftAlignment);
                                //page.Canvas.DrawString($"Patient Name         : {print.PatientName}", font, brush, 0, 50, leftAlignment);
                                //page.Canvas.DrawString($"Date of birth        : {print.DOB}", font, brush, 0, 70, leftAlignment);
                                //page.Canvas.DrawString($"Appointment DateTime : {print.DateTime.ToString("yyyy.MM.dd HH:mm")}", font, brush, 0, 90, leftAlignment);
                                //PdfImage image = PdfImage.FromFile(@"img.png");
                                //page.Canvas.DrawImage(image, 0, 110, 100, 100);

                                //doc.SaveToFile("Sample.pdf");
                                //Thread.Sleep(1000);
                                //doc.LoadFromFile("Sample.pdf");
                                //doc.Print();

                                PdfDocument doc = new PdfDocument();
                                PdfPageBase page = doc.Pages.Add();
                                PdfFont font = new PdfFont(PdfFontFamily.TimesRoman, 36f);
                                PdfSolidBrush brush = new PdfSolidBrush(Color.Black);
                                PdfStringFormat leftAlignment = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                                page.Canvas.DrawString($"Identifier           : {print.Identifier}", font, brush, 0, 10, leftAlignment);
                                page.Canvas.DrawString($"Code                 : {print.Code}", font, brush, 0, 30, leftAlignment);
                                page.Canvas.DrawString($"Patient Name         : {print.PatientName}", font, brush, 0, 50, leftAlignment);
                                page.Canvas.DrawString($"Date of birth        : {print.DOB}", font, brush, 0, 70, leftAlignment);
                                page.Canvas.DrawString($"Appointment DateTime : {print.DateTime.ToString("yyyy.MM.dd HH:mm")}", font, brush, 0, 90, leftAlignment);
                                PdfImage image = PdfImage.FromFile(@"img.png");
                                page.Canvas.DrawImage(image, 0, 110, 500, 500);

                                doc.SaveToFile("Sample.pdf");
                                Thread.Sleep(1000);
                                doc.LoadFromFile("Sample.pdf");
                                doc.Print();


                                Thread.Sleep(5000);
                                string qr = "Delete from printingjobs where Id=" + print.Id;
                                connection.Execute(qr);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    Thread.Sleep(5000);
                }
            }
            // Call api for print job
           

            

        }
    }
}
