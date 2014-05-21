using System;
using System.Linq;
using LoggingModels;

namespace SaveToDb
{
    //For getting the latest responsetime from Tellus
    public static class Response
    {        
        public static string GetLatestResponse()
        {
            var exceptionLogger = new ExceptionLogger();            

            using (var db=new LoggingContext())
            {
                try
                {                    
                    var execUpdates = db.ExecutedUpdates.Where(r => r.UpdateId != 0 && r.Type == UpdateType.DailyUpdate);
                    if (execUpdates.Any())
                    {
                        var time = execUpdates.ToList().Last().ResponseTime;
                        return time;
                    }                                                                
                }
                catch (Exception e)
                {
                    exceptionLogger.LogException(e);
                }                                                
            }
            return null;
        }
    }
}
