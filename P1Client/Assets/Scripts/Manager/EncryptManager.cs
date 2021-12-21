//
// Created on Wed May 15 2019
//
// Copyright (c) 2019 ChangGeun YI
//

 using System;
 using System.IO;
 using System.Text;
 using System.Security.Cryptography;


namespace NodapParty
{	
	public sealed class EncryptManager : SingletonObject<EncryptManager>
    {
        public enum kTYPE
        {
            None,
            Base64,
            MD5
        }

        #region Base64.

        /// <summary>
        /// 입력된 문자열을 Base64로 암호화 합니다.
        /// </summary>
		public string EncryptBase64(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes);
		}

        /// <summary>
        /// 입력된 문자열을 Base64로 복호화 합니다.
        /// </summary>
		public string DecryptBase64(string text)
        {
			var bytes = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(bytes);
		}

        #endregion Base64.


		#region MD5.

        /// <summary>
        /// 입력된 문자열을 MD5로 암호화 합니다.
        /// </summary>
		public string EncryptMD5(string text)
        {
			var hash = MD5.Create();
            var buf = hash.ComputeHash(Encoding.UTF8.GetBytes(text));

            StringBuilder sb = new StringBuilder();
			int len = buf.Length;
			for (int i = 0; i < len; i++)
            {
				sb.Append(buf[i].ToString("x2"));
			}

            return sb.ToString();
		}

        /// <summary>
        /// 입력된 문자열과 HASH 데이터 MD5의 동일성을 확인합니다.
        /// </summary>
		public bool IsVerifyMD5(string text, string hash)
        {
            string newHash = EncryptMD5(text);
			return (StringComparer.OrdinalIgnoreCase.Compare(newHash, hash) == 0) ? true : false;
		}

		#endregion MD5.
	}
}