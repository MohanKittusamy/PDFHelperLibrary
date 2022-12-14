//	Number Format Information static class
//	Adobe readers expect decimal separator to be a period.
//	Some countries define decimal separator as a comma.
//	The project uses NFI.DecSep to force period for all regions.

using System.Globalization;
using System.Drawing;

namespace PDFWriter
{
    /////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Number Format Information static class
    /// </summary>
    /// <remarks>
    /// Adobe readers expect decimal separator to be a period.
    /// Some countries define decimal separator as a comma.
    /// The project uses NFI.DecSep to force period for all regions.
    /// </remarks>
    /////////////////////////////////////////////////////////////////////
    public static class NFI
    {
        /// <summary>
        /// Define period as number decimal separator.
        /// </summary>
        /// <remarks>
        /// NumberFormatInfo is used with string formatting to set the
        /// decimal separator to a period regardless of region.
        /// </remarks>
        public static NumberFormatInfo PeriodDecSep { get; private set; }

        // static constructor
        static NFI()
        {
            // number format (decimal separator is period)
            PeriodDecSep = new NumberFormatInfo();
            PeriodDecSep.NumberDecimalSeparator = ".";
            return;
        }
    }
}
