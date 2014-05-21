using System.Collections.Generic;
using Visit.CbisAPI.Products;

namespace Visit.CbisAPI
{
	public interface IProductService
	{
		Product GetOne(int id, int languageId);
		IList<Product> GetAll(int languageId);
		IList<Product> GetWithFilter(int languageId, int? categoryId, int? templateId, ProductFilter productFilter,
		                             bool imagesFirst);
		IList<ProductMapItem> GetSimpleWithFilter(int languageId, int? categoryId, ProductFilter productFilter);
		IList<ProductMapItem> GetAllSimple(int languageId);

	}
}