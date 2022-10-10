//	Annotation link action
using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// Link to location marker within the document
    /// </summary>
    public class PdfAnnotLinkAction : PdfAnnotation
    {
        internal string LocMarkerName { get; set; }

        /// <summary>
        /// Go to annotation action constructor
        /// </summary>
        /// <param name="Document">PDF document</param>
        /// <param name="LocMarkerName">Location marker name</param>
        public PdfAnnotLinkAction
                (
                PdfDocument Document,
                string LocMarkerName
                ) : base(Document, "/Link")
        {
            // save location marker
            this.LocMarkerName = LocMarkerName;

            // create a list of location links
            if (base.Document.LinkAnnotArray == null) base.Document.LinkAnnotArray = new List<PdfAnnotation>();
            base.Document.LinkAnnotArray.Add(this);
            return;
        }

        /// <summary>
        /// Duplicate annotation link action
        /// </summary>
        /// <param name="Other">Original link action</param>
        public PdfAnnotLinkAction
                (
                PdfAnnotLinkAction Other
                ) : base(Other.Document, "/Link")
        {
            // save location marker
            LocMarkerName = Other.LocMarkerName;

            // create a list of location links
            if (Document.LinkAnnotArray == null) Document.LinkAnnotArray = new List<PdfAnnotation>();
            Document.LinkAnnotArray.Add(this);

            // copy base values
            CreateCopy(Other);
            return;
        }
    }
}
