using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using DomainModels.Domain.Enums;
using SaveToDb;
using Product = DomainModels.Domain.Product;
using DayOfWeek = DomainModels.Domain.Enums.Weekdays;
using Gatherer.TellusConverterHelpers;

using DomainModels.Domain;
using Category = DomainModels.Domain.Category;
using Facility = DomainModels.Domain.Facility;
using FacilityCategory = DomainModels.Domain.FacilityCategory;

using HtmlAgilityPack;

namespace Gatherer
{

    public class TellusConverter
    {
        public ExceptionLogger Logger;
                
        public string ProductsReceived;

        public TellusConverter()
        {
            Logger = new ExceptionLogger();
        }

        //------------------------ LOCAL ORGS ------------------------//

        //Gets an xml-string of dbowners(=local orgs in our model) from tellus,
        //and converts it to LocalOrg objects
        public Collection<LocalOrg> GetLocalOrgs(string stringOfDbowners)
        {
            var localOrgs = new Collection<LocalOrg>();
            var xDoc = XDocument.Parse(stringOfDbowners); //convert string to XDocument            

            foreach (var dbowner in xDoc.Descendants("dbowner"))
            {
                //instanciate new localorg & set provider:
                var org = new LocalOrg {Provider = ExternalProvider.TellUs};
                //Id:
                if (dbowner.Attribute("dbownerId") != null)
                {
                    try
                    {
                        org.ExternalId = int.Parse(dbowner.Attribute("dbownerId").Value); //parse id from string to int                                                
                    }
                    catch (FormatException fe)
                    {                        
                        Logger.LogException(fe);
                    }
                }
                //name:
                var name = dbowner.Element("name");
                if (name != null)
                    org.Name = name.Value;
                //website:
                if (dbowner.Element("contactInformation") != null &&
                    dbowner.Element("contactInformation").Element("web") != null)
                {
                    org.Website = dbowner.Element("contactInformation").Element("web").Value;
                }
                if (!localOrgs.Contains(org, new LocalOrgComparer()))
                localOrgs.Add(org);
            }//end of foreach
            return localOrgs;
        }//End of GetLocalOrgs


        //------------------------ OWNERS ------------------------//

        //gets an xml-string of customers (=owners in our model) from tellus,
        //and converts it to collection of Owners
        public Collection<Owner> GetOwners(string stringOfCustomers, Collection<LocalOrg> localorgs, Collection<Owner> owners)
        {
            var xDoc = XDocument.Parse(stringOfCustomers);

            foreach (var customer in xDoc.Descendants("customer"))
            {
                //instanciate new owner & set provider:
                var owner = new Owner {Provider = ExternalProvider.TellUs, Products = new Collection<Product>()};
                //name:
                var name = customer.Element("name");
                if (name != null)
                {
                    owner.Name = name.Value;
                }
                //externalId:
                if (customer.Attribute("customerId") != null)
                {
                    try
                    {
                        owner.ExternalId = int.Parse(customer.Attribute("customerId").Value);
                    }
                    catch (FormatException fe)
                    {
                        Logger.LogException(fe);
                    }
                }
                //localorg:
                if (customer.Attribute("dbownerId") != null)
                {
                    try
                    {
                       var localorgId = int.Parse(customer.Attribute("dbownerId").Value);                        
                    }
                    catch (FormatException fe)
                    {
                        Logger.LogException(fe);
                    }
                }

                if (!owners.Contains(owner, new OwnerComparer()))
                    owners.Add(owner);
            }//End of foreach
            return owners;
        }//End of GetOwners


        // ------------------ SAVE PRODUCTS IN OWNERS ---------------------------//

        public int SaveProductsInOwners(Collection<Owner> owners, Collection<Product> products)
        {
            int i = 0;
            foreach (var owner in owners)
            {
                foreach (var product in products)
                {
                    if (product.Owner.ExternalId == owner.ExternalId &&
                        !owner.Products.Contains(product, new ProductExIdComparer()))
                    {
                        owner.Products.Add(product);
                        i++;
                    }
                }
            }
            return i;
        }


        // ------------------- CONVERT TO PRODUCT ------------------------//

