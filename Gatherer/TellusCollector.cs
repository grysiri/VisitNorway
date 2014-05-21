using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Security;
using DomainModels.Domain;
using DomainModels.Domain.Enums;
using Gatherer.TellUsServiceReference;
/*
    * Structure of tellus-converting:
    *  1. convert localorgs with TellusConverter.GetLocalOrgs
    *      param:  string of dbowners from tellus
    *      
    *  2. for each language:
    *      2a  convert owners with TellusConverter.GetOwners
    *          param:  string of customers from tellus, 
    *                  collection of LocalOrgs,
    *                  collection of Owners
    *      2b  convert products with TellusConverter.GetProductInfo
    *          param:  string of products from tellus,
    *                  collection of Products,
    *                  collection of Owners,
    *                  collection of LocalOrgs
    *                  
    *  3. save Products in Owners list of Products with SaveProductsInOwners
    *      param:  collection of Owners,
    *              collection of Products
    * 
    *  4. return collection of Productinfos as queryable
    */
using SaveToDb;


namespace Gatherer
{
    public class TellusCollector
    {
        private string _apiKey = "";
        private string _productsReceived = "";
        private string _lastResponse;
        private int _numberOfCalls; //to keep count of number of calls to tellus service
        private TellusConverter tc;
        

        public TellusCollector()
        {
            _numberOfCalls = 0;
            tc=new TellusConverter();            
        }     

        //get all data from tellus and converts it to our model
        //using TellusConverter
        public IQueryable<Product> TellusCollect()
        {
            _numberOfCalls++;            
            //collections av owners, products og productinfos for temporary storage
            Collection<LocalOrg> localorgs;
            var owners = new Collection<Owner>();
            var products = new Collection<Product>();
            var productinfos = new Collection<ProductInfo>();
            
            //list with language strings (languages to be collected)
            var languages = new List<string>()
            {
                "en",                                  
                "no",
                "de",
                "es",
                "it",                               
                "fr"
            };
            

            //to be used when collecting from all counties. Check http://bit.ly/1uchIiP for complete list of counties
            var counties = new List<string>
            {
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9",
                "10",
                "11",
                "12",                
                "14",
                "15",
                "16",
                "17",
                "18",
                "19",
                "20"
            };

            //Territories without countyId, checks on municipalityId.    
            const string spitsbergen = "2111";
            const string bjornOya = "2121";
            const string hopen = "2131";
            const string janMayen = "2211";

            var municipalities = new List<string> {spitsbergen,bjornOya,hopen,janMayen};

            using (var tellUsClient = new TellusFeedcopyv24SoapClient())
            {

                Console.WriteLine("**  TELLUSCOLLECTOR  **\n");

                try
                {
                    //First getting local orgs, which are language-independent                    
                    Console.WriteLine("Collectiong all local orgs...");
                    var dbOwnerListStringEn = tellUsClient.GetDBOwnerList(_apiKey, "no",
                        "220", null, null);
                    
                    //List for timestamps returned by the tellus feed                    
                    var receivedStrings = new List<string>();

                    Console.WriteLine("Converting all local orgs...");
                    localorgs = tc.GetLocalOrgs(dbOwnerListStringEn);                    
                    
                    //Getting all the products in each language, converts them and stores in collections                    
                    foreach (var lang in languages)
                    {
                        //TODO foreach county in counties
                        foreach (var county in counties)
                        {
                            Console.WriteLine("Collecting products in " + lang + " from county #"+county);
                            var info = tellUsClient.GetProductList(_apiKey, lang, null, null,
                                null, "no", county, null, null, null, "220", null, null);
                            //3
                            Console.WriteLine("Collecting all owners in " + lang + " from county #" + county);
                            var customerListSpråk = tellUsClient.GetCustomerList(_apiKey, lang,
                                null, null, null, "no", county, null, null, null, "220", null, null);
                            //3                    
                            Console.WriteLine("Converting all owners...");
                            owners = tc.GetOwners(customerListSpråk, localorgs, owners);

                            Console.WriteLine("Converting all productInfos...");
                            tc.GetProductInfo(info, products, owners, productinfos, localorgs);

                            Console.WriteLine("Products received: " + tc.ProductsReceived);
                            receivedStrings.Add(tc.ProductsReceived);
                        }
                            
                    }
                    
                    //Uses the first timestamp received (the oldest)                    
                    if (receivedStrings.Any())
                    {
                        _productsReceived = receivedStrings.First();                                              
                    }
                                        
                    //Writes total number of products and productinfos found                     
                    Console.WriteLine("\nNumber of products: " + products.Count());
                    Console.WriteLine("Number of product infos: " + productinfos.Count());
                    Console.WriteLine("Products in norwegian: " + productinfos.Count(p => p.Language == LanguageCode.No));                    

                    foreach (var productInfo in productinfos)
                    {
                        productInfo.Product.ProductInfos.Add(productInfo);
                    }

                    return products.AsQueryable();
                }
                catch (TimeoutException te)
                {
                    //timeout from TellusService, tries again
                    Console.WriteLine("TimeOutException " + _numberOfCalls);
                    tc.Logger.LogException(te);
                    if (_numberOfCalls < 10) //check if has been called ten times, in that case give up
                        return TellusCollect();
                    return null;
                }
                catch(Exception e) 
                {
                    Console.WriteLine("en annen Exception " + e);
                    tc.Logger.LogException(e);
                }
                
                }
                return null;
            }                     
        
        public Collection<int> TellusDeleted()
        {
            _lastResponse = Response.GetLatestResponse();
            var tc = new TellusConverter();
            var deletedProducts = new Collection<int>();                   
            
            using (var tellusDeletedClient = new TellusFeedcopyv24SoapClient())
            {
                //checks products have been collected before
                if (_productsReceived !=null)
                {
                    //if there has been a previous gathering of products, the timestamp of that gathering will be sent as a parameter to the tellus feed, which will return
                    //a list of products that have been deleted since the last call.
                    var del = tc.DeletedProducts(tellusDeletedClient.GetProductList(_apiKey, "no", null,
                           null, null, "no", "3", null, null, null, "220", null, _lastResponse));                    
                    
                    foreach (var delProd in del)
                    {                        
                        deletedProducts.Add(delProd);
                    }
                    Console.WriteLine("--- --- ---");
                    Console.WriteLine("Products deleted since: " + _lastResponse + " : " + del.Count()); 
                }
                //if there is no registered last gathering, the timestamp will be null and the tellus feed will return all of its products 
                else
                {
                    var del = tc.DeletedProducts(tellusDeletedClient.GetProductList(_apiKey, "no", null,
                           null, null, "no", "3", null, null, null, "220", null, null));                    

                    foreach (var delProd in del)
                    {                        
                        deletedProducts.Add(delProd);
                    }
                    Console.WriteLine("--- --- ---");
                }                
            }
            return deletedProducts;
        }

        public String GetResponseTime()
        {
            return _productsReceived;
        }
    }
}