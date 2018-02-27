using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualBasic;

namespace Jurassic.Com.Tools
{

    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// encryption related utility functions
    /// </summary>
    public static class Encryption
    {
        // *** (ADM) DON'T CHANGE THIS KEYS AND VI Key = "34TyUmw2", Vi = "H54yuPs9" ***
        private const String DEFAULT_SOURCE_KEY = "34TyUmw2";
        private const String DEFAULT_SOURCE_VI = "H54yuPs9";

        /// <summary>
        /// this funciton will decrypted the text
        /// </summary>
        /// <param name="strSource">encrypted strSource</param>
        /// <returns></returns>
        public static string Decrypt(string strSource)
        {
            // Parameters : 
            //  strSource - decrypted strSource 
            // 
            // Descriptions : 
            //  this funciton will decrypted the text
            // 
            // Author/Date : 2008-10-08, Cuttlebone
            if (strSource.IsEmpty())
            {
                return null;
            }
            try
            {
                return DecryptString(strSource, DEFAULT_SOURCE_KEY, DEFAULT_SOURCE_VI);
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// this funciton will encrypt the string 
        /// </summary>
        /// <param name="strSource">encrypt source string</param>
        /// <returns></returns>
        public static string Encrypt(string strSource)
        {
            // Parameters : 
            //  strSource - encrypted strSource 
            // 
            // Descriptions : 
            //  this funciton will encrypt the string 
            // 
            // Author/Date : 2008-10-08, Cuttlebone

            return EncryptString(strSource, DEFAULT_SOURCE_KEY, DEFAULT_SOURCE_VI);
        }

        /// <summary>
        /// For DES algo, key and vi size are 8 bytes each.
        ///  ** IF you want to print out the resulting byte array(eg. key), you can try 
        ///  MsgBox(System.Text.Encoding.ASCII.GetChars(bytKey)) or 
        ///  MsgBox(System.Text.Encoding.Unicode.GetStrings(bytKey)) 
        /// </summary>
        /// <param name="strSource">strSource is the source string </param>
        /// <param name="strKey">aryKey is the encryption KEY</param>
        /// <param name="strVi">aryVi is the encryption VECTOR</param>
        /// <returns></returns>
        private static string EncryptString(string strSource, string strKey, string strVi)
        {
            // Parameters : 
            //  strSource is the source string 
            //  aryKey is the encryption KEY
            //  aryVi is the encryption VECTOR 
            // 
            // Descriptions : 
            //  Note : For DES algo, key and vi size are 8 bytes each. 
            // 
            //  ** IF you want to print out the resulting byte array(eg. key), you can try 
            //  MsgBox(System.Text.Encoding.ASCII.GetChars(bytKey)) or 
            //  MsgBox(System.Text.Encoding.Unicode.GetStrings(bytKey)) 
            // 
            // Author/Date : 2008-10-08, Cuttlebone

            strKey = Prepare_Key(strKey);
            strVi = Prepare_Key(strVi);

            byte[] bytResult = null;

            // Fix up the KEY and VI values 
            // They must be 8 bytes for DES 
            byte[] bytDesKey = Des_StrToByte(strKey);
            byte[] bytDesVi = Des_StrToByte(strVi);

            // Translate the source into bytes 
            byte[] bytSource = Encoding.Unicode.GetBytes(strSource.ToCharArray());

            // The DES algo object 
            DESCryptoServiceProvider objDes = new DESCryptoServiceProvider();
            // Set the max key size, 64 bits 
            objDes.KeySize = 64;

            // The Encryptor object 
            ICryptoTransform objEncryptor = objDes.CreateEncryptor(bytDesKey, bytDesVi);

            // The temp memory stream to hold our final encrypted result 
            MemoryStream objMemory = new MemoryStream();

            // Now create a crypto stream which combine the algorithm and the output stream 
            // together. 
            // We can think of it as : The crypto stream defined how the data is stored. 
            // the stream is where it is stored, the algo is how it is encrypted. 
            CryptoStream objCryptoStream = new CryptoStream(objMemory, objEncryptor, CryptoStreamMode.Write);

            try
            {
                // put the source into the crypto stream. 
                // Since we have defined and algo, the system will 
                // encrypt the data during the process 
                objCryptoStream.Write(bytSource, 0, bytSource.Length);

                // stream has buffer, so we do a flush to make sure 
                // the underlying destination is up to date 
                objCryptoStream.FlushFinalBlock();
            }
            finally
            {
                // When we are done, do some clean up of those streams 
                bytResult = objMemory.ToArray();

                objCryptoStream.Close();
            }

            return BytesToString(objMemory.ToArray());
        }
        /// <summary>
        /// For DES algo, key and vi size have sizes of 8 bytes each (64 bits)
        /// </summary>
        /// <param name="strSource">bytSource is the source string (encrypted)</param>
        /// <param name="strKey">aryKey is the encryption KEY</param>
        /// <param name="strVi">aryVi is the encryption VECTOR</param>
        /// <returns></returns>
        private static string DecryptString(string strSource, string strKey, string strVi)
        {
            // Parameters : 
            //  bytSource is the source string (encrypted) 
            //  aryKey is the encryption KEY 
            //  aryVi is the encryption VECTOR 
            // 
            // Descriptions : 
            //  Note : For DES algo, key and vi size have sizes of 8 bytes each (64 bits) 
            // 
            // Author/Date : 2008-10-08, Cuttlebone

            strKey = Prepare_Key(strKey);
            strVi = Prepare_Key(strVi);

            // Translate the source into bytes 
            byte[] bytSource = StringToBytes(strSource);

            // the final result 
            byte[] bytResult = new byte[bytSource.Length + 1];

            // Fix up the KEY and VI values 
            // They must be 8 bytes 
            byte[] bytDesKey = Des_StrToByte(strKey);
            byte[] bytDesVi = Des_StrToByte(strVi);


            // The DES algo object 
            DESCryptoServiceProvider objDes = new DESCryptoServiceProvider();
            // Set the max key size, 64 bits 
            objDes.KeySize = 64;

            // The Decryptor object 
            ICryptoTransform objDecryptor = objDes.CreateDecryptor(bytDesKey, bytDesVi);

            // The memory stream to hold our source (encrypted) 
            MemoryStream objMemory = new MemoryStream(bytSource);

            // Now create a crypto stream which combine the algorithm and the source stream 
            // We can think of it as : The crypto stream defined how the data is being read. 
            // The stream is where the encrypted data is stored, the algo is how to decrypt it 
            CryptoStream objCryptoStream = new CryptoStream(objMemory, objDecryptor, CryptoStreamMode.Read);

            // Number of bytes decoded 
            int intCount = 0;
            try
            {
                // Read out the data from the stream 
                // system will decrypt the data during the read 
                // We have to be careful here, the encrypted data may be larger than the decrypted data 
                // intCount stored the actual decrypted bytes from the system. 
                // the actual size is what we needed. 
                // We need this actual value since we have declared byeResult to have the 
                // same size as the excrypted source, which may be too large for the actual result 
                intCount = objCryptoStream.Read(bytResult, 0, bytSource.Length);
            }
            finally
            {
                objMemory.Close();
                objCryptoStream.Close();
            }

            return Encoding.Unicode.GetString(bytResult, 0, intCount);
        }

        /// <summary>
        ///  For DES algo, the KEY and VI size are 64 bits (8 bytes) each 
        ///  For easy usage and declaration, we allow user to passed in key and vi values in 
        ///  string format (8 caracters). We then convert these unicode strings into its corresponding 
        ///  byte values.eg, "A" --> byte(97) 
        /// </summary>
        /// <param name="strBlock">a byte array (8 bytes)</param>
        /// <returns></returns>
        private static byte[] Des_StrToByte(string strBlock)
        {
            // Parameters : 
            //  byeBlock - a byte array (8 bytes) 
            // 
            // Descriptions : 
            //  For DES algo, the KEY and VI size are 64 bits (8 bytes) each 
            //  For easy usage and declaration, we allow user to passed in key and vi values in 
            //  string format (8 caracters). We then convert these unicode strings into its corresponding 
            //  byte values.eg, "A" --> byte(97) 
            // 
            // Author/Date : 2008-10-08, Cuttlebone

            // Default the result to 8 byrtes with value 0 
            byte[] bytValue = { 0, 0, 0, 0, 0, 0, 0, 0 };
            int intCounter = 0;

            int intLen = strBlock.Length;
            char[] chrTemp = strBlock.ToCharArray();

            // Lopr throught each character to convert it to BYTE 
            for (intCounter = 0; intCounter <= 7; intCounter++)
            {
                if (intCounter < intLen)
                {
                    // first we get the ASCII value for the char, then we convert that to 
                    // a BYTE value 
                    // bytValue[intCounter] = (byte)JStr.Asc(chrTemp[intCounter]);
                    bytValue[intCounter] = (byte)chrTemp[intCounter];
                }
            }

            return bytValue;
        }
        /// <summary>
        /// prepare the keys 
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        private static string Prepare_Key(string strKey)
        {
            // Descriptions : 
            //  prepare the keys 
            // 
            // Author/Date : 2008-10-08, Cuttlebone

            int intCounter = 0;
            string strTempKey = "";
            string strTemp = "";
            int intCode = 0;
            for (intCounter = 1; intCounter <= strKey.Length; intCounter++)
            {
                strTemp = strKey.Substring(strKey.Length - intCounter, 1);
                // intCode = JStr.Asc(strTemp) + intCounter;
                intCode = Convert.ToChar(strTemp) + intCounter;
                if (intCode > 255)
                {
                    intCode = intCode - (intCounter * 2);
                }
                // strTemp = JStr.Chr(intCode).ToString();
                strTemp = ((char)intCode).ToString();
                strTempKey += strTemp;
            }

            return strTempKey;
        }
        /// <summary>
        ///  conversion a byte array into a safe string representation 
        ///  We cannot directly convert the byte to string, as the 
        ///  string may contain a lot of unexpetced chars ... 
        /// </summary>
        /// <param name="objSource"></param>
        /// <returns></returns>
        private static string BytesToString(byte[] objSource)
        {
            // Parameters : 
            // 
            // Descriptions : 
            //  conversion a byte array into a safe string representation 
            //  We cannot directly convert the byte to string, as the 
            //  string may contain a lot of unexpetced chars ... 
            // 
            // Return : 
            // Author/Date : 2008-10-08, Cuttlebone
            int intCounter = 0;
            string strResult = "";

            for (intCounter = 0; intCounter <= objSource.Length - 1; intCounter++)
            {
                // we convert a byte to its HEX representaion string 
                // ie. each byte need 2 chars, eg, code 255 = FF 
                // strResult += Conversion.Hex((int)objSource[intCounter]).PadLeft(2, '0');
                strResult += Convert.ToString(objSource[intCounter], 16).PadLeft(2, '0').ToUpper();
            }
            return strResult;
        }

        /// <summary>
        /// Convert a string (previous processed by BytesToString()), back to its byte array.
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        private static byte[] StringToBytes(string strSource)
        {
            // Parameters : 
            // 
            // Descriptions : 
            //  Convert a string (previous processed by BytesToString()), back to its byte array. 
            // 
            // Return : 
            // Author/Date : 2008-10-08, Cuttlebone

            if ((strSource.Length % 2) != 0)
            {
                // bad string, it should be a multiple of 2 
                return null;
            }

            int intCounter = 0;
            int intParts = (int)strSource.Length / 2;
            byte[] objResult = new byte[intParts];
            for (intCounter = 0; intCounter <= intParts - 1; intCounter++)
            {
                //objResult[intCounter] = (byte)(int)Conversion.Val("&H" + strSource.Substring(intCounter * 2, 2));
                // objResult[intCounter] = byte.Parse(strSource.Substring(intCounter * 2, 2), System.Globalization.NumberStyles.HexNumber);
                if (!byte.TryParse(strSource.Substring(intCounter * 2, 2), System.Globalization.NumberStyles.HexNumber, null, out objResult[intCounter]))
                {
                    break;
                }
            }

            return objResult;
        }

        /// <summary>
        /// 将字符串加密MD5
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string MD5(string s)
        {
            string result = "";
            try
            {
                MD5 getmd5 = new MD5CryptoServiceProvider();
                byte[] targetStr = getmd5.ComputeHash(UnicodeEncoding.UTF8.GetBytes(s));
                result = BitConverter.ToString(targetStr).Replace("-", "");
                return result;
            }
            catch (Exception)
            {
                return "0";
            }
        }
    }

    /// <summary>
    /// encryption/deccryption serialize data.
    /// </summary>
    public static class DataProtection
    {
        /// <summary>
        /// specify an entropy so other DPAPI applications can't see the data
        /// </summary>
        public readonly static byte[] EntropyData = ASCIIEncoding.ASCII.GetBytes("2CB23592-5C2B-4ddb-A57C-FEDF3A70E9AD");

        /// <summary>
        /// encrypt the data using DPAPI, returns a base64-encoded encrypted string
        /// </summary>
        /// <param name="data"></param>
        /// <param name="store"></param>
        /// <returns></returns>
        public static string Encrypt(string data, Store store)
        {
            // holds the result string
            string result = "";

            // blobs used in the CryptProtectData call
            Win32.DATA_BLOB inBlob = new Win32.DATA_BLOB();
            Win32.DATA_BLOB entropyBlob = new Win32.DATA_BLOB();
            Win32.DATA_BLOB outBlob = new Win32.DATA_BLOB();

            try
            {
                // setup flags passed to the CryptProtectData call
                int flags = Win32.CRYPTPROTECT_UI_FORBIDDEN |
                    (int)((store == Store.Machine) ? Win32.CRYPTPROTECT_LOCAL_MACHINE : 0);

                // setup input blobs, the data to be encrypted and entropy blob
                SetBlobData(ref inBlob, ASCIIEncoding.ASCII.GetBytes(data));
                SetBlobData(ref entropyBlob, EntropyData);

                // call the DPAPI function, returns true if successful and fills in the outBlob
                if (Win32.CryptProtectData(ref inBlob, "", ref entropyBlob, IntPtr.Zero, IntPtr.Zero, flags, ref outBlob))
                {
                    byte[] resultBits = GetBlobData(ref outBlob);
                    if (resultBits != null)
                        result = Convert.ToBase64String(resultBits);
                }
            }
            finally
            {
                // clean up
                if (inBlob.pbData.ToInt32() != 0)
                    Marshal.FreeHGlobal(inBlob.pbData);

                if (entropyBlob.pbData.ToInt32() != 0)
                    Marshal.FreeHGlobal(entropyBlob.pbData);
            }

            return result;
        }
        /// <summary>
        /// decrypt the data using DPAPI, data is a base64-encoded encrypted string
        /// </summary>
        /// <param name="data"></param>
        /// <param name="store"></param>
        /// <returns></returns>
        public static string Decrypt(string data, Store store)
        {
            // holds the result string
            string result = "";

            // blobs used in the CryptUnprotectData call
            Win32.DATA_BLOB inBlob = new Win32.DATA_BLOB();
            Win32.DATA_BLOB entropyBlob = new Win32.DATA_BLOB();
            Win32.DATA_BLOB outBlob = new Win32.DATA_BLOB();

            try
            {
                // setup flags passed to the CryptUnprotectData call
                int flags = Win32.CRYPTPROTECT_UI_FORBIDDEN |
                    (int)((store == Store.Machine) ? Win32.CRYPTPROTECT_LOCAL_MACHINE : 0);

                // the CryptUnprotectData works with a byte array, convert string data
                byte[] bits = Convert.FromBase64String(data);

                // setup input blobs, the data to be decrypted and entropy blob
                SetBlobData(ref inBlob, bits);
                SetBlobData(ref entropyBlob, EntropyData);

                // call the DPAPI function, returns true if successful and fills in the outBlob
                if (Win32.CryptUnprotectData(ref inBlob, null, ref entropyBlob, IntPtr.Zero, IntPtr.Zero, flags, ref outBlob))
                {
                    byte[] resultBits = GetBlobData(ref outBlob);
                    if (resultBits != null)
                        result = ASCIIEncoding.ASCII.GetString(resultBits);
                }
            }
            finally
            {
                // clean up
                if (inBlob.pbData.ToInt32() != 0)
                    Marshal.FreeHGlobal(inBlob.pbData);

                if (entropyBlob.pbData.ToInt32() != 0)
                    Marshal.FreeHGlobal(entropyBlob.pbData);
            }

            return result;
        }

        /// <summary>
        /// helper method that fills in a DATA_BLOB, copies 
        /// data from managed to unmanaged memory
        /// </summary>
        /// <param name="blob"></param>
        /// <param name="bits"></param>
        private static void SetBlobData(ref Win32.DATA_BLOB blob, byte[] bits)
        {
            blob.cbData = bits.Length;
            blob.pbData = Marshal.AllocHGlobal(bits.Length);
            Marshal.Copy(bits, 0, blob.pbData, bits.Length);
        }
        /// <summary>
        /// helper method that gets data from a DATA_BLOB, 
        /// copies data from unmanaged memory to managed
        /// </summary>
        /// <param name="blob"></param>
        /// <returns></returns>
        private static byte[] GetBlobData(ref Win32.DATA_BLOB blob)
        {
            // return an empty string if the blob is empty
            if (blob.pbData.ToInt32() == 0)
                return null;

            // copy information from the blob
            byte[] data = new byte[blob.cbData];
            Marshal.Copy(blob.pbData, data, 0, blob.cbData);
            Win32.LocalFree(blob.pbData);

            return data;
        }

        /// <summary>
        /// encrypt api functional
        /// </summary>
        private class Win32
        {
            internal const int CRYPTPROTECT_UI_FORBIDDEN = 0x1;
            internal const int CRYPTPROTECT_LOCAL_MACHINE = 0x4;

            [StructLayout(LayoutKind.Sequential)]
            internal struct DATA_BLOB
            {
                public int cbData;
                public IntPtr pbData;
            }

            [DllImport("crypt32", CharSet = CharSet.Auto)]
            internal static extern bool CryptProtectData(ref DATA_BLOB pDataIn, string szDataDescr, ref DATA_BLOB pOptionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, int dwFlags, ref DATA_BLOB pDataOut);

            [DllImport("crypt32", CharSet = CharSet.Auto)]
            internal static extern bool CryptUnprotectData(ref DATA_BLOB pDataIn, StringBuilder szDataDescr, ref DATA_BLOB pOptionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, int dwFlags, ref DATA_BLOB pDataOut);

            [DllImport("kernel32")]
            internal static extern IntPtr LocalFree(IntPtr hMem);
        }
        /// <summary>
        /// use local machine or user to encrypt and decrypt the data
        /// </summary>
        public enum Store
        {
            /// <summary>
            /// machine
            /// </summary>
            Machine = 0,
            /// <summary>
            /// user
            /// </summary>
            User = 1
        }
    }


    /*
    public sealed class Encryption
    {
        // *** (PWD) DON'T CHANGE THIS KEYS AND VI Key = "0DwpKye2", Vi = "20eTiL02" ***
        private const String DEFAULT_PWD_KEY = "0DwpKye2";
        private const String DEFAULT_PWD_VI = "20eTiL02";

        // *** (ADM) DON'T CHANGE THIS KEYS AND VI Key = "34TyUmw2", Vi = "H54yuPs9" ***
        private const String DEFAULT_ADM_KEY = "34TyUmw2";
        private const String DEFAULT_ADM_VI = "H54yuPs9";

        /// <summary>
        /// this funciton will decrypted the adm password from SQL server
        /// </summary>
        /// <param name="strPwd">encrypted admin password</param>
        /// <returns></returns>
        public static string DecryptPreLogin(string strPwd)
        {
            // Parameters : 
            //  strPwd - encrypted admin password 
            // 
            // Descriptions : 
            //  this funciton will decrypted the adm password from SQL server 
            // 
            // Author/Date : 2008-10-08, Cuttlebone

            try
            {
                return DecryptAdmin(strPwd);
            }
            catch (Exception objE)
            {
                Fzsys.Library.DebugHelper.SysError.Add(objE);
                return "";
            }
        }
        /// <summary>
        /// this funciton will encrypt the user password from SQL server 
        /// </summary>
        /// <param name="strPwd">clear text user password</param>
        /// <returns></returns>
        public static string EncryptUserPwd(string strPwd)
        {
            // Parameters : 
            //  strPwd - clear text user password 
            // 
            // Descriptions : 
            //  this funciton will encrypt the user password from SQL server 
            // 
            // Author/Date : 2008-10-08, Cuttlebone

            try
            {
                return EncryptUser(strPwd);
            }
            catch (Exception objE)
            {
                Fzsys.Library.DebugHelper.SysError.Add(objE);
                return "";
            }
        }
        /// <summary>
        /// this funciton will encrypt the admin password for SQL server
        /// </summary>
        /// <param name="strPwd">clear text admin password</param>
        /// <returns></returns>
        public static string EncryptAdminPwd(string strPwd)
        {
            // Parameters : 
            //  strPwd - clear text admin password 
            // 
            // Descriptions : 
            //  this funciton will encrypt the admin password for SQL server 
            // 
            // Author/Date : 2008-10-08, Cuttlebone

            try
            {
                return EncryptAdmin(strPwd);
            }
            catch (Exception objE)
            {
                Fzsys.Library.DebugHelper.SysError.Add(objE);
                return "";
            }
        }
        /// <summary>
        /// Test
        /// </summary>
        public static void Testing()
        {
            string strOriginal = null;
            string strEncrypted = null;
            string strDecrypted = null;

            strOriginal = System.Guid.NewGuid().ToString();

            strEncrypted = EncryptAdmin(strOriginal);
            strDecrypted = DecryptAdmin(strEncrypted);

            MessageBox.Show("Original:" + " Size:" + strOriginal.Length.ToString() + SysConstants.vbCrLf + 
                strOriginal + SysConstants.vbCrLf + SysConstants.vbCrLf + 
                "Encrypted:" + " Size:" + strEncrypted.Length.ToString() + SysConstants.vbCrLf + 
                strEncrypted + SysConstants.vbCrLf + SysConstants.vbCrLf + 
                "Decrypted:" + " Size: " + strDecrypted.Length.ToString() + SysConstants.vbCrLf + 
                strDecrypted);
        }

        /// <summary>
        /// This function will encryt a password and return the encrypted result
        /// </summary>
        /// <param name="strSource">The password need to be encrypted</param>
        /// <returns></returns>
        private static string EncryptUser(string strSource)
        {
            // Parameters : 
            //  strSource - The password need to be encrypted 
            // 
            // Descriptions : 
            //  This function will encryt a password and return the encrypted result 
            // 
            // Author/Date : 2008-10-08, Cuttlebone

            return EncryptString(strSource, DEFAULT_PWD_KEY, DEFAULT_PWD_VI);
        }
        /// <summary>
        /// The password need to be encrypted
        /// </summary>
        /// <param name="strSource">This function will encryt an admin password and return the encrypted result </param>
        /// <returns></returns>
        private static string EncryptAdmin(string strSource)
        {
            // Parameters : 
            //  strSource - The password need to be encrypted 
            // 
            // Descriptions : 
            //  This function will encryt an admin password and return the encrypted result 
            // 
            // Author/Date : 2008-10-08, Cuttlebone

            return EncryptString(strSource, DEFAULT_ADM_KEY, DEFAULT_ADM_VI);
        }
        /// <summary>
        /// This function will decrypt an encrypted admin password and return the result 
        /// </summary>
        /// <param name="strSource">The password need to be encrypted</param>
        /// <returns></returns>
        private static string DecryptAdmin(string strSource)
        {
            // Parameters : 
            //  strSource - The password need to be encrypted 
            // 
            // Descriptions : 
            //  This function will decrypt an encrypted admin password and return the result 
            // 
            // Author/Date : 2008-10-08, Cuttlebone

            // testing routine 
            // to end the loop, keep SHIFT pressed when clciking ok at the message box 
            //Dim a As String 
            //Dim b As String 
            //Dim c As String 
            //Dim intIndex As Integer 
            //Dim interror As Integer = 0 

            //For intIndex = 1 To 100 
            // a = Guid.NewGuid.ToString 
            // b = EncryptString(a, DEFAULT_ADM_KEY, DEFAULT_ADM_VI) 
            // c = DecryptString(b, DEFAULT_ADM_KEY, DEFAULT_ADM_VI) 
            // If a <> c Then 
            // interror += 1 
            // End If 

            // MsgBox(a + vbCrLf + b + vbCrLf + c + vbCrLf + interror.ToString + vbCrLf + "Keep SHIFT pressed when click OK to stop the loop") 
            // If (Windows.Forms.Control.ModifierKeys And Windows.Forms.Keys.Shift) = Windows.Forms.Keys.Shift Then 
            // Exit For 
            // End If 
            //Next 

            //MsgBox(DecryptString(strSource, DEFAULT_ADM_KEY, DEFAULT_ADM_VI)) 

            return DecryptString(strSource, DEFAULT_ADM_KEY, DEFAULT_ADM_VI);
        }
        /// <summary>
        /// For DES algo, key and vi size are 8 bytes each.
        ///  ** IF you want to print out the resulting byte array(eg. key), you can try 
        ///  MsgBox(System.Text.Encoding.ASCII.GetChars(bytKey)) or 
        ///  MsgBox(System.Text.Encoding.Unicode.GetStrings(bytKey)) 
        /// </summary>
        /// <param name="strSource">strSource is the source string </param>
        /// <param name="strKey">aryKey is the encryption KEY</param>
        /// <param name="strVi">aryVi is the encryption VECTOR</param>
        /// <returns></returns>
        private static string EncryptString(string strSource, string strKey, string strVi)
        {
            // Parameters : 
            //  strSource is the source string 
            //  aryKey is the encryption KEY
            //  aryVi is the encryption VECTOR 
            // 
            // Descriptions : 
            //  Note : For DES algo, key and vi size are 8 bytes each. 
            // 
            //  ** IF you want to print out the resulting byte array(eg. key), you can try 
            //  MsgBox(System.Text.Encoding.ASCII.GetChars(bytKey)) or 
            //  MsgBox(System.Text.Encoding.Unicode.GetStrings(bytKey)) 
            // 
            // Author/Date : 2008-10-08, Cuttlebone

            strKey = Prepare_Key(strKey);
            strVi = Prepare_Key(strVi);

            byte[] bytResult = null;

            // Fix up the KEY and VI values 
            // They must be 8 bytes for DES 
            byte[] bytDesKey = Des_StrToByte(strKey);
            byte[] bytDesVi = Des_StrToByte(strVi);

            // Translate the source into bytes 
            byte[] bytSource = Encoding.Unicode.GetBytes(strSource.ToCharArray());

            // The DES algo object 
            DESCryptoServiceProvider objDes = new DESCryptoServiceProvider();
            // Set the max key size, 64 bits 
            objDes.KeySize = 64;

            // The Encryptor object 
            ICryptoTransform objEncryptor = objDes.CreateEncryptor(bytDesKey, bytDesVi);

            // The temp memory stream to hold our final encrypted result 
            MemoryStream objMemory = new MemoryStream();

            // Now create a crypto stream which combine the algorithm and the output stream 
            // together. 
            // We can think of it as : The crypto stream defined how the data is stored. 
            // the stream is where it is stored, the algo is how it is encrypted. 
            CryptoStream objCryptoStream = new CryptoStream(objMemory, objEncryptor, CryptoStreamMode.Write);

            try
            {
                // put the source into the crypto stream. 
                // Since we have defined and algo, the system will 
                // encrypt the data during the process 
                objCryptoStream.Write(bytSource, 0, bytSource.Length);

                // stream has buffer, so we do a flush to make sure 
                // the underlying destination is up to date 
                objCryptoStream.FlushFinalBlock();
            }
            catch 
            {
                bytResult = null;
            }
            finally
            {
                // When we are done, do some clean up of those streams 
                bytResult = objMemory.ToArray();

                objMemory.Close();
                objCryptoStream.Close();
            }

            return BytesToString(objMemory.ToArray());
        }
        /// <summary>
        /// For DES algo, key and vi size have sizes of 8 bytes each (64 bits)
        /// </summary>
        /// <param name="strSource">bytSource is the source string (encrypted)</param>
        /// <param name="strKey">aryKey is the encryption KEY</param>
        /// <param name="strVi">aryVi is the encryption VECTOR</param>
        /// <returns></returns>
        private static string DecryptString(string strSource, string strKey, string strVi)
        {
            // Parameters : 
            //  bytSource is the source string (encrypted) 
            //  aryKey is the encryption KEY 
            //  aryVi is the encryption VECTOR 
            // 
            // Descriptions : 
            //  Note : For DES algo, key and vi size have sizes of 8 bytes each (64 bits) 
            // 
            // Author/Date : 2008-10-08, Cuttlebone

            try
            {
                strKey = Prepare_Key(strKey);
                strVi = Prepare_Key(strVi);

                // Translate the source into bytes 
                byte[] bytSource = StringToBytes(strSource);

                // the final result 
                byte[] bytResult = new byte[bytSource.Length + 1];

                // Fix up the KEY and VI values 
                // They must be 8 bytes 
                byte[] bytDesKey = Des_StrToByte(strKey);
                byte[] bytDesVi = Des_StrToByte(strVi);


                // The DES algo object 
                DESCryptoServiceProvider objDes = new DESCryptoServiceProvider();
                // Set the max key size, 64 bits 
                objDes.KeySize = 64;

                // The Decryptor object 
                ICryptoTransform objDecryptor = objDes.CreateDecryptor(bytDesKey, bytDesVi);

                // The memory stream to hold our source (encrypted) 
                MemoryStream objMemory = new MemoryStream(bytSource);

                // Now create a crypto stream which combine the algorithm and the source stream 
                // We can think of it as : The crypto stream defined how the data is being read. 
                // The stream is where the encrypted data is stored, the algo is how to decrypt it 
                CryptoStream objCryptoStream = new CryptoStream(objMemory, objDecryptor, CryptoStreamMode.Read);

                // Number of bytes decoded 
                int intCount = 0;
                try
                {
                    // Read out the data from the stream 
                    // system will decrypt the data during the read 
                    // We have to be careful here, the encrypted data may be larger than the decrypted data 
                    // intCount stored the actual decrypted bytes from the system. 
                    // the actual size is what we needed. 
                    // We need this actual value since we have declared byeResult to have the 
                    // same size as the excrypted source, which may be too large for the actual result 
                    intCount = objCryptoStream.Read(bytResult, 0, bytSource.Length);
                }
                catch 
                {
                    bytResult = null;
                }
                finally
                {
                    objMemory.Close();
                    objCryptoStream.Close();
                }

                return Encoding.Unicode.GetString(bytResult, 0, intCount);
            }
            catch 
            {
                // ignore error 
                //Fzsys.Library.DebugHelper.SysError.Add(objE) 
                return "";
            }
        }
        /// <summary>
        ///  For DES algo, the KEY and VI size are 64 bits (8 bytes) each 
        ///  For easy usage and declaration, we allow user to passed in key and vi values in 
        ///  string format (8 caracters). We then convert these unicode strings into its corresponding 
        ///  byte values.eg, "A" --> byte(97) 
        /// </summary>
        /// <param name="strBlock">a byte array (8 bytes)</param>
        /// <returns></returns>
        private static byte[] Des_StrToByte(string strBlock)
        {
            // Parameters : 
            //  byeBlock - a byte array (8 bytes) 
            // 
            // Descriptions : 
            //  For DES algo, the KEY and VI size are 64 bits (8 bytes) each 
            //  For easy usage and declaration, we allow user to passed in key and vi values in 
            //  string format (8 caracters). We then convert these unicode strings into its corresponding 
            //  byte values.eg, "A" --> byte(97) 
            // 
            // Author/Date : 2008-10-08, Cuttlebone

            // Default the result to 8 byrtes with value 0 
            byte[] bytValue = { 0, 0, 0, 0, 0, 0, 0, 0 };
            int intCounter = 0;

            int intLen = strBlock.Length;
            char[] chrTemp = strBlock.ToCharArray();

            // Lopr throught each character to convert it to BYTE 
            for (intCounter = 0; intCounter <= 7; intCounter++)
            {
                if (intCounter < intLen)
                {
                    // first we get the ASCII value for the char, then we convert that to 
                    // a BYTE value 
                    bytValue[intCounter] = (byte)JStr.Asc(chrTemp[intCounter]);
                }
            }

            return bytValue;
        }
        /// <summary>
        /// prepare the keys 
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        private static string Prepare_Key(string strKey)
        {
            // Descriptions : 
            //  prepare the keys 
            // 
            // Author/Date : 2008-10-08, Cuttlebone

            int intCounter = 0;
            string strTempKey = "";
            string strTemp = "";
            int intCode = 0;
            for (intCounter = 1; intCounter <= strKey.Length; intCounter++)
            {
                strTemp = strKey.Substring(strKey.Length - intCounter, 1);
                intCode = JStr.Asc(strTemp) + intCounter;
                if (intCode > 255)
                {
                    intCode = intCode - (intCounter * 2);
                }
                strTemp = JStr.Chr(intCode).ToString();
                strTempKey += strTemp;
            }

            return strTempKey;
        }
        /// <summary>
        ///  conversion a byte array into a safe string representation 
        ///  We cannot directly convert the byte to string, as the 
        ///  string may contain a lot of unexpetced chars ... 
        /// </summary>
        /// <param name="objSource"></param>
        /// <returns></returns>
        private static string BytesToString(byte[] objSource)
        {
            // Parameters : 
            // 
            // Descriptions : 
            //  conversion a byte array into a safe string representation 
            //  We cannot directly convert the byte to string, as the 
            //  string may contain a lot of unexpetced chars ... 
            // 
            // Return : 
            // Author/Date : 2008-10-08, Cuttlebone

            try
            {
                int intCounter = 0;
                string strResult = "";

                for (intCounter = 0; intCounter <= objSource.Length - 1; intCounter++)
                {
                    // we convert a byte to its HEX representaion string 
                    // ie. each byte need 2 chars, eg, code 255 = FF 
                    strResult += Conversion.Hex((int)objSource[intCounter]).PadLeft(2, '0');
                }
                return strResult;
            }
            catch (Exception objE)
            {
                Fzsys.Library.DebugHelper.SysError.Add(objE);
                return "";
            }
        }
        /// <summary>
        /// Convert a string (previous processed by BytesToString()), back to its byte array.
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        private static byte[] StringToBytes(string strSource)
        {
            // Parameters : 
            // 
            // Descriptions : 
            //  Convert a string (previous processed by BytesToString()), back to its byte array. 
            // 
            // Return : 
            // Author/Date : 2008-10-08, Cuttlebone

            try
            {
                if ((strSource.Length % 2) != 0) {
                    // bad string, it should be a multiple of 2 
                    return null;
                }

                int intCounter = 0;
                int intParts = (int)strSource.Length / 2;
                byte[] objResult = new byte[intParts];


                for (intCounter = 0; intCounter <= intParts - 1; intCounter++)
                {
                    objResult[intCounter] = (byte)(int)Conversion.Val("&H" + (strSource.Substring((intCounter * 2), 2)));
                }

                return objResult;
            }
            catch (Exception objE)
            {
                Fzsys.Library.DebugHelper.SysError.Add(objE);
                return null;
            }
        } 

    }
    */
}
