using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Visit.CbisAPI.Categories;
using Visit.CbisAPI.Products;
using Visit.CbisAPI.Helpers;

namespace Visit.CbisAPI
{
	public class CategoryService : ICategoryService
	{
		private string _apiKey;
		private CategoriesSoapClient _client;
		private const int MaxItemsToFetch = 20000;
		private const int PageSize = 1000;

		public CategoryService(string apiKey, CategoriesSoapClient client)
		{
			_apiKey = apiKey;
			_client = client;
		}

		public TreeOfCategory GetCategoryTree(int language)
		{
			return _client.ListAll(_apiKey, language, 0);
		}

		public List<TreeNodeOfCategory> GetCategories(int parentCategory, int language)
		{
			TreeOfCategory tree = _client.ListAll(_apiKey, language, parentCategory);

			return tree.ToNodeList();
		}
	}
}
