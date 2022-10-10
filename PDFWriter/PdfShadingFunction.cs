//	PdfShadingFunction
//	Support class for both axial and radial shading resources.
using System.Drawing;

namespace PDFWriter
{
    ////////////////////////////////////////////////////////////////////
    /// <summary>
    /// PDF shading function class
    /// </summary>
    /// <remarks>
    /// PDF function to convert a number between 0 and 1 into a
    /// color red green and blue based on the sample color array.
    /// </remarks>
    ////////////////////////////////////////////////////////////////////
    public class PdfShadingFunction : PdfObject
    {
        ////////////////////////////////////////////////////////////////////
        /// <summary>
        /// PDF Shading function constructor
        /// </summary>
        /// <param name="Document">Document object parent of this function.</param>
        /// <param name="ColorArray">Array of colors.</param>
        ////////////////////////////////////////////////////////////////////
        public PdfShadingFunction
                (
                PdfDocument Document,   // PDF document object
                Color[] ColorArray      // Array of colors. Minimum 2.
                ) : base(Document, ObjectType.Stream)
        {
            // build dictionary
            Constructorhelper(ColorArray.Length);

            // add color array to contents stream
            foreach (Color Color in ColorArray)
            {
                ObjectValueList.Add(Color.R);   // red
                ObjectValueList.Add(Color.G);   // green
                ObjectValueList.Add(Color.B);   // blue
            }
            return;
        }

        private void Constructorhelper
                (
                int Length
                )
        {
            // test for error
            if (Length < 2) throw new ApplicationException("Shading function color array must have two or more items");

            // the shading function is a sampled function
            Dictionary.Add("/FunctionType", "0");

            // input variable is between 0 and 1
            Dictionary.Add("/Domain", "[0 1]");

            // output variables are red, green and blue color components between 0 and 1
            Dictionary.Add("/Range", "[0 1 0 1 0 1]");

            // each color components in the stream is 8 bits
            Dictionary.Add("/BitsPerSample", "8");

            // number of colors in the stream must be two or more
            Dictionary.AddFormat("/Size", "[{0}]", Length);
            return;
        }
    }
}