        public Product ConvertToProduct(XElement product)
        {
            //instanciate new product & set provider:
            var newprod = new Product()
            {
                ExternalProvider = ExternalProvider.TellUs,
                OpeningTimes = new Collection<OpeningTime>(),
                SpecialOpenings = new Collection<SpecialOpening>(),
                Categories = new Collection<Category>(),
                Thirdparties = new Collection<Thirdparty>(),
                ProductInfos = new Collection<ProductInfo>()
            };
            //externalId:
            if (product.Attribute("id") != null)
            {
                try
                {
                    newprod.ExternalId = int.Parse(product.Attribute("id").Value);
                }
                catch (FormatException fe)
                {
                    Logger.LogException(fe);
                }
            }
            //address:
            var address = product.Element("address");
            if (address != null)
            {
                if (address.Element("street") != null)
                {
                    newprod.StreetAddress1 = "";
                    foreach (var street in address.Descendants("street"))
                        newprod.StreetAddress1 += street.Value + " ";
                }
                if (address.Element("postalArea") != null &&
                    address.Element("postalArea").Attribute("postalCode") != null)
                    newprod.PostalCode = address.Element("postalArea").Attribute("postalCode").Value;
            }
            //lat&long:
            var geoloc = product.Element("geoLocation");
            if (geoloc != null && geoloc.Element("longitude") != null && geoloc.Element("latitude") != null)
            {
                newprod.Longitude = geoloc.Element("longitude").Value;
                newprod.Latitude = geoloc.Element("latitude").Value;
            }
            //contactinfo:
            if (product.Element("contactInformation") != null &&
                product.Element("contactInformation").Element("web") != null)
            {
                newprod.Website = product.Element("contactInformation").Element("web").Value;
            }
            if (product.Element("contactList") != null && product.Element("contactList").Element("contact") != null)
            {
                var contact = product.Element("contactList").Element("contact");
                if (contact.Element("telephone") != null)
                    newprod.Phone = contact.Element("telephone").Value;
                if (contact.Element("email") != null)
                    newprod.Email = contact.Element("email").Value;
            }
            //created and modified:
            if (product.Attribute("created") != null)
                newprod.CreatedDate = StringToDateTime(product.Attribute("created").Value);
            if (product.Attribute("modified") != null)
                newprod.ModifiedDate = StringToDateTime(product.Attribute("modified").Value);
            
            //categories:
            //newprod.Categories = new Collection<Category>();
            foreach (var category in product.Descendants("categoryList").Descendants("category"))
            {
                //hvis har subkategorier, lagre disse EDIT:har visst alltid subtype1list, ikke alltid subtype2list
                //(får med parentkategoriene automatisk da subkategoriene har parentID)                
                if (category.Element("categorySubType1List").HasElements)
                    foreach (var subcategory1 in category.Descendants("categorySubType1List").Descendants("categorySubType1"))
                    {

                        Category c = Categories.AddCategory(subcategory1.Attribute("id").Value, category.Attribute("id").Value);                        
                        if (c != null && !newprod.Categories.Contains(c, new IdAndNameComparer()))
                            newprod.Categories.Add(c);

                        //to access categorySubType2 (we don't use them):
                        //foreach (var subcategory2 in subcategory1.Descendants("categorySubType2List").Descendants("categorySubType2")) {}
                    }
            }

            //thirdparties:
            if (product.Element("externalSystemProductMappingList") != null)
            {                
                foreach (var external in product.Element("externalSystemProductMappingList").Descendants("externalSystemProductMapping"))
                {
                    var third = new Thirdparty { Provider = ExternalProvider.TellUs };
                    if (external.Element("externalSystem") != null)
                    {
                        if (external.Element("externalSystem").Attribute("id") != null)
                        {
                            try
                            {
                                third.ExternalId =
                                    int.Parse(external.Element("externalSystem").Attribute("id").Value);
                            }
                            catch (FormatException fe)
                            {
                                Logger.LogException(fe);
                            }
                        }
                        if (external.Element("externalSystem").Element("name") != null)
                            third.Name = external.Element("externalSystem").Element("name").Value;
                    }
                    if (external.Element("externalURI") != null)
                        third.Uri = external.Element("externalURI").Value;                    

                    if (third.Uri == null && external.Element("externalId") != null)
                        third.Uri = external.Element("externalId").Value;

                    newprod.Thirdparties.Add(third);
                }
            }


            //Standard opening hours. Schedulelist without date intervals are considered standard. 
            if (product.Element("scheduleList") != null)
            {                
                foreach (var schedule in product.Element("scheduleList").Descendants("schedule"))
                {
                    //if schedule has a fromDate element it is considered a special schedule
                    if (!schedule.Elements("fromDate").Any())
                    {
                        var opening = new OpeningTime() {Weekday = new List<Weekdays>()};                        

                        if (schedule.Elements("fromTime").Any())
                        {
                            try
                            {
                                opening.FromTime = schedule.Element("fromTime").Value;
                            }
                            catch (Exception fromTimeException)
                            {
                                Logger.LogException(fromTimeException);
                            }
                        }

                        if (schedule.Elements("toTime").Any())
                        {
                            try
                            {
                                opening.ToTime = schedule.Element("toTime").Value;
                            }
                            catch (Exception toTimeException)
                            {
                                Logger.LogException(toTimeException);
                            }
                        }

                        if (schedule.Elements("daysOfWeek").Elements().Any())
                        {
                            foreach (var day in schedule.Element("daysOfWeek").Descendants())
                            {                               
                                var wkday = day.Name.ToString();
                                switch (wkday)
                                {
                                    case "monday":
                                        opening.Weekday.Add(Weekdays.Monday);
                                        opening.Monday = true;
                                        break;
                                    case "tuesday":
                                        opening.Weekday.Add(Weekdays.Tuesday);
                                        opening.Tuesday = true;
                                        break;
                                    case "wednesday":
                                        opening.Weekday.Add(Weekdays.Wednesday);
                                        opening.Wednesday = true;
                                        break;
                                    case "thursday":
                                        opening.Weekday.Add(Weekdays.Thursday);
                                        opening.Thursday = true;
                                        break;
                                    case "friday":
                                        opening.Weekday.Add(Weekdays.Friday);
                                        opening.Friday = true;
                                        break;
                                    case "saturday":
                                        opening.Weekday.Add(Weekdays.Saturday);
                                        opening.Saturday = true;
                                        break;
                                    case "sunday":
                                        opening.Weekday.Add(Weekdays.Sunday);
                                        opening.Sunday = true;
                                        break;
                                    default:
                                        throw new InvalidEnumArgumentException("Error in switching on weekday");
                                }
                            }
                            opening.Weekday.Sort(new WeekdayComparer());
                        }//end of if daysOfWeek
                        
                        newprod.OpeningTimes.Add(opening);

                    }//End of if !schedule.fromDate
                }//End of foreach schedule
                
            }

            //Special opening hours, seasonal, vacations, etc. Every scheduleList that contains explicit date intervals are considered as special opening hours
            if (product.Element("scheduleList") != null)
            {                
                foreach (var schedule in product.Element("scheduleList").Descendants("schedule"))
                {
                    if (schedule.Elements("fromDate").Any() || schedule.Elements("toDate").Any())
                    {
                        var special = new SpecialOpening() {Weekday = new List<Weekdays>()};                        

                        //Data may be inconsistent, to and from values not always provided, at least from Tellus
                        if (schedule.Elements("fromDate").Any())
                        {
                            try
                            {
                                DateTime fd = Convert.ToDateTime(schedule.Element("fromDate").Attribute("date").Value);
                                special.FromDate = fd;
                            }
                            catch (FormatException fe)
                            {
                                Logger.LogException(fe);
                            }
                        }

                        if (schedule.Elements("toDate").Any())
                        {
                            try
                            {
                                DateTime td = Convert.ToDateTime(schedule.Element("toDate").Attribute("date").Value);
                                special.ToDate = td;
                            }
                            catch (FormatException fe)
                            {
                                Logger.LogException(fe);
                            }
                        }

                        if (schedule.Elements("fromTime").Any())
                        {
                            try
                            {
                                DateTime ft = Convert.ToDateTime(schedule.Element("fromTime").Attribute("time").Value);
                                special.FromTime = ft;
                            }
                            catch (FormatException fe)
                            {
                                Logger.LogException(fe);
                            }
                        }

                        if (schedule.Elements("toTime").Any())
                        {
                            try
                            {
                                DateTime tt = Convert.ToDateTime(schedule.Element("toTime").Attribute("time").Value);
                                special.ToTime = tt;
                            }
                            catch (FormatException fe)
                            {
                                Logger.LogException(fe);
                            }
                           
                        }
                            
                        if (schedule.Elements("daysOfWeek").Elements().Any())
                        {
                            foreach (var day in schedule.Element("daysOfWeek").Descendants())
                            {                                
                                var wkday = day.Name.ToString();

                                switch (wkday)
                                {
                                    case "monday":
                                        special.Weekday.Add(Weekdays.Monday);
                                        special.Monday = true;
                                        break;
                                    case "tuesday":
                                        special.Weekday.Add(Weekdays.Tuesday);
                                        special.Tuesday = true;
                                        break;
                                    case "wednesday":
                                        special.Weekday.Add(Weekdays.Wednesday);
                                        special.Wednesday = true;
                                        break;
                                    case "thursday":
                                        special.Weekday.Add(Weekdays.Thursday);
                                        special.Thursday = true;
                                        break;
                                    case "friday":
                                        special.Weekday.Add(Weekdays.Friday);
                                        special.Friday = true;
                                        break;
                                    case "saturday":
                                        special.Weekday.Add(Weekdays.Saturday);
                                        special.Saturday = true;
                                        break;
                                    case "sunday":
                                        special.Weekday.Add(Weekdays.Sunday);
                                        special.Sunday = true;
                                        break;
                                }
                            }

                        }
                        special.Weekday.Sort(new WeekdayComparer());
                        special.Weekdayarray = special.Weekday.ToArray();
                        newprod.SpecialOpenings.Add(special);
                    }//End of if schedule has dates                        
                }//end of foreach schedule
            }//end of if schedulelist                                                 
            return newprod;


        }


