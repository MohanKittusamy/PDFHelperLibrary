//	Draw barcode control class

using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// Draw barcode control class
    /// </summary>
    public class PdfDrawBarcodeCtrl
    {
        /// <summary>
        /// Narrow bar width
        /// </summary>
        public double NarrowBarWidth;

        /// <summary>
        /// Bar code height
        /// </summary>
        public double Height;

        /// <summary>
        /// Bar code justify (left, center, right)
        /// </summary>
        public BarcodeJustify Justify;

        /// <summary>
        /// Bar code color.
        /// The default is black and it is recommended to keep it black
        /// </summary>
        public Color Color = Color.Black;

        /// <summary>
        /// Bar code value draw text control
        /// </summary>
        public PdfDrawTextCtrl TextCtrl;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PdfDrawBarcodeCtrl() { }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="Other">Bar code control to duplicate</param>
        public PdfDrawBarcodeCtrl
                (
                PdfDrawBarcodeCtrl Other
                )
        {
            NarrowBarWidth = Other.NarrowBarWidth;
            Height = Other.Height;
            Justify = Other.Justify;
            Color = Other.Color;
            TextCtrl = Other.TextCtrl;
            return;
        }
    }
}
