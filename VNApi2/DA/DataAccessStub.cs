using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DomainModels.Domain;
using DomainModels.Domain.Enums;
using Category = DomainModels.Domain.Category;
using FacilityCategory = DomainModels.Domain.FacilityCategory;
using Media = DomainModels.Domain.Media;
using Product = DomainModels.Domain.Product;
using SpecialOpening = DomainModels.Domain.SpecialOpening;
using Thirdparty = DomainModels.Domain.Thirdparty;

namespace VNApi2.DA
{
    public class DataAccessStub : IDataAccess
    {
        ProductInfo pinfo;

        public DataAccessStub()
        {
            int? i = 1; 
            pinfo = new ProductInfo()
            {
                 Name = "Hotell", 
                 Id = 1, 
                 Medias = new Collection<Media>(), 
                 Facilities = new Collection<FacilityCategory>(),
                 Description = "",
                 Language = LanguageCode.No,
                 Transportation = ""
            };
            Media m = new Media();
            m.Instances = new Collection<MediaInstance>();
            pinfo.Medias.Add(m);
            FacilityCategory f = new FacilityCategory();
            f.List = new List<Facility>();
            pinfo.Facilities.Add(f);
            pinfo.Product = new Product()
            {
                Id = i,
                Latitude = "71.111", 
                Longitude = "99.999", 
                Categories = new Collection<Category>(), 
                OpeningTimes = new Collection<OpeningTime>(), 
                ProductInfos = new Collection<ProductInfo>(), 
                SpecialOpenings = new Collection<SpecialOpening>(){new SpecialOpening(){Friday = true, FromDate = DateTime.Now, FromTime = DateTime.Now}}, 
                Thirdparties = new Collection<Thirdparty>()
            };
            
        }

        public IQueryable<ProductInfo> GetAll(LanguageCode lang)
        {
            var list = new List<ProductInfo>();
            list.Add(pinfo);
            list.Add(pinfo);
            var result = list.AsQueryable();

            return result;
        }

        public ProductInfo GetSingle(LanguageCode lang, int id)
        {
            if (lang == LanguageCode.No)
                return pinfo;
            return null;
        }


        public IQueryable<ProductInfo> GetByLocalOrg(string org, LanguageCode lang)
        {
            var list = new List<ProductInfo> {pinfo, pinfo};
            return list.AsQueryable();
        }

        public IQueryable<ProductInfo> GetByMunicipality(LanguageCode lang, string municipality)
        {
            if (municipality.Equals("null"))
                return null;

            var list = new List<ProductInfo> { pinfo, pinfo };
            return list.AsQueryable();
        }

        public IQueryable<ProductInfo> GetByCounty(LanguageCode lang, string county)
        {
            if (county.Equals("null"))            
                return null;

            var list = new List<ProductInfo> {pinfo, pinfo};
            return list.AsQueryable();
        }

        public IQueryable<LocalOrg> GetAlLocalOrgs()
        {
            var list = new List<LocalOrg>();
            var localorg = new LocalOrg() { Name = "VisitOslo", Id = 2 };
            list.Add(localorg);
            list.Add(localorg);
            return list.AsQueryable();
        }

        public IQueryable<ProductInfo> GetByLocalOrgId(int id, LanguageCode lang)
        {
            var list = new List<ProductInfo> { pinfo, pinfo };
            return list.AsQueryable();
        }

        public IQueryable<ProductInfo> GetByCategory(LanguageCode lang, string category)
        {
            if (category.Equals("null"))
                return null;

            var list = new List<ProductInfo> { pinfo, pinfo };
            return list.AsQueryable();
        }

        public IQueryable<ProductInfo> GetByPostalAreaName(LanguageCode lang, string area)
        {
            if (area.Equals("null"))
                return null;

            var list = new List<ProductInfo> {pinfo, pinfo};
            return list.AsQueryable();
        }
    }
}