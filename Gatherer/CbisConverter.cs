using System;
using System.Collections.ObjectModel;
using System.Linq;
using DomainModels.Domain;
using DomainModels.Domain.Enums;
using Gatherer.CbisConverterHelpers;
using Gatherer.CbisServiceReference;
using Visit.CbisAPI.Categories;
using SearchResultOfProduct = Visit.CbisAPI.Products.SearchResultOfProduct;
using System.Collections.Generic;
using Category = DomainModels.Domain.Category;
using Media = DomainModels.Domain.Media;
using Product = Visit.CbisAPI.Products.Product;
using SaveToDb;
using ProductStatus = Visit.CbisAPI.Products.ProductStatus;


namespace Gatherer
{
    public class CbisConverter
    {
        public ExceptionLogger logger;

        public CbisConverter()
        {
            logger = new ExceptionLogger();
        }

        // ----------------------------------------- ORGANIZATIONS --------------------------------------
        // ---------------LOCAL ORGS-----------------
        public Collection<LocalOrg> GetLocalOrgs(Organization[] orgs)
        {
            try
            {
                var orgList = new Collection<LocalOrg>();               
                foreach (var org in orgs)
                {
                    if (org.OrganizationType == OrganizationType.Operator || org.OrganizationType == OrganizationType.MultiOperator)
                    {
                        var localOrg = new LocalOrg()
                        {
                            Name = org.Name,
                            ExternalId = org.Id,
                            Provider = ExternalProvider.CBIS
                        };
                        orgList.Add(localOrg);                       
                    }
                }                
                return orgList;
            }
            catch (Exception e)
            {
                logger.LogException(e);
            }
            return null;
        }

        // ---------------OWNERS-----------------

        public Collection<Owner> GetOwners(Organization[] orgs)
        {
            try
            {
                var ownerList = new Collection<Owner>();                
                foreach (var org in orgs)
                {
                    if (org.OrganizationType == OrganizationType.ProductOwner)
                    {
                        var owner = new Owner()
                        {
                            Name = org.Name,
                            ExternalId = org.Id,
                            Provider = ExternalProvider.CBIS,
                            Products = new Collection<DomainModels.Domain.Product>()
                        };
                        ownerList.Add(owner);                        
                    }
                }                
                return ownerList;
            }
            catch (Exception e)
            {
                logger.LogException(e);
            }
            return null;
        }


        // ----------------------------------------- 1. PRODUCT -------------------------------------------

        // --------- Checking and collecting
        public IQueryable<DomainModels.Domain.Product> GetProductList
            (SearchResultOfProduct prods,
            Collection<LocalOrg> orgs,
            Collection<Owner> owns,
            IQueryable<DomainModels.Domain.Product> pList,
            Organization[] exOrgs,
            CbisServiceReference.Attribute[] atts,
            List<int> inactives)
        {
            try
            {
                var prodList = new List<DomainModels.Domain.Product>();

                if (pList == null)
                {
                    foreach (var prod in prods.Items)
                    {
                        if (prod.Status!= ProductStatus.Active)
                        {
                            inactives.Add(prod.Id);
                        }
                        else
                        {
                            var newProd = new DomainModels.Domain.Product()
                            {
                                OpeningTimes = new Collection<OpeningTime>(),
                                SpecialOpenings = new Collection<SpecialOpening>(),
                                Categories = new Collection<Category>(),
                                Thirdparties = new Collection<Thirdparty>(),
                                ProductInfos = new Collection<ProductInfo>()
                            };
                            try
                            {
                                newProd = AddOneProduct(prod, orgs, owns, exOrgs, atts);
                            }
                            catch (NullReferenceException e)
                            {
                                logger.LogException(e);
                            }
                            catch (Exception e)
                            {
                                logger.LogException(e);
                            }
                            prodList.Add(newProd);
                        }
                    }
                    return prodList.AsQueryable();
                }

                //if the list has already been filled with products from one language, it will now be checked if there is any additional 
                //products in the other languages that needs to be added.

                prodList.AddRange(pList);                
                foreach (var prod in prods.Items)
                {
                    if (prod.Status != ProductStatus.Active)
                    {                        
                        inactives.Add(prod.Id);
                    }
                    else
                    {
                        var has = pList.Any(cus => cus.ExternalId == prod.Id);
                        if (has) continue;
                        var newProd = AddOneProduct(prod, orgs, owns, exOrgs, atts);
                        prodList.Add(newProd);
                    }
                    
                }
                return prodList.AsQueryable();
            }
            catch (Exception e)
            {
                logger.LogException(e);
            }
            //If productlist is empty, create all accessable products
            return null;
        }


