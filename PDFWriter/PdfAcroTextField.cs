//	Acro text field
using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// Acro text field
    /// </summary>
    public class PdfAcroTextField : PdfAcroWidgetField
    {
        /// <summary>
        /// Acro text field maximum length
        /// </summary>
        public int TextMaxLength;

        /// <summary>
        /// Text field value (/V)
        /// </summary>
        public string FieldValue;

        /// <summary>
        /// Text field default value (/DV)
        /// </summary>
        public string DefaultValue;

        /// <summary>
        /// Multi-line text
        /// </summary>
        public bool Multiline
        {
            get
            {
                return (FieldFlags & FieldFlag.Multiline) != 0;
            }
            set
            {
                if (value)
                {
                    FieldFlags |= FieldFlag.Multiline;
                }
                else
                {
                    FieldFlags &= ~FieldFlag.Multiline;
                }
                return;
            }
        }

        /// <summary>
        /// Password text field
        /// </summary>
        public bool Password
        {
            get
            {
                return (FieldFlags & FieldFlag.Password) != 0;
            }
            set
            {
                if (value)
                {
                    FieldFlags |= FieldFlag.Password;
                }
                else
                {
                    FieldFlags &= ~FieldFlag.Password;
                }
                return;
            }
        }

        /// <summary>
        /// File select
        /// </summary>
        public bool FileSelect
        {
            get
            {
                return (FieldFlags & FieldFlag.FileSelect) != 0;
            }
            set
            {
                if (value)
                {
                    FieldFlags |= FieldFlag.FileSelect;
                }
                else
                {
                    FieldFlags &= ~FieldFlag.FileSelect;
                }
                return;
            }
        }

        /// <summary>
        /// Do not spell check
        /// </summary>
        public bool DoNotSpellCheck
        {
            get
            {
                return (FieldFlags & FieldFlag.DoNotSpellCheck) != 0;
            }
            set
            {
                if (value)
                {
                    FieldFlags |= FieldFlag.DoNotSpellCheck;
                }
                else
                {
                    FieldFlags &= ~FieldFlag.DoNotSpellCheck;
                }
                return;
            }
        }

        /// <summary>
        /// Do not scroll
        /// </summary>
        public bool DoNotScroll
        {
            get
            {
                return (FieldFlags & FieldFlag.DoNotScroll) != 0;
            }
            set
            {
                if (value)
                {
                    FieldFlags |= FieldFlag.DoNotScroll;
                }
                else
                {
                    FieldFlags &= ~FieldFlag.DoNotScroll;
                }
                return;
            }
        }

        /// <summary>
        /// Acro text field constructor
        /// </summary>
        /// <param name="AcroForm">Acro form object (parent of all fields)</param>
        /// <param name="FieldName">Field name</param>
        public PdfAcroTextField
                (
                PdfAcroForm AcroForm,
                string FieldName
                ) : base(AcroForm.Document, FieldName)
        {
            // field type is text
            Dictionary.AddName("/FT", "Tx");

            // add the field to acro fields structure
            AcroForm.AddField(this);
            return;
        }

        /// <summary>
        /// Draw text field (XObject appearance stream)
        /// </summary>
        public void DrawTextField()
        {
            // Create xobject
            PdfXObject XObject = new PdfXObject(Document, AnnotRect.Width, AnnotRect.Height);

            // clear field area
            PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
            DrawCtrl.Paint = DrawPaint.Fill;
            DrawCtrl.BackgroundTexture = BackgroundColor;
            XObject.DrawGraphics(DrawCtrl, XObject.BBox);

            // start of marked content
            XObject.BeginMarkedContent("/Tx");

            // draw the text
            if (FieldValue != null)
            {
                // calculate position
                double XPos = 0;
                if (TextCtrl.Justify == TextJustify.Center) XPos = 0.5 * XObject.BBox.Right;
                else if (TextCtrl.Justify == TextJustify.Right) XPos = XObject.BBox.Right;

                // draw
                XObject.DrawText(TextCtrl, XPos, TextCtrl.TextDescent, FieldValue);
            }

            // end of marked content
            XObject.EndMarkedContent();

            // add to annotation appearance dictionary
            AddAppearance(XObject, AppearanceType.Normal);
            return;
        }

        /// <summary>
        /// close object before writing to PDF file
        /// </summary>
        internal override void CloseObject()
        {
            // text justify
            if (TextCtrl != null && (TextCtrl.Justify == TextJustify.Center || TextCtrl.Justify == TextJustify.Right))
                Dictionary.AddInteger("/Q", (int)TextCtrl.Justify);

            // Acro text field maximum length
            if (TextMaxLength > 0) Dictionary.AddInteger("/MaxLen", TextMaxLength);

            // Text field value (/V)
            if (FieldValue != null) Dictionary.AddPdfString("/V", FieldValue);

            // Text field default value (/DV)
            if (DefaultValue != null) Dictionary.AddPdfString("/DV", DefaultValue);

            // close PdfAnnotation object
            base.CloseObject();
            return;
        }
    }
}
