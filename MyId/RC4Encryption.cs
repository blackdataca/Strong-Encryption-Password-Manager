using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MyId
{


    public class RC4Encryption
    {
        private byte[] S = new byte[256];
        private byte[] K = new byte[256];

        public void HexToBin(string hexData, out byte[] binData)
        {
            binData = new byte[hexData.Length / 2];
            for (int i = 0; i < hexData.Length; i += 2)
            {
                binData[i / 2] = Convert.ToByte(hexData.Substring(i, 2), 16);
            }
        }

        public void RCInitKey(string myKey)
        {
            int mkeyLen = myKey.Length;
            for (int i = 0; i < 256; i++)
            {
                S[i] = (byte)i;
                K[i] = (byte)myKey[i % mkeyLen];
            }

            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                j = (j + S[i] + K[i]) % 256;
                byte t = S[i];
                S[i] = S[j];
                S[j] = t;
            }
        }

        public byte[] A2C(byte[] input)
        {
            byte[] c = new byte[input.Length / 2];
            for (int i = 0; i < input.Length; i += 2)
            {
                c[i / 2] = (byte)(input[i] + input[i + 1] * 256);
            }
            return c;
        }

        public byte[] A2C_ANSI(byte[] input)
        {
            byte[] c = new byte[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                c[i] = (byte)input[i];
            }
            return c;
        }

        public byte[] RC(byte[] input, string myKey)
        {
            RCInitKey(myKey);
            int i = 0;
            int j = 0;
            byte[] tempstr = new byte[input.Length];
            byte[] e_text = new byte[input.Length];
            for (int n = 0; n < input.Length; n++)
            {
                i = (i + 1) % 256;
                j = (j + S[i]) % 256;
                byte t = S[i];
                S[i] = S[j];
                S[j] = t;
                t = (byte)((S[i] + S[j]) % 256);
                int xorIndex = S[t];
                tempstr[n] = (byte)(input[n] ^ xorIndex);
            }
            e_text = A2C(tempstr);
            return e_text;
        }

        public byte[] RC_ANSI(byte[] input, string myKey)
        {
            RCInitKey(myKey);
            int i = 0;
            int j = 0;
            byte[] tempstr = new byte[input.Length];
            byte[] e_text = new byte[input.Length];
            for (int n = 0; n < input.Length; n++)
            {
                i = (i + 1) % 256;
                j = (j + S[i]) % 256;
                byte t = S[i];
                S[i] = S[j];
                S[j] = t;
                t = (byte)((S[i] + S[j]) % 256);
                int xorIndex = S[t];
                tempstr[n] = (byte)(input[n] ^ xorIndex);
            }
            e_text = A2C_ANSI(tempstr);
            return e_text;
        }
        public string Bin2Hex(byte[] input)
        {
            return BitConverter.ToString(input).Replace("-", "").ToLower();
        }

        public string MyEncrypt(string input, string myKey)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] e_text = RC(inputBytes, myKey);
            string hexString = BitConverter.ToString(e_text).Replace("-", "");
            return hexString.ToLower();
        }

        public string MyDecrypt(string input, string myKey)
        {
            byte[] tempstr;
            HexToBin(input, out tempstr);
            byte[] p_text = RC(tempstr, myKey);
            return Encoding.ASCII.GetString(p_text);
        }

        public string MyEncrypt_ANSI(string input, string key)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] e_text = RC_ANSI(inputBytes, key);
            string hexString = BitConverter.ToString(e_text).Replace("-", "");
            return "<enc>" + hexString.ToLower() + "</enc>";
        }

        public string MyDecrypt_ANSI(string input, string key)
        {
            if (input.StartsWith("<enc>"))
            {
                input = input.Substring(5, input.Length - 11);
                byte[] tempstr;
                HexToBin(input, out tempstr);
                byte[] p_text = RC_ANSI(tempstr, key);
                return Encoding.ASCII.GetString(p_text);
            }
            else
            {
                return input;
            }
        }

        public string MyEncrypt_Field(string input, string key)
        {
            if (input.Length < 8)
            {
                input += (char)129;
                input = input.PadRight(8, ' ');
            }
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] e_text = RC_ANSI(inputBytes, key);
            string hexString = BitConverter.ToString(e_text).Replace("-", "");
            return hexString.ToLower();
        }

        public string MyDecrypt_Field(string input, string key)
        {
            byte[] tempstr;
            HexToBin(input, out tempstr);
            byte[] p_text = RC_ANSI(tempstr, key);
            string decodedText = Encoding.ASCII.GetString(p_text);
            if (decodedText.Length == 8)
            {
                int pos = decodedText.LastIndexOf((char)129);
                if (pos >= 0)
                {
                    decodedText = decodedText.Substring(0, pos);
                }
            }
            return decodedText;
        }

        public static string UniqId(string prefix, bool more_entropy)
        {
            if (string.IsNullOrEmpty(prefix))
                prefix = string.Empty;

            if (!more_entropy)
            {
                return (prefix + System.Guid.NewGuid().ToString().Replace("-", "")).Substring(0, 13);
            }
            else
            {
                return (prefix + System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 14)) + "." + System.Guid.NewGuid().ToString().Substring(0, 8);
            }
        }
    }

    

}