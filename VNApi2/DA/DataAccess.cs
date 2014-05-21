using System;
using System.Linq;
using DomainModels.Domain;
using DomainModels.Domain.Enums;
using SaveToDb;

namespace VNApi2.DA
{
    public class DataAccess : IDataAccess
    {
        public ProductContext Db = new ProductContext();             

        //get all products in one language
        public IQueryable<ProductInfo> GetAll(LanguageCode lang)
        {
            var exceptionLogger = new ExceptionLogger();
            
                try
                {
                    var prodInfo = Db.ProductInfos.Include("Product.Zipcode.Postalarea.Municipality.County")
                        .Where(pi => pi.Language == lang);                    
                    
                    return prodInfo;   
                }
                catch (Exception e)
                {                    
                    exceptionLogger.LogException(e);
                }                
            
            return null;
        }

        //get single product
        public ProductInfo GetSingle(LanguageCode lang, int id)
        {             
                try
                {                    
                    var productInfo =
                        Db.ProductInfos.Include("Product.Zipcode.Postalarea.Municipality.County")
                            .Single(p => p.Language == lang && p.Product.Id == id);
                    return productInfo;
                }
                catch (Exception e)
                {
                    var ex = e.ToString();
                }            
            return null;
        }


        public IQueryable<LocalOrg> GetAlLocalOrgs()
        {
            return Db.LocalOrgs;
        }

        public LocalOrg GetSingleLocalOrg(int id)
        {
            var org = Db.LocalOrgs.Single(o => o.Id==id);
            return org;
        }


        public IQueryable<ProductInfo> GetByLocalOrgId(int id, LanguageCode lang)
        {
            var all = Db.ProductInfos.Include("Product.Zipcode.Postalarea.Municipality.County")
                .Where(pi => pi.Language == lang && pi.Product.LocalOrg.Id == id);
            return all;
        }

        public IQueryable<ProductInfo> GetByCategory(LanguageCode lang, string category)
        {
            try
            {
                var cat = Db.Categories.FirstOrDefault(c => c.CategoryName == category);
                var all = Db.ProductInfos.Include("Product.Zipcode.Postalarea.Municipality.County")
                    .Where(pi => (pi.Language == lang) && pi.Product.Categories.Any(c=>c.CategoryName == cat.CategoryName));
                return all;
            }
            catch (Exception e)
            {
                var ex = e.ToString();
                return null;
            }
        }


        public IQueryable<ProductInfo> GetByPostalAreaName(LanguageCode lang, string area)
        {
            var exceptionLogger = new ExceptionLogger();
            try
            {                
                var productInfo = Db.ProductInfos.Include("Product.Zipcode.Postalarea.Municipality.County")
                    .Where(p => p.Product.Zipcode.Postalarea.PostalareaName.Equals(area) && p.Language == lang);               
                return productInfo.AsQueryable();
            }
            catch (Exception e)
            {
                exceptionLogger.LogException(e);
                return null;
            }
        }

        public IQueryable<ProductInfo> GetByMunicipality(LanguageCode lang, string municipality)
        {
            var exceptionLogger = new ExceptionLogger();
            try
            {
                var productInfo = Db.ProductInfos.Include("Product.Zipcode.Postalarea.Municipality.County")
                    .Where( p => p.Product.Zipcode.Postalarea.Municipality.MunicipalityName.Equals(municipality) &&
                            p.Language == lang);
                return productInfo.AsQueryable();
            }
            catch (Exception e)
            {
                exceptionLogger.LogException(e);
                return null;
            }
        }

        public IQueryable<ProductInfo> GetByCounty(LanguageCode lang, string county)
        {
            var exceptionLogger = new ExceptionLogger();
            try
            {
                var productInfo = Db.ProductInfos.Include("Product.Zipcode.Postalarea.Municipality.County")
                    .Where(p => p.Product.Zipcode.Postalarea.Municipality.County.CountyName.Equals(county) &&
                                p.Language == lang);
                return productInfo.AsQueryable();
            }
            catch (Exception e)
            {
                exceptionLogger.LogException(e);
                return null;
            }
        }
    }
}