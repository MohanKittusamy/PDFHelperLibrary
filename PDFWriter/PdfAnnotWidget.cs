//	Annotation widget

using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// Annotation widget for user interactive fields
    /// </summary>
    public class PdfAnnotWidget : PdfAnnotation
    {
        /// <summary>
        /// Border color (/BC)
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// Background color (/BG)
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Caption (/CA)
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Widget annotation (base for fields)
        /// </summary>
        /// <param name="Document">PDF document</param>
        internal PdfAnnotWidget
                (
                PdfDocument Document
                ) : base(Document, "/Widget")
        {
            BorderColor = Color.Empty;
            BackgroundColor = Color.Empty;
            return;
        }

        /// <summary>
        /// close object before writing to PDF file
        /// </summary>
        internal override void CloseObject()
        {
            // all but radio button
            if (GetType() != typeof(PdfAcroRadioButton))
            {
                // test for at least one color is defined
                if (BorderColor != Color.Empty || BackgroundColor != Color.Empty || Caption != null) // || CaptionPosition != CaptionPosStyle.NoIcon)
                {
                    // add appearance characteristics dictionary
                    PdfDictionary AppCharDict = new PdfDictionary(this);
                    Dictionary.AddDictionary("/MK", AppCharDict);

                    // border color
                    // add color array appearance characteristics dictionary
                    if (BorderColor != Color.Empty) AppCharDict.Add("/BC", PdfContents.ColorToString(BorderColor, ColorToStr.Array));

                    // background color
                    // add color array appearance characteristics dictionary
                    if (BackgroundColor != Color.Empty) AppCharDict.Add("/BG", PdfContents.ColorToString(BackgroundColor, ColorToStr.Array));

                    // caption
                    if (Caption != null) AppCharDict.AddPdfString("/CA", Caption);

                    // caption position style
                    // if(CaptionPosition != CaptionPosStyle.NoIcon) AppCharDict.AddInteger("/TP", (int) CaptionPosition);
                }
            }

            // close PdfAnnotation object
            base.CloseObject();
            return;
        }
    }
}
