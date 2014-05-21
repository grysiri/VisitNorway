using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Visit.CbisAPI.Products;
using Visit.CbisAPI.Models;

namespace Visit.CbisAPI
{
	public class ProductService : IProductService
	{
		private string _apiKey;
		private ProductsSoapClient _client;
		private const int MaxItemsToFetch = 20000;
		private const int PageSize = 1000;

		public ProductService(string apiKey, ProductsSoapClient client)
		{
			_apiKey = apiKey;
			_client = client;
		}

		public Product GetOne(int id, int languageId)
		{
			var product = _client.GetById(_apiKey, languageId, id, 0);
			return product;
		}

		public IList<Product> GetAll(int languageId)
		{
			return GetWithFilter(languageId, null, null, null, false);
		}

		public IList<Product> GetWithFilter(int languageId, int? categoryId, int? templateId, ProductFilter productFilter, bool imagesFirst)
		{
			var products = new List<Product>();
			int fetched = 0;
			int page = 0;
			while (fetched < MaxItemsToFetch)
			{
				SearchResultOfProduct result = null;
				if (imagesFirst)
					result = _client.ListAllWithImagesFirst(_apiKey, languageId, categoryId ?? 0, templateId ?? 0, page, PageSize, productFilter ?? new ProductFilter());
				else
					result = _client.ListAll(_apiKey, languageId, categoryId ?? 0, templateId ?? 0, page, PageSize, productFilter ?? new ProductFilter());
				if (result.Items.Length > 0)
				{
					products.AddRange(result.Items);
					++page;
				}
				else
					break;
			}

			return products;
		}

		public IList<ProductMapItem> GetSimpleWithFilter(int languageId, int? categoryId, ProductFilter productFilter)
		{
			var ids = _client.ListIds(_apiKey, languageId, categoryId ?? 0, productFilter ?? new ProductFilter());

			return ids;
		}

		public IDictionary<int, Arena> GetArenas(int languageId)
		{
			IDictionary<int, Arena> result = new Dictionary<int, Arena>();
			bool done = false;
			int page = 0;

			while (!done)
			{
				var partial = _client.ListAll(_apiKey, languageId, 0, 0, page++, 1000, new ProductFilter() { ProductType = ProductType.Arena });
				if (partial.Items.Length == 0)
					return result;

				partial.Items.Select(a =>
				{
					AttributeData lat = a.Attributes.Where(s => s.AttributeId == (int)Visit.CbisAPI.Attributes.Latitude).FirstOrDefault();
					AttributeData lng = a.Attributes.Where(s => s.AttributeId == (int)Visit.CbisAPI.Attributes.Longitude).FirstOrDefault();
					return new Arena()
					{
						Id = a.Id,
						Name = a.Name,
						Latitude = lat == null ? null : (double?)((Value)lat.Value).Data,
						Longitude = lng == null ? null : (double?)((Value)lng.Value).Data
					};
				}).ToList().ForEach(a => result.Add(a.Id, a));
			}

			return result;
		}

		public Arena GetArena(int arenaId, int languageId)
		{
			var arenaProduct = this.GetOne(arenaId, languageId);
			Arena arena = null;
			if (arenaProduct != null)
			{
				AttributeData lat =
					arenaProduct.Attributes.Where(s => s.AttributeId == (int) Visit.CbisAPI.Attributes.Latitude).FirstOrDefault();
				AttributeData lng =
					arenaProduct.Attributes.Where(s => s.AttributeId == (int) Visit.CbisAPI.Attributes.Longitude).FirstOrDefault();
				arena = new Arena
				        	{
				        		Id = arenaProduct.Id,
				        		Name = arenaProduct.Name,
				        		Latitude = lat == null ? null : (double?) ((Value) lat.Value).Data,
				        		Longitude = lng == null ? null : (double?) ((Value) lng.Value).Data
				        	};
			}

			return arena;
		}

		public TreeOfGeoNode GetGeoNodes(int languageId)
		{
			return _client.GetGeoTree(_apiKey, languageId);
		}

		public IList<ProductMapItem> GetAllSimple(int languageId)
		{
			return GetSimpleWithFilter(languageId, null, null);
		}

		public IList<ProductUpdate> GetUpdateInformation()
		{
			var updateInfo = _client.GetUpdateInformation(_apiKey);

			return updateInfo;

		}
	}
}
