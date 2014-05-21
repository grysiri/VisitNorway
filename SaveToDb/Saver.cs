using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using DomainModels.Domain;
using LoggingModels;

namespace SaveToDb
{
    public class Saver
    {
        public Logger Dblogger;

        public Saver()
        {
            Dblogger = new Logger();
            using (var db = new ProductContext())
            {
                try
                {
                    if (!db.Categories.Any())
                        //Adds the static set of categories if table is empty
                    {
                        foreach (var c in DomainCategories.Categories)
                        db.Categories.Add(c);
                        db.SaveChanges();
                    }

                    //Adds postal tables if empty, from static scripts. 
                    if (!db.Counties.Any())
                    {
                        var cfile = new FileInfo("countiesQuery.sql");
                        string cscript = cfile.OpenText().ReadToEnd();
                        db.Database.ExecuteSqlCommand(cscript);
                        db.SaveChanges();
                    }

                    if (!db.Municipalities.Any())
                    {
                        var munifile = new FileInfo("muniQuery.sql");
                        string muniscript = munifile.OpenText().ReadToEnd();
                        db.Database.ExecuteSqlCommand(muniscript);
                        db.SaveChanges();
                    }

                    if (!db.Postalareas.Any())
                    {
                        var pfile = new FileInfo("postareaQuery.sql");
                        string pscript = pfile.OpenText().ReadToEnd();
                        db.Database.ExecuteSqlCommand(pscript);
                        db.SaveChanges();
                    }

                    if (!db.Zipcodes.Any())
                    {

                        var zfile = new FileInfo("zipQuery.sql");
                        string zscript = zfile.OpenText().ReadToEnd();
                        db.Database.ExecuteSqlCommand(zscript);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Dblogger.ExceptionLogger.LogException(e);
                }
            }

        } //end constr


        public void Save(List<Product> allprods)
        {
            var addedProductList = new List<Product>();
            try
            {
                foreach (var newprod in allprods)
                {
                    //check if exists
                    using (var db = new ProductContext())
                    {
                        if (
                            db.Products.Any(
                                p =>
                                    p.ExternalId == newprod.ExternalId &&
                                    p.ExternalProvider == newprod.ExternalProvider))
                        {
                            var prodFromDb =
                                db.Products.Single(
                                    p =>
                                        p.ExternalId == newprod.ExternalId &&
                                        p.ExternalProvider == newprod.ExternalProvider);
                            //check if needs to be updated
                            if (newprod.ModifiedDate > prodFromDb.ModifiedDate)
                            {
                                UpdateExistingProduct(db, newprod, prodFromDb);
                            }
                        }
                        else
                        {
                            addedProductList.Add(SaveNewProduct(db, newprod));
                        }
                        db.SaveChanges();
                    }
                }
                LogNewProductInfos(addedProductList);
                    
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        } //end save


        private Product SaveNewProduct(ProductContext db, Product newprod)
        {
            //localorg, owner & postal from db if already there
            UpdatePostalLocalOrgAndOwner(db, newprod);

            //set categories from db
            var allcategories = db.Categories.ToList();
            var thesecategories = newprod.Categories.ToList();
            newprod.Categories = new Collection<Category>();
            foreach (var cat in thesecategories)
                newprod.Categories.Add(allcategories.Find(c => c.CategoryName.Equals(cat.CategoryName)));

            db.Products.Add(newprod);
            db.SaveChanges();
            return newprod;
        }

        private void UpdateExistingProduct(ProductContext db, Product newprod, Product prodFromDb)
        {
            Console.WriteLine(prodFromDb.Id + " \t" + DateTime.Now);
            newprod.Id = prodFromDb.Id;
            db.Entry(prodFromDb).CurrentValues.SetValues(newprod);

            //delete and create new objects:
            var tobedeleted = prodFromDb.Thirdparties.ToList();
            foreach (var thirdparty in tobedeleted)
                db.Thirdparties.Remove(thirdparty);
            foreach (var thirdparty in newprod.Thirdparties)
                prodFromDb.Thirdparties.Add(thirdparty);

            UpdatePostalLocalOrgAndOwner(db, newprod);
            prodFromDb.LocalOrg = newprod.LocalOrg;
            prodFromDb.Owner = newprod.Owner;
            //postal:
            if (newprod.Zipcode != null)
            {
                prodFromDb.Zipcode = newprod.Zipcode;
                var pa = newprod.Zipcode.Postalarea;
                prodFromDb.Zipcode.Postalarea = pa;
            }            

            //openingtime:
            var otobedeleted = prodFromDb.OpeningTimes.ToList();
            foreach (var o in otobedeleted)
                db.OpeningTimes.Remove(o);
            foreach (var o in newprod.OpeningTimes)
                prodFromDb.OpeningTimes.Add(o);
            //specialopening:
            var stobedeleted = prodFromDb.SpecialOpenings.ToList();
            foreach (var s in stobedeleted)
                db.SpecialOpenings.Remove(s);
            prodFromDb.SpecialOpenings = new Collection<SpecialOpening>();
            foreach (var s in newprod.SpecialOpenings)
                prodFromDb.SpecialOpenings.Add(s);
            //categories:
            prodFromDb.Categories = new Collection<Category>();
            var allcategories = db.Categories.ToList();
            foreach (var cat in newprod.Categories)
                prodFromDb.Categories.Add(allcategories.Find(c => c.CategoryName.Equals(cat.CategoryName)));

            //prodinfos:
            var prodinfos = prodFromDb.ProductInfos.ToList();
            foreach (var pinfo in prodinfos)
            {
                //medias:
                var mtobedeleted = pinfo.Medias.ToList();
                foreach (var media in mtobedeleted)
                {
                    var itobedeleted = media.Instances.ToList();
                    foreach (var instance in itobedeleted)
                        db.MediaInstances.Remove(instance);
                    db.Medias.Remove(media);
                }
                //facilities:
                var factobedeleted = pinfo.Facilities.ToList();
                foreach (var fac in factobedeleted)
                {
                    var underfactobedeleted = fac.List.ToList();
                    foreach (var ufac in underfactobedeleted)
                        db.Facilities.Remove(ufac);
                    db.Facilities.Remove(fac);
                }
                prodFromDb.ProductInfos.Remove(pinfo);
                db.ProductInfos.Remove(pinfo);
            }
            foreach (var productInfo in newprod.ProductInfos)
            {
                productInfo.Product = prodFromDb;
                prodFromDb.ProductInfos.Add(productInfo);
                Dblogger.LoggUpdatedProductInfo(new UpdatedProductInfo()
                {
                    Date = DateTime.Now,
                    Lang = (int)productInfo.Language,
                    PInfo = (int)prodFromDb.Id
                });
            }
            db.SaveChanges();
        }



        private void UpdatePostalLocalOrgAndOwner(ProductContext db, Product newprod)
        {           
            if (db.Zipcodes.Any())
            {               
                var zip = (from z in db.Zipcodes.Include("Postalarea.Municipality.County")                          
                           where z.Zipcode.Equals(newprod.PostalCode)
                           select z)
                          .FirstOrDefault();

                if (zip!=null && zip.Postalarea!=null && zip.Postalarea.Municipality!=null && zip.Postalarea.Municipality.County!=null)
                {
                    newprod.Zipcode = zip;
                }
            }

            if (db.LocalOrgs.Any() && newprod.LocalOrg != null)
            {
                var localorg = db.LocalOrgs.FirstOrDefault( l => l.ExternalId == newprod.LocalOrg.ExternalId && l.Provider == newprod.LocalOrg.Provider);
                if (localorg != null)
                    newprod.LocalOrg = localorg;
            }

            if (db.Owners.Any() && newprod.Owner != null)
            {
                var owner =
                    db.Owners.FirstOrDefault(
                        l => l.ExternalId == newprod.Owner.ExternalId && l.Provider == newprod.Owner.Provider);
                if (owner != null)
                    newprod.Owner = owner;
            }
        }

        public void LogNewProductInfos(List<Product> addedProductList)
        {
            using (var db = new ProductContext())
            {
                var allProdInfosInDb = db.ProductInfos.ToList();
                foreach (var addedProduct in addedProductList)
                {
                    foreach (var addedProductInfo in addedProduct.ProductInfos)
                    {
                        var prodInfo =
                            allProdInfosInDb.Single(
                                pi => pi.Product.ExternalId == addedProductInfo.Product.ExternalId &&
                                      pi.Language == addedProductInfo.Language);
                        var addedPi = new AddedProductInfo()
                        {
                            Date = DateTime.Now,
                            Lang = (int) prodInfo.Language,
                            PInfo = (int) prodInfo.Id
                        };
                        Dblogger.LoggAddedProductInfo(addedPi);
                    }
                }
                db.SaveChanges();
            }
        }

        public void UpdateAll(List<Product> all)
        {
            Dblogger = new Logger();
            var addedProductList = new List<Product>();

            try
            {
                foreach (var newprod in all)
                {
                    //if exists
                    using (var db = new ProductContext())
                    {
                        var prodFromDb = db.Products.FirstOrDefault(p =>
                            p.ExternalId == newprod.ExternalId &&
                            p.ExternalProvider == newprod.ExternalProvider);
                        if (prodFromDb != null)
                            UpdateExistingProduct(db, newprod, prodFromDb);
                        else
                            addedProductList.Add(SaveNewProduct(db, newprod));
                    }
                }
            }
            catch
                (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }

            if (addedProductList.Any())
                LogNewProductInfos(addedProductList);
        }

        public class ProdinfoComparer : IEqualityComparer<ProductInfo>
        {

            public bool Equals(ProductInfo x, ProductInfo y)
            {
                return (x.Product.ExternalId == y.Product.ExternalId &&
                        x.Product.ExternalProvider == y.Product.ExternalProvider);
            }

            public int GetHashCode(ProductInfo obj)
            {
                throw new NotImplementedException();
            }
        }
    }
}
