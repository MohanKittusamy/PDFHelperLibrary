//	Annotation sticky note
using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// Sticky note icon
    /// </summary>
    public enum StickyNoteIcon
    {
        /// <summary>
        /// Comment (note: no icon)
        /// </summary>
        Comment,
        /// <summary>
        /// Key
        /// </summary>
        Key,
        /// <summary>
        /// Note (default)
        /// </summary>
        Note,
        /// <summary>
        /// Help
        /// </summary>
        Help,
        /// <summary>
        /// New paragraph
        /// </summary>
        NewParagraph,
        /// <summary>
        /// Paragraph
        /// </summary>
        Paragraph,
        /// <summary>
        /// Insert
        /// </summary>
        Insert,
        /// <summary>
        /// No icon
        /// </summary>
        NoIcon,
    }

    /// <summary>
    /// Display sticky note
    /// </summary>
    public class PdfAnnotStickyNote : PdfAnnotation
    {
        internal string Note;
        internal StickyNoteIcon Icon;

        /// <summary>
        /// Sticky note annotation action constructor
        /// </summary>
        /// <param name="Document">PDF document</param>
        /// <param name="Note">Sticky note text</param>
        /// <param name="Icon">Sticky note icon</param>
        public PdfAnnotStickyNote
                (
                PdfDocument Document,
                string Note,
                StickyNoteIcon Icon = StickyNoteIcon.NoIcon
                ) : base(Document, "/Text")
        {
            // save arguments
            this.Note = Note;
            this.Icon = Icon;

            // call constructor
            Constructor();
            return;
        }

        /// <summary>
        /// Sticky note annotation action constructor
        /// </summary>
        /// <param name="Other">Other stick note object</param>
        public PdfAnnotStickyNote
                (
                PdfAnnotStickyNote Other
                ) : base(Other.Document, "/Text")
        {
            // save arguments
            Note = Other.Note;
            Icon = Other.Icon;

            // call constructor
            Constructor();

            // copy base values
            CreateCopy(Other);
            return;
        }

        private void Constructor()
        {
            // action reference dictionary
            Dictionary.AddPdfString("/Contents", Note);

            // no icon
            if (Icon == StickyNoteIcon.NoIcon)
            {
                // no icon (override icon with empty appearance xobject)
                PdfXObject XObject = new PdfXObject(Document, 0, 0);
                AddAppearance(XObject, AppearanceType.Normal);
            }

            // icon
            else
            {
                Dictionary.AddName("/Name", Icon.ToString());
            }
            return;
        }
    }
}
