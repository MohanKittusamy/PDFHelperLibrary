//	PDFJavaScript
//	Acro field support for Java Script

using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// Jave Script class
    /// </summary>
    public class PdfJavaScript : PdfObject
    {
        /// <summary>
        /// Java script class constructor
        /// </summary>
        /// <param name="Document">PDF document class</param>
        /// <param name="JavaScript">Java script text</param>
        public PdfJavaScript
                (
                PdfDocument Document,
                string JavaScript
                ) : base(Document)
        {
            Dictionary.AddPdfString("/JS", JavaScript);
            Dictionary.AddName("/S", "JavaScript");
            return;
        }
    }
}
