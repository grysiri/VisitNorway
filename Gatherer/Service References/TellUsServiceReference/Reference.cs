﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gatherer.TellUsServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Name="Tellus Feed &copy; v2.4Soap", Namespace="http://feed.tellus.no/v2.4", ConfigurationName="TellUsServiceReference.TellusFeedcopyv24Soap")]
    public interface TellusFeedcopyv24Soap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://feed.tellus.no/v2.4/GetProductList", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetProductList(string LicenceKey, string LanguageId, string DBOwnerIdList, string CategoryIdList, string ContentQuality, string CountryId, string CountyIdList, string MunicipalityIdList, string FromDate, string ToDate, string DistributionChannelId, string TopicFlagIdList, string DeltaTimeStamp);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://feed.tellus.no/v2.4/GetProductList", ReplyAction="*")]
        System.Threading.Tasks.Task<string> GetProductListAsync(string LicenceKey, string LanguageId, string DBOwnerIdList, string CategoryIdList, string ContentQuality, string CountryId, string CountyIdList, string MunicipalityIdList, string FromDate, string ToDate, string DistributionChannelId, string TopicFlagIdList, string DeltaTimeStamp);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://feed.tellus.no/v2.4/GetCustomerList", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetCustomerList(string LicenceKey, string LanguageId, string DBOwnerIdList, string CategoryIdList, string ContentQuality, string CountryId, string CountyIdList, string MunicipalityIdList, string FromDate, string ToDate, string DistributionChannelId, string TopicFlagIdList, string DeltaTimeStamp);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://feed.tellus.no/v2.4/GetCustomerList", ReplyAction="*")]
        System.Threading.Tasks.Task<string> GetCustomerListAsync(string LicenceKey, string LanguageId, string DBOwnerIdList, string CategoryIdList, string ContentQuality, string CountryId, string CountyIdList, string MunicipalityIdList, string FromDate, string ToDate, string DistributionChannelId, string TopicFlagIdList, string DeltaTimeStamp);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://feed.tellus.no/v2.4/GetDBOwnerList", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetDBOwnerList(string LicenceKey, string CountryId, string DistributionChannelId, string TopicFlagIdList, string DeltaTimeStamp);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://feed.tellus.no/v2.4/GetDBOwnerList", ReplyAction="*")]
        System.Threading.Tasks.Task<string> GetDBOwnerListAsync(string LicenceKey, string CountryId, string DistributionChannelId, string TopicFlagIdList, string DeltaTimeStamp);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://feed.tellus.no/v2.4/GetProduct", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetProduct(string LicenceKey, string LanguageId, string ProductId, string DistributionChannelId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://feed.tellus.no/v2.4/GetProduct", ReplyAction="*")]
        System.Threading.Tasks.Task<string> GetProductAsync(string LicenceKey, string LanguageId, string ProductId, string DistributionChannelId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://feed.tellus.no/v2.4/GetCustomCategoryList", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetCustomCategoryList(string LicenceKey, string LanguageId, string ParentId, string DistributionChannelId, string IncludeMapping);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://feed.tellus.no/v2.4/GetCustomCategoryList", ReplyAction="*")]
        System.Threading.Tasks.Task<string> GetCustomCategoryListAsync(string LicenceKey, string LanguageId, string ParentId, string DistributionChannelId, string IncludeMapping);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface TellusFeedcopyv24SoapChannel : Gatherer.TellUsServiceReference.TellusFeedcopyv24Soap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TellusFeedcopyv24SoapClient : System.ServiceModel.ClientBase<Gatherer.TellUsServiceReference.TellusFeedcopyv24Soap>, Gatherer.TellUsServiceReference.TellusFeedcopyv24Soap {
        
        public TellusFeedcopyv24SoapClient() {
        }
        
        public TellusFeedcopyv24SoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TellusFeedcopyv24SoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TellusFeedcopyv24SoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TellusFeedcopyv24SoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetProductList(string LicenceKey, string LanguageId, string DBOwnerIdList, string CategoryIdList, string ContentQuality, string CountryId, string CountyIdList, string MunicipalityIdList, string FromDate, string ToDate, string DistributionChannelId, string TopicFlagIdList, string DeltaTimeStamp) {
            return base.Channel.GetProductList(LicenceKey, LanguageId, DBOwnerIdList, CategoryIdList, ContentQuality, CountryId, CountyIdList, MunicipalityIdList, FromDate, ToDate, DistributionChannelId, TopicFlagIdList, DeltaTimeStamp);
        }
        
        public System.Threading.Tasks.Task<string> GetProductListAsync(string LicenceKey, string LanguageId, string DBOwnerIdList, string CategoryIdList, string ContentQuality, string CountryId, string CountyIdList, string MunicipalityIdList, string FromDate, string ToDate, string DistributionChannelId, string TopicFlagIdList, string DeltaTimeStamp) {
            return base.Channel.GetProductListAsync(LicenceKey, LanguageId, DBOwnerIdList, CategoryIdList, ContentQuality, CountryId, CountyIdList, MunicipalityIdList, FromDate, ToDate, DistributionChannelId, TopicFlagIdList, DeltaTimeStamp);
        }
        
        public string GetCustomerList(string LicenceKey, string LanguageId, string DBOwnerIdList, string CategoryIdList, string ContentQuality, string CountryId, string CountyIdList, string MunicipalityIdList, string FromDate, string ToDate, string DistributionChannelId, string TopicFlagIdList, string DeltaTimeStamp) {
            return base.Channel.GetCustomerList(LicenceKey, LanguageId, DBOwnerIdList, CategoryIdList, ContentQuality, CountryId, CountyIdList, MunicipalityIdList, FromDate, ToDate, DistributionChannelId, TopicFlagIdList, DeltaTimeStamp);
        }
        
        public System.Threading.Tasks.Task<string> GetCustomerListAsync(string LicenceKey, string LanguageId, string DBOwnerIdList, string CategoryIdList, string ContentQuality, string CountryId, string CountyIdList, string MunicipalityIdList, string FromDate, string ToDate, string DistributionChannelId, string TopicFlagIdList, string DeltaTimeStamp) {
            return base.Channel.GetCustomerListAsync(LicenceKey, LanguageId, DBOwnerIdList, CategoryIdList, ContentQuality, CountryId, CountyIdList, MunicipalityIdList, FromDate, ToDate, DistributionChannelId, TopicFlagIdList, DeltaTimeStamp);
        }
        
        public string GetDBOwnerList(string LicenceKey, string CountryId, string DistributionChannelId, string TopicFlagIdList, string DeltaTimeStamp) {
            return base.Channel.GetDBOwnerList(LicenceKey, CountryId, DistributionChannelId, TopicFlagIdList, DeltaTimeStamp);
        }
        
        public System.Threading.Tasks.Task<string> GetDBOwnerListAsync(string LicenceKey, string CountryId, string DistributionChannelId, string TopicFlagIdList, string DeltaTimeStamp) {
            return base.Channel.GetDBOwnerListAsync(LicenceKey, CountryId, DistributionChannelId, TopicFlagIdList, DeltaTimeStamp);
        }
        
        public string GetProduct(string LicenceKey, string LanguageId, string ProductId, string DistributionChannelId) {
            return base.Channel.GetProduct(LicenceKey, LanguageId, ProductId, DistributionChannelId);
        }
        
        public System.Threading.Tasks.Task<string> GetProductAsync(string LicenceKey, string LanguageId, string ProductId, string DistributionChannelId) {
            return base.Channel.GetProductAsync(LicenceKey, LanguageId, ProductId, DistributionChannelId);
        }
        
        public string GetCustomCategoryList(string LicenceKey, string LanguageId, string ParentId, string DistributionChannelId, string IncludeMapping) {
            return base.Channel.GetCustomCategoryList(LicenceKey, LanguageId, ParentId, DistributionChannelId, IncludeMapping);
        }
        
        public System.Threading.Tasks.Task<string> GetCustomCategoryListAsync(string LicenceKey, string LanguageId, string ParentId, string DistributionChannelId, string IncludeMapping) {
            return base.Channel.GetCustomCategoryListAsync(LicenceKey, LanguageId, ParentId, DistributionChannelId, IncludeMapping);
        }
    }
}
