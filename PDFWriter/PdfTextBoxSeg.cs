//	TextBox
//  Support class for PdfContents class. Format text to fit column.
using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// TextBox line segment class
    /// </summary>
    public class PdfTextBoxSeg : PdfDrawTextCtrl
    {
        /// <summary>
        /// Gets segment width.
        /// </summary>
        public double SegWidth { get; internal set; }

        /// <summary>
        /// Gets segment space character count.
        /// </summary>
        public int SpaceCount { get; internal set; }

        /// <summary>
        /// Gets segment text.
        /// </summary>
        public string Text { get; internal set; }

        /// <summary>
        /// Text box segment constructor.
        /// </summary>
        /// <param name="TextCtrl">Segment text control.</param>
        public PdfTextBoxSeg
                (
                PdfDrawTextCtrl TextCtrl
                ) : base(TextCtrl)
        {
            Justify = TextJustify.Left;
            Text = string.Empty;
            return;
        }

        /// <summary>
        /// Text box segment copy constructor.
        /// </summary>
        /// <param name="Other">Segment text control.</param>
        public PdfTextBoxSeg
                (
                PdfTextBoxSeg Other
                ) : base(Other)
        {
            Justify = TextJustify.Left;
            Text = string.Empty;          
            return;
        }
    }
}
