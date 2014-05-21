using System;
using System.Collections.ObjectModel;
using System.Linq;
using DomainModels.Domain;
using DomainModels.Domain.Enums;
using LoggingModels;

namespace SaveToDb
{
    public class Deleter
    {
        public ExternalProvider ExternalProvider { get; set; }

        public void Delete(Collection<int> deletedProducts)
        {
            var exceptionLogger = new ExceptionLogger();
            var dbLogger = new Logger();

            using (var db=new ProductContext())
            {
                var allProds = db.Products.ToList();
                foreach (var deletedProduct in deletedProducts)
                {
                    try
                    {                        
                        var productsInDb = allProds.FindAll(p => p.ExternalId == deletedProduct && p.ExternalProvider == this.ExternalProvider);

                        foreach (var productToDelete in productsInDb)
                        {
                            if (productToDelete!=null)
                            {                                
                                var productDeleted = new DeletedProduct
                                {
                                    ExternalId = productToDelete.ExternalId, 
                                    Provider = (int)productToDelete.ExternalProvider,
                                    Date = DateTime.Now
                                };                                
                                    
                                //ProductInfo
                                var prodinfos= db.ProductInfos.ToList();
                                foreach (var prodinfo in prodinfos)
                                {
                                    if (prodinfo.Product.ExternalId == productToDelete.ExternalId &&
                                        prodinfo.Product.ExternalProvider == productToDelete.ExternalProvider)
                                    {
                                        //Media && MediaInstance
                                        var mediaList = prodinfo.Medias.ToList();
                                        foreach (var media in mediaList)
                                        {
                                            var mediaInstancesToDelete = media.Instances.ToList();
                                            foreach (var instanceToDelete in mediaInstancesToDelete)
                                            {
                                                db.MediaInstances.Remove(instanceToDelete);
                                            }
                                            db.Medias.Remove(media);
                                        }

                                        //Facilities
                                        var facilitiesList = prodinfo.Facilities.ToList();
                                        foreach (var facility in facilitiesList)
                                        {
                                            var facilitiesListToDelete = facility.List.ToList();
                                            foreach (var facilityToDelete in facilitiesListToDelete)
                                            {
                                                db.Facilities.Remove(facilityToDelete);
                                            }
                                            db.Facilities.Remove(facility);
                                        }
                                        db.ProductInfos.Remove(prodinfo);


                                    } //end if prodinfo
                                }

                                //Openingtimes
                                if (productToDelete.OpeningTimes!=null)
                                {
                                    var openingtimesToDelete = productToDelete.OpeningTimes.ToList();
                                    foreach (var openingTime in openingtimesToDelete)
                                    {
                                        db.OpeningTimes.Remove(openingTime);
                                    }   
                                }                        

                                //SpecialOpening
                                if (productToDelete.SpecialOpenings!=null)
                                {
                                    var specialopeningtimesToDelete = productToDelete.SpecialOpenings.ToList();
                                    foreach (var specialOpening in specialopeningtimesToDelete)
                                    {
                                        db.SpecialOpenings.Remove(specialOpening);
                                    }
                                }

                                //thirdparties
                                if (productToDelete.Thirdparties!=null)
                                {
                                    var thirdpartiesToDelete = productToDelete.Thirdparties.ToList();
                                    foreach (var thirdparty in thirdpartiesToDelete)
                                    {
                                        db.Thirdparties.Remove(thirdparty);
                                    }
                                }

                                db.Products.Remove(productToDelete);                                                                                

                                dbLogger.LoggDeletedProducts(productDeleted);
                                

                                db.SaveChanges(); 
                                }      
                            } //-end foreach productToDelete
                    }
                                            
                    catch (Exception e)
                    {                        
                        exceptionLogger.LogException(e);
                    }
                    
                }
            }
        }
    }
}
