#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Globalization;

#endregion

namespace LmUtils
{
    public static class ConvertUtilities
    {
        public const string LmDateTimeFormat = "yyyy.MM.dd HH:mm";
        public const string LmDateTimeLongFormat = "yyyy.MM.dd HH:mm:ss";
        public const string LmDateFormat = "yyyy.MM.dd";
        public const string LmTimeFormat = "HH:mm";
        public const string LmLongTimeFormat = "HH:mm:ss";
        public const string MsSqlDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public const string MsSqlDateFormat = "yyyy-MM-dd";
        public const string MsSqlTimeFormat = "HH:mm:ss";
        public const string FileDateTimeFormat = "yyyy-MM-dd HH-mm-ss";
        public const string FileDateFormat = "yyyy-MM-dd";
        public const string FileTimeFormat = "HH-mm-ss";

        #region Date, time
        public static DateTime MsSqlDatetimeToDateTime(string val)
        {
            return DateTime.ParseExact(val, MsSqlDateTimeFormat, null);
        }

        public static DateTime LmDateToDateTime(string val)
        {
            return DateTime.ParseExact(val, LmDateFormat, null);
        }

        public static DateTime LmDateTimeToDateTime(string val)
        {
            return DateTime.ParseExact(val, LmDateTimeFormat, null);
        }

        public static DateTime LmDateTimeLongToDateTime(string val)
        {
            return DateTime.ParseExact(val, LmDateTimeLongFormat, null);
        }

        public static string TimeSpanToLmTime(TimeSpan t)
        {
            return String.Format("{0:00}:{0:00}", t.Hours, t.Minutes);
        }

        public static string TimeSpanToLmLongTime(TimeSpan t)
        {
            return String.Format("{0:00}:{0:00}:{0:00}", t.Hours, t.Minutes, t.Seconds);
        }
        public static string TimeSpanToLmLongLongTime(TimeSpan t)
        {
            return String.Format("{0:00}:{0:00}:{0:00}:{0:000}", t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
        }
        #endregion

        #region IP
        public static uint IPStringToUint32(string ip_str)
        {
            string[] ip = ip_str.Split('.');
            uint ip_num = 0;
            for (int k = 0; k < 4; k++)
            {
                ip_num += (uint)(System.Convert.ToByte(ip[k].Replace(" ", "")) << ((3 - k) * 8));
            }

            return ip_num;
        }

        public static uint IPStringToUint32Reverse(string ip_str)
        {
            string[] ip = ip_str.Split('.');
            uint ip_num = 0;
            for (int k = 0; k < 4; k++)
            {
                ip_num += (uint)(System.Convert.ToByte(ip[k].Replace(" ", "")) << ((k) * 8));
            }

            return ip_num;
        }
        #endregion

        #region Byte array / int, uint
        public static uint ByteArrayToUInt32(byte[] num)
        {
            uint val = (uint)((uint)num[3] << 24) + ((uint)num[2] << 16) + ((uint)num[1] << 8) + (uint)num[0];
            return val;
        }
        public static byte[] UInt32ToByteArray(uint num)
        {
            byte[] val = new byte[4];
            val[0] = (byte)(num);
            val[1] = (byte)(num >> 8);
            val[2] = (byte)(num >> 16);
            val[3] = (byte)(num >> 24);

            return val;
        }
        public static uint ByteArrayToUInt32Reverse(byte[] num)
        {
            uint val = (uint)((uint)num[0] << 24) + ((uint)num[1] << 16) + ((uint)num[2] << 8) + (uint)num[3];
            return val;
        }
        public static byte[] UInt32ToByteArrayReverse(uint num)
        {
            byte[] val = new byte[4];
            val[3] = (byte)(num);
            val[2] = (byte)(num >> 8);
            val[1] = (byte)(num >> 16);
            val[0] = (byte)(num >> 24);

            return val;
        }

        public static int ByteArrayToInt32(byte[] num)
        {
            int val = (int)((int)num[3] << 24) + ((int)num[2] << 16) + ((int)num[1] << 8) + (int)num[0];
            return val;
        }
        public static byte[] Int32ToByteArray(int num)
        {
            byte[] val = new byte[4];
            val[0] = (byte)(num);
            val[1] = (byte)(num >> 8);
            val[2] = (byte)(num >> 16);
            val[3] = (byte)(num >> 24);

            return val;
        }
        public static int ByteArrayToInt32Reverse(byte[] num)
        {
            int val = (int)((int)num[0] << 24) + ((int)num[1] << 16) + ((int)num[2] << 8) + (int)num[3];
            return val;
        }
        public static byte[] Int32ToByteArrayReverse(int num)
        {
            byte[] val = new byte[4];
            val[3] = (byte)(num);
            val[2] = (byte)(num >> 8);
            val[1] = (byte)(num >> 16);
            val[0] = (byte)(num >> 24);

            return val;
        }
        #endregion

        #region Color
        public static string ColorToHtmlColor(Color color)
        {
            return string.Format("{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
        }
        public static Color HtmlColorToColor(string color_hex)
        {
            if (color_hex.Length > 0 && color_hex[0] == '#')
                color_hex = color_hex.Substring(1);

            color_hex = "FF" + color_hex;
            return Color.FromArgb(System.Convert.ToInt32(color_hex, 16));
        }

        public static string ColorToArgbString(Color color)
        {
            return String.Format("{0}, {1}, {2}, {3}", color.A, color.R, color.G, color.B);
        }
        public static Color ArgbStringToColor(string color_argb)
        {
            color_argb = color_argb.Replace(" ", "");
            string[] color_parts = color_argb.Split(',');

            return Color.FromArgb(Convert.ToInt32(color_parts[0]), Convert.ToInt32(color_parts[1]), Convert.ToInt32(color_parts[2]), Convert.ToInt32(color_parts[3]));
        }
        #endregion

        #region Hashtable
        public static Hashtable HashtableTranspose(Hashtable t)
        {
            Hashtable res = new Hashtable();
            foreach (DictionaryEntry de in t)
            {
                res.Add(de.Value, de.Key);
            }

            return res;
        }
        #endregion

        #region Double

        public static double StringToDouble(string str)
        {
            if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",")
                str = str.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            else
                str = str.Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            return Convert.ToDouble(str);
        }

        #endregion
    }
}
