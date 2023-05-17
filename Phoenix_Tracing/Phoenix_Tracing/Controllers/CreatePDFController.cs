using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Phoenix_Tracing.Controllers
{
    public class CreatePDFController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BuildPDF()
        {
            //PDF belgesi oluştur.
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Periyodik Rapor";

            //Boş bir sayfa oluşturma.
            PdfPage page = document.AddPage();

            //Çizim için bir XGraphics nesnesi alın
            // Pdf sayfasını doldurmak için XGraphics nesnesi oluşturma.
            XGraphics gfx = XGraphics.FromPdfPage(page);

            //Yazı tipini oluşturma
            XFont font = new XFont("Times New Roman", 20, XFontStyle.BoldItalic);

            return View();
        }
    }
}