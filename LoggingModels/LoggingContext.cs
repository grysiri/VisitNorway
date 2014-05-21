using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LoggingModels
{
    public class LoggingContext : DbContext
    {

        public DbSet<ExecutedUpdate> ExecutedUpdates { get; set; }
        //public DbSet<AddedProduct> AddedProducts { get; set; }
        public DbSet<AddedProductInfo> AddedProductinfos { get; set; }
        public DbSet<UpdatedProductInfo> UpdatedProductInfos { get; set; }
        public DbSet<DeletedProduct> DeletedProducts { get; set; }

        public LoggingContext() : base("name=VNLogging")
        {
            Database.CreateIfNotExists();
        }
    }
}
