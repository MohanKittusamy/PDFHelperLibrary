/////////////////////////////////////////////////////////////////////
//
//	PDFWriter.Lib II
//	PDF File Write C# Class Library.
//
//	PdfPage
//	PDF page class. An indirect PDF object.
//


using System.Text;
using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// PDF page class
    /// </summary>
    /// <remarks>
    /// PDF page class represent one page in the document.
    /// </remarks>
    public class PdfPage : PdfObject
    {
        internal double Width; // in points
        internal double Height; // in points
        internal List<PdfContents> ContentsArray;

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="Document">Parent PDF document object</param>
        /// <remarks>
        /// Page size is taken from PdfDocument
        /// </remarks>
        ////////////////////////////////////////////////////////////////////
        public PdfPage
                (
                PdfDocument Document
                ) : base(Document, ObjectType.Dictionary, "/Page")
        {
            Width = Document.PageSize.Width;
            Height = Document.PageSize.Height;
            ConstructorHelper();
            return;
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Document">Parent PDF document object</param>
        /// <param name="PageSize">Paper size for this page</param>
        /// <remarks>
        /// PageSize override the default page size
        /// </remarks>
        ////////////////////////////////////////////////////////////////////
        public PdfPage
                (
                PdfDocument Document,
                SizeD PageSize
                ) : base(Document, ObjectType.Dictionary, "/Page")
        {
            Width = ScaleFactor * PageSize.Width;
            Height = ScaleFactor * PageSize.Height;
            ConstructorHelper();
            return;
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Document">Parent PDF document object</param>
        /// <param name="PaperType">Paper type</param>
        /// <param name="Landscape">If Lanscape is true, width and height are swapped.</param>
        /// <remarks>
        /// PaperType and orientation override the default page size.
        /// </remarks>
        ////////////////////////////////////////////////////////////////////
        public PdfPage
                (
                PdfDocument Document,
                PaperType PaperType,
                bool Landscape
                ) : base(Document, ObjectType.Dictionary, "/Page")
        {
            // get standard paper size
            Width = PdfDocument.PaperTypeSize[(int)PaperType].Width;
            Height = PdfDocument.PaperTypeSize[(int)PaperType].Height;

            // for landscape swap width and height
            if (Landscape)
            {
                double Temp = Width;
                Width = Height;
                Height = Temp;
            }

            // exit
            ConstructorHelper();
            return;
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Document">Parent PDF document object</param>
        /// <param name="Width">Page width</param>
        /// <param name="Height">Page height</param>
        /// <remarks>
        /// Width and Height override the default page size
        /// </remarks>
        ////////////////////////////////////////////////////////////////////
        public PdfPage
                (
                PdfDocument Document,
                double Width,
                double Height
                ) : base(Document, ObjectType.Dictionary, "/Page")
        {
            this.Width = ScaleFactor * Width;
            this.Height = ScaleFactor * Height;
            ConstructorHelper();
            return;
        }

        /// <summary>
        /// Clone Constructor
        /// </summary>
        /// <param name="Page">Existing page object</param>
        public PdfPage
                (
                PdfPage Page
                ) : base(Page.Document, ObjectType.Dictionary, "/Page")
        {
            Width = Page.Width;
            Height = Page.Height;
            ConstructorHelper();
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Constructor common method
        ////////////////////////////////////////////////////////////////////

        private void ConstructorHelper()
        {
            // save this page as current page
            Document.CurrentPage = this;

            // add page to parent array of pages
            Document.PageArray.Add(this);

            // link page to parent
            Dictionary.AddIndirectReference("/Parent", Document.PagesObject);

            // add page size in points
            Dictionary.AddFormat("/MediaBox", "[0 0 {0} {1}]", Round(Width), Round(Height));

            // exit
            return;
        }

        /// <summary>
        /// Page size
        /// </summary>
        /// <returns>Page size</returns>
        /// <remarks>Page size in user units of measure. If Width is less than height
        /// orientation is portrait. Otherwise orientation is landscape.</remarks>
        public SizeD PageSize()
        {
            return new SizeD(Width / ScaleFactor, Height / ScaleFactor);
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Add existing contents to page
        /// </summary>
        /// <param name="Contents">Contents object</param>
        ////////////////////////////////////////////////////////////////////
        public void AddContents
                (
                PdfContents Contents
                )
        {
            // set page contents flag
            Contents.PageContents = true;

            // add content to content array
            if (ContentsArray == null) ContentsArray = new List<PdfContents>();
            ContentsArray.Add(Contents);

            // exit
            return;
        }

        /// <summary>
        /// Gets the current contents of this page
        /// </summary>
        /// <returns>Page's current contents</returns>
        public PdfContents GetCurrentContents()
        {
            return ContentsArray == null || ContentsArray.Count == 0 ? null : ContentsArray[^1];
        }

        ////////////////////////////////////////////////////////////////////
        // close object before writing to PDF file
        ////////////////////////////////////////////////////////////////////

        internal override void CloseObject()
        {
            // we have at least one contents object
            if (ContentsArray != null)
            {
                // page has one contents object
                if (ContentsArray.Count == 1)
                {
                    Dictionary.AddFormat("/Contents", "[{0} 0 R]", ContentsArray[0].ObjectNumber);
                    Dictionary.Add("/Resources", BuildResourcesDictionary(ContentsArray[0].ResObjects));
                }

                // page is made of multiple contents
                else
                {
                    // contents dictionary entry
                    StringBuilder ContentsStr = new StringBuilder("[");

                    // build contents dictionary entry
                    foreach (PdfContents Contents in ContentsArray) ContentsStr.AppendFormat("{0} 0 R ", Contents.ObjectNumber);

                    // add terminating bracket
                    ContentsStr.Length--;
                    ContentsStr.Append(']');
                    Dictionary.Add("/Contents", ContentsStr.ToString());

                    // resources array of all contents objects
                    List<PdfObject> ResObjects = new List<PdfObject>();

                    // loop for all contents objects
                    foreach (PdfContents Contents in ContentsArray)
                    {
                        // make sure we have resources
                        if (Contents.ResObjects != null)
                        {
                            // loop for resources within this contents object
                            foreach (PdfObject ResObject in Contents.ResObjects)
                            {
                                // check if we have it already
                                int Ptr = ResObjects.BinarySearch(ResObject);
                                if (Ptr < 0) ResObjects.Insert(~Ptr, ResObject);
                            }
                        }
                    }

                    // save to dictionary
                    Dictionary.Add("/Resources", BuildResourcesDictionary(ResObjects));
                }
            }

            // exit
            return;
        }
    }
}