        //-----------Adding one instance of a product
        public DomainModels.Domain.Product AddOneProduct(
            Product prod, 
            Collection<LocalOrg> orgs, 
            Collection<Owner> owns,
            Organization[] exOrgs,
            CbisServiceReference.Attribute[] atts
            )
        {            

            //--------COLLECTING LINKS
            String web = null;
            if (prod.Link1Url != null)
            {
                web = prod.Link1Url;
                if (prod.Link2Url != null)
                    web += ", " + prod.Link2Url;
                if (prod.Link3Url != null)
                    web += ", " + prod.Link3Url;
            }

            //---CHECKING if revisionDate is set
            DateTime modDate;
            if (prod.RevisionDate != null)
                modDate = (DateTime)prod.RevisionDate;
            else if (prod.PublishedDate != null)
                modDate = (DateTime) prod.PublishedDate;
            else modDate = DateTime.Now;
           

            //-------ADDING PRODUCT
            var newProd = new DomainModels.Domain.Product
            {
                ExternalProvider = ExternalProvider.CBIS,
                ExternalId = prod.Id,
                StreetAddress1 = prod.StreetAddress1,
                PostalCode = prod.PostalCode,
                Latitude = Convert.ToString(prod.Latitude),
                Longitude = Convert.ToString(prod.Longitude),
                Website = web,
                CreatedDate = prod.PublishedDate,
                ModifiedDate = modDate,
                OpeningTimes = new Collection<OpeningTime>(),
                SpecialOpenings = new Collection<SpecialOpening>(),
                Categories = new Collection<Category>(),
                Thirdparties = new Collection<Thirdparty>(),
                ProductInfos = new Collection<ProductInfo>()
            };

            // ----- GETTING ORGS/OWNER
            try
            {
                var connectedOrg = orgs.FirstOrDefault(o => o.ExternalId.Equals(prod.OrganizationId));
                newProd.LocalOrg = connectedOrg;
            }
            catch (Exception e)
            { /*There is no connection between organization and product.  */ }

            try
            {
                var connectedOwner = owns.FirstOrDefault(o => o.ExternalId.Equals(prod.OrganizationId));
                newProd.Owner = connectedOwner;
            }
            catch (Exception e)
            { /*There is no connection between organization and product.  */ }
            
            try
            {
                var connectedExOrg = exOrgs.FirstOrDefault(o => o.Id == prod.OrganizationId);
                if (connectedExOrg.Phone != null) newProd.Phone = connectedExOrg.Phone;
                if (connectedExOrg.Email != null) newProd.Email = connectedExOrg.Email;
            }
            catch (Exception e)
            { /*There is no connection between organization and product.  */  }


            //--------COLLECTING CATEGORIES
            if (prod.Categories != null)
            {
                foreach (var cat in prod.Categories)
                {
                    var catId = cat.Id;
                    var category = Categories.ConvertCategory(catId);
                    newProd.Categories.Add(category);
                }
            }
            return newProd;
        }



        // ----------------------------------------- 2. PRODUCTINFO -------------------------------------------

        public IQueryable<ProductInfo> GetProductInfoList(SearchResultOfProduct prods, int lang, IQueryable<DomainModels.Domain.Product> products)
        {
            var prodInfoList = new List<ProductInfo>();
            if (prods!=null)
            foreach (var prod in prods.Items)
            {
                //ADDING PRODUCTINFO
                LanguageCode langCode = Language.ConvertLanguages(lang);
                var newProdInfo = new ProductInfo()
                {
                    Language = langCode,
                    Name = prod.Name,
                    Description = prod.Introduction + prod.Description,
                    Transportation = prod.Directions,
                    Medias = new Collection<Media>(),
                    Facilities = new Collection<FacilityCategory>()
                };

                try
                {
                    newProdInfo.Product = products.Single(p => p.ExternalId == prod.Id);
                }
                catch (NullReferenceException e)
                { logger.LogException(e); }
                catch (Exception e)
                { logger.LogException(e); }

                prodInfoList.Add(newProdInfo);
            }
            return prodInfoList.AsQueryable();
        }



