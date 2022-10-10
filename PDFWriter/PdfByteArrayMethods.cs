//	Byte array methods
using System.Security.Cryptography;
using System.Text;
using System.Drawing;

namespace PDFWriter
{
    /// <summary>
    /// Class to manipulate byte array
    /// </summary>
    public static class PdfByteArrayMethods
    {
        ////////////////////////////////////////////////////////////////////
        // Convert byte array to PDF string
        // used for document id and encryption
        ////////////////////////////////////////////////////////////////////
        internal static string ByteArrayToPdfHexString
                (
                byte[] ByteArray
                )
        {
            // convert to hex string
            StringBuilder HexText = new StringBuilder("<");
            for (int index = 0; index < ByteArray.Length; index++) HexText.AppendFormat("{0:x2}", (int)ByteArray[index]);
            HexText.Append(">");
            return HexText.ToString();
        }

        ////////////////////////////////////////////////////////////////////
        // format short string to byte array 
        ////////////////////////////////////////////////////////////////////
        internal static byte[] FormatToByteArray
                (
                string FormatStr,
                params object[] List
                )
        {
            // string format
            return ToByteArray(string.Format(FormatStr, List));
        }

        ////////////////////////////////////////////////////////////////////
        // format short string to byte array 
        ////////////////////////////////////////////////////////////////////
        internal static byte[] ToByteArray
                (
                string Str
                )
        {
            // byte array
            byte[] ByteArray = new byte[Str.Length];

            // convert content from string to binary
            // do not use Encoding.ASCII.GetBytes(...)
            for (int Index = 0; Index < ByteArray.Length; Index++) ByteArray[Index] = (byte)Str[Index];
            return ByteArray;
        }

        ////////////////////////////////////////////////////////////////////
        // Create random byte array
        ////////////////////////////////////////////////////////////////////
        internal static byte[] RandomByteArray
                (
                int Length
                )
        {
            byte[] ByteArray = new byte[Length];
            //			using(RNGCryptoServiceProvider RandNumGen = new RNGCryptoServiceProvider())
            using (RandomNumberGenerator RandNumGen = RandomNumberGenerator.Create())
            {
                RandNumGen.GetBytes(ByteArray);
            }
            return ByteArray;
        }

    }
}
