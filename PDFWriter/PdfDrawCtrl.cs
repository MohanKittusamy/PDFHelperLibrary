//	Draw geometric shape control class

using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// Shape enumeration
    /// </summary>
    public enum DrawShapes
    {
        /// <summary>
        /// Rectangle
        /// </summary>
        Rectangle,

        /// <summary>
        /// Rounded rectangle
        /// </summary>
        RoundedRect,

        /// <summary>
        /// Inverted rounded rectangle
        /// </summary>
        InvRoundedRect,

        /// <summary>
        /// Oval (circle)
        /// </summary>
        Oval,
    }

    /// <summary>
    /// Paint control enumeration
    /// </summary>
    public enum DrawPaint
    {
        /// <summary>
        /// Border only
        /// </summary>
        Border,

        /// <summary>
        /// Fill and no border
        /// </summary>
        Fill,

        /// <summary>
        /// Border and fill the area
        /// </summary>
        BorderAndFill,
    }

    internal enum BackgroundTextureType
    {
        Color,
        TilingPattern,
        Image,
        Shading,
    }

    /// <summary>
    /// Draw control class for rectangle, rounded rectangle and oval
    /// </summary>
    public class PdfDrawCtrl
    {
        /// <summary>
        /// Set shape
        /// </summary>
        public DrawShapes Shape { get; set; }

        /// <summary>
        /// Set paint (border and fill)
        /// </summary>
        public DrawPaint Paint { get; set; }

        /// <summary>
        /// Border line width
        /// </summary>
        public double BorderWidth
        {
            get
            {
                return _BorderWidth;
            }

            set
            {
                // test border width
                if (value < 0) throw new ApplicationException("Border width must be non negative");
                _BorderWidth = value;
                return;
            }
        }
        private double _BorderWidth;

        /// <summary>
        /// Border color
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// Border alpha 
        /// </summary>
        public double BorderAlpha
        {
            get
            {
                return _BorderAlpha;
            }
            set
            {
                if (value < 0 || value > 1) throw new ApplicationException("Border alpha must be 0 to 1");
                _BorderAlpha = value;
                return;
            }
        }
        private double _BorderAlpha;

        /// <summary>
        /// Rounded rectangle corner radius
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Background texture
        /// Color, tilling pattern, image, axial shading, radial shading
        /// </summary>
        public object BackgroundTexture
        {
            get
            {
                return _BackgroundTexture;
            }
            set
            {
                if (value.GetType() == typeof(Color)) _BackgroundTextureType = BackgroundTextureType.Color;
                else if (value.GetType() == typeof(PdfTilingPattern)) _BackgroundTextureType = BackgroundTextureType.TilingPattern;
                else if (value.GetType() == typeof(PdfImage)) _BackgroundTextureType = BackgroundTextureType.Image;
                else if (value.GetType() == typeof(PdfAxialShading)) _BackgroundTextureType = BackgroundTextureType.Shading;
                else if (value.GetType() == typeof(PdfRadialShading)) _BackgroundTextureType = BackgroundTextureType.Shading;

                else throw new ApplicationException("PdfDrawCtrl invalid fill color type");
                _BackgroundTexture = value;
            }
        }
        internal object _BackgroundTexture;
        internal BackgroundTextureType _BackgroundTextureType;

        /// <summary>
        /// Background color alpha
        /// </summary>
        public double BackgroundAlpha
        {
            get
            {
                return _BackgroundAlpha;
            }
            set
            {
                if (value < 0 || value > 1) throw new ApplicationException("Background alpha must be 0 to 1");
                _BackgroundAlpha = value;
                return;
            }
        }
        private double _BackgroundAlpha;

        /// <summary>
        /// Blend mode
        /// </summary>
        public BlendMode Blend { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PdfDrawCtrl()
        {
            _BackgroundAlpha = 1;
            BorderColor = Color.Black;
            _BorderAlpha = 1;
            return;
        }

        /// <summary>
        /// Border width in points
        /// </summary>
        /// <param name="Contents">PdfContents or XObject</param>
        /// <param name="Width">Width in points (1/72 of inch)</param>
        public void BorderWidthInPoints
                (
                PdfContents Contents,
                double Width
                )
        {
            BorderWidth = Width / Contents.ScaleFactor;
            return;
        }
    }
}
