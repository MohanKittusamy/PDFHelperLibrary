//	PdfTypeOneFontInfo
//  Support class for type one font information.

using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// Type one font information class
    /// </summary>
    public class PdfTypeOneFontInfo
    {
        /// <summary>
        /// Font name
        /// </summary>
        public string FontName;

        /// <summary>
        /// Font bounding box left side
        /// </summary>
        public int BBoxLeft;

        /// <summary>
        /// Font bounding box bottom side
        /// </summary>
        public int BBoxBottom;

        /// <summary>
        /// Font bounding box right side
        /// </summary>
        public int BBoxRight;

        /// <summary>
        /// Font bounding box top side
        /// </summary>
        public int BBoxTop;

        /// <summary>
        /// Characters array of information
        /// </summary>
        public short[,] CharInfo;

        /// <summary>
        /// Type one font info constructor
        /// </summary>
        /// <param name="FontName">Font name</param>
        /// <param name="BBoxLeft">Bounding box left</param>
        /// <param name="BBoxBottom">Bounding box bottom</param>
        /// <param name="BBoxRight">Bounding box right</param>
        /// <param name="BBoxTop">Bounding box top</param>
        /// <param name="CharInfo">Characters information array</param>
        public PdfTypeOneFontInfo
                (
                string FontName,
                int BBoxLeft,
                int BBoxBottom,
                int BBoxRight,
                int BBoxTop,
                short[,] CharInfo
                )
        {
            this.FontName = FontName;
            this.BBoxLeft = BBoxLeft;
            this.BBoxBottom = BBoxBottom;
            this.BBoxRight = BBoxRight;
            this.BBoxTop = BBoxTop;
            this.CharInfo = CharInfo;
            return;
        }

        /// <summary>
        /// Character width
        /// </summary>
        /// <param name="Chr">Character code</param>
        /// <returns>Character width</returns>
        public int CharWidth
                (
                char Chr
                )
        {
            return CharInfo[Chr, 0];
        }

        /// <summary>
        /// Character bounding box left side
        /// </summary>
        /// <param name="Chr">Character code</param>
        /// <returns>Bounding box left</returns>
        public int CharBBoxLeft
                (
                char Chr
                )
        {
            return CharInfo[Chr, 1];
        }

        /// <summary>
        /// Character bounding box bottom side
        /// </summary>
        /// <param name="Chr">Character code</param>
        /// <returns>Bounding box bottom</returns>
        public int CharBBoxBottom
                (
                char Chr
                )
        {
            return CharInfo[Chr, 2];
        }

        /// <summary>
        /// Character bounding box right side
        /// </summary>
        /// <param name="Chr">Character code</param>
        /// <returns>Bounding box right</returns>
        public int CharBBoxRight
                (
                char Chr
                )
        {
            return CharInfo[Chr, 3];
        }

        /// <summary>
        /// Character bounding box top side
        /// </summary>
        /// <param name="Chr">Character code</param>
        /// <returns>Bounding box top</returns>
        public int CharBBoxTop
                (
                char Chr
                )
        {
            return CharInfo[Chr, 4];
        }
    }
}
