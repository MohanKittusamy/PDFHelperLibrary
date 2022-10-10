//	Annotations file attachment
using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// File attachement icon
    /// </summary>
    public enum FileAttachIcon
    {
        /// <summary>
        /// PushPin (28 by 40) (default)
        /// </summary>
        PushPin,

        /// <summary>
        /// Graph (40 by 40)
        /// </summary>
        Graph,

        /// <summary>
        /// Paperclip (14 by 34)
        /// </summary>
        Paperclip,

        /// <summary>
        /// Tag (40 by 32)
        /// </summary>
        Tag,

        /// <summary>
        /// no icon 
        /// </summary>
        NoIcon,
    }

    /// <summary>
    /// Save or view embedded file
    /// </summary>
    public class PdfAnnotFileAttachment : PdfAnnotation
    {
        internal PdfEmbeddedFile EmbeddedFile;
        internal FileAttachIcon Icon;

        /// <summary>
        /// Icon aspect ratio
        /// </summary>
        public static readonly double[] IconAspectRatio =
            {
            0.7, // push pin
			1.0, // graph
			0.4128, // paper clip
			1.25, // tag
			};

        /// <summary>
        /// File attachement constructor
        /// </summary>
        /// <param name="Document">PDF document</param>
        /// <param name="EmbeddedFile">Embedded file</param>
        /// <param name="Icon">Icon enumeration</param>
        public PdfAnnotFileAttachment
                (
                PdfDocument Document,
                PdfEmbeddedFile EmbeddedFile,
                FileAttachIcon Icon = FileAttachIcon.NoIcon
                ) : base(Document, "/FileAttachment")
        {
            // save embeded file and icon
            this.EmbeddedFile = EmbeddedFile;
            this.Icon = Icon;

            // common constructor code
            Constructor();
            return;
        }

        /// <summary>
        /// File attachment copy constructor
        /// </summary>
        /// <param name="Other">Another file attachment</param>
        public PdfAnnotFileAttachment
                (
                PdfAnnotFileAttachment Other
                ) : base(Other.Document, "/FileAttachment")
        {
            // save embeded file and icon
            EmbeddedFile = Other.EmbeddedFile;
            Icon = Other.Icon;

            // common constructor code
            Constructor();

            // copy base values
            CreateCopy(Other);
            return;
        }

        private void Constructor()
        {
            // add embedded file to annotation dictionary
            Dictionary.AddIndirectReference("/FS", EmbeddedFile);

            // no icon
            if (Icon == FileAttachIcon.NoIcon)
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
