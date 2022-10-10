//	Acro button field
using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// Named action enumeration
    /// </summary>
    public enum NamedActionCode
    {
        /// <summary>
        /// Undefined
        /// </summary>
        Undefined,

        /// <summary>
        /// Go to next page
        /// </summary>
        NextPage,  // go to the next page of the document.

        /// <summary>
        /// Go to previous page
        /// </summary>
        PrevPage,  // go to the previous page of the document.

        /// <summary>
        /// Go to first page
        /// </summary>
        FirstPage, // go to the first page of the document.

        /// <summary>
        /// Go to last page
        /// </summary>
        LastPage,  // go to the last page of the document
    }

    /// <summary>
    /// Caption position style enumeration
    /// </summary>
    public enum CaptionPosStyle
    {
        /// <summary>
        /// No icon, caption only
        /// </summary>
        NoIcon = 0,         // No icon; caption only

        /// <summary>
        /// No caption, icon only
        /// </summary>
        NoCaption = 1,      // No caption; icon only

        /// <summary>
        /// Caption below the icon
        /// </summary>
        CapBelowIcon = 2,   // Caption below the icon

        /// <summary>
        /// Caption above the icon
        /// </summary>
        CapAboveIcon = 3,   // Caption above the icon

        /// <summary>
        /// Caption to the right of the icon
        /// </summary>
        CapRightOfIcon = 4, // Caption to the right of the icon

        /// <summary>
        /// Caption to the left of the icon
        /// </summary>
        CapLeftOfIcon = 5,  // Caption to the left of the icon

        /// <summary>
        /// Caption overlaid directly on the icon
        /// </summary>
        CapOverIcon = 6,    // Caption overlaid directly on the icon
    }

    /// <summary>
    /// Acro button field 
    /// </summary>
    public class PdfAcroButtonField : PdfAcroWidgetField
    {
        /// <summary>
        /// named action
        /// </summary>
        public NamedActionCode NamedAction { get; set; }

        /// <summary>
        /// Java script action
        /// </summary>
        public string JavaScriptAction { get; set; }

        /// <summary>
        /// Display icon
        /// </summary>
        public PdfImage Icon { get; set; }

        /// <summary>
        /// Acro field choice constructor
        /// </summary>
        /// <param name="AcroForm">Acro form parent</param>
        /// <param name="FieldName">Field partial name (/T)</param>
        public PdfAcroButtonField
                (
                PdfAcroForm AcroForm,
                string FieldName
                ) : base(AcroForm.Document, FieldName)
        {
            // field type is button
            Dictionary.AddName("/FT", "Btn");

            // field type is push button
            FieldFlags |= FieldFlag.PushButton;

            // add the field to acro fields structure
            AcroForm.AddField(this);
            return;
        }

        /// <summary>
        /// Draw button field (add xobject to appearance dictionary /AP)
        /// </summary>
        /// <param name="Type">Appearance type enumeration: Normal, Rollover, Down</param>
        public void DrawButtonField
                (
                AppearanceType Type
                )
        {
            // Create xobject
            PdfXObject XObject = new PdfXObject(Document, AnnotRect.Width, AnnotRect.Height);

            // button draw control
            PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
            DrawCtrl.Paint = DrawPaint.BorderAndFill;
            DrawCtrl.BackgroundTexture = BackgroundColor;
            DrawCtrl.BorderWidth = 1 / XObject.ScaleFactor;
            DrawCtrl.BorderColor = BorderColor;
            XObject.DrawGraphics(DrawCtrl, XObject.BBox);

            // draw caption at the ceter of the button
            double PosX = 0.5 * (AnnotRect.Width - TextCtrl.TextWidth(Caption));
            double PosY = 0.5 * (AnnotRect.Height - TextCtrl.LineSpacing) + TextCtrl.TextDescent;

            // draw field value text
            XObject.DrawText(TextCtrl, PosX, PosY, Caption);

            // add to annotation appearance dictionary
            AddAppearance(XObject, Type);
            return;
        }

        /// <summary>
        /// close object before writing to PDF file
        /// </summary>
        internal override void CloseObject()
        {
            // java script
            if (JavaScriptAction != null)
            {
                Dictionary.Add("/A", string.Format("<</S/JavaScript/JS {0}>>", TextToPdfString(JavaScriptAction, this)));
            }

            // named action
            // /A<</S/Named/N/NextPage>>
            else if (NamedAction != NamedActionCode.Undefined)
            {
                Dictionary.Add("/A", string.Format("<</S/Named/N/{0}>>", NamedAction.ToString()));
            }

            // close PdfAnnotation object
            base.CloseObject();
            return;
        }
    }
}
