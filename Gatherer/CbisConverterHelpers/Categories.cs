using System;
using System.Linq;
using DomainModels.Domain;
using SaveToDb;

namespace Gatherer.CbisConverterHelpers
{
    class Categories
    {

        public static Category ConvertCategory(int exCatId)
        {
            ExceptionLogger logger = new ExceptionLogger();
            //IMPORTANT: This conversion is based on the testing database given from CBIS. The real list of Categories might be much 
            //richer, but we have yet not been granted access to it. http://puu.sh/7hoo1.png 

            try
            {
                switch (exCatId)
                {
                    case 25157:  //"Boende"  
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Overnatting);
                    case 25158: //"Hotel"
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Hotell);
                    case 25159: //"Hytter"
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Hytter);
                    case 25160: //"Se & Göra"
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Aktiviteter);
                    case 25161: //"Evenemang"
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Arrangementer);
                    default:
                        return null;
                }
            }
            catch (Exception e)
            {
                logger.LogException(e);
                return null;
            }
           
            
        }

    }
}
