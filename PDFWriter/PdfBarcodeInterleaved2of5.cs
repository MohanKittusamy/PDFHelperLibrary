//	Barcode interleaved 2 of 5 class
using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// Barcode interleaved 2 of 5 class
    /// </summary>
    public class PdfBarcodeInterleaved2of5 : PdfBarcode
    {
        /// <summary>
        /// Code table for interleave 2 of 5 barcode
        /// </summary>
        public static readonly byte[,] CodeTable =
            {
            {1, 1, 2, 2, 1},		// 0
			{2, 1, 1, 1, 2},		// 1
			{1, 2, 1, 1, 2},		// 2
			{2, 2, 1, 1, 1},		// 3
			{1, 1, 2, 1, 2},		// 4
			{2, 1, 2, 1, 1},		// 5
			{1, 2, 2, 1, 1},		// 6
			{1, 1, 1, 2, 2},		// 7
			{2, 1, 1, 2, 1},		// 8
			{1, 2, 1, 2, 1},		// 9
			};

        ////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Barcode width
        /// </summary>
        /// <param name="BarIndex">Code array index</param>
        /// <returns>float bar width</returns>
        ////////////////////////////////////////////////////////////////////
        public override int BarWidth
                (
                int BarIndex
                )
        {
            // leading bars
            if (BarIndex < 4) return 1;

            // ending bars
            if (BarIndex >= BarCount - 3) return BarIndex == BarCount - 3 ? 2 : 1;

            // code index
            BarIndex -= 4;
            int CodeIndex = 2 * (BarIndex / 10);
            if ((BarIndex & 1) != 0) CodeIndex++;

            // code
            int Code = _CodeArray[CodeIndex];

            return CodeTable[Code, BarIndex % 10 / 2];
        }

        /// <summary>
        /// Barcode interleave 2 of 5 constructor
        /// </summary>
        /// <param name="Text">Text</param>
        /// <param name="AddChecksum">Add checksum digit</param>
        public PdfBarcodeInterleaved2of5
                (
                string Text,
                bool AddChecksum = false
                )
        {
            // test argument
            if (string.IsNullOrWhiteSpace(Text))
                throw new ApplicationException("Barcode Interleave 2 of 5: Input text is null or empty");

            // save text
            this.Text = Text;

            // text length
            int Length = Text.Length;
            if (AddChecksum) Length++;
            if ((Length & 1) != 0)
                throw new ApplicationException("Barcode Interleave 2 of 5: Text length must be even (including checksum)");

            // barcode array
            _CodeArray = new int[Length];

            // make sure it is all digits
            int CodePtr = 0;
            foreach (char Chr in Text)
            {
                if (Chr < '0' || Chr > '9')
                    throw new ApplicationException("Barcode interleave 2 of 5: Invalid character (must be 0 to 9)");
                _CodeArray[CodePtr++] = Chr - '0';
            }

            // calculate checksum
            if (AddChecksum) Checksum();

            // set number of bars
            BarCount = 7 + 10 * (Length / 2);

            // set total width
            TotalWidth = 8 + 14 * (Length / 2);

            // exit
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Code EAN-13 checksum calculations
        ////////////////////////////////////////////////////////////////////

        private void Checksum()
        {
            // calculate checksum
            int ChkSum = 3 * _CodeArray[0];
            int End = _CodeArray.Length - 1;
            for (int Index = 1; Index < End; Index += 2) ChkSum += _CodeArray[Index] + 3 * _CodeArray[Index + 1];

            // final checksum
            ChkSum = ChkSum % 10;
            if (ChkSum != 0) ChkSum = 10 - ChkSum;
            _CodeArray[End] = ChkSum;

            // add it to text
            Text = Text + ((char)(ChkSum + '0')).ToString();
            return;
        }
    }
}