        // ----------------------------------------- 3. ADD MEDIA -----------------------------------------

        public IQueryable<DomainModels.Domain.Media> GetMediaList(SearchResultOfProduct prods, int lang, IQueryable<ProductInfo> pInfos)
        //This method is runned when adding a product. Parameters contain External Product Model, to fetch the media, and the internal ProductInfo (With .Product) that the new Media should be connected to.
        {
            var mediaList = new List<DomainModels.Domain.Media>();
            if (prods.Items!=null)
            foreach (var prod in prods.Items)
            {
               
                    if(prod.Images!=null)
                    foreach (var media in prod.Images)
                    {
                        var newMedia = new DomainModels.Domain.Media()
                        {
                            Description = media.Description,
                            Copyright = media.CopyrightBy,
                            Instances = new Collection<MediaInstance>()
                        };

                        try
                        {
                            var connectedProduct = pInfos.Single(p => p.Product.ExternalId == prod.Id);
                            connectedProduct.Medias.Add(newMedia);
                        }
                        catch (Exception e)
                        { /* There is no related product */ }
                    
                        


                        //----------------- PICTURES ---------------
                        if (media.MediaType.Equals(Visit.CbisAPI.Products.MediaTypes.Image))
                        {
                            //No width/height defined
                            if (media.Width == 0 || media.Height == 0)
                            {
                                var instance = new MediaInstance()
                                {
                                    Uri = media.Url,
                                    Size = MediaSize.Unknown
                                };
                                newMedia.MediaType = MediaType.Image;
                                newMedia.Instances.Add(instance);
                            }

                            //Thumbnails med MediaInstance:
                            else if (media.Width < 150 && media.Height < 100)
                            {
                                var instance = new MediaInstance()
                                {
                                    Uri = media.Url,
                                    Height = media.Height,
                                    Width = media.Width,
                                    Size = MediaSize.Thumbnail
                                };
                                newMedia.MediaType = MediaType.Image;
                                newMedia.Instances.Add(instance);
                            }
                            //Large pictures
                            else if (media.Width > 600 && media.Height > 400)
                            {
                                var instance = new MediaInstance()
                                {
                                    Uri = media.Url,
                                    Height = media.Height,
                                    Width = media.Width,
                                    Size = MediaSize.Small
                                };
                                newMedia.MediaType = MediaType.Image;
                                newMedia.Instances.Add(instance);
                            }
                            //Big pictures
                            else 
                            {
                                var instance = new MediaInstance()
                                {
                                    Uri = media.Url,
                                    Height = media.Height,
                                    Width = media.Width,
                                    Size = MediaSize.Large
                                };
                                newMedia.MediaType = MediaType.Image;
                                newMedia.Instances.Add(instance);
                            }
                        }


                        //----------------- YOUTUBE ---------------
                        else if (media.MediaType.Equals(MediaTypes.Youtube))
                        {
                            newMedia.MediaType = MediaType.Youtube;
                            newMedia.EmbeddedUri = media.Url;
                        }

                        //----------------- OTHER ---------------
                        else
                        {
                            newMedia.MediaType = MediaType.Other;
                            newMedia.EmbeddedUri = media.Url;
                        }
                        mediaList.Add(newMedia);
                    
                    }
                }
               
           

            return mediaList.AsQueryable();
        }


        // ----------------------------------------- CATEGORIES -------------------------------------------
        // this method doesn't actually do much, but can be used to list the available categories

        public void GetCategories(TreeOfCategory cats)
        {
            //Console.WriteLine("INCOMING CATEGORY SET");
            if (cats!=null)
            foreach (var cat in cats.Nodes)
            {
                var parent = cat.Data.ParentCategoryId;
                //Console.WriteLine(cat.Data.Id + "  -  " + cat.Data.Name + "\tParent: " + parent + "\tStatus: " + cat.Data.Status);
                foreach (var child in cat.Children)
                {
                    //Console.WriteLine("  " + child.Data.Id + "   -   " + child.Data.Name + "\tParent: " + child.Data.ParentCategoryId + "\tStatus: " + child.Data.Status);
                    foreach (var grandchild in child.Children)
                    {
                        //Console.WriteLine("    " + grandchild.Data.Id + "   -   " + grandchild.Data.Name + "\tParent: " + grandchild.Data.ParentCategoryId + "\tStatus: " + grandchild.Data.Status);
                    }
                }
            }
        }
    }
}
