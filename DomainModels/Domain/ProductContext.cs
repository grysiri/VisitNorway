using System.Data.Entity;

namespace DomainModels.Domain
{
    public class ProductContext : DbContext
    {

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInfo> ProductInfos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<FacilityCategory> FacilityCategories { get; set; }
        public DbSet<LocalOrg> LocalOrgs { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<MediaInstance> MediaInstances { get; set; }
        public DbSet<OpeningTime> OpeningTimes { get; set; }
        public DbSet<Owner> Owners { get; set; }        
        public DbSet<SpecialOpening> SpecialOpenings { get; set; }
        public DbSet<Thirdparty> Thirdparties { get; set; }
       

        public DbSet<County> Counties { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<Postalarea> Postalareas { get; set; }
        public DbSet<Zipcodes> Zipcodes { get; set; }

        public ProductContext()
            : base("name=VNProduct")
        {
            Database.CreateIfNotExists();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
    