        //------------------------ PRODUCTINFO ------------------------//     
        public Collection<ProductInfo> GetProductInfo(string s, Collection<Product> products, Collection<Owner> owners, Collection<ProductInfo> prodInfoList, Collection<LocalOrg> localorgs)
        {            
            var xDoc = XDocument.Parse(s);

            foreach (var prodlist in xDoc.Descendants("productList"))
            {                    
                ProductsReceived = prodlist.Attribute("responseSent").Value;
            }

            foreach (var product in xDoc.Descendants("product"))
            {
                //New prodinfo 
                var newProdInfo = new ProductInfo(){Medias = new Collection<Media>(), Facilities = new Collection<FacilityCategory>()};
                
                //Setting externalId. To connect to an instance of product with the same Id            
                if (product.Attribute("id") != null)
                {
                    try
                    {
                        //set the product if it already exists
                        newProdInfo.Product = products.Single(p => p.ExternalId == int.Parse(product.Attribute("id").Value));                        
                    }
                    catch (FormatException fe)
                    {
                        Logger.LogException(fe);
                    }
                    catch (Exception e) //if didn't find the matching Product
                    {   
                        //create new product
                        var newprod = ConvertToProduct(product);
                        try
                        {
                            SaveOwnerAndLocalOrg(newprod,
                                owners.Single(o =>
                                    o.ExternalId == int.Parse(product.Attribute("customerId").Value))/* &&
                                    (o.LocalOrg.ExternalId == int.Parse(product.Attribute("dbownerId").Value))*/, localorgs.Single(l => l.ExternalId == int.Parse(product.Attribute("dbownerId").Value)));
                            products.Add(newprod);
                            //find the product in collection of products and set it:
                            newProdInfo.Product =
                                products.Single(p => p.ExternalId == int.Parse(product.Attribute("id").Value));
                        }
                        catch (Exception e2) //didn't find owner or didn't find the product just made
                        {
                            Logger.LogException(e2);
                            newProdInfo.Product = newprod;
                        }
                    }
                }                


                //Setting ProdInfos' language
                if (product.Attribute("lang") != null)
                {
                    var langCode = product.Attribute("lang").Value.ToLower();

                    switch (langCode)
                    {
                        case "no":                        
                            newProdInfo.Language = LanguageCode.No;
                            break;
                        case "en":
                            newProdInfo.Language = LanguageCode.En;
                            break;
                        case "es":
                            newProdInfo.Language = LanguageCode.Es;
                            break;
                        case "cn":
                            newProdInfo.Language = LanguageCode.Cn;
                            break;
                        case "dk":
                            newProdInfo.Language = LanguageCode.Dk;
                            break;
                        case "fr":
                            newProdInfo.Language = LanguageCode.Fr;
                            break;
                        case "de":
                            newProdInfo.Language = LanguageCode.De;
                            break;
                        case "it":
                            newProdInfo.Language = LanguageCode.It;
                            break;
                        case "ja":
                            newProdInfo.Language = LanguageCode.Ja;
                            break;
                        case "nl":
                            newProdInfo.Language = LanguageCode.Nl;
                            break;
                        case "pl":
                            newProdInfo.Language = LanguageCode.Pl;
                            break;
                        case "ru":
                            newProdInfo.Language = LanguageCode.Ru;
                            break;
                        case "se":
                            newProdInfo.Language = LanguageCode.Se;
                            break;
                        case "engb":
                            newProdInfo.Language = LanguageCode.EnGb;
                            break;
                        case "enus":
                            newProdInfo.Language = LanguageCode.EnUs;
                            break;
                        case "nnno":
                            newProdInfo.Language = LanguageCode.NnNo;
                            break;
                        case "fi":
                            newProdInfo.Language = LanguageCode.Fi;
                            break;
                        case "hu":
                            newProdInfo.Language = LanguageCode.Hu;
                            break;
                        case "lt":
                            newProdInfo.Language = LanguageCode.Lt;
                            break;
                        case "lv":
                            newProdInfo.Language = LanguageCode.Fr;
                            break;
                        case "et":
                            newProdInfo.Language = LanguageCode.Et;
                            break;
                        case "pt":
                            newProdInfo.Language = LanguageCode.Pt;
                            break;
                        case "cs":
                            newProdInfo.Language = LanguageCode.Cs;
                            break;
                        case "enie":
                            newProdInfo.Language = LanguageCode.EnIe;
                            break;
                        case "ar":
                            newProdInfo.Language = LanguageCode.Ar;
                            break;
                        case "he":
                            newProdInfo.Language = LanguageCode.He;
                            break;
                        case "hi":
                            newProdInfo.Language = LanguageCode.Hi;
                            break;
                        case "ko":
                            newProdInfo.Language = LanguageCode.Ko;
                            break;
                        case "sl":
                            newProdInfo.Language = LanguageCode.Sl;
                            break;
                        case "tr":
                            newProdInfo.Language = LanguageCode.Tr;
                            break;
                        case "is":
                            newProdInfo.Language = LanguageCode.Is;
                            break;
                        case "ro":
                            newProdInfo.Language = LanguageCode.Ro;
                            break;
                        case "el":
                            newProdInfo.Language = LanguageCode.El;
                            break;
                        case "sk":
                            newProdInfo.Language = LanguageCode.Sk;
                            break;
                        case "frca":
                            newProdInfo.Language = LanguageCode.FrCa;
                            break;
                        case "idid":
                            newProdInfo.Language = LanguageCode.IdId;
                            break;
                    }
                }
               
                //Setting the name of the product
                if (product.Element("name") != null && product.Element("name").Value != null)
                {
                    newProdInfo.Name = product.Element("name").Value;                       
                }

                //Setting description (if language is the same as prodInfos language) 
                if (product.Element("textList") != null)
                {                                        
                    foreach (var textlist in product.Element("textList").Descendants("text"))
                    {
                        //must convert ToLower because enum is written in UPPERlower-format
                        var textLang = textlist.Attribute("lang").Value.ToLower();
                        var prodInfLang = newProdInfo.Language.ToString().ToLower();

                        if (textLang.Equals(prodInfLang))
                        {
                            if (textLang.Equals(prodInfLang))
                            {
                                if (textlist.Attribute("type").Value.Equals("HOVED"))
                                {
                                    newProdInfo.Description = textlist.Value;
                                }                                                                            
                            }//end if HOVED

                            if (textlist.Attribute("type").Value.Equals("TRANSPORT"))
                            {                                
                                //removing excess newline
                                var  txtVal= textlist.Value;                                
                                var removed=txtVal.Replace("\n", " ");
                                newProdInfo.Transportation = removed;                                                                
                            }//End if TRANSPORT

                        }//End of if textLang.Equals(prodInfLang)                                                

                        //MEDIA - Video is under textlist from Tellus     
                        if (textlist.Attribute("type").Value.Equals("FILM"))
                        {
                            //Isolating url from html
                            var doc=new HtmlDocument();
                            doc.LoadHtml(textlist.Value);

                            var newMedia = new Media() {Instances = new Collection<MediaInstance>()};

                            foreach (var attr in doc.DocumentNode.Descendants())
                            {
                                if (attr.Name == "iframe")
                                {                                    
                                    if (attr.Attributes["src"] != null)
                                    {
                                        var src = attr.Attributes["src"].Value;                                                                      
                                                                           
                                        if (src.Contains("youtube"))
                                        {
                                            newMedia.MediaType = MediaType.Youtube;
                                            newMedia.EmbeddedUri = src;
                                        }
                                        else if (src.Contains("vimeo"))
                                        {
                                            newMedia.MediaType = MediaType.Vimeo;
                                            newMedia.EmbeddedUri = src;
                                        }

                                        else
                                        {
                                            newMedia.MediaType = MediaType.Other;
                                            newMedia.EmbeddedUri = src;
                                        }
                                       
                                    }//end if src                                    
                                }//end if-iframe                            
                            }//end foreach attr           
                            
                            newProdInfo.Medias.Add(newMedia);
                        }//end if value.Equals("FILM")                        
                    
                    }//End of foreach textlist                

                }//End of if textlist!=null

                //Media               
                if (product.Element("mediaList")!=null)
                {
                    foreach (var media in product.Element("mediaList").Descendants("media"))
                    {
                        var newMedia = new Media() {Instances = new Collection<MediaInstance>() };
                        newMedia.Instances = new Collection<MediaInstance>();

                        //Different types of imagefiles
                        if (media.Attribute("mime").Value.Equals("image/jpg") ||
                            media.Attribute("mime").Value.Equals("image/jpeg") ||
                            media.Attribute("mime").Value.Equals("image/pjpeg") ||
                            media.Attribute("mime").Value.Equals("image/bmp") ||
                            media.Attribute("mime").Value.Equals("image/x-png") ||
                            media.Attribute("mime").Value.Equals("image/gif")
                            )
                        {
                            //mediatype image
                            newMedia.MediaType = MediaType.Image;

                            //Name
                            var xNameElement = media.Element("name");
                            if (xNameElement != null)
                            {
                                newMedia.Name = xNameElement.Value;                                
                            }

                            //Description
                            var xDescriptionElement = media.Element("description");
                            if (xDescriptionElement != null)
                            {
                                newMedia.Description = xDescriptionElement.Value;                                
                            }

                            //Copyright
                            var xMediaElement = media.Element("copyright");
                            if (xMediaElement != null)
                            {
                                newMedia.Copyright = xMediaElement.Value;
                            }

                            var xThumbnail = media.Element("thumbnail");
                            if (xThumbnail != null)
                            {
                                var thumb = new MediaInstance {Size = MediaSize.Thumbnail};

                                if (xThumbnail.Attribute("width") != null)
                                {
                                    try
                                    {
                                        thumb.Width = int.Parse(xThumbnail.Attribute("width").Value);
                                    }
                                    catch (FormatException fe)
                                    {
                                        Logger.LogException(fe);
                                    }
                                } //end if width

                                if (xThumbnail.Attribute("height") != null)
                                {
                                    try
                                    {
                                        thumb.Height = int.Parse(xThumbnail.Attribute("height").Value);
                                    }
                                    catch (FormatException fe)
                                    {
                                        Logger.LogException(fe);
                                    }
                                } //end if height

                                var xThumbUri = xThumbnail.Attribute("URI").Value;
                                if (xThumbUri != null)
                                {
                                    thumb.Uri = xThumbUri;
                                }
                                newMedia.Instances.Add(thumb);
                            } //End if thumbnail

                            var xSmall = media.Element("small");
                            if (xSmall != null)
                            {
                                var small = new MediaInstance {Size = MediaSize.Small};

                                if (xSmall.Attribute("width") != null)
                                {
                                    try
                                    {
                                        small.Width = int.Parse(xSmall.Attribute("width").Value);
                                    }
                                    catch (FormatException fe)
                                    {
                                        Logger.LogException(fe);
                                    }
                                } //End if width

                                if (xSmall.Attribute("height") != null)
                                {
                                    try
                                    {
                                        small.Height = int.Parse(xSmall.Attribute("height").Value);
                                    }
                                    catch (FormatException fe)
                                    {
                                        Logger.LogException(fe);
                                    }
                                } //End if height

                                var xSmallUri = xSmall.Attribute("URI").Value;
                                if (xSmallUri != null)
                                {
                                    small.Uri = xSmallUri;
                                }
                                newMedia.Instances.Add(small);
                            } //End if small

                            var xLarge = media.Element("large");
                            if (xLarge != null)
                            {
                                var large = new MediaInstance {Size = MediaSize.Large};

                                if (xLarge.Attribute("width") != null)
                                {
                                    try
                                    {
                                        large.Width = int.Parse(xLarge.Attribute("width").Value);
                                    }
                                    catch (FormatException fe)
                                    {
                                        Logger.LogException(fe);
                                    }
                                } //end if widht

                                if (xLarge.Attribute("height") != null)
                                {
                                    try
                                    {
                                        large.Height = int.Parse(xLarge.Attribute("height").Value);
                                    }
                                    catch (FormatException fe)
                                    {
                                        Logger.LogException(fe);
                                    }
                                } //end if height

                                var xLargeUri = xLarge.Attribute("URI").Value;
                                if (xLargeUri != null)
                                {
                                    large.Uri = xLargeUri;
                                }
                                newMedia.Instances.Add(large);
                            } //end if large
                        } //end if mime="image/..."
                          
                        //Only pictures are provided in media-tag, youtube, vimeo etc are under textlist                        
                        else
                        {
                            //Mediatype other
                            newMedia.MediaType = MediaType.Other;                            

                            //Name
                            var xNameElement = media.Element("name");
                            if (xNameElement != null)
                            {
                                newMedia.Name = xNameElement.Value;
                            }

                            //Description
                            var xDescriptionElement = media.Element("description");
                            if (xDescriptionElement != null)
                            {
                                newMedia.Description = xDescriptionElement.Value;
                            }

                            //RAW - mediafiles will have a raw-element referencing to the media file
                            var xRawElement = media.Element("raw");
                            if (xRawElement != null)
                            {
                                var xUriAttribute = xRawElement.Attribute("URI");
                                if (xUriAttribute != null)
                                {
                                    newMedia.EmbeddedUri = xUriAttribute.Value;
                                }
                            }//End if raw
                        }//End else     
                        
                        newProdInfo.Medias.Add(newMedia);    

                    }//end foreach media
                } //end if medialist

                //Language-dependent facilities (straight  from tellus)
                foreach (var facilitycategory in product.Descendants("facilityCategoryList").Descendants("facilityCategory"))
                {
                    var facat = new FacilityCategory()
                    {
                        Id = int.Parse(facilitycategory.Attribute("id").Value),
                        FacilityName = facilitycategory.Element("name").Value,
                        List = new List<Facility>()
                    };
                    if (!newProdInfo.Facilities.Contains(facat, new FComparer()))
                        newProdInfo.Facilities.Add(facat);
                    else
                        facat = newProdInfo.Facilities.Single(f => f.Id == facat.Id && f.FacilityName == facat.FacilityName);

                    foreach (var facility in facilitycategory.Descendants("facilityList").Descendants("facility"))
                    {
                        var fac = new Facility()
                        {
                            Id = int.Parse(facility.Attribute("id").Value),
                            FacilityName = facility.Element("name").Value
                        };
                       
                        if (facility.Element("comment") != null && !facility.Element("comment").Value.Equals(""))
                        {
                            fac.Value = facility.Element("comment").Value;
                        }
                        facat.List.Add(fac);
                    }
                }             
                prodInfoList.Add(newProdInfo);    
            }//end of foreach product in xDoc.Descendants        

            return prodInfoList;
        }//End of getProductInfo


