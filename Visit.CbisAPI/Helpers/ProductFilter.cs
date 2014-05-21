using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Visit.CbisAPI.Helpers;

namespace Visit.CbisAPI.Products
{
	public static class ProductFilterHelper
	{
		public static string AsString(this ProductFilter filter)
		{
			List<object> param = new List<object>();
			{
				param.Add(filter.ArenaIds == null ? "" : filter.ArenaIds.Implode(","));
				param.Add(filter.GeoNodeIds == null ? "" : filter.GeoNodeIds.Implode(","));
				param.Add(filter.StartDate == null ? "null" : filter.StartDate.Value.Ticks.ToString());
				param.Add(filter.EndDate == null ? "null" : filter.EndDate.Value.Ticks.ToString());
				param.Add(filter.FreeText);
				param.Add(filter.Highlights.ToString());
				param.Add(((int)filter.OrderBy).ToString());
				param.Add(((int)filter.SortOrder).ToString());
				param.Add(filter.MaxLatitude == null ? "null" : filter.MaxLatitude.Value.ToString());
				param.Add(filter.MinLatitude == null ? "null" : filter.MinLatitude.Value.ToString());
				param.Add(filter.MaxLongitude == null ? "null" : filter.MaxLongitude.Value.ToString());
				param.Add(filter.MinLongitude == null ? "null" : filter.MinLongitude.Value.ToString());
				param.Add(filter.InterestsIds == null ? "" : filter.InterestsIds.Implode(","));
				param.Add(filter.SubCategoryId);
				param.Add(filter.ProductType == null ? "null" : filter.ProductType.ToString());
				param.Add(filter.WithOccasionsOnly);
				if(filter.MultiAttributes == null)
					param.Add("()");
				else
					param.Add("(" + filter.MultiAttributes.Select(m => "(" + m.AttributeId + ":" + m.MultiAttributeIds == null ? "" : m.MultiAttributeIds.Implode(",") + ")").Implode(",") + ")");
				//param.Add("(" + filter.MultiAttributes == null ? "()" : (filter.MultiAttributes.Select(m => "(" + m.AttributeId + ":" + m.MultiAttributeIds == null ? "" : m.MultiAttributeIds.Implode(",") + ")").Implode(",")) + ")");
				param.Add(filter.ExcludeProductsWithoutOccasions);
				param.Add(filter.ExcludeProductsNotInCurrentLanguage);
				param.Add(filter.IncludeArchivedProducts);
				param.Add(filter.IncludeInactiveProducts);
			};
			return string.Format("{0}%{1}%{2}%{3}%{4}%{5}%{6}%{7}%{8}%{9}%{10}%{11}%{12}%{13}%{14}%{15}%{16}%{17}%{18}%{19}%{20}", param.ToArray());
		}
	}
}
