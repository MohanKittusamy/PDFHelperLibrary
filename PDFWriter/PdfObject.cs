//	PdfObject
//	Base class for all PDF indirect object classes.
using System.Text;
using System.Drawing;

namespace PDFWriter
{
    /////////////////////////////////////////////////////////////////////
    // Resource code enumeration
    /////////////////////////////////////////////////////////////////////
    internal enum ResCode
    {
        // must be in this order
        Font,
        Pattern,
        Shading,
        XObject,
        ExtGState,
        OpContent,
        Length
    }

    internal enum ObjectType
    {
        Free,
        Other,
        Dictionary,
        Stream,
    }

    internal enum XRefObjType
    {
        Free,
        InFile,
        ObjStm,
    }

    internal enum ColorToStr
    {
        NonStroking, // rg or g
        Stroking, // RG or G
        Array, // [R G B] or [G]
    }

    ////////////////////////////////////////////////////////////////////
    /// <summary>
    /// PDF indirect object base class
    /// </summary>
    /// <remarks>
    /// PDF indirect object base class.
    /// User program cannot call it directly.
    /// </remarks>
    ////////////////////////////////////////////////////////////////////
    public class PdfObject : IComparable<PdfObject>
    {
        /// <summary>
        /// PDF document object
        /// </summary>
        public PdfDocument Document { get; internal set; }

        /// <summary>
        /// Scale factor
        /// </summary>
        /// <remarks>Convert from user unit of measure to points.</remarks>
        public double ScaleFactor { get; internal set; }

        internal int ObjectNumber; // PDF indirect object number
        internal string ResourceCode; // resource code automatically generated by the program
        internal long FilePosition; // PDF file position for this indirect object
        internal ObjectType ObjectType; // object type
        internal XRefObjType XRefType;
        internal int StrmParent; // parent object number
        internal int StrmIndexNo; // index within parent
        internal List<byte> ObjectValueList;
        internal byte[] ObjectValueArray;
        internal PdfDictionary Dictionary; // indirect objects dictionary or stream dictionary
        internal bool NoCompression;

        private static readonly string[] ResCodeStr = { "/Font <<", "/Pattern <<", "/Shading <<", "/XObject <<", "/ExtGState <<", "/Properties <<" };
        internal static readonly string ResCodeLetter = "FPSXGO";

        internal PdfObject() { }

        ////////////////////////////////////////////////////////////////////
        // Constructor for objects with /Type in their dictionary
        // Note: access is internal. Used by derived classes only
        ////////////////////////////////////////////////////////////////////

        internal PdfObject
                (
                PdfDocument Document,
                ObjectType ObjType = ObjectType.Dictionary,
                string PdfDictType = null   // object type (i.e. /Catalog, /Pages, /Font, /XObject, /OCG)
                )
        {
            // save link to main document object
            this.Document = Document;

            // save type
            ObjectType = ObjType;

            // save scale factor
            ScaleFactor = Document.ScaleFactor;

            // no compression
            NoCompression = Document.Debug;

            // switch based on object type
            switch (ObjectType)
            {
                case ObjectType.Free:
                    XRefType = XRefObjType.Free;
                    break;

                case ObjectType.Other:
                    XRefType = XRefObjType.ObjStm;
                    ObjectValueList = new List<byte>();
                    break;

                case ObjectType.Dictionary:
                    XRefType = XRefObjType.ObjStm;
                    Dictionary = new PdfDictionary(this);
                    break;

                case ObjectType.Stream:
                    XRefType = XRefObjType.InFile;
                    Dictionary = new PdfDictionary(this);
                    ObjectValueList = new List<byte>();
                    break;
            }

            // if object name is specified, create a dictionary and add /Type Name entry
            if (!string.IsNullOrEmpty(PdfDictType)) Dictionary.Add("/Type", PdfDictType);

            // set PDF indirect object number to next available number
            ObjectNumber = Document.ObjectArray.Count;

            // add the new object to object array
            Document.ObjectArray.Add(this);
            return;
        }

        /// <summary>
        /// Compare the resource codes of two PDF objects.
        /// </summary>
        /// <param name="Other">Other PdfObject</param>
        /// <returns>Compare result</returns>
        /// <remarks>
        /// Used by PdfContents to maintain resource objects in sorted order.
        /// </remarks>
        public int CompareTo
                (
                PdfObject Other     // the second object
                )
        {
            return string.Compare(ResourceCode, Other.ResourceCode);
        }

        // Convert user coordinates or line width to points.
        // The result is rounded to 6 decimal places and converted to Single.
        internal float ToPt
                (
                double Value // coordinate value in user unit of measure
                )
        {
            double ReturnValue = ScaleFactor * Value;
            if (Math.Abs(ReturnValue) < 0.0001) ReturnValue = 0;
            return (float)ReturnValue;
        }

