//	PdfAxialShading
//	PDF Axial shading indirect object.
using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// Mapping mode for axial and radial shading
    /// </summary>
    public enum MappingMode
    {
        /// <summary>
        /// Relative to bounding box
        /// </summary>
        Relative,
        /// <summary>
        /// Absolute
        /// </summary>
        Absolute
    }

    ////////////////////////////////////////////////////////////////////
    /// <summary>
    /// PDF axial shading resource class
    /// </summary>
    /// <remarks>
    /// Derived class from PdfObject
    /// </remarks>
    ////////////////////////////////////////////////////////////////////
    public class PdfAxialShading : PdfObject
    {
        /// <summary>
        /// Bounding box rectangle
        /// </summary>
        public PdfRectangle BBox { get; set; }

        /// <summary>
        /// Direction rectangle
        /// </summary>
        public PdfRectangle Direction { get; set; }

        /// <summary>
        /// Mapping mode
        /// </summary>
        public MappingMode Mapping { get; set; }

        private bool ExtendShadingBefore = true;
        private bool ExtendShadingAfter = true;

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        /// PDF axial shading constructor.
        /// </summary>
        /// <param name="Document">Parent PDF document object</param>
        /// <param name="ShadingFunction">Shading function</param>
        ////////////////////////////////////////////////////////////////////
        public PdfAxialShading
                (
                PdfDocument Document,
                PdfShadingFunction ShadingFunction
                ) : base(Document)
        {
            // create resource code
            ResourceCode = Document.GenerateResourceNumber('S');

            // color space red, green and blue
            Dictionary.Add("/ColorSpace", "/DeviceRGB");

            // shading type axial
            Dictionary.Add("/ShadingType", "2");

            // add shading function to shading dictionary
            Dictionary.AddIndirectReference("/Function", ShadingFunction);

            // bounding box
            BBox = new PdfRectangle(0, 0, 1, 1);

            // assume the direction of color change is along x axis
            Direction = new PdfRectangle(0, 0, 1, 0);
            Mapping = MappingMode.Relative;
            return;
        }

        /*
                /// <summary>
                /// PDF axial shading constructor for unit bounding box
                /// </summary>
                /// <param name="Document">Parent PDF document object</param>
                /// <param name="MediaBrush">System.Windows.Media brush</param>
                public PdfAxialShading
                        (
                        PdfDocument Document,
                        SysMedia.LinearGradientBrush MediaBrush
                        ) : this(Document, new PdfShadingFunction(Document, MediaBrush))
                    {
                    Direction = new PdfRectangle(MediaBrush.StartPoint.X, MediaBrush.StartPoint.Y, MediaBrush.EndPoint.X, MediaBrush.EndPoint.Y);
                    Mapping = MediaBrush.MappingMode == SysMedia.BrushMappingMode.RelativeToBoundingBox ? MappingMode.Relative : MappingMode.Absolute;
                    return;
                    }
        */

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets anti-alias parameter
        /// </summary>
        /// <param name="Value">Anti-alias true or false</param>
        ////////////////////////////////////////////////////////////////////
        public void AntiAlias
                (
                bool Value
                )
        {
            Dictionary.AddBoolean("/AntiAlias", Value);
            return;
        }

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Extend shading beyond axis
        /// </summary>
        /// <param name="Before">Before (true or false)</param>
        /// <param name="After">After (true or false)</param>
        ////////////////////////////////////////////////////////////////////
        public void ExtendShading
                (
                bool Before,
                bool After
                )
        {
            ExtendShadingBefore = Before;
            ExtendShadingAfter = After;
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // close object before writing to PDF file
        ////////////////////////////////////////////////////////////////////
        internal override void CloseObject()
        {
            // bounding box
            Dictionary.AddRectangle("/BBox", BBox);

            // relative axit direction
            if (Mapping == MappingMode.Relative)
            {
                Direction = new PdfRectangle(BBox.Left * (1.0 - Direction.Left) + BBox.Right * Direction.Left,
                    BBox.Bottom * (1.0 - Direction.Bottom) + BBox.Top * Direction.Bottom,
                    BBox.Left * (1.0 - Direction.Right) + BBox.Right * Direction.Right,
                    BBox.Bottom * (1.0 - Direction.Top) + BBox.Top * Direction.Top);
            }

            // direction rectangle
            Dictionary.AddRectangle("/Coords", Direction);

            // extend shading
            Dictionary.AddFormat("/Extend", "[{0} {1}]", ExtendShadingBefore ? "true" : "false", ExtendShadingAfter ? "true" : "false");

            // exit
            return;
        }
    }
}
