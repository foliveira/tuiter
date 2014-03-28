namespace Tuiter.Source.Utils.Web
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Text;

    internal sealed class HttpUtilities
    {
        private static readonly char[] SEntityEndingChars = new[] {';', '&'};

        private static string ASCIIGetString(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            return Encoding.ASCII.GetString(bytes, 0, bytes.Length);
        }

        private static int HexToInt(char h)
        {
            if ((h >= '0') && (h <= '9'))
            {
                return (h - '0');
            }
            if ((h >= 'a') && (h <= 'f'))
            {
                return ((h - 'a') + 10);
            }
            if ((h >= 'A') && (h <= 'F'))
            {
                return ((h - 'A') + 10);
            }
            return -1;
        }

        internal static string HtmlDecode(string s)
        {
            if (s == null)
            {
                return null;
            }
            if (s.IndexOf('&') < 0)
            {
                return s;
            }
            var sb = new StringBuilder();
            var output = new StringWriter(sb);
            HtmlDecode(s, output);
            return sb.ToString();
        }

        internal static void HtmlDecode(string s, TextWriter output)
        {
            if (s == null) return;

            if (s.IndexOf('&') < 0)
            {
                output.Write(s);
            }
            else
            {
                int length = s.Length;
                for (int i = 0; i < length; i++)
                {
                    char ch = s[i];
                    if (ch == '&')
                    {
                        int num3 = s.IndexOfAny(SEntityEndingChars, i + 1);
                        if ((num3 > 0) && (s[num3] == ';'))
                        {
                            string entity = s.Substring(i + 1, (num3 - i) - 1);
                            if ((entity.Length > 1) && (entity[0] == '#'))
                            {
                                try
                                {
                                    if ((entity[1] == 'x') || (entity[1] == 'X'))
                                    {
                                        ch = (char) int.Parse(entity.Substring(2), NumberStyles.AllowHexSpecifier);
                                    }
                                    else
                                    {
                                        ch = (char) int.Parse(entity.Substring(1));
                                    }
                                    i = num3;
                                }
                                catch (FormatException)
                                {
                                    i++;
                                }
                                catch (ArgumentException)
                                {
                                    i++;
                                }
                            }
                            else
                            {
                                i = num3;
                                char ch2 = HtmlEntities.Lookup(entity);
                                if (ch2 != '\0')
                                {
                                    ch = ch2;
                                }
                                else
                                {
                                    output.Write('&');
                                    output.Write(entity);
                                    output.Write(';');
                                    return;
                                }
                            }
                        }
                    }
                    output.Write(ch);
                }
            }
        }

        internal static char IntToHex(int n)
        {
            if (n <= 9)
            {
                return (char) (n + 0x30);
            }
            return (char) ((n - 10) + 0x61);
        }

        private static bool IsNonAsciiByte(byte b)
        {
            if (b < 0x7f)
            {
                return (b < 0x20);
            }
            return true;
        }

        internal static bool IsSafe(char ch)
        {
            if ((((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z'))) || ((ch >= '0') && (ch <= '9')))
            {
                return true;
            }
            switch (ch)
            {
                case '\'':
                case '(':
                case ')':
                case '*':
                case '-':
                case '.':
                case '_':
                case '!':
                    return true;
            }
            return false;
        }

        internal static string UrlDecode(string str)
        {
            return str == null ? null : UrlDecode(str, Encoding.UTF8);
        }

        internal static string UrlDecode(byte[] bytes, Encoding e)
        {
            return bytes == null ? null : UrlDecode(bytes, 0, bytes.Length, e);
        }

        internal static string UrlDecode(string str, Encoding e)
        {
            return str == null ? null : UrlDecodeStringFromStringInternal(str, e);
        }

        internal static string UrlDecode(byte[] bytes, int offset, int count, Encoding e)
        {
            if ((bytes == null) && (count == 0))
            {
                return null;
            }
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if ((offset < 0) || (offset > bytes.Length))
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((count < 0) || ((offset + count) > bytes.Length))
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return UrlDecodeStringFromBytesInternal(bytes, offset, count, e);
        }

        private static byte[] UrlDecodeBytesFromBytesInternal(byte[] buf, int offset, int count)
        {
            int length = 0;
            var sourceArray = new byte[count];
            for (int i = 0; i < count; i++)
            {
                int index = offset + i;
                byte num4 = buf[index];
                if (num4 == 0x2b)
                {
                    num4 = 0x20;
                }
                else if ((num4 == 0x25) && (i < (count - 2)))
                {
                    int num5 = HexToInt((char) buf[index + 1]);
                    int num6 = HexToInt((char) buf[index + 2]);
                    if ((num5 >= 0) && (num6 >= 0))
                    {
                        num4 = (byte) ((num5 << 4) | num6);
                        i += 2;
                    }
                }
                sourceArray[length++] = num4;
            }
            if (length < sourceArray.Length)
            {
                var destinationArray = new byte[length];
                Array.Copy(sourceArray, destinationArray, length);
                sourceArray = destinationArray;
            }
            return sourceArray;
        }

        private static string UrlDecodeStringFromBytesInternal(byte[] buf, int offset, int count, Encoding e)
        {
            var decoder = new UrlDecoder(count, e);
            for (int i = 0; i < count; i++)
            {
                int index = offset + i;
                byte b = buf[index];
                if (b == 0x2b)
                {
                    b = 0x20;
                }
                else if ((b == 0x25) && (i < (count - 2)))
                {
                    if ((buf[index + 1] == 0x75) && (i < (count - 5)))
                    {
                        int num4 = HexToInt((char) buf[index + 2]);
                        int num5 = HexToInt((char) buf[index + 3]);
                        int num6 = HexToInt((char) buf[index + 4]);
                        int num7 = HexToInt((char) buf[index + 5]);
                        if (((num4 < 0) || (num5 < 0)) || ((num6 < 0) || (num7 < 0)))
                        {
                            break;
                        }
                        var ch = (char) ((((num4 << 12) | (num5 << 8)) | (num6 << 4)) | num7);
                        i += 5;
                        decoder.AddChar(ch);
                        continue;
                    }
                    int num8 = HexToInt((char) buf[index + 1]);
                    int num9 = HexToInt((char) buf[index + 2]);
                    if ((num8 >= 0) && (num9 >= 0))
                    {
                        b = (byte) ((num8 << 4) | num9);
                        i += 2;
                    }
                }
                decoder.AddByte(b);
            }
            return decoder.GetString();
        }

        private static string UrlDecodeStringFromStringInternal(string s, Encoding e)
        {
            int length = s.Length;
            var decoder = new UrlDecoder(length, e);
            for (int i = 0; i < length; i++)
            {
                char ch = s[i];
                if (ch == '+')
                {
                    ch = ' ';
                }
                else if ((ch == '%') && (i < (length - 2)))
                {
                    if ((s[i + 1] == 'u') && (i < (length - 5)))
                    {
                        int num3 = HexToInt(s[i + 2]);
                        int num4 = HexToInt(s[i + 3]);
                        int num5 = HexToInt(s[i + 4]);
                        int num6 = HexToInt(s[i + 5]);
                        if (((num3 < 0) || (num4 < 0)) || ((num5 < 0) || (num6 < 0)))
                        {
                            break;
                        }
                        ch = (char) ((((num3 << 12) | (num4 << 8)) | (num5 << 4)) | num6);
                        i += 5;
                        decoder.AddChar(ch);
                        continue;
                    }
                    int num7 = HexToInt(s[i + 1]);
                    int num8 = HexToInt(s[i + 2]);
                    if ((num7 >= 0) && (num8 >= 0))
                    {
                        var b = (byte) ((num7 << 4) | num8);
                        i += 2;
                        decoder.AddByte(b);
                        continue;
                    }
                }
                if ((ch & 0xff80) == 0)
                {
                    decoder.AddByte((byte) ch);
                }
                else
                {
                    decoder.AddChar(ch);
                }
            }
            return decoder.GetString();
        }

        internal static byte[] UrlDecodeToBytes(byte[] bytes)
        {
            return bytes == null ? null : UrlDecodeToBytes(bytes, 0, bytes.Length);
        }

        internal static byte[] UrlDecodeToBytes(string str)
        {
            return str == null ? null : UrlDecodeToBytes(str, Encoding.UTF8);
        }

        internal static byte[] UrlDecodeToBytes(string str, Encoding e)
        {
            return str == null ? null : UrlDecodeToBytes(e.GetBytes(str));
        }

        internal static byte[] UrlDecodeToBytes(byte[] bytes, int offset, int count)
        {
            if ((bytes == null) && (count == 0))
            {
                return null;
            }
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if ((offset < 0) || (offset > bytes.Length))
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((count < 0) || ((offset + count) > bytes.Length))
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return UrlDecodeBytesFromBytesInternal(bytes, offset, count);
        }

        internal static string UrlEncode(byte[] bytes)
        {
            return bytes == null ? null : ASCIIGetString(UrlEncodeToBytes(bytes));
        }

        internal static string UrlEncode(string str)
        {
            return str == null ? null : UrlEncode(str, Encoding.UTF8);
        }

        internal static string UrlEncode(string str, Encoding e)
        {
            return str == null ? null : ASCIIGetString(UrlEncodeToBytes(str, e));
        }

        internal static string UrlEncode(byte[] bytes, int offset, int count)
        {
            return bytes == null ? null : ASCIIGetString(UrlEncodeToBytes(bytes, offset, count));
        }

        private static byte[] UrlEncodeBytesToBytesInternal(byte[] bytes, int offset, int count,
                                                            bool alwaysCreateReturnValue)
        {
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < count; i++)
            {
                var ch = (char) bytes[offset + i];
                if (ch == ' ')
                {
                    num++;
                }
                else if (!IsSafe(ch))
                {
                    num2++;
                }
            }
            if ((!alwaysCreateReturnValue && (num == 0)) && (num2 == 0))
            {
                return bytes;
            }
            var buffer = new byte[count + (num2*2)];
            int num4 = 0;
            for (int j = 0; j < count; j++)
            {
                byte num6 = bytes[offset + j];
                var ch2 = (char) num6;
                if (IsSafe(ch2))
                {
                    buffer[num4++] = num6;
                }
                else if (ch2 == ' ')
                {
                    buffer[num4++] = 0x2b;
                }
                else
                {
                    buffer[num4++] = 0x25;
                    buffer[num4++] = (byte) IntToHex((num6 >> 4) & 15);
                    buffer[num4++] = (byte) IntToHex(num6 & 15);
                }
            }
            return buffer;
        }

        private static byte[] UrlEncodeBytesToBytesInternalNonAscii(byte[] bytes, int offset, int count,
                                                                    bool alwaysCreateReturnValue)
        {
            int num = 0;
            for (int i = 0; i < count; i++)
            {
                if (IsNonAsciiByte(bytes[offset + i]))
                {
                    num++;
                }
            }
            if (!alwaysCreateReturnValue && (num == 0))
            {
                return bytes;
            }
            var buffer = new byte[count + (num*2)];
            int num3 = 0;
            for (int j = 0; j < count; j++)
            {
                byte b = bytes[offset + j];
                if (IsNonAsciiByte(b))
                {
                    buffer[num3++] = 0x25;
                    buffer[num3++] = (byte) IntToHex((b >> 4) & 15);
                    buffer[num3++] = (byte) IntToHex(b & 15);
                }
                else
                {
                    buffer[num3++] = b;
                }
            }
            return buffer;
        }

        internal static string UrlEncodeNonAscii(string str, Encoding e)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (e == null)
            {
                e = Encoding.UTF8;
            }
            byte[] bytes = e.GetBytes(str);
            bytes = UrlEncodeBytesToBytesInternalNonAscii(bytes, 0, bytes.Length, false);
            return ASCIIGetString(bytes);
        }

        internal static string UrlEncodeSpaces(string str)
        {
            if ((str != null) && (str.IndexOf(' ') >= 0))
            {
                str = str.Replace(" ", "%20");
            }
            return str;
        }

        internal static byte[] UrlEncodeToBytes(string str)
        {
            return str == null ? null : UrlEncodeToBytes(str, Encoding.UTF8);
        }

        internal static byte[] UrlEncodeToBytes(byte[] bytes)
        {
            return bytes == null ? null : UrlEncodeToBytes(bytes, 0, bytes.Length);
        }

        internal static byte[] UrlEncodeToBytes(string str, Encoding e)
        {
            if (str == null)
            {
                return null;
            }
            byte[] bytes = e.GetBytes(str);
            return UrlEncodeBytesToBytesInternal(bytes, 0, bytes.Length, false);
        }

        internal static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
        {
            if ((bytes == null) && (count == 0))
            {
                return null;
            }
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if ((offset < 0) || (offset > bytes.Length))
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if ((count < 0) || ((offset + count) > bytes.Length))
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return UrlEncodeBytesToBytesInternal(bytes, offset, count, true);
        }

        internal static string UrlEncodeUnicode(string str)
        {
            return str == null ? null : UrlEncodeUnicodeStringToStringInternal(str, false);
        }

        private static string UrlEncodeUnicodeStringToStringInternal(string s, bool ignoreAscii)
        {
            int length = s.Length;
            var builder = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                char ch = s[i];
                if ((ch & 0xff80) == 0)
                {
                    if (ignoreAscii || IsSafe(ch))
                    {
                        builder.Append(ch);
                    }
                    else if (ch == ' ')
                    {
                        builder.Append('+');
                    }
                    else
                    {
                        builder.Append('%');
                        builder.Append(IntToHex((ch >> 4) & '\x000f'));
                        builder.Append(IntToHex(ch & '\x000f'));
                    }
                }
                else
                {
                    builder.Append("%u");
                    builder.Append(IntToHex((ch >> 12) & '\x000f'));
                    builder.Append(IntToHex((ch >> 8) & '\x000f'));
                    builder.Append(IntToHex((ch >> 4) & '\x000f'));
                    builder.Append(IntToHex(ch & '\x000f'));
                }
            }
            return builder.ToString();
        }

        internal static byte[] UrlEncodeUnicodeToBytes(string str)
        {
            return str == null ? null : Encoding.ASCII.GetBytes(UrlEncodeUnicode(str));
        }

        internal static string UrlPathEncode(string str)
        {
            if (str == null)
            {
                return null;
            }
            int index = str.IndexOf('?');
            if (index >= 0)
            {
                return (UrlPathEncode(str.Substring(0, index)) + str.Substring(index));
            }
            return UrlEncodeSpaces(UrlEncodeNonAscii(str, Encoding.UTF8));
        }

        #region Nested type: HtmlEntities

        internal static class HtmlEntities
        {
            private static readonly string[] EntitiesList = new[]
                                                                {
                                                                    "\"-quot", "&-amp", "<-lt", ">-gt", "\x00a0-nbsp",
                                                                    "\x00a1-iexcl", "\x00a2-cent", "\x00a3-pound",
                                                                    "\x00a4-curren", "\x00a5-yen", "\x00a6-brvbar",
                                                                    "\x00a7-sect", "\x00a8-uml", "\x00a9-copy",
                                                                    "\x00aa-ordf", "\x00ab-laquo",
                                                                    "\x00ac-not", "\x00ad-shy", "\x00ae-reg",
                                                                    "\x00af-macr", "\x00b0-deg", "\x00b1-plusmn",
                                                                    "\x00b2-sup2", "\x00b3-sup3", "\x00b4-acute",
                                                                    "\x00b5-micro", "\x00b6-para", "\x00b7-middot",
                                                                    "\x00b8-cedil", "\x00b9-sup1", "\x00ba-ordm",
                                                                    "\x00bb-raquo",
                                                                    "\x00bc-frac14", "\x00bd-frac12", "\x00be-frac34",
                                                                    "\x00bf-iquest", "\x00c0-Agrave", "\x00c1-Aacute",
                                                                    "\x00c2-Acirc", "\x00c3-Atilde", "\x00c4-Auml",
                                                                    "\x00c5-Aring", "\x00c6-AElig", "\x00c7-Ccedil",
                                                                    "\x00c8-Egrave", "\x00c9-Eacute", "\x00ca-Ecirc",
                                                                    "\x00cb-Euml",
                                                                    "\x00cc-Igrave", "\x00cd-Iacute", "\x00ce-Icirc",
                                                                    "\x00cf-Iuml", "\x00d0-ETH", "\x00d1-Ntilde",
                                                                    "\x00d2-Ograve", "\x00d3-Oacute", "\x00d4-Ocirc",
                                                                    "\x00d5-Otilde", "\x00d6-Ouml", "\x00d7-times",
                                                                    "\x00d8-Oslash", "\x00d9-Ugrave", "\x00da-Uacute",
                                                                    "\x00db-Ucirc",
                                                                    "\x00dc-Uuml", "\x00dd-Yacute", "\x00de-THORN",
                                                                    "\x00df-szlig", "\x00e0-agrave", "\x00e1-aacute",
                                                                    "\x00e2-acirc", "\x00e3-atilde", "\x00e4-auml",
                                                                    "\x00e5-aring", "\x00e6-aelig", "\x00e7-ccedil",
                                                                    "\x00e8-egrave", "\x00e9-eacute", "\x00ea-ecirc",
                                                                    "\x00eb-euml",
                                                                    "\x00ec-igrave", "\x00ed-iacute", "\x00ee-icirc",
                                                                    "\x00ef-iuml", "\x00f0-eth", "\x00f1-ntilde",
                                                                    "\x00f2-ograve", "\x00f3-oacute", "\x00f4-ocirc",
                                                                    "\x00f5-otilde", "\x00f6-ouml", "\x00f7-divide",
                                                                    "\x00f8-oslash", "\x00f9-ugrave", "\x00fa-uacute",
                                                                    "\x00fb-ucirc",
                                                                    "\x00fc-uuml", "\x00fd-yacute", "\x00fe-thorn",
                                                                    "\x00ff-yuml", "Œ-OElig", "œ-oelig", "Š-Scaron",
                                                                    "š-scaron", "Ÿ-Yuml", "ƒ-fnof", "ˆ-circ", "˜-tilde",
                                                                    "Α-Alpha", "Β-Beta", "Γ-Gamma", "Δ-Delta",
                                                                    "Ε-Epsilon", "Ζ-Zeta", "Η-Eta", "Θ-Theta", "Ι-Iota",
                                                                    "Κ-Kappa", "Λ-Lambda", "Μ-Mu", "Ν-Nu", "Ξ-Xi",
                                                                    "Ο-Omicron", "Π-Pi", "Ρ-Rho", "Σ-Sigma", "Τ-Tau",
                                                                    "Υ-Upsilon",
                                                                    "Φ-Phi", "Χ-Chi", "Ψ-Psi", "Ω-Omega", "α-alpha",
                                                                    "β-beta", "γ-gamma", "δ-delta", "ε-epsilon",
                                                                    "ζ-zeta", "η-eta", "θ-theta", "ι-iota", "κ-kappa",
                                                                    "λ-lambda", "μ-mu",
                                                                    "ν-nu", "ξ-xi", "ο-omicron", "π-pi", "ρ-rho",
                                                                    "ς-sigmaf", "σ-sigma", "τ-tau", "υ-upsilon", "φ-phi"
                                                                    , "χ-chi", "ψ-psi", "ω-omega", "ϑ-thetasym",
                                                                    "ϒ-upsih", "ϖ-piv",
                                                                    " -ensp", " -emsp", " -thinsp", "‌-zwnj", "‍-zwj",
                                                                    "‎-lrm", "‏-rlm", "–-ndash", "—-mdash", "‘-lsquo",
                                                                    "’-rsquo", "‚-sbquo", "“-ldquo", "”-rdquo",
                                                                    "„-bdquo", "†-dagger",
                                                                    "‡-Dagger", "•-bull", "…-hellip", "‰-permil",
                                                                    "′-prime", "″-Prime", "‹-lsaquo", "›-rsaquo",
                                                                    "‾-oline", "⁄-frasl", "€-euro", "ℑ-image",
                                                                    "℘-weierp", "ℜ-real", "™-trade", "ℵ-alefsym",
                                                                    "←-larr", "↑-uarr", "→-rarr", "↓-darr", "↔-harr",
                                                                    "↵-crarr", "⇐-lArr", "⇑-uArr", "⇒-rArr", "⇓-dArr",
                                                                    "⇔-hArr", "∀-forall", "∂-part", "∃-exist", "∅-empty"
                                                                    , "∇-nabla",
                                                                    "∈-isin", "∉-notin", "∋-ni", "∏-prod", "∑-sum",
                                                                    "−-minus", "∗-lowast", "√-radic", "∝-prop",
                                                                    "∞-infin", "∠-ang", "∧-and", "∨-or", "∩-cap",
                                                                    "∪-cup", "∫-int",
                                                                    "∴-there4", "∼-sim", "≅-cong", "≈-asymp", "≠-ne",
                                                                    "≡-equiv", "≤-le", "≥-ge", "⊂-sub", "⊃-sup",
                                                                    "⊄-nsub", "⊆-sube", "⊇-supe", "⊕-oplus", "⊗-otimes",
                                                                    "⊥-perp",
                                                                    "⋅-sdot", "⌈-lceil", "⌉-rceil", "⌊-lfloor",
                                                                    "⌋-rfloor", "〈-lang", "〉-rang", "◊-loz", "♠-spades",
                                                                    "♣-clubs", "♥-hearts", "♦-diams"
                                                                };

            private static Hashtable _entitiesLookupTable;
            private static readonly object LookupLockObject = new object();

            internal static char Lookup(string entity)
            {
                if (_entitiesLookupTable == null)
                {
                    lock (LookupLockObject)
                    {
                        if (_entitiesLookupTable == null)
                        {
                            var hashtable = new Hashtable();
                            foreach (string str in EntitiesList)
                            {
                                hashtable[str.Substring(2)] = str[0];
                            }
                            _entitiesLookupTable = hashtable;
                        }
                    }
                }
                object obj2 = _entitiesLookupTable[entity];
                if (obj2 != null)
                {
                    return (char) obj2;
                }
                return '\0';
            }
        }

        #endregion

        #region Nested type: UrlDecoder

        private class UrlDecoder
        {
            private readonly int _bufferSize;
            private readonly char[] _charBuffer;
            private readonly Encoding _encoding;

            private byte[] _byteBuffer;
            private int _numBytes;
            private int _numChars;

            internal UrlDecoder(int bufferSize, Encoding encoding)
            {
                _bufferSize = bufferSize;
                _encoding = encoding;
                _charBuffer = new char[bufferSize];
            }

            internal void AddByte(byte b)
            {
                if (_byteBuffer == null)
                {
                    _byteBuffer = new byte[_bufferSize];
                }
                _byteBuffer[_numBytes++] = b;
            }

            internal void AddChar(char ch)
            {
                if (_numBytes > 0)
                {
                    FlushBytes();
                }
                _charBuffer[_numChars++] = ch;
            }

            private void FlushBytes()
            {
                if (_numBytes <= 0) return;

                _numChars += _encoding.GetChars(_byteBuffer, 0, _numBytes, _charBuffer, _numChars);
                _numBytes = 0;
            }

            internal string GetString()
            {
                if (_numBytes > 0)
                {
                    FlushBytes();
                }

                return _numChars > 0 ? new string(_charBuffer, 0, _numChars) : string.Empty;
            }
        }

        #endregion
    }
}