        // Round unscaled numbers.
        // The value is rounded to 6 decimal places and converted to Single
        internal float Round
                (
                double Value // a number to be saved in contents
                )
        {
            if (Math.Abs(Value) < 0.0001) return 0;
            return (float)Math.Round(Value, 4, MidpointRounding.AwayFromZero); // Value;
        }

        // append string to object's value list
        // each char is converted to byte
        internal void ObjectValueAppend
                (
                string Str
                )
        {
            // convert content from string to binary
            foreach (char Chr in Str) ObjectValueList.Add((byte)Chr);
            return;
        }

        internal void ObjectValueFormat
                (
                string FormatStr,
                params object[] List
                )
        {
            // format input arguments
            string Str = string.Format(NFI.PeriodDecSep, FormatStr, List);

            // convert content from string to binary
            foreach (char Chr in Str) ObjectValueList.Add((byte)Chr);
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // C# string text to PDF strings only
        ////////////////////////////////////////////////////////////////////
        internal string TextToPdfString
                (
                string Text,
                PdfObject Parent
                )
        {
            // convert C# string to byte array
            byte[] ByteArray = TextToByteArray(Text);

            // encryption object
            PdfEncryption Encryption = Document.Encryption;

            // encryption is active. PDF string must be encrypted except for encryption dictionary
            if (Encryption != null && Parent != null && Encryption != Parent && Parent.XRefType == XRefObjType.InFile)
                ByteArray = Encryption.EncryptByteArray(Parent.ObjectNumber, ByteArray);

            // convert byte array to PDF string format
            return ByteArrayToPdfString(ByteArray);
        }

        ////////////////////////////////////////////////////////////////////
        // C# string text to byte array
        // This method is used for PDF strings only
        ////////////////////////////////////////////////////////////////////
        internal byte[] TextToByteArray
                (
                string Text
                )
        {
            // scan input text for Unicode characters and for non printing characters
            bool Unicode = false;
            foreach (char TestChar in Text)
            {
                // test for non printable characters
                if (TestChar < ' ' || TestChar > '~' && TestChar < 160)
                    throw new ApplicationException("Text string must be made of printable characters");

                // test for Unicode string
                if (TestChar > 255) Unicode = true;
            }

            // declare output byte array
            byte[] ByteArray;

            // all characters are one byte long
            if (!Unicode)
            {
                // save each imput character in one byte
                ByteArray = new byte[Text.Length];
                int Index = 0;
                foreach (char TestChar in Text) ByteArray[Index++] = (byte)TestChar;
            }

            // Unicode case. we have some two bytes characters
            else
            {
                // allocate output byte array
                ByteArray = new byte[2 * Text.Length + 2];

                // add Unicode marker at the start of the string
                ByteArray[0] = 0xfe;
                ByteArray[1] = 0xff;

                // save each character as two bytes
                int Index = 2;
                foreach (char TestChar in Text)
                {
                    ByteArray[Index++] = (byte)(TestChar >> 8);
                    ByteArray[Index++] = (byte)TestChar;
                }
            }

            // return output byte array
            return ByteArray;
        }

        ////////////////////////////////////////////////////////////////////
        // byte array to PDF string
        // This method is used for PDF strings only
        ////////////////////////////////////////////////////////////////////
        internal string ByteArrayToPdfString
                (
                byte[] ByteArray
                )
        {
            // create output string with open and closing parenthesis
            StringBuilder Str = new StringBuilder("(");
            foreach (byte TestByte in ByteArray)
            {
                // CR and NL must be replaced by \r and \n
                // Otherwise PDF readers will convert CR or NL or CR-NL to NL
                if (TestByte == '\r') Str.Append("\\r");
                else if (TestByte == '\n') Str.Append("\\n");

                // the three characters \ ( ) must be preceded by \
                else
                {
                    if (TestByte == (byte)'\\' || TestByte == (byte)'(' || TestByte == (byte)')') Str.Append('\\');
                    Str.Append((char)TestByte);
                }
            }
            Str.Append(')');
            return Str.ToString();
        }

        ////////////////////////////////////////////////////////////////////
        // Convert resource dictionary to one String.
        // This method is called at the last step of document creation
        // from within PdfDocument.CreateFile(FileName).
        // it is relevant to page contents, X objects and tiled pattern
        // Return value is resource dictionary string.
        ////////////////////////////////////////////////////////////////////

        internal string BuildResourcesDictionary
                (
                List<PdfObject> ResObjects // list of resource objects for this contents
                )
        {
            // resource object list is empty
            // if there are no resources an empty dictionary must be returned
            if (ResObjects == null || ResObjects.Count == 0)
            {
                return "<</ProcSet [/PDF/Text]>>";
            }

            // resources dictionary content initialization
            StringBuilder Resources = new StringBuilder("<<");

            // for page object
            Resources.Append("/ProcSet [/PDF/Text/ImageB/ImageC/ImageI]\n");

            // add all resources
            char ResCodeType = ' ';
            foreach (PdfObject Resource in ResObjects)
            {
                // resource code is /Xnnn
                if (Resource.ResourceCode[1] != ResCodeType)
                {
                    // terminate last type
                    if (ResCodeType != ' ') Resources.Append(">>\n");

                    // start new type
                    ResCodeType = Resource.ResourceCode[1];
                    Resources.Append(ResCodeStr[ResCodeLetter.IndexOf(ResCodeType)]);
                }

                // append resource code
                if (Resource.GetType() != typeof(PdfFont))
                {
                    Resources.Append(string.Format("{0} {1} 0 R", Resource.ResourceCode, Resource.ObjectNumber));
                }
                else
                {
                    PdfFont Font = (PdfFont)Resource;
                    if (Font.FontResCodeUsed)
                        Resources.Append(string.Format("{0} {1} 0 R", Font.ResourceCode, Font.ObjectNumber));
                    if (Font.FontResGlyphUsed)
                        Resources.Append(string.Format("{0} {1} 0 R", Font.ResourceCodeGlyph, Font.GlyphIndexFont.ObjectNumber));
                }
            }

            // terminate last type and resource dictionary
            Resources.Append(">>\n>>");

            // exit
            return Resources.ToString();
        }

        internal virtual void CloseObject()
        {
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // write stream object to output PDF file
        ////////////////////////////////////////////////////////////////////
        internal void WriteStreamObject
                (
                int ObjectsCount,
                List<byte> Stream
                )
        {
            // position of first object
            int First = ObjectValueList.Count;

            // copy stream data
            ObjectValueList.AddRange(Stream);

            // add N pairs and first position
            Dictionary.AddInteger("/N", ObjectsCount);
            Dictionary.AddInteger("/First", First);

            // write to file
            WriteToPdfFile();

            // exit
            return;
        }


        ////////////////////////////////////////////////////////////////////
        // Write stream object to PDF file
        // Called by PdfDocument.CreateFile(FileName) method
        // to output one indirect stream PDF object.
        ////////////////////////////////////////////////////////////////////
        internal void WriteToPdfFile()
        {
            // already done or not in file object
            if (FilePosition != 0 || XRefType != XRefObjType.InFile) return;

            // save file position for this object
            FilePosition = Document.PdfFile.BaseStream.Position;

            // write object header
            Document.PdfFile.WriteFormat("{0} 0 obj\n", ObjectNumber);

            // stream object
            if (ObjectType == ObjectType.Stream)
            {
                // convert byte list to array
                if (ObjectValueList.Count > 0) ObjectValueArray = ObjectValueList.ToArray();

                // object value is empty
                if (ObjectValueArray == null) ObjectValueArray = new byte[0];

                // compress the stream and update dictionary if successful
                if (!NoCompression && PdfDocument.CompressStream(ref ObjectValueArray)) Dictionary.Add("/Filter", "/FlateDecode");

                // encryption
                if (Document.Encryption != null)
                    ObjectValueArray = Document.Encryption.EncryptByteArray(ObjectNumber, ObjectValueArray);

                // stream length
                Dictionary.AddInteger("/Length", ObjectValueArray.Length);

                // write dictionary
                Document.PdfFile.Write(Dictionary.ToByteArray());

                // write stream reserved word
                Document.PdfFile.WriteString("stream\n");

                // write content to pdf file
                Document.PdfFile.Write(ObjectValueArray);

                // write end of stream
                Document.PdfFile.WriteString("\nendstream\nendobj\n");
            }

            else
            {
                // write dictionary
                Document.PdfFile.Write(Dictionary.ToByteArray());

                // write end of object
                Document.PdfFile.WriteString("\nendobj\n");
            }

            // this indirect object was written to output file
            // tell the garbage collector that these objects are not used any more
            Dictionary = null;
            ObjectValueList = null;
            ObjectValueArray = null;
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Write root object to PDF file
        ////////////////////////////////////////////////////////////////////
        internal void WriteRootToPdfFile()
        {
            // convert root object from ObjStm to InFile
            XRefType = XRefObjType.InFile;

            // save file position for this object
            FilePosition = Document.PdfFile.BaseStream.Position;

            // write object header
            Document.PdfFile.WriteFormat("{0} 0 obj\n", ObjectNumber);

            // write dictionary
            Document.PdfFile.Write(Dictionary.ToByteArray());

            // write end of object
            Document.PdfFile.WriteString("\nendobj\n");
            return;
        }
    }
}