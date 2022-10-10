//	Location marker
//	Internal class for managing document's location markers. 
using System.Text;
using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// Location marker scope
    /// </summary>
    public enum LocMarkerScope
    {
        /// <summary>
        /// Location marker is local
        /// </summary>
        LocalDest,

        /// <summary>
        /// Location marker is global and local
        /// </summary>
        NamedDest,
    }

    /// <summary>
    /// Named destination fit constants
    /// </summary>
    public enum DestFit
    {
        /// <summary>
        /// Fit entire page. No arguments.
        /// </summary>
        /// <remarks>
        /// Display the page designated by page, with its contents magnified
        /// just enough to fit the entire page within the window both horizontally
        /// and vertically. If the required horizontal and vertical magnification
        /// factors are different, use the smaller of the two, centering the page
        /// within the window in the other dimension.
        /// </remarks>
        Fit,

        /// <summary>
        /// Display the page with top coordinate at top of the window. One argument.
        /// </summary>
        /// <remarks>
        /// Display the page designated by page, with the vertical coordinate top
        /// positioned at the top edge of the window and the contents of the page
        /// magnified just enough to fit the entire width of the page within the
        /// window. A null value for top specifies that the current value of that
        /// parameter is to be retained unchanged.
        /// </remarks>
        FitH,

        /// <summary>
        /// Display the page with left coordinate at left side of the window. One argument.
        /// </summary>
        /// <remarks>
        /// Display the page designated by page, with the horizontal coordinate left
        /// positioned at the left edge of the window and the contents of the page
        /// magnified just enough to fit the entire height of the page within the
        /// window. A null value for left specifies that the current value of that
        /// parameter is to be retained unchanged.
        /// </remarks>
        FitV,

        /// <summary>
        /// Display the page within rectangle. Four argument.
        /// </summary>
        /// <remarks>
        /// Display the page designated by page, with its contents magnified just enough
        /// to fit the rectangle specified by the coordinates left, bottom, right, and
        /// topentirely within the window both horizontally and vertically. If the required
        /// horizontal and vertical magnification factors are different, use the smaller
        /// of the two, centering the rectangle within the window in the other dimension.
        /// A null value for any of the parameters may result in unpredictable behavior.
        /// </remarks>
        FitR,

        /// <summary>
        /// Fit entire page including boundig box. No arguments.
        /// </summary>
        /// <remarks>
        /// Display the page designated by page, with its contents magnified just enough
        /// to fit its bounding box entirely within the window both horizontally and
        /// vertically. If the required horizontal and vertical magnification factors are
        /// different, use the smaller of the two, centering the bounding box within the
        /// window in the other dimension.
        /// </remarks>
        FitB,

        /// <summary>
        /// Display the page with top coordinate at top of the window. One argument.
        /// </summary>
        /// <remarks>
        /// Display the page designated by page, with the vertical coordinate top positioned
        /// at the top edge of the window and the contents of the page magnified just enough
        /// to fit the entire width of its bounding box within the window. A null value for
        /// top specifies that the current value of that parameter is to be retained unchanged.
        /// </remarks>
        FitBH,

        /// <summary>
        /// Display the page with left coordinate at left side of the window. One argument.
        /// </summary>
        /// <remarks>
        /// Display the page designated by page, with the horizontal coordinate left positioned
        /// at the left edge of the window and the contents of the page magnified just enough
        /// to fit the entire height of its bounding box within the window. A null value for
        /// left specifies that the current value of that parameter is to be retained unchanged.
        /// </remarks>
        FitBV,
    }

    internal class PdfLocationMarker : IComparable<PdfLocationMarker>
    {
        internal static string[] FitString =
            {
            "/Fit",
            "/FitH",
            "/FitV",
            "/FitR",
            "/FitB",
            "/FitBH",
            "/FitBV",
            };

        internal static int[] FitArguments = { 0, 1, 1, 4, 0, 1, 1 };

        internal string LocMarkerName;
        internal LocMarkerScope Scope;
        internal string DestStr;

        // Do not call this constructor from your code
        internal PdfLocationMarker
                (
                string LocMarkerName,
                PdfPage LoctionMarkerPage,
                LocMarkerScope Scope,
                DestFit FitArg,
                params double[] SideArg
                )
        {
            if (SideArg.Length != FitArguments[(int)FitArg])
                throw new ApplicationException("AddDestination invalid number of arguments");
            this.LocMarkerName = LocMarkerName;
            this.Scope = Scope;
            StringBuilder BuildDest = new StringBuilder();
            BuildDest.AppendFormat("[{0} 0 R {1}", LoctionMarkerPage.ObjectNumber, FitString[(int)FitArg]);
            foreach (double Side in SideArg) BuildDest.AppendFormat(NFI.PeriodDecSep, " {0}", LoctionMarkerPage.ToPt(Side));
            BuildDest.Append(']');
            DestStr = BuildDest.ToString();
            return;
        }

        internal PdfLocationMarker
                (
                string LocMarkerName
                )
        {
            this.LocMarkerName = LocMarkerName;
            return;
        }

        public int CompareTo
                (
                PdfLocationMarker Other
                )
        {
            return string.CompareOrdinal(LocMarkerName, Other.LocMarkerName);
        }
    }
}
