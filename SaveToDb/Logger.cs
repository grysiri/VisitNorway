using System;
using System.Linq;
using LoggingModels;


namespace SaveToDb
{
    public class Logger
    {
        public ExceptionLogger ExceptionLogger;        

        public Logger()
        { ExceptionLogger = new ExceptionLogger();}
        
        //--------------------------------  READ LOGS  ----------------------------------

        public void WriteLoggs()
        {
            using (var db = new LoggingContext())
            {
                var updateList = db.ExecutedUpdates.ToList();
                
                try
                {
                    if (updateList.Any())
                    {
                        // -   WRITING ALL EXECUTED UPDATES
                        Console.WriteLine("EXECUTED UPDATES");
                        Console.WriteLine("-----------------------------------------------------");
                        foreach (var update in updateList)
                        {
                            var updateDay = update.Date.Day;
                            var updateMonth = update.Date.Month;
                            var updateYear = update.Date.Year;
                            Console.WriteLine(update.Date + " - " + update.Type);
                            
                            //Writing a list of numbers (how many addings, updates, deletions)
                            var addedPi = db.AddedProductinfos.Count(p => p.Date.Day.Equals(updateDay)&&p.Date.Month.Equals(updateMonth)&&p.Date.Year.Equals(updateYear));
                            if (addedPi > 0) Console.WriteLine("\tAdded productinfos: \t" + addedPi);
                            var updatedP = db.UpdatedProductInfos.Count(p => p.Date.Day.Equals(updateDay) && p.Date.Month.Equals(updateMonth) && p.Date.Year.Equals(updateYear));
                            if (updatedP > 0) Console.WriteLine("\tUpdated products: \t" + updatedP);
                            var deletedP = db.DeletedProducts.Count(p => p.Date.Day.Equals(updateDay) && p.Date.Month.Equals(updateMonth) && p.Date.Year.Equals(updateYear));
                            if (deletedP > 0) Console.WriteLine("\tDeleted products: \t" + deletedP);
                        }
                        Console.WriteLine("\n\n");
                    }
                    else Console.WriteLine("No updates has been executed.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public DateTime? LastReset()
        {
            using (var db = new LoggingContext())
            {
                var resetlist = db.ExecutedUpdates.Where(u => u.Type == UpdateType.MonthlyReset).ToList();
                if (resetlist.Any())
                    return resetlist.Last().Date;
            }
            return null;
        }


        //--------------------------------  ADD LOGGS  ----------------------------------

        public void LoggAddedProductInfo(AddedProductInfo pinfo) 
        {

            using (var db = new LoggingContext())
            {
                try
                { db.AddedProductinfos.Add(pinfo); db.SaveChanges(); }
                catch (Exception e) { ExceptionLogger.LogException(e); }
            }
        }


        public void LoggUpdatedProductInfo(UpdatedProductInfo p) 
        {            
            using (var db = new LoggingContext())
            {
                try
                { db.UpdatedProductInfos.Add(p); db.SaveChanges(); }
                catch (Exception e) { ExceptionLogger.LogException(e); }
            }
        }


        public void LoggDeletedProducts(DeletedProduct p)
        {           
            using (var db = new LoggingContext())
            {
                try
                { db.DeletedProducts.Add(p); db.SaveChanges(); }
                catch (Exception e) { ExceptionLogger.LogException(e); }
            }
        }

        public void LoggExecutedUpdates(ExecutedUpdate e)
        {         
            using (var db=new LoggingContext())
            {
                try
                {
                    db.ExecutedUpdates.Add(e);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {                    
                    ExceptionLogger.LogException(ex);
                }
            }
        }
    }
}