        //returnere liste over alle deletedproducts
        public Collection<int> DeletedProducts(string s)
        {
            var deletedProductsCollection = new Collection<int>();
            var xDoc = XDocument.Parse(s);            

            foreach (var delProd in xDoc.Descendants("deletedProduct"))
            {
                try
                {
                    var delProdId = int.Parse(delProd.Attribute("id").Value);
                    deletedProductsCollection.Add(delProdId);
                }
                catch (FormatException fe)
                {
                    Logger.LogException(fe);
                }
            }                        
            return deletedProductsCollection;
        }

        

        //------------------------ HELPER METHODS ------------------------//

        //helper for converting created-date and modified-date in product attributes
        //which are formatted like "yyyy-mm-ddThh:mm:ss"
        private DateTime? StringToDateTime(string stringOfDateAndTime)
        {
            var dateandtime = stringOfDateAndTime.Split(new char[]{'T'}); //splits date and time
            var date = dateandtime[0].Split(new char[] {'-'}); //splits date
            var time = dateandtime[1].Split(new char[] {':'}); //splits time
                                   
            try
            {
                var year = int.Parse(date[0]);
                var month = int.Parse(date[1]);
                var day = int.Parse(date[2]);
                var hour = int.Parse(time[0]);
                var minute = int.Parse(time[1]);
                var second = int.Parse(time[2]);                
                var newdatetime = new DateTime(year, month, day, hour, minute, second);
                return newdatetime;

            }
            catch (FormatException fe)
            {
                Logger.LogException(fe);
                return null;
            }            
        }        

