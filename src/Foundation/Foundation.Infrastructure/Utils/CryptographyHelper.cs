/*------------------------------Summary------------------------------------------------------
 * Product Name : Energy Management Open Platform Software
 * File Name	: CryptographyHelper.cs
 * Author	    : Figo
 * Date Created : 2011-10-18
 * Description  : Helper for cryptography
 * Copyright    : Schneider Electric (China) Co., Ltd.
--------------------------------------------------------------------------------------------*/

using SE.DSP.Foundation.Infrastructure.Constant;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    public static class CryptographyHelper
    {
        public static string MD5(string plaintext)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] ciphertext = md5.ComputeHash(UnicodeEncoding.UTF8.GetBytes(plaintext));
            return BitConverter.ToString(ciphertext).Replace(ASCII.SUBTRACT, string.Empty);
        }
    }
}