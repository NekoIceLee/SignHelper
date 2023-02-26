using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignHelper
{
    public class SignLogic
    {
        public class TodaySign
        {
            public required bool MoringUp { get; init; }
            public required bool MoringOff { get; init; }
            public required bool NoonUp { get; init; }
            public required bool NoonOff { get; init; }
            public required bool NightUp { get; init; }
            public required bool NightOff { get; init; }
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
        public static TodaySign GetTodaySign(IEnumerable<DateTime> times)
        {
            return new TodaySign
            {
                MoringUp = times.Any(t => t.Hour <= 8),
                MoringOff = times.Any(t => t.Hour == 12),
                NoonUp = times.Any(t => t.Hour == 13),
                NoonOff = times.Any(t => t.Hour == 18 && t.Minute < 15),
                NightUp = times.Any(t => t.Hour == 18 && t.Minute >= 15),
                NightOff = times.Any(t => t.Hour >= 21),
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
