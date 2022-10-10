//	PdfXObject
//	PDF X Object resource class.
using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// PDF X object resource class
    /// </summary>
    public class PdfXObject : PdfContents
    {
        // bounding rectangle
        /// <summary>
        /// XObject bounding rectangle
        /// </summary>
        public PdfRectangle BBox { get; }

        /// <summary>
        /// PDF X Object constructor
        /// </summary>
        /// <param name="Document">PDF document</param>
        /// <param name="Width">X Object width</param>
        /// <param name="Height">X Object height</param>
        public PdfXObject
                (
                PdfDocument Document,
                double Width = 1.0,
                double Height = 1.0
                ) : base(Document, "/XObject")
        {
            // create resource code
            ResourceCode = Document.GenerateResourceNumber('X');

            // add subtype to dictionary
            Dictionary.Add("/Subtype", "/Form");
            //			Dictionary.Add("/FormType", "1");
            //			Dictionary.Add("/Matrix", "[1 0 0 1 0 0]");

            // set boundig box rectangle
            BBox = new PdfRectangle(0, 0, Width, Height);
            Dictionary.AddRectangle("/BBox", BBox);
            return;
        }

        /// <summary>
        /// Layer control
        /// </summary>
        /// <param name="Layer">PdfLayer object</param>
        public void LayerControl
                (
                PdfObject Layer
                )
        {
            Dictionary.AddIndirectReference("/OC", Layer);
            return;
        }
    }
}
