using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DomainModels.Domain;
using Gatherer.CbisServiceReference;
using Visit.CbisAPI.Categories;
using ProductFilter = Visit.CbisAPI.Products.ProductFilter;
using ProductsSoapClient = Visit.CbisAPI.Products.ProductsSoapClient;
using CategorySoapClient = Visit.CbisAPI.Categories.CategoriesSoapClient;
using Media = DomainModels.Domain.Media;
using Product = DomainModels.Domain.Product;


namespace Gatherer
{
    public class CbisCollector
    {
        private Collection<LocalOrg> _localOrgList;
        private Collection<Owner> _ownerList;
        private IQueryable<Product> _productList;
        private IQueryable<ProductInfo> _productInfoList;
        private List<ProductInfo> _productInfoListAllLang;
        private IQueryable<Media> _mediaList;
        private List<Media> _mediaListAllLang;
        private Visit.CbisAPI.Products.SearchResultOfProduct _products;
        private CbisServiceReference.Attribute[] _attributes;
        private string _apiKey = "";
        private Organization[] _localOrgs;
        public List<int> InactiveProducts;
        public List<int> _inactiveList;

        public IQueryable<Product> CbisCollect()
        {
            _productInfoListAllLang = new List<ProductInfo>();
            _mediaListAllLang = new List<Media>();
            InactiveProducts = new List<int>();
            _inactiveList = new List<int>();
            

            // ----------------------- COLLECTING FROM CBIS ---------------------------------

            var cc = new CbisConverter();
            var langList = new int[] { 1,2,3,4,5,6,8,9,11,12,13,19,21 };
            
            //--------Using the external service reference Gatherer.CbisServiceReference------------ 
            using (var external = new CbisServiceReference.ProductsSoapClient())
            {
                try
                {
                    _localOrgs = external.GetProductOwners(_apiKey);
                    _localOrgList = cc.GetLocalOrgs(_localOrgs);
                    _ownerList = cc.GetOwners(_localOrgs);
                    _attributes = external.GetAttributes(_apiKey, 5);
                }
                catch (Exception e)
                {
                    cc.logger.LogException(e);
                }
            }
            
            //---------Using the internal project reference Visit.CbisAPI--------------
            using (var internalCategory = new CategoriesSoapClient())
            {
                try
                {
                    var categories = internalCategory.ListAll(_apiKey, 5, 0);
                    cc.GetCategories(categories);
                }
                catch (Exception e)
                {
                    cc.logger.LogException(e);
                }
                
            }

            using (var internalProduct = new ProductsSoapClient())
            {
                try
                {
                    foreach (int lang in langList)
                    {
                        //collecting list of product per languagecode
                        _products = internalProduct.ListAll(_apiKey, lang, 0, 0, 0, 20, new ProductFilter());
                        //preparing list of products (not language-dependent)
                        _productList = cc.GetProductList(_products, _localOrgList, _ownerList, _productList, _localOrgs, _attributes, InactiveProducts);
                        //fetching the inactive products that are collected inside the parameter of _productList
                        _inactiveList.AddRange(InactiveProducts);
                        //preparing list of productinfo (language-dependent)
                        _productInfoList = cc.GetProductInfoList(_products, lang, _productList);
                        //adding the language-dependent productinfos from one language to a multilanguagelist
                        _productInfoListAllLang.AddRange(_productInfoList);
                        //fetching the medias with the currently active language code
                        _mediaList = cc.GetMediaList(_products, lang, _productInfoList);
                        //adding this product's media to the multilanguage list
                        _mediaListAllLang.AddRange(_mediaList);
                    }

                }
                catch (Exception e)
                {
                    cc.logger.LogException(e);
                }
                
            }

            foreach (var productInfo in _productInfoListAllLang)
            {
                productInfo.Product.ProductInfos.Add(productInfo);
            }

            return _productList.AsQueryable();
        }


        public Collection<int> GetInactiveProducts()
        {
            if (_inactiveList!=null)
            {
                var collection = new Collection<int>(_inactiveList);
                return collection;
            }
            return null;
        }
    }  
}
