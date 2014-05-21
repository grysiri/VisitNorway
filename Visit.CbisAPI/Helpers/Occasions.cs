using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Visit.CbisAPI.Products
{
	public static class Occasions
	{
		/// <summary>
		/// Determines whether a weekdays enumeration contains a certain day.
		/// </summary>
		/// <param name="weekDays">The week days.</param>
		/// <param name="day">The day.</param>
		/// <returns>
		/// 	<c>true</c> or <c>false</c> depending on wether the WeeKDays occurrs on the specified day.
		/// </returns>
		public static bool IsOnDay(this WeekDays weekDays, DayOfWeek day)
		{
			// Mumbo-jumbo, converting by bitshifting. When I say jump left, you ask how many bits!
			int weekDay = 1 << ((7 + (int)day - 1) % 7);
			return ((int)weekDays & weekDay) > 0;
		}

		/// <summary>
		/// Determines whether a weekdays enumeration contains a certain day.
		/// </summary>
		/// <param name="weekDays">The week days.</param>
		/// <param name="day">The day.</param>
		/// <returns>
		/// 	<c>true</c> or <c>false</c> depending on wether the WeeKDays occurrs on the specified day.
		/// </returns>
		public static IList<DayOfWeek> DaysOfWeek(this WeekDays weekDays)
		{
			IList<DayOfWeek> days = new List<DayOfWeek>();

			for (int i = 0; i < 7; ++i)
			{
				// Transform to 1,2,4,8... instead of 0,1,2,3...
				int w = (1 << i);

				// Check if the current day exists in the weekDays enum
				if (((int)weekDays & w) == w)
				{
					// Convert to a DayOfWeek (0 = Sunday)
					days.Add((DayOfWeek)((i + 1) % 7));
				}
			}

			return days;
		}

		/// <summary>
		/// Determines whether a occasion object occurrs on a certain day.
		/// </summary>
		/// <param name="occasion">The occasion.</param>
		/// <param name="day">The day.</param>
		/// <returns>
		/// 	<c>true</c> or <c>false</c> depending on wether the occasion occurrs on the specified day.
		/// </returns>
		public static bool IsOnDay(this OccasionObject occasion, DayOfWeek day)
		{
			return occasion.ValidDays.IsOnDay(day);
		}

		/// <summary>
		/// Determines whether a occasion list occurrs on a certain day.
		/// </summary>
		/// <param name="occasions">The occasion.</param>
		/// <param name="day">The day.</param>
		/// <returns>
		/// 	<c>true</c> or <c>false</c> depending on wether any of the occasions occurrs on the specified day.
		/// </returns>
		public static bool IsOnDay(this IEnumerable<OccasionObject> occasions, DayOfWeek day)
		{
			return occasions.Any(o => o.ValidDays.IsOnDay(day));
		}

		/// <summary>
		/// Determines wether an occasion period has any occasions in a specified period
		/// </summary>
		/// <param name="occasion">The occasion.</param>
		/// <param name="start">The start date.</param>
		/// <param name="end">The end date.</param>
		/// <returns></returns>
		public static bool OccurrsInPeriod(this OccasionObject occasion, DateTime start, DateTime end)
		{
			if ((occasion.StartDate != null && occasion.StartDate > DateTime.MinValue && occasion.StartDate > end)
				|| (occasion.EndDate != null && occasion.EndDate > DateTime.MinValue && occasion.EndDate < start))
				return false;

			DateTime ocStart = new DateTime((occasion.StartDate == null ? 0 : occasion.StartDate.Value.Ticks) + (occasion.StartTime == null ? 0 : occasion.StartTime.Value.TimeOfDay.Ticks));
			DateTime ocEnd = new DateTime((occasion.EndDate == null ? 0 : occasion.EndDate.Value.Ticks) + (occasion.EndTime == null ? 0 : occasion.EndTime.Value.TimeOfDay.Ticks));
			ocStart = ocStart < start ? start : ocStart;
			ocEnd = ocEnd > end ? end : ocEnd;

			if (ocStart > ocEnd)
				return false;

			IList<int> offsets = occasion.ValidDays.CyclicOffsets(ocStart.DayOfWeek);
			if (offsets.Count == 0)
				return false;

			return (ocEnd.Date - ocStart.Date).Days > offsets.Min();
		}

		/// <summary>
		/// Determines wether an occasion list has any occasions occurring in a specified period
		/// </summary>
		/// <param name="occasions">The occasion list.</param>
		/// <param name="start">The start date.</param>
		/// <param name="end">The end date.</param>
		/// <returns></returns>
		public static bool OccurrsInPeriod(this IEnumerable<OccasionObject> occasions, DateTime start, DateTime end)
		{
			return occasions.Any(o => o.OccurrsInPeriod(start, end));
		}

		/// <summary>
		/// Gets all dates/times for an occasion. If the occasion does not have a start and end time no dates are returned.
		/// </summary>
		/// <param name="occasion">The occasion.</param>
		/// <returns></returns>
		public static IList<DateTime> GetOccuringDateTimes(this OccasionObject occasion)
		{
			if (occasion.StartDate == null || occasion.StartDate == DateTime.MinValue || occasion.EndDate == null || occasion.EndDate == DateTime.MinValue)
				return new List<DateTime>();

			return occasion.GetOccuringDateTimes(new DateTime(occasion.StartDate.Value.Date.Ticks + (occasion.StartTime == null ? 0 : occasion.StartTime.Value.TimeOfDay.Ticks))
												, new DateTime(occasion.EndDate.Value.Date.Ticks + (occasion.EndTime == null ? 0 : occasion.EndTime.Value.TimeOfDay.Ticks)));
		}

		/// <summary>
		/// Gets all dates/times for an occasion. If the occasion does not have a start and end time no dates are returned.
		/// </summary>
		/// <param name="occasions">The occasion.</param>
		/// <returns></returns>
		public static IList<DateTime> GetOccuringDateTimes(this IEnumerable<OccasionObject> occasions)
		{
			IList<DateTime> result = new List<DateTime>();
			foreach (OccasionObject obj in occasions)
			{
				result.Concat(obj.GetOccuringDateTimes());
			}

			return result.OrderBy(d => d).ToList();
		}

		/// <summary>
		/// Gets all dates/times for an occasion between specified dates
		/// </summary>
		/// <param name="occasion">The occasion.</param>
		/// <param name="start">The start date.</param>
		/// <param name="end">The end date.</param>
		/// <returns></returns>
		public static IList<DateTime> GetOccuringDateTimes(this OccasionObject occasion, DateTime start, DateTime end)
		{
			IList<DateTime> dates = new List<DateTime>();

			// If the occasion has a start date and and it does not occurr in the period, return empty
			if ((occasion.StartDate != null && occasion.StartDate > DateTime.MinValue && occasion.StartDate > end.Date)
				|| (occasion.EndDate != null && occasion.EndDate > DateTime.MinValue && occasion.EndDate < start.Date))
				return dates;

			DateTime ocStart = new DateTime((occasion.StartDate == null ? 0 : occasion.StartDate.Value.Ticks));
			DateTime ocEnd = new DateTime((occasion.EndDate == null ? 0 : occasion.EndDate.Value.Ticks));

			ocStart = ocStart < start ? start : ocStart;
			ocEnd = ocEnd > end ? end : ocEnd;

			ocStart = ocStart.Date.AddTicks(occasion.StartTime == null ? 0 : occasion.StartTime.Value.TimeOfDay.Ticks);
			ocEnd = ocEnd.Date.AddTicks(occasion.EndTime == null ? 0 : occasion.EndTime.Value.TimeOfDay.Ticks);

			IList<int> offsets = occasion.ValidDays.CyclicOffsets(ocStart.DayOfWeek);

			DateTime current = ocStart;
			while (current.Date <= ocEnd.Date)
			{
				foreach (int offset in offsets)
				{
					DateTime t = current.AddDays(offset);
					if (t.Date <= ocEnd.Date)
						dates.Add(t);
				}

				current = current.AddDays(7);
			}

			return dates.OrderBy(d => d).ToList();
		}

		/// <summary>
		/// Gets all dates/times for an occasion between specified dates
		/// </summary>
		/// <param name="occasions">The occasion.</param>
		/// <param name="start">The start date.</param>
		/// <param name="end">The end date.</param>
		/// <returns></returns>
		public static IList<DateTime> GetOccuringDateTimes(this IEnumerable<OccasionObject> occasions, DateTime start, DateTime end)
		{
			IList<DateTime> result = new List<DateTime>();
			foreach (OccasionObject obj in occasions)
			{
				result.Concat(obj.GetOccuringDateTimes(start, end));
			}

			return result.OrderBy(d => d).ToList();
		}


		/// <summary>
		/// Computes cyclic offsets for calculating recurring dates, zero based from specified date
		/// </summary>
		/// <param name="days">WeekDays enumeration of days</param>
		/// <param name="day">Start day</param>
		/// <returns></returns>
		private static IList<int> CyclicOffsets(this WeekDays days, DayOfWeek day)
		{
			int dayOffset = 7 - (7 + (int)day - 1) % 7;

			IList<int> weekdays = new List<int>();
			if ((days & WeekDays.Monday) > 0)
				weekdays.Add(dayOffset % 7);
			if ((days & WeekDays.Tuesday) > 0)
				weekdays.Add((dayOffset + 1) % 7);
			if ((days & WeekDays.Wednesday) > 0)
				weekdays.Add((dayOffset + 2) % 7);
			if ((days & WeekDays.Thursday) > 0)
				weekdays.Add((dayOffset + 3) % 7);
			if ((days & WeekDays.Friday) > 0)
				weekdays.Add((dayOffset + 4) % 7);
			if ((days & WeekDays.Saturday) > 0)
				weekdays.Add((dayOffset + 5) % 7);
			if ((days & WeekDays.Sunday) > 0)
				weekdays.Add((dayOffset + 6) % 7);

			return weekdays;
		}

		public static DateTime? GetNextOccurrance(this IEnumerable<OccasionObject> occasions)
		{
			if (occasions.Count() == 0)
				return null;
			OccasionObject oc = null;
			var nextDate = from o in occasions
						   where o.StartDate > DateTime.Now
						   orderby o.StartDate ascending
						   select o;
			oc = nextDate.FirstOrDefault<OccasionObject>();

			if (oc == null)   
			{
				var lastDate = from o in occasions
							   orderby o.StartDate descending
							   select o;
				return lastDate.FirstOrDefault<OccasionObject>().StartDate.Value;
			}
			else
				return oc.StartDate.Value;
		}
	}
}
