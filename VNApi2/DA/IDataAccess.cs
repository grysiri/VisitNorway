using System;
using System.Linq;
using DomainModels.Domain;
using DomainModels.Domain.Enums;

namespace VNApi2.DA
{
    public interface IDataAccess
    {
        IQueryable<ProductInfo> GetAll(LanguageCode lang);
        ProductInfo GetSingle(LanguageCode lang, int id);
        IQueryable<ProductInfo> GetByPostalAreaName(LanguageCode lang, string area);
        IQueryable<ProductInfo> GetByMunicipality(LanguageCode lang, string municipality);
        IQueryable<ProductInfo> GetByCounty(LanguageCode lang, string county);
        IQueryable<LocalOrg> GetAlLocalOrgs();
        IQueryable<ProductInfo> GetByLocalOrgId(int id, LanguageCode lang);
        IQueryable<ProductInfo> GetByCategory(LanguageCode lang, string category);        
    }
}