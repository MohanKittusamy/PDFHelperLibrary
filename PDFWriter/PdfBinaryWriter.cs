//	PdfBinaryWriter
//	Extension to standard C# BinaryWriter class.

using System.Text;
using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// PDF binary writer class
    /// </summary>
    /// <remarks>
    /// Extends .NET BinaryWriter class.
    /// </remarks>
    public class PdfBinaryWriter : BinaryWriter
    {
        /// <summary>
        /// PDF binary writer constructor
        /// </summary>
        /// <param name="Stream">File or memory stream</param>
        public PdfBinaryWriter
                (
                Stream Stream
                ) : base(Stream, Encoding.UTF8) { }

        /// <summary>
        /// Write String.
        /// </summary>
        /// <param name="Str">Input string</param>
        /// <remarks>
        /// Convert each character from two bytes to one byte.
        /// </remarks>
        public void WriteString
                (
                string Str
                )
        {
            // write to pdf file
            Write(PdfByteArrayMethods.ToByteArray(Str));
            return;
        }

        /// <summary>
        /// Write String.
        /// </summary>
        /// <param name="Str">Input string</param>
        /// <remarks>
        /// Convert each character from two bytes to one byte.
        /// </remarks>
        public void WriteString
                (
                StringBuilder Str
                )
        {
            // write to pdf file
            Write(PdfByteArrayMethods.ToByteArray(Str.ToString()));
            return;
        }

        /// <summary>
        /// Combine format string with write string.
        /// </summary>
        /// <param name="FormatStr">Standard format string</param>
        /// <param name="List">Array of objects</param>
        public void WriteFormat
                (
                string FormatStr,
                params object[] List
                )
        {
            // write to pdf file
            Write(PdfByteArrayMethods.ToByteArray(string.Format(FormatStr, List)));
            return;
        }
    }
}
