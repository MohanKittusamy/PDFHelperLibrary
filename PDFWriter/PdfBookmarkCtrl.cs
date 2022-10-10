//	PdfBookmark control class

using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// Add bookmark control structure
    /// </summary>
    public class PdfBookmarkCtrl
    {
        /// <summary>
        /// Zoom factor. 1.0 is 100%. 0.0 is no change from existing zoom.
        /// </summary>
        public double Zoom = 1;

        /// <summary>
        /// Bookmark color
        /// </summary>
        public Color Color;

        /// <summary>
        /// Bookmark text style: normal, bold, italic, bold-italic
        /// </summary>
        public BookmarkTextStyle TextStyle;

        /// <summary>
        /// Open children on click
        /// true: display children, false: hide children
        /// </summary>
        public bool OpenEntries;
    }
}
