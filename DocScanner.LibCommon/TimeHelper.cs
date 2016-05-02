using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocScanner.LibCommon
{
    public static class TimeHelper
    {
        // Methods
        public static DateTime FromDateTime(int date, int tm)
        {
            return new DateTime(date / 0x2710, (date / 100) % 100, date % 100, tm / 0x2710, (tm / 100) % 100, tm % 100);
        }

        public static string GetCurrentTimeStamp()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        public static void SetCurrentTime(out int Date, out int Time)
        {
            DateTime now = DateTime.Now;
            Date = ((now.Year * 0x2710) + (now.Month * 100)) + now.Day;
            Time = ((now.Hour * 0x2710) + (now.Minute * 100)) + now.Second;
        }

        public static int ToHMS(this DateTime tm)
        {
            return (((tm.Hour * 0x2710) + (tm.Minute * 100)) + tm.Second);
        }

        public static string ToViewDate(int date)
        {
            int num = date / 0x2710;
            int num2 = (date / 100) % 100;
            int num3 = date % 100;
            string[] textArray1 = new string[] { num.ToString(), "-", num2.ToString(), "-", num3.ToString() };
            return string.Concat(textArray1);
        }

        public static string ToViewTime(this DateTime tm)
        {
            return tm.ToShortDateString();
        }

        public static string ToViewTime(int time)
        {
            int num = time / 0x2710;
            int num2 = (time / 100) % 100;
            int num3 = time % 100;
            string[] textArray1 = new string[] { num.ToString(), ":", num2.ToString(), ":", num3.ToString() };
            return string.Concat(textArray1);
        }

        public static string ToViewTime(this string time)
        {
            string[] textArray1 = new string[] { time.Substring(0, 4), "-", time.Substring(4, 2), "-", time.Substring(6, 2), " ", time.Substring(8, 2), ":", time.Substring(10, 2), ":", time.Substring(12, 2) };
            return string.Concat(textArray1);
        }

        public static string ToViewTime(int date, int tm)
        {
            return FromDateTime(date, tm).ToViewTime();
        }

        public static int ToYMD(this DateTime tm)
        {
            return (((tm.Year * 0x2710) + (tm.Month * 100)) + tm.Day);
        }
    }

}