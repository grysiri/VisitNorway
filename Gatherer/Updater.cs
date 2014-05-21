using System;
using System.Collections.Generic;
using System.Linq;
using DomainModels.Domain;
using DomainModels.Domain.Enums;
using LoggingModels;
using SaveToDb;

namespace Gatherer
{
    public static class Updater
    {      
        public static void MonthlyReset()
        {
            var _logger = new ExceptionLogger();
            var saver = new Saver();
            var dblogger = new Logger();
            var cCol = new CbisCollector();
            var tCol = new TellusCollector();
            var _alt = new List<Product>();
            try
            {
                _alt.AddRange(cCol.CbisCollect());
                _alt.AddRange(tCol.TellusCollect());
            }
            catch (ArgumentNullException ae)
            {
                _logger.LogException(ae);
            }


            try
            {
                saver.UpdateAll(_alt);
                var responseTime = tCol.GetResponseTime();
            
                var update = new ExecutedUpdate() {Date = DateTime.Now, Type = UpdateType.MonthlyReset, ResponseTime = responseTime};
                dblogger.LoggExecutedUpdates(update);
            }
            catch (Exception e){_logger.LogException(e);}   
            
        }

        public static void DailyUpdate()
        {
            var _logger = new ExceptionLogger();
            var saver = new Saver();
            var dblogger = new Logger();
            var cCol = new CbisCollector();
            var tCol = new TellusCollector();
            var _alt = new List<Product>();
            _alt.AddRange(cCol.CbisCollect());
            _alt.AddRange(tCol.TellusCollect());

            var deletedTellus = tCol.TellusDeleted();
            var deleter = new Deleter() { ExternalProvider = ExternalProvider.TellUs };
            try
            {
                if (deletedTellus.Any())
                { deleter.Delete(deletedTellus); }
            }
            catch (Exception e)
            { _logger.LogException(e); }
            var deletedCbis = cCol.GetInactiveProducts();            
            var deleter2 = new Deleter() { ExternalProvider = ExternalProvider.CBIS };
            try
            {
                if (deletedCbis != null)
                {deleter2.Delete(deletedCbis);}
            }
            catch (Exception e)
            {_logger.LogException(e);}


            try
            {saver.Save(_alt);}
            catch (Exception e)
            {_logger.LogException(e);}

            var responseTime = tCol.GetResponseTime();
            var update = new ExecutedUpdate() {Date = DateTime.Now, Type = UpdateType.DailyUpdate, ResponseTime = responseTime};
            dblogger.LoggExecutedUpdates(update);
        }
    }
}
