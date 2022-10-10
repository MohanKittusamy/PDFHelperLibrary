//	Acro radio button widget
using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// Radio button widget
    /// A group of radio buttons will make one field
    /// </summary>
    public class PdfAcroRadioButton : PdfAcroWidgetField
    {
        /// <summary>
        /// This radio button widget is on
        /// </summary>
        public bool Check { get; set; }

        /// <summary>
        /// On-state name (the off-state is always "Off") 
        /// </summary>
        internal string OnStateName;

        /// <summary>
        /// ZapfDingbats font color
        /// </summary>
        public Color RadioButtonColor;

        /// <summary>
        /// Acro field radio button constructor
        /// </summary>
        /// <param name="AcroForm">Acro form object</param>
        /// <param name="GroupName">Group of radio buttons name</param>
        /// <param name="OnStateName">Radio button on value</param>
        public PdfAcroRadioButton
                (
                PdfAcroForm AcroForm,
                string GroupName,
                string OnStateName
                ) : base(AcroForm.Document, GroupName)
        {
            // test argument
            if (string.IsNullOrWhiteSpace(OnStateName)) throw new ApplicationException("Radio button On-state must be defined");

            // save on-state name
            this.OnStateName = OnStateName;

            // radio button color
            BackgroundColor = Color.White;
            BorderColor = Color.Black;
            RadioButtonColor = Color.Black;

            // add the field to acro fields structure
            AcroForm.AddField(this);
            return;
        }

        /// <summary>
        /// Draw radio button
        /// </summary>
        /// <param name="AppType">Appearance type (Normal, Down, Rollover)</param>
        /// <param name="Selected">Selected</param>
        public void DrawRadioButton
                (
                AppearanceType AppType,
                bool Selected
                )
        {
            // Normal off appearance stream
            PdfXObject XObject = new PdfXObject(Document, AnnotRect.Width, AnnotRect.Height);

            // clear field area and draw border within field area
            PdfDrawCtrl DrawCtrl = new PdfDrawCtrl();
            DrawCtrl.Shape = DrawShapes.Oval;
            DrawCtrl.Paint = DrawPaint.BorderAndFill;
            DrawCtrl.BackgroundTexture = BackgroundColor;
            DrawCtrl.BorderColor = BorderColor;
            DrawCtrl.BorderWidth = 1 / Document.ScaleFactor;
            XObject.DrawGraphics(DrawCtrl, XObject.BBox);

            if (Selected)
            {
                // reduce box size by two point (border one point and clear space one point)
                PdfRectangle SymbolRect = XObject.BBox.AddMargin(-2 / Document.ScaleFactor);

                DrawCtrl = new PdfDrawCtrl();
                DrawCtrl.Shape = DrawShapes.Oval;
                DrawCtrl.Paint = DrawPaint.Fill;
                DrawCtrl.BackgroundTexture = BorderColor;
                XObject.DrawGraphics(DrawCtrl, SymbolRect);
            }

            // add to appearance dictionary
            AddAppearance(XObject, AppType, Selected ? OnStateName : "Off");
            return;
        }

        /// <summary>
        /// close object before writing to PDF file
        /// </summary>
        internal override void CloseObject()
        {
            // selected value
            Dictionary.AddName("/AS", Check ? OnStateName : "Off");

            // close PdfAnnotation object
            base.CloseObject();
            return;
        }
    }
}
