using System;
using System.ComponentModel.DataAnnotations;


namespace LoggingModels
{

    //Model for logging updates, deletions etc

    public class ExecutedUpdate
    {
        [Key]public int UpdateId { get; set; }
        public DateTime Date { get; set; }
        public UpdateType Type { get; set; }
        public String ResponseTime { get; set; }
    }

    public abstract class ProductUpdate
    {
        [Key]public int ProdUpdateId { get; set; }
        public DateTime Date { get; set; }
    }

    public class AddedProduct : ProductUpdate
    {
        public int Product { get; set; }
    }

    public class AddedProductInfo : ProductUpdate
    {
        public int PInfo { get; set; }
        public int Lang { get; set; }
    }

    public class UpdatedProductInfo : ProductUpdate
    // If one product is updated, so is all it's dependent productinfos. 
    {
        public int PInfo { get; set; }
        public int Lang { get; set; }
    }

    public class DeletedProduct : ProductUpdate
    //If a Product is deleted, so is all it's dependent productinfos.
    {
        public int ExternalId { get; set; } //here we want to keep track of the external ID from Provider
        public int Provider { get; set; } //... and the provider.
    }

    public enum UpdateType
    {
        MonthlyReset=0,
        WeeklyUpdate=1,
        DailyUpdate=2
    }
}
