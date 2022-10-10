//	Annotation web link

using System.Text;
using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// Web link annotation action
    /// </summary>
    public class PdfAnnotWebLink : PdfAnnotation
    {
        internal PdfWebLink WebLink;

        /// <summary>
        /// Web link constructor
        /// </summary>
        /// <param name="Document">PDF document</param>
        /// <param name="WebLinkStr">Web link string</param>
        public PdfAnnotWebLink
                (
                PdfDocument Document,
                string WebLinkStr
                ) : base(Document, "/Link")
        {
            // encode unicode characters
            StringBuilder OutputLink = new StringBuilder();
            foreach (char Chr in WebLinkStr)
            {
                if (Chr <= ' ') OutputLink.AppendFormat("%{0:x2}", (int)Chr);
                else if (Chr <= '~') OutputLink.Append(Chr);
                else if (Chr <= 255) OutputLink.AppendFormat("%{0:x2}", (int)Chr);
                else
                {
                    byte[] UtfBytes = Encoding.UTF8.GetBytes(Chr.ToString());
                    foreach (byte Byte in UtfBytes)
                    {
                        OutputLink.AppendFormat("%{0:x2}", (int)Byte);
                    }
                }
            }

            // save normalized web link
            WebLink = PdfWebLink.Create(base.Document, OutputLink.ToString());

            // add web link to annotation dictionary
            Dictionary.AddIndirectReference("/A", WebLink);
            return;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="Other">Annotation to copy</param>
        public PdfAnnotWebLink
                (
                PdfAnnotWebLink Other
                ) : base(Other.Document, "/Link")
        {
            // save location marker
            WebLink = Other.WebLink;

            // add web link to annotation dictionary
            Dictionary.AddIndirectReference("/A", WebLink);

            // call base create copy
            CreateCopy(Other);
            return;
        }
    }
}
