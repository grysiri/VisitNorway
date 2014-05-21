using System;
using System.Collections.Generic;
using System.Linq;
using DomainModels.Domain.Enums;
using DomainModels.Domain;
using SaveToDb;

namespace Gatherer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            new Start();
        }
    }

    public class Start
    {
        private List<Product> _allProducts;
        ExceptionLogger _logger = new ExceptionLogger();
        private ConsoleKeyInfo v;
        public Start()
        {
            var cCol = new CbisCollector();
            var tCol = new TellusCollector();         
            var dblogger = new Logger();
            _allProducts = new List<Product>();
            bool timeToRunReset = true; //set to true just in case

            Console.SetBufferSize(100, 9999);
            Console.WriteLine("\n ** WELCOME **\n");

            var lastreset = dblogger.LastReset();
            if (lastreset != null)
            {
                Console.WriteLine("\n------------------------------\n");
                Console.WriteLine("Last reset: " + lastreset);
                var time = DateTime.Now - (DateTime) lastreset;
                timeToRunReset = CheckIfTimeToRunReset(time.TotalDays);
                if (CheckIfTimeToRunReset(time.TotalDays))
                    Console.WriteLine("Days since last reset: " + time.TotalDays + ", it's time to do a new monthly reset!");
                else if (time.TotalDays>1)
                    Console.WriteLine("Days since last reset: " + time.TotalDays);
                else
                    Console.WriteLine("Days since last reset: " + time.TotalHours);
                
            }

            


   /* Uncomment the following to allow the user to see an overview of updates */

         //Console.WriteLine("\nWould you like to see an overview of the lastest updates?");
         //    Console.WriteLine("y/n");
         //    v = Console.ReadKey();
         //    if (v.KeyChar.ToString().Equals("y"))
         //    {
         //        Console.WriteLine("------------------------------\n");
         //        dblogger.WriteLoggs();
         //    }


  /* Uncomment the following AND comment out "Ask about auto update" 
   * to make the program automatically decide if it should run daily update or reset */

            //if (timeToRunReset)
            //    Updater.MonthlyReset();
            //else
            //    Updater.DailyUpdate();
             


     /*  ASK ABOUT AUTO UPDATE */

            Console.WriteLine("Would you like to run an automatic update?");
            Console.WriteLine("y/n");
            v = Console.ReadKey();
            if (v.KeyChar.ToString().Equals("y"))
            {
                Console.WriteLine("\nChoose type: Full reset (r) / Update (u)");
                v = Console.ReadKey();
                if (v.KeyChar.ToString().Equals("r"))
                {
                    var startTime = DateTime.Now;
                    Console.WriteLine("\nStarting reset: " + startTime + "\n (Be patient, this may take a while!)");
                    Updater.MonthlyReset();
                    var endTime = DateTime.Now;
                    var diff = endTime - startTime;
                    Console.WriteLine("Reset finished at: " + endTime + " . This took : " + diff.Hours + " hours, " + diff.Minutes + " minutes and " + diff.Seconds + " seconds.");
                }

                if (v.KeyChar.ToString().Equals("u"))
                {
                    var startTime = DateTime.Now;
                    Console.WriteLine("\nStarting update: " + startTime);
                    Updater.DailyUpdate();
                    var endTime = DateTime.Now;
                    var diff = endTime-startTime;
                    Console.WriteLine("Update finished at: " + endTime + " . This took : " + diff.Hours + " hours, " + diff.Minutes + " minutes and " + diff.Seconds + " seconds." );
                }
            }

            /*  MANUAL UPDATE */

     /* Uncomment the following to allow the user to manage the update manually */
            
            //Console.WriteLine("\n\nWould you like to run a manual update?");
            //Console.WriteLine("y/n");
            //v = Console.ReadKey();
            //if (v.KeyChar.ToString().Equals("y"))
            //{
            //    Console.WriteLine("Would you like to collect from CBIS?");
            //    Console.Write("y/n:");
            //    v = Console.ReadKey();
            //    if (v.KeyChar.ToString().Equals("y"))
            //    {
            //        Console.WriteLine();                    
            //        _allProducts.AddRange(cCol.CbisCollect());

            //        Console.WriteLine("\nThat's it from CBIS! ");
            //    }

            //    Console.WriteLine("\n\nWould you like to collect from Tellus?");
            //    Console.Write("y/n:");
            //    v = Console.ReadKey();
            //    if (v.KeyChar.ToString().Equals("y"))
            //    {
            //        Console.WriteLine();
            //        _allProducts.AddRange(tCol.TellusCollect());
            //        Console.ReadKey();
            //    }

            //    if (_allProducts != null && _allProducts.Any())
            //    {
            //        Console.WriteLine("Number of objects: " + _allProducts.Count);                    
            //    }


            //    Console.WriteLine("\n\nWould you like to collect the deleted productus? ");
            //    Console.WriteLine("y/n");
            //    v = Console.ReadKey();
            //    if (v.KeyChar.ToString().Equals("y"))
            //    {
            //        Console.WriteLine("");

            //        //---deleting from Tellus---

            //        var deletedTellus = tCol.TellusDeleted();
            //        var deleter = new Deleter() {ExternalProvider = ExternalProvider.TellUs};
            //        try
            //        {                        
            //            if (deletedTellus.Any())
            //            {
            //                Console.WriteLine("\nDeleting products from Tellus...");
            //                deleter.Delete(deletedTellus);
            //            }
            //            else
            //            {
            //                Console.WriteLine("\nNothing to delete from Tellus.");
            //            }
            //        }
            //        catch (Exception e)
            //        {
            //            Console.WriteLine("\nDelete-exception!");
            //            _logger.LogException(e);
            //        }

            //        //---deleting from CBIS---

            //        var deletedCbis = cCol.GetInactiveProducts();

            //        var deleter2 = new Deleter() {ExternalProvider = ExternalProvider.CBIS};
            //        try
            //        {
            //            if (deletedCbis != null)
            //            {
            //                Console.WriteLine("\nDeleting products from CBIS...");
            //                deleter2.Delete(deletedCbis);
            //            }
            //            else
            //            {
            //                Console.WriteLine("\nNothing to delete from CBIS");
            //            }
            //        }
            //        catch (Exception e)
            //        {
            //            Console.WriteLine("\nDelete-exception!");
            //            _logger.LogException(e);
            //        }
            //    }

            //    if (_allProducts.Any())
            //    {
            //        var saver = new Saver();

            //        try
            //        {
            //            Console.WriteLine("\nSaving products to database....");
            //            saver.Save(_allProducts);
            //        }
            //        catch (Exception e)
            //        {
            //            Console.WriteLine("\nSaving-exception!");
            //            _logger.LogException(e);
            //        }
            //    }
            //    dblogger.WriteLoggs();               
            //}

            Console.WriteLine("\n\nThat's it! You can close the console now. PRESS ENTER");
            Console.ReadLine();
        }

        private bool CheckIfTimeToRunReset(double p)
        {
            return p >= 30;
        }
    }
}
