//	PdfKerningAdjust class
using System.Drawing;

namespace PDFWriter
{
    /////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Kerning adjustment class
    /// </summary>
    /// <remarks>
    /// Text position adjustment for TJ operator.
    /// The adjustment is for a font height of one point.
    /// Mainly used for font kerning.
    /// </remarks>
    /////////////////////////////////////////////////////////////////////
    public class PdfKerningAdjust
    {
        /// <summary>
        /// Gets or sets Text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets adjustment
        /// </summary>
        /// <remarks>
        /// Adjustment units are in PDF design unit. To convert to user units: Adjust * FontSize / (1000.0 * ScaleFactor)
        /// </remarks>
        public double Adjust { get; set; }

        /// <summary>
        /// Kerning adjustment constructor
        /// </summary>
        /// <param name="Text">Text</param>
        /// <param name="Adjust">Adjustment</param>
        public PdfKerningAdjust
                (
                string Text,
                double Adjust
                )
        {
            this.Text = Text;
            this.Adjust = Adjust;
            return;
        }
    }
}
