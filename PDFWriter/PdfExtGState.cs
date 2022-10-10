//	PdfExtGState
//	External graphics state dictionary.
using System.Drawing;

namespace PDFWriter
{
    internal class PdfExtGState : PdfObject, IComparable<PdfExtGState>
    {
        internal string Key;
        internal string Value;

        // search constructor
        internal PdfExtGState
                (
                string Key,
                string Value
                )
        {
            // save value
            this.Key = Key;
            this.Value = Value;

            // exit
            return;
        }

        // object constructor
        internal PdfExtGState
                (
                PdfDocument Document,
                string Key,
                string Value
                ) : base(Document, ObjectType.Dictionary, "/ExtGState")
        {
            // save value
            this.Key = Key;
            this.Value = Value;

            // create resource code
            ResourceCode = Document.GenerateResourceNumber('G');
            return;
        }

        internal static PdfExtGState CreateExtGState
                (
                PdfDocument Document,
                string Key,
                string Value
                )
        {
            if (Document.ExtGStateArray == null) Document.ExtGStateArray = new List<PdfExtGState>();

            // search list for a duplicate
            int Index = Document.ExtGStateArray.BinarySearch(new PdfExtGState(Key, Value));

            // this value is a duplicate
            if (Index >= 0) return Document.ExtGStateArray[Index];

            // new blend object
            PdfExtGState ExtGState = new PdfExtGState(Document, Key, Value);

            // save new string in array
            Document.ExtGStateArray.Insert(~Index, ExtGState);

            // update dictionary
            ExtGState.Dictionary.Add(Key, Value);

            // exit
            return ExtGState;
        }

        /// <summary>
        /// Compare two PdfExtGState objects.
        /// </summary>
        /// <param name="Other">Other object.</param>
        /// <returns>Compare result.</returns>
        public int CompareTo
                (
                PdfExtGState Other
                )
        {
            int Cmp = string.Compare(Key, Other.Key);
            if (Cmp != 0) return Cmp;
            return string.Compare(Value, Other.Value);
        }
    }
}
