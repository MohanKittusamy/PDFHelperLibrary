//	PdfSymbol
//	Convert character font to series of lines and Bezier courves 

using System.Drawing.Drawing2D;
using System.Drawing;

namespace PDFWriter
{
    internal enum SymbolPointType
    {
        Start = 0, // The starting point of a GraphicsPath object.
        Line = 1, // A line segment.
        Bezier = 3, // Bézier curve.
        PathTypeMask = 7, // A mask point.	
        DashMode = 16, // The corresponding segment is dashed.
        PathMarker = 32, // A path marker.
        CloseSubpath = 128, // The endpoint of a subpath.
    }

    /// <summary>
    /// PdfSymbol class
    /// </summary>
    public class PdfSymbol
    {
        internal int Len;
        internal RectangleF Bounds;
        internal PointF[] Points;
        internal byte[] Types;

        /// <summary>
        /// PdfSymbol class constructor
        /// </summary>
        /// <param name="FontFamilyName">Font family name</param>
        /// <param name="Style">Font style</param>
        /// <param name="CharCode">Character code</param>
        public PdfSymbol
                (
                string FontFamilyName,
                FontStyle Style,
                int CharCode
                )
        {
            // convert character to graphics path
            GraphicsPath GP = new GraphicsPath();
            GP.AddString(((char)CharCode).ToString(), new FontFamily(FontFamilyName), (int)Style, 1000, Point.Empty, StringFormat.GenericDefault);
            Bounds = GP.GetBounds();

            // number of points
            Len = GP.PointCount;

            // points coordinats
            Points = GP.PathPoints;
            Types = GP.PathTypes;
            return;
        }
    }
}