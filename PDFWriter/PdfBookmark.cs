//	PdfBookmark
//	Bookmars or document outline support.
using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// Bookmark text style enumeration
    /// </summary>
    public enum BookmarkTextStyle
    {
        /// <summary>
        /// Normal
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Italic
        /// </summary>
        Italic = 1,

        /// <summary>
        /// Bold
        /// </summary>
        Bold = 2,

        /// <summary>
        /// Bold and italic
        /// </summary>
        BoldItalic = 3,
    }

    /// <summary>
    /// PDF bookmark class
    /// </summary>
    /// <remarks>
    /// For more information go to <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#BookmarkSupport">2.9 Bookmark Support</a>
    /// </remarks>
    public class PdfBookmark : PdfObject
    {
        private readonly bool OpenEntries;
        private PdfBookmark Parent;
        private PdfBookmark FirstChild;
        private PdfBookmark PrevSibling;
        private PdfBookmark NextSibling;
        private PdfBookmark LastChild;
        private int Count;

        ////////////////////////////////////////////////////////////////////
        // Bookmarks (Document Outline) Root Constructor
        // Must be called from PdfDocument.GetBookmarksRoot() method
        // This constructor is called one time only
        ////////////////////////////////////////////////////////////////////

        internal PdfBookmark
                (
                PdfDocument Document
                ) : base(Document)
        {
            // open first level bookmarks
            OpenEntries = true;

            // add /Outlines to catalog dictionary
            Document.CatalogObject.Dictionary.AddIndirectReference("/Outlines", this);
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Create bookmark item
        // Must be called from AddBookmark method below
        // This constructor is called for each bookmark
        ////////////////////////////////////////////////////////////////////

        private PdfBookmark
                (
                PdfDocument Document,
                bool OpenEntries
                ) : base(Document)
        {
            // open first level bookmarks
            this.OpenEntries = OpenEntries;
            return;
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Add child bookmark
        /// </summary>
        /// <param name="BookmarkCtrl">Bookmark control</param>
        /// <param name="Title">Bookmark title.</param>
        /// <param name="Page">Page</param>
        /// <param name="XPos">Horizontal position</param>
        /// <param name="YPos">Vertical position.</param>
        /// <returns>Bookmark object</returns>
        /// <remarks>
        /// Add bookmark as a child to this bookmark.
        /// This method creates a new child bookmark item attached
        /// to this parent
        /// </remarks>
        ////////////////////////////////////////////////////////////////////
        public PdfBookmark AddBookmark
                (
                PdfBookmarkCtrl BookmarkCtrl,
                string Title,
                PdfPage Page,
                double XPos,
                double YPos
                )
        {
            // create new bookmark
            PdfBookmark Bookmark = new PdfBookmark(Document, BookmarkCtrl.OpenEntries);

            // attach to parent
            Bookmark.Parent = this;

            // this bookmark is first child
            if (FirstChild == null)
            {
                FirstChild = Bookmark;
                LastChild = Bookmark;
            }

            // this bookmark is not first child
            else
            {
                LastChild.NextSibling = Bookmark;
                Bookmark.PrevSibling = LastChild;
                LastChild = Bookmark;
            }

            // the parent of this bookmark displays all children
            if (OpenEntries)
            {
                // update count
                Count++;
                for (PdfBookmark TestParent = Parent; TestParent != null && TestParent.OpenEntries; TestParent = TestParent.Parent)
                    TestParent.Count++;
            }
            // the parent of this bookmark does not display all children
            else
            {
                Count--;
            }

            // build dictionary
            Bookmark.Dictionary.AddPdfString("/Title", Title);
            Bookmark.Dictionary.AddIndirectReference("/Parent", this);
            Bookmark.Dictionary.AddFormat("/Dest", "[{0} 0 R /XYZ {1} {2} {3}]", Page.ObjectNumber, ToPt(XPos), ToPt(YPos), Round(BookmarkCtrl.Zoom));
            if (BookmarkCtrl.Color != Color.Empty) Bookmark.Dictionary.Add("/C", PdfContents.ColorToString(BookmarkCtrl.Color, ColorToStr.Array));
            if (BookmarkCtrl.TextStyle != BookmarkTextStyle.Normal) Bookmark.Dictionary.AddInteger("/F", (int)BookmarkCtrl.TextStyle);
            return Bookmark;
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets child bookmark
        /// </summary>
        /// <param name="IndexArray">Array of indices</param>
        /// <returns>Child bookmark or null if not found.</returns>
        /// <remarks>
        /// Gets PdfBookmark object based on index.
        /// You can have multiple indices separated by commas
        /// i.e. GetChild(2, 3);
        /// Index is zero based. In the example we are looking first for
        /// the third bookmark child and then the forth bookmark of the 
        /// next level.
        /// </remarks>
        ////////////////////////////////////////////////////////////////////
        public PdfBookmark GetChild
                (
                params int[] IndexArray
                )
        {
            PdfBookmark Bookmark = this;
            PdfBookmark Child = null;
            for (int Level = 0; Level < IndexArray.Length; Level++, Bookmark = Child)
            {
                // get index number for level
                int Index = IndexArray[Level];

                // find the child at this level with the given index
                for (Child = Bookmark.FirstChild; Index > 0 && Child != null; Child = Child.NextSibling, Index--) ;

                // not found
                if (Child == null) return null;
            }

            return Child;
        }

        ////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////
        internal override void CloseObject()
        {
            // update dictionary
            if (FirstChild != null) Dictionary.AddIndirectReference("/First", FirstChild);
            if (LastChild != null) Dictionary.AddIndirectReference("/Last", LastChild);
            if (Count != 0) Dictionary.AddInteger("/Count", Count);

            // all but root
            if (Parent != null)
            {
                if (PrevSibling != null) Dictionary.AddIndirectReference("/Prev", PrevSibling);
                if (NextSibling != null) Dictionary.AddIndirectReference("/Next", NextSibling);
            }

            // exit
            return;
        }
    }
}
