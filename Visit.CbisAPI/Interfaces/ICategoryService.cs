using System.Collections.Generic;
using Visit.CbisAPI.Categories;

namespace Visit.CbisAPI
{
	public interface ICategoryService
	{
		List<TreeNodeOfCategory> GetCategories(int parentCategory, int language);
	}
}