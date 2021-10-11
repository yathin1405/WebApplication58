using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication58.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using Syncfusion.Pdf.Grid;
using System.IO;
using System.Net.Mail;
using Newtonsoft.Json.Linq;
using PdfSharp.Pdf.Advanced;
using PdfFont = Syncfusion.Pdf.Graphics.PdfFont;

namespace WebApplication58.Controllers
{
    public class QuotesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly PdfFont timesRoman;

        public async Task<ActionResult> Index()
        {
            return View(await db.Quotes.ToListAsync());
        }

        // GET: Quotes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quote quote = await db.Quotes.FindAsync(id);
            if (quote == null)
            {
                return HttpNotFound();
            }
            return View(quote);
        }

        // GET: Quotes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Quotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserID,FirstName,LastName,EmailAddress,PhoneNumber,NumAdults,NumKids,DepartureDate,ReturnDate,TourL,CruiseL,FlightL,HotelL,estimatedPrice")] Quote quote)
        {

            double estPrice = 1500;
            int numKids, numAdults = 0;
            if (ModelState.IsValid)
            {
                if (quote.CruiseL == 0)
                {
                    estPrice += 3500;
                }
                if (quote.FlightL == 0)
                {
                    estPrice += 12000;
                }
                if (quote.HotelL == 0)
                {
                    estPrice += 2000;
                }
                if (quote.TourL == 0)
                {
                    estPrice += 3500;
                }

                //Runs through database and checks if date booked is available
                var Dates = from dates in db.Quotes
                            select dates.DepartureDate;
                var EndDates = from dates in db.Quotes
                               select dates.ReturnDate;


                foreach (var item in Dates)
                {
                    if (item == quote.DepartureDate)
                    {
                        return View("Unavailable");
                    }
                }
                numKids = quote.NumKids;
                numAdults = quote.NumAdults;
                estPrice *= (numAdults + numKids / 5);
                quote.EmailAddress = User.Identity.Name;
                quote.estimatedPrice = estPrice;
                db.Quotes.Add(quote);
                await db.SaveChangesAsync();
                return RedirectToAction("ViewRecent");
            }
            return View(quote);
        }
        public async Task<ActionResult> ViewRecent()
        {
            return View(await db.Quotes.Where(m => m.EmailAddress == User.Identity.Name).ToListAsync());
        }
        public async Task<ActionResult> EmailQuote(int? id)
        {
            Quote quote = await db.Quotes.FindAsync(id);

            //New Email.
            //Creates a new PDF document
            PdfDocument document = new PdfDocument();
            //Adds page settings
            document.PageSettings.Orientation = PdfPageOrientation.Portrait;
            document.PageSettings.Margins.All = 50;
            //Adds a page to the document
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;
            //Loads the image from disk
            //PdfImage image = PdfImage.FromFile(Server.MapPath("~/Photos/EmailLogo.PNG"));
            //RectangleF bounds = new RectangleF(10, 10, 200, 200);
            //Draws the image to the PDF page
            //page.Graphics.DrawImage(image, bounds);
            //PdfBrush solidBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
            //bounds = new RectangleF(0, bounds.Bottom + 90, graphics.ClientSize.Width, 30);
            ////Draws a rectangle to place the heading in that region.
            //graphics.DrawRectangle(solidBrush, bounds);
            //Creates a font for adding the heading in the page
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 14);
            //Creates a text element to add the invoice number
            PdfTextElement element = new PdfTextElement("Estimate #" + quote.UserID.ToString() + " for" + " " + quote.FirstName + " " + quote.LastName, subHeadingFont);
            element.Brush = PdfBrushes.White;

            //Draws the heading on the page
            //PdfLayoutResult res = element.Draw(page, new PointF(10, bounds.Top + 8));
            string currentDate = "Estimate Created On " + System.DateTime.Now.ToString(); //NoToString
            //Measures the width of the text to place it in the correct location
            //SizeF textSize = subHeadingFont.MeasureString(currentDate);
            //PointF textPosition = new PointF(graphics.ClientSize.Width - textSize.Width - 10, res.Bounds.Y);
            //Draws the date by using DrawString method
            //graphics.DrawString(currentDate, subHeadingFont, element.Brush, textPosition);
            //PdfSharp.Pdf.Advanced.PdfFont timesRoman = new PdfStandardFont(PdfFontFamily.TimesRoman, 10);
            //Creates text elements to add the address and draw it to the page.
            element = new PdfTextElement("Estimate for " + User.Identity.Name, timesRoman);
            element.Brush = new PdfSolidBrush(new PdfColor(126, 155, 203));
            //res = element.Draw(page, new PointF(10, res.Bounds.Bottom + 25));
            element = new PdfTextElement("Total Price R " + quote.estimatedPrice.ToString(), timesRoman);
            element.Brush = new PdfSolidBrush(new PdfColor(126, 155, 203));
            //res = element.Draw(page, new PointF(10, res.Bounds.Bottom + 25));
            PdfPen linePen = new PdfPen(new PdfColor(126, 151, 173), 0.70f);
            //PointF startPoint = new PointF(0, res.Bounds.Bottom + 3);
            //PointF endPoint = new PointF(graphics.ClientSize.Width, res.Bounds.Bottom + 3);
            //Draws a line at the bottom of the address
            //graphics.DrawLine(linePen, startPoint, endPoint);

            //Creates the datasource for the table
            DataTable invoiceDetails = new DataTable();

            //Add columns to the DataTable
            invoiceDetails.Columns.Add("Number of Adults");
            invoiceDetails.Columns.Add("Number of Kids");
            invoiceDetails.Columns.Add("Tours:");
            invoiceDetails.Columns.Add("Hotels:");
            invoiceDetails.Columns.Add("Flights:");
            invoiceDetails.Columns.Add("Cruises:");

            //Add rows to the DataTable
            invoiceDetails.Rows.Add(new object[] { quote.NumAdults, quote.NumKids, quote.TourL, quote.HotelL, quote.FlightL, quote.CruiseL });

            //Creates text elements to add the address and draw it to the page.
            //Creates a PDF grid
            PdfGrid grid = new PdfGrid();
            //Adds the data source
            grid.DataSource = invoiceDetails;
            //Creates the grid cell styles
            PdfGridCellStyle cellStyle = new PdfGridCellStyle();
            cellStyle.Borders.All = PdfPens.White;
            PdfGridRow header = grid.Headers[0];
            //Creates the header style
            PdfGridCellStyle headerStyle = new PdfGridCellStyle();
            headerStyle.Borders.All = new PdfPen(new PdfColor(126, 151, 173));
            headerStyle.BackgroundBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
            headerStyle.TextBrush = PdfBrushes.White;
            headerStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 14f, PdfFontStyle.Regular);

            //Adds cell customizations
            for (int i = 0; i < header.Cells.Count; i++)
            {
                if (i == 0 || i == 1)
                    header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                else
                    header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
            }

            //Applies the header style
            header.ApplyStyle(headerStyle);
            cellStyle.Borders.Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.70f);
            cellStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12f);
            cellStyle.TextBrush = new PdfSolidBrush(new PdfColor(131, 130, 136));
            //Creates the layout format for grid
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            // Creates layout format settings to allow the table pagination
            layoutFormat.Layout = PdfLayoutType.Paginate;
            //Draws the grid to the PDF page.
            //PdfGridLayoutResult gridResult = grid.Draw(page, new RectangleF(new PointF(0, res.Bounds.Bottom + 40), new SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)), layoutFormat);

            MemoryStream outputStream = new MemoryStream();
            document.Save(outputStream);
            outputStream.Position = 0;

            var invoicePdf = new System.Net.Mail.Attachment(outputStream, System.Net.Mime.MediaTypeNames.Application.Pdf);
            string docname = "Estimate.pdf";
            invoicePdf.ContentDisposition.FileName = docname;

            MailMessage mail = new MailMessage();
            string emailTo = User.Identity.Name;
            MailAddress from = new MailAddress("21529840@dut4life.ac.za");
            mail.From = from;
            mail.Subject = "Your estimate #" + quote.UserID;
            mail.Body = "Dear " + quote.FirstName + ", find your estimate in the attached PDF document.";
            mail.To.Add(emailTo);

            mail.Attachments.Add(invoicePdf);

            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp-mail.outlook.com";
            smtp.EnableSsl = true;
            NetworkCredential networkCredential = new NetworkCredential("21529840@dut4life.ac.za", "Dut970911");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = networkCredential;
            smtp.Port = 587;
            smtp.Send(mail);
            //Clean-up.
            //Close the document.
            document.Close(true);
            //Dispose of email.
            mail.Dispose();


            return RedirectToAction("ViewRecent");
        }

        // GET: Quotes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quote quote = await db.Quotes.FindAsync(id);
            if (quote == null)
            {
                return HttpNotFound();
            }
            return View(quote);
        }

        // POST: Quotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserID,FirstName,LastName,EmailAddress,PhoneNumber,NumAdults,NumKids,DepartureDate,ReturnDate,TourL,CruiseL,FlightL,HotelL,estimatedPrice")] Quote quote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quote).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(quote);
        }

        // GET: Quotes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quote quote = await db.Quotes.FindAsync(id);
            if (quote == null)
            {
                return HttpNotFound();
            }
            return View(quote);
        }

        // POST: Quotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Quote quote = await db.Quotes.FindAsync(id);
            db.Quotes.Remove(quote);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
