using System;
using System.Collections.Generic;
using AutoMapper;
using DomainModels.Domain;
using DomainModels.Domain.Enums;
using VNApi2.Models;
using Category = DomainModels.Domain.Category;
using Facility = DomainModels.Domain.Facility;
using FacilityCategory = DomainModels.Domain.FacilityCategory;
using Media = DomainModels.Domain.Media;
using MediaInstance = DomainModels.Domain.MediaInstance;
using Product = VNApi2.Models.Product;
using SpecialOpening = DomainModels.Domain.SpecialOpening;
using Thirdparty = DomainModels.Domain.Thirdparty;

namespace VNApi2.BLL
{
    public class DomainToApiModel : Profile
    {
        public DomainToApiModel()
        {
            Mapper.CreateMap<LocalOrg, LocalOrganization>().ForMember(p => p.Id, m => m.MapFrom(pi => (int)pi.Id));
            //MEDIAE
            Mapper.CreateMap<MediaInstance, Models.MediaInstance>();//.ForMember(dest => dest.Size, m => m.ResolveUsing(i => i.Size/*Enum.GetName(typeof(MediaSize), i.Size)*/));
            Mapper.CreateMap<Media, Models.Media>()
                .ForMember(dest => dest.Instances, m => m.MapFrom(q => Mapper.Map<List<Models.MediaInstance>>(q.Instances)));

            //PRODUCTINFOES
            Mapper.CreateMap<ProductInfo, Product>()
                .ForMember(p => p.Id, m => m.MapFrom(pi => (int)pi.Product.Id))
                .ForMember(p=>p.StreetAddress, m=>m.MapFrom(pi=>pi.Product.StreetAddress1))
                .ForMember(p => p.PostalCode, m=>m.MapFrom(pi=> pi.Product.Zipcode.Zipcode))
                .ForMember(p=>p.PostalArea, m=>m.MapFrom(pi => pi.Product.Zipcode.Postalarea.PostalareaName))
                .ForMember(p=>p.Municipality, m=>m.MapFrom(pi => pi.Product.Zipcode.Postalarea.Municipality.MunicipalityName))
                .ForMember(p=>p.County, m=>m.MapFrom(pi=>pi.Product.Zipcode.Postalarea.Municipality.County.CountyName))
                .ForMember(p=>p.Latitude, m=>m.MapFrom(pi=>pi.Product.Latitude))
                .ForMember(p => p.Longitude, m => m.MapFrom(pi => pi.Product.Longitude))
                .ForMember(p => p.Website, m => m.MapFrom(pi => pi.Product.Website))
                .ForMember(p => p.Phone, m => m.MapFrom(pi => pi.Product.Phone))
                .ForMember(p => p.Email, m => m.MapFrom(pi => pi.Product.Email))
                .ForMember(p => p.Mediae, m => m.MapFrom(pi => Mapper.Map<List<Models.Media>>(pi.Medias)))
                .ForMember(p => p.LocalOrgName, m => m.MapFrom(pi => pi.Product.LocalOrg.Name))
                .ForMember(p => p.LocalOrgWebsite, m => m.MapFrom(pi => pi.Product.LocalOrg.Website))
                .ForMember(p => p.OwnerName, m => m.MapFrom(pi => pi.Product.Owner.Name))
                .ForMember(p => p.Thirdparties, m => m.MapFrom(pi => pi.Product.Thirdparties))
                .ForMember(p => p.Categories, m => m.MapFrom(pi => pi.Product.Categories))
                .ForMember(p => p.SpecialOpenings, m => m.MapFrom(pi => pi.Product.SpecialOpenings))
                .ForMember(p => p.StandardOpeningTimes, m => m.MapFrom(pi => pi.Product.OpeningTimes))  ;

            //FACILITIES
            Mapper.CreateMap<FacilityCategory, Models.FacilityCategory>()
                .ForMember(f => f.Name, m => m.MapFrom(fa => fa.FacilityName))
                .ForMember(f => f.SubFacilities, m => m.MapFrom(fa => fa.List));
            Mapper.CreateMap<Facility, SubFacility>()
                .ForMember(f => f.Name, m => m.MapFrom(fa => fa.FacilityName))
                .ForMember(f => f.Value, m => m.MapFrom(fa => fa.Value));

            //CATEGORIES
            Mapper.CreateMap<Category, Models.Category>();

            //THIRDPARTIES
            Mapper.CreateMap<Thirdparty, Models.Thirdparty>();

            //SPECIALOPENINGS
            Mapper.CreateMap<SpecialOpening, Models.SpecialOpening>().ConvertUsing(new SpecialOpeningConverter());

            //OPENINGTIMES
            Mapper.CreateMap<OpeningTime, StandardOpeningTime>().
                ForMember(p => p.Weekday, m => m.MapFrom(o => o.Weekday));
        }
    }

    class SpecialOpeningConverter : ITypeConverter<SpecialOpening, Models.SpecialOpening>
    {
        public Models.SpecialOpening Convert(ResolutionContext context)
        {
            var sp = new Models.SpecialOpening();
            var domainsp = (SpecialOpening)context.SourceValue;

            if (domainsp.FromDate != null)
                sp.FromDate = ((DateTime) domainsp.FromDate).ToShortDateString();
            if (domainsp.ToDate != null)
                sp.ToDate = ((DateTime)domainsp.ToDate).ToShortDateString();
            if (domainsp.FromTime != null)
                sp.FromTime = ((DateTime)domainsp.FromTime).ToString("t");
            if (domainsp.ToTime != null)
                sp.ToTime = ((DateTime)domainsp.ToTime).ToString("t");

            sp.Weekday = new List<string>();
            if (domainsp.Monday)
                sp.Weekday.Add(Weekdays.Monday.ToString());
            if (domainsp.Tuesday)
                sp.Weekday.Add(Weekdays.Tuesday.ToString());
            if (domainsp.Wednesday)
                sp.Weekday.Add(Weekdays.Wednesday.ToString());
            if (domainsp.Thursday)
                sp.Weekday.Add(Weekdays.Thursday.ToString());
            if (domainsp.Friday)
                sp.Weekday.Add(Weekdays.Friday.ToString());
            if (domainsp.Saturday)
                sp.Weekday.Add(Weekdays.Saturday.ToString());
            if (domainsp.Sunday)
                sp.Weekday.Add(Weekdays.Sunday.ToString());
            return sp;
        }
    }
}