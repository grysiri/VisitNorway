using System.Collections.Generic;

namespace DomainModels.Domain.Enums
{
    public enum Weekdays
    {
        Monday=0,
        Tuesday=1,
        Wednesday=2,
        Thursday=3,
        Friday=4,
        Saturday=5,
        Sunday=6
    }

    public class WeekdayComparer : IComparer<Weekdays>
    {
        public int Compare(Weekdays x, Weekdays y)
        {
            if ((int) x < (int) y)
                return -1;
            if ((int) x > (int) y)
                return 1;
            return 0;
        }
    }
}