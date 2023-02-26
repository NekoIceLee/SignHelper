using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignHelper
{
    public class SignLogic
    {
        
        public enum SignStat
        {
            NotSigned,
            Later,
            Signed,
            SignedLate,
        }
        public class TodaySign
        {
            public required SignStat MoringUp { get; init; }
            public required SignStat MoringOff { get; init; }
            public required SignStat NoonUp { get; init; }
            public required SignStat NoonOff { get; init; }
            public required SignStat NightUp { get; init; }
            public required SignStat NightOff { get; init; }
            public override string ToString()
            {
                return $"{{\"MorningUp\":\"{MoringUp}\"," +
                    $"\"MoringOff\":\"{MoringOff}\"," +
                    $"\"NoonUp\":\"{NoonUp}\"," +
                    $"\"NoonOff\":\"{NoonOff}\"," +
                    $"\"NightUp\":\"{NightUp}\"," +
                    $"\"NightOff\":\"{NightOff}\",}}";
            }
        }
        static SignStat caculateStat(DateTime time, TimeSpan start, TimeSpan late, TimeSpan end)
        {
            if (time == default)
            {
                var nowtime = DateTime.Now.TimeOfDay;
                if (nowtime < late)
                {
                    return SignStat.Later;
                }
                return SignStat.NotSigned;
            }
            var td = time.TimeOfDay;
            if (td < late)
            {
                return SignStat.Signed;
            }
            return SignStat.SignedLate;
        }
        static (DateTime first, DateTime second) SplitDatetime(IEnumerable<DateTime> times) 
        {
            if (times.Count() < 2)
            {
                return (default, default);
            }
            var timelist = times.OrderBy(t => t.Ticks).ToList();
            var first = times.First();
            var last = times.Last();
            foreach(var t in timelist)
            {
                if (t - first < new TimeSpan(0, 10, 0))
                {
                    first = t;
                    continue;
                }
                last = t;
                break;
            }
            return (first, last);
        }
        public static TodaySign GetTodaySign(IEnumerable<DateTime> times)
        {
            var timecache = times.ToArray();
            var (morningoff, noonup) = SplitDatetime(timecache.Where(t => t.Hour == 12 || t.Hour == 13));
            var (noonoff, nightup) = SplitDatetime(timecache.Where(t => t.Hour == 18));
            return new TodaySign
            {
                MoringUp = caculateStat(timecache.FirstOrDefault(t => t.Hour <= 8), DateTime.Parse("07:00").TimeOfDay, DateTime.Parse("08:30").TimeOfDay, DateTime.Parse("09:00").TimeOfDay),
                MoringOff = caculateStat(morningoff, DateTime.Parse("12:00").TimeOfDay, DateTime.Parse("13:25").TimeOfDay, DateTime.Parse("13:30").TimeOfDay),
                NoonUp = caculateStat(noonup, DateTime.Parse("13:00").TimeOfDay, DateTime.Parse("13:30").TimeOfDay, DateTime.Parse("14:00").TimeOfDay),
                NoonOff = caculateStat(noonoff, DateTime.Parse("17:59").TimeOfDay, DateTime.Parse("18:25").TimeOfDay, DateTime.Parse("18:30").TimeOfDay),
                NightUp = caculateStat(nightup, DateTime.Parse("18:00").TimeOfDay, DateTime.Parse("18:30").TimeOfDay, DateTime.Parse("19:00").TimeOfDay),
                NightOff = caculateStat(timecache.FirstOrDefault(t => t.Hour >= 21), DateTime.Parse("20:59").TimeOfDay, DateTime.Parse("21:00").TimeOfDay, DateTime.Parse("23:59").TimeOfDay),
            };
        }
        public static string GetSignTypeStr(DateTime times)
        {
            return times.Hour switch
            {
                var h when h <= 8 => "MorningUp",
                12 => "MorningOff",
                13 => "NoonUp",
                var h when h == 18 && times.Minute < 15 => "NoonOff",
                var h when h == 18 && times.Minute >= 15 => "NoonOff",
                var h when h >= 21 => "NightOff",
                _ => "",
            };
        }
    }
}
