//	TextBox
//  Support class for PdfContents class. Format text to fit column.

using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// TextBoxLine class
    /// </summary>
    public class PdfTextBoxLine
    {
        /// <summary>
        /// Gets line ascent.
        /// </summary>
        public double Ascent { get; internal set; }

        /// <summary>
        /// Gets line descent.
        /// </summary>
        public double Descent { get; internal set; }

        /// <summary>
        /// Line is end of paragraph.
        /// </summary>
        public bool EndOfParagraph { get; internal set; }

        /// <summary>
        /// Gets array of line segments.
        /// </summary>
        public PdfTextBoxSeg[] SegArray { get; internal set; }

        /// <summary>
        /// Gets line height.
        /// </summary>
        public double LineHeight
        {
            get
            {
                return Ascent + Descent;
            }
        }

        /// <summary>
        /// TextBoxLine constructor.
        /// </summary>
        /// <param name="Ascent">Line ascent.</param>
        /// <param name="Descent">Line descent.</param>
        /// <param name="EndOfParagraph">Line is end of paragraph.</param>
        /// <param name="SegArray">Segments' array.</param>
        public PdfTextBoxLine
                (
                double Ascent,
                double Descent,
                bool EndOfParagraph,
                PdfTextBoxSeg[] SegArray
                )
        {
            this.Ascent = Ascent;
            this.Descent = Descent;
            this.EndOfParagraph = EndOfParagraph;
            this.SegArray = SegArray;
            return;
        }
    }
}