        //combine product, owner and localorg
        private static void SaveOwnerAndLocalOrg(Product p, Owner o, LocalOrg l)
        {
            p.Owner = o;
            p.LocalOrg = l;            
        }


        // ---------------- COMPARERS ------------------- //

        //comparator for categories, compares ids and names
        private class IdAndNameComparer : IEqualityComparer<Category>
        {
            public bool Equals(Category x, Category y)
            {
                return (x.Id == y.Id && x.CategoryName.Equals(y.CategoryName));
            }

            public int GetHashCode(Category obj) //do not use this
            {
                throw new NotImplementedException();
            }
        }

        //comparator for facilities, compares ids and names
        public class FComparer : IEqualityComparer<Facility>
        {
            public bool Equals(Facility x, Facility y)
            {
                return (x.Id == y.Id && x.FacilityName.Equals(y.FacilityName));
            }

            public int GetHashCode(Facility obj) //do not use this
            {
                throw new NotImplementedException();
            }
        }

        //compares two owners by their external id & their localorgs external id
        public class OwnerComparer : IEqualityComparer<Owner>
        {
            public bool Equals(Owner x, Owner y)
            {
                return (x.ExternalId == y.ExternalId);
            }

            public int GetHashCode(Owner obj)
            {
                throw new NotImplementedException();
            }
        }

        //compares two localorgs by their external id
        public class LocalOrgComparer : IEqualityComparer<LocalOrg>
        {
            public bool Equals(LocalOrg x, LocalOrg y)
            {
                return (x.ExternalId == y.ExternalId);
            }

            public int GetHashCode(LocalOrg obj)
            {
                throw new NotImplementedException();
            }
        }

        //compares two products by their external id
        public class ProductExIdComparer : IEqualityComparer<Product>
        {
            public bool Equals(Product x, Product y)
            {
                return x.ExternalId == y.ExternalId;
            }

            public int GetHashCode(Product obj)
            {
                throw new NotImplementedException();
            }
        }
    }
}
