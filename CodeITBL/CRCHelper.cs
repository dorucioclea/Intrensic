using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CodeITBL
{
    public class CRCHelper
    {       
        public uint CalculateMD5(BufferedStream stream)
        {
            
            using (var md5 = MD5.Create())
            {            
                return BitConverter.ToUInt32(md5.ComputeHash(stream),0);
            }
        }

        public uint CalculateMD5(byte[] pBuf)
        {
            using (var md5 = MD5.Create())
            {
                return BitConverter.ToUInt32(md5.ComputeHash(pBuf), 0);
            }
        }

        MD5 md5Incremental = null;
        byte[] dummy;
        public void CalculateMD5Incremental(byte[] pBuf, int bytesRead)
        {
            if (md5Incremental == null)
                md5Incremental = MD5.Create();
            dummy = pBuf;
            md5Incremental.TransformBlock(pBuf, 0, bytesRead, null, 0);
        }

        public string GetMD5IncrementalResult()
        {
            if (md5Incremental == null)
                return string.Empty;

            md5Incremental.TransformFinalBlock(dummy, 0, 0);

            return Convert.ToBase64String(md5Incremental.Hash);
        }
    }
}
