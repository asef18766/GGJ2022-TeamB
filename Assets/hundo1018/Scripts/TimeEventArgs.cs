using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.hundo1018.Scripts
{
    public class TimeEventArgs
    {
        public float RemainingTime { get; set; }
        public float RemainingRate { get; set; }
        public bool IsNight { get; set; }
        public TimeEventArgs(float remainingTime,float remainingRate)
        {
            RemainingTime = remainingTime;
            RemainingRate = remainingRate;
        }
        public TimeEventArgs(bool isNight)
        {
            IsNight = isNight;
        }
    }
}
