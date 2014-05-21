using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DomainModels.Domain.Enums;
using VNApi2.DA;
using VNApi2.Models;
using Product = VNApi2.Models.Product;

namespace VNApi2.BLL
{
    public class Logic
    {
        private IDataAccess _dal;

        public Logic()
        {
            _dal = new DataAccess();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nb-NO");
        }

        public Logic(string test)
        {
            _dal = new DataAccessStub();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nb-NO");
        }

        public IQueryable<Product> GetAll(string language)
        {
            try
            {
                var productsbylang = _dal.GetAll(ConvertHelper.GetLanguageCode(language)).ToList();                
                var mappedproducts = Mapper.Map<List<Product>>(productsbylang).AsQueryable();

                return mappedproducts;
            }
            catch (Exception e)
            {
                var ex = e.ToString();
                return null;
            }
        }


        public Product GetSingle(string language, string id)
        {
            var single = _dal.GetSingle(ConvertHelper.GetLanguageCode(language), ConvertHelper.StringToInt(id));
            try
            {
                return Mapper.Map<Product>(single);
            }
            catch (Exception e)
            {
                var ex = e.ToString();
                return null;
            }
        }

        public IQueryable<Product> GetByLocalOrgId(int id, string lang)
        {
            try
            {
                var byOrg = _dal.GetByLocalOrgId(id, ConvertHelper.GetLanguageCode(lang));
                return Mapper.Map<List<Product>>(byOrg).AsQueryable();
            }
            catch (Exception e)
            {
                var ex = e.ToString();
                return null;
            }
        }

        public IQueryable<LocalOrganization> GetAllLocalOrgsOrganizations()
        {
            return _dal.GetAlLocalOrgs().Project().To<LocalOrganization>();
        }

        public IQueryable<Product> GetNearby(string language, string latitude, string longitude)
        {
            var nearbyprods = new List<Product>();
            var lat = ConvertHelper.StringToDouble(latitude);
            var lon = ConvertHelper.StringToDouble(longitude);
            try
            {
            var allprods = Mapper.Map<List<Product>>(_dal.GetAll(ConvertHelper.GetLanguageCode(language)));
            foreach (var prod in allprods)
            {
                if (prod.Latitude != null && prod.Longitude != null)
                {
                    var thislat = ConvertHelper.StringToDouble(prod.Latitude);
                    var thislon = ConvertHelper.StringToDouble(prod.Longitude);
                    if (GetDistanceFromLatLonInKm(thislat, thislon, lat, lon) < 1)
                        nearbyprods.Add(prod);
                }
            }

            return nearbyprods.AsQueryable();
            }
            catch (Exception e)
            {
                e.ToString();
                throw;
            }
            
        }

        public IQueryable<Product> GetByPost(string language, string area)
        {
            try
            {
                var byPost = _dal.GetByPostalAreaName(ConvertHelper.GetLanguageCode(language), area);
                return Mapper.Map<List<Product>>(byPost).AsQueryable();
            }
            catch (Exception e)
            {
                var ex = e.ToString();
                return null;
            }
        }

        public IQueryable<Product> GetByMunicipality(string language, string municipality)
        {
            try
            {
                var byMunicipality = _dal.GetByMunicipality(ConvertHelper.GetLanguageCode(language), municipality);
                return Mapper.Map<List<Product>>(byMunicipality).AsQueryable();
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
        }

        public IQueryable<Product> GetByCounty(string language, string county)
        {
            try
            {
                var buCounty = _dal.GetByCounty(ConvertHelper.GetLanguageCode(language), county);
                return Mapper.Map<List<Product>>(buCounty).AsQueryable();
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
        }


        double GetDistanceFromLatLonInKm(double lat1,double lon1,double lat2,double lon2) {
          var r = 6371; // Radius of the earth in km
          var dLat = deg2rad(lat2-lat1);  // deg2rad below
          var dLon = deg2rad(lon2-lon1); 
          var a = 
            Math.Sin(dLat/2) * Math.Sin(dLat/2) +
            Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * 
            Math.Sin(dLon/2) * Math.Sin(dLon/2)
            ; 
          var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a)); 
          var d = r * c; // Distance in km
          return d;
        }


        double deg2rad(double deg)
        {
            return deg*(Math.PI/180);
        }

        public IQueryable<Product> GetByCategory(string language, string category)
        {
            try
            {
                var bycat = _dal.GetByCategory(ConvertHelper.GetLanguageCode(language), category);
                if (bycat.Any())
                    return Mapper.Map<List<Product>>(bycat).AsQueryable();
            }
            catch (System.Reflection.TargetException e)
            {
                var ex = e.ToString();
            }
            catch (Exception e)
            {
                var ex = e.ToString();
                
            }
            return null;
        }
    }


    public static class ConvertHelper
    {
        public static LanguageCode GetLanguageCode(string lang)
        {
            switch (lang)
            {
                case "no":
                    return LanguageCode.No;
                case "en":
                    return LanguageCode.En;
                case "es":
                    return LanguageCode.Es;
                case "de":
                    return LanguageCode.De;
                case "fr":
                    return LanguageCode.Fr;
                case "it":
                    return LanguageCode.It;
                default:
                    throw new FormatException("Error: language is not valid.");
            }
        }

        public static int StringToInt(string id)
        {
            return Int32.Parse(id);
        }

        public static double StringToDouble(string s)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            return Double.Parse(s);
        }

    }
  
}
