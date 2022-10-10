//	PdfInfo
//	PDF document information dictionary.
using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// PDF document information dictionary
    /// </summary>
    public class PdfInfo : PdfObject
    {
        /// <summary>
        /// Constructor for PdfInfo class
        /// </summary>
        /// <param name="Document">Main document class</param>
        /// <returns>PdfInfo object</returns>
        /// <remarks>
        /// <para>The constructor initialize the /Info dictionary with 4 key value pairs. </para>
        /// <list type="table">
        /// <item><description>Creation date set to current local system date</description></item>
        /// <item><description>Modification date set to current local system date</description></item>
        /// <item><description>Creator is PDFWriter.Lib C# Class Library Version No</description></item>
        /// <item><description>Producer is PDFWriter.Lib C# Class Library Version No</description></item>
        /// </list>
        /// </remarks>
        public static PdfInfo CreatePdfInfo
                (
                PdfDocument Document
                )
        {
            // create a new default info object
            if (Document.InfoObject == null)
            {
                // create and add info object to trailer dictionary
                Document.InfoObject = new PdfInfo(Document);
            }

            // exit with either existing object or a new one
            return Document.InfoObject;
        }

        /// <summary>
        /// Protected constructor
        /// </summary>
        /// <param name="Document">Main document object</param>
        protected PdfInfo
                (
                PdfDocument Document
                ) : base(Document, ObjectType.Dictionary)
        {
            // set creation and modify dates
            DateTime LocalTime = DateTime.Now;
            CreationDate(LocalTime);
            ModDate(LocalTime);

            // set creator and producer
            Creator("PDFWriter.Lib C# Class Library Version " + PdfDocument.RevisionNumber);
            Producer("PDFWriter.Lib C# Class Library Version " + PdfDocument.RevisionNumber);
            return;
        }

        /// <summary>
        /// Sets document creation date and time
        /// </summary>
        /// <param name="Date">Creation date and time</param>
        public void CreationDate
                (
                DateTime Date
                )
        {
            Dictionary.AddPdfString("/CreationDate", string.Format("D:{0}", Date.ToString("yyyyMMddHHmmss")));
            return;
        }

        /// <summary>
        /// Sets document last modify date and time
        /// </summary>
        /// <param name="Date">Modify date and time</param>
        public void ModDate
                (
                DateTime Date
                )
        {
            Dictionary.AddPdfString("/ModDate", string.Format("D:{0}", Date.ToString("yyyyMMddHHmmss")));
            return;
        }

        /// <summary>
        /// Sets document title
        /// </summary>
        /// <param name="Title">Title</param>
        public void Title
                (
                string Title
                )
        {
            Dictionary.AddPdfString("/Title", Title);
            return;
        }

        /// <summary>
        /// Sets document author 
        /// </summary>
        /// <param name="Author">Author</param>
        public void Author
                (
                string Author
                )
        {
            Dictionary.AddPdfString("/Author", Author);
            return;
        }

        /// <summary>
        /// Sets document subject
        /// </summary>
        /// <param name="Subject">Subject</param>
        public void Subject
                (
                string Subject
                )
        {
            Dictionary.AddPdfString("/Subject", Subject);
            return;
        }

        /// <summary>
        /// Sets keywords associated with the document
        /// </summary>
        /// <param name="Keywords">Keywords list</param>
        public void Keywords
                (
                string Keywords
                )
        {
            Dictionary.AddPdfString("/Keywords", Keywords);
            return;
        }

        /// <summary>
        /// Sets the name of the application that created the document
        /// </summary>
        /// <param name="Creator">Creator</param>
        public void Creator
                (
                string Creator
                )
        {
            Dictionary.AddPdfString("/Creator", Creator);
            return;
        }

        /// <summary>
        /// Sets the name of the application that produced the document
        /// </summary>
        /// <param name="Producer">Producer</param>
        public void Producer
                (
                string Producer
                )
        {
            Dictionary.AddPdfString("/Producer", Producer);
            return;
        }
    }
}
