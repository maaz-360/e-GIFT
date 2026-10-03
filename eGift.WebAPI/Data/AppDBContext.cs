using eGift.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace eGift.WebAPI.Data
{
    public class AppDBContext : DbContext
    {
        #region Constructors
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        #endregion

        #region DbSet Properties
        public DbSet<AddressModel> Addresses { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<CityModel> Cities { get; set; }
        public DbSet<CountryModel> Countries { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<GenderModel> Genders { get; set; }
        public DbSet<LoginModel> Logins { get; set; }
        public DbSet<OrderDetailsModel> OrderDetails { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<StateModel> States { get; set; }
        public DbSet<SubCategoryModel> SubCategories { get; set; }
        public DbSet<MyCartModel> MyCarts { get; set; }
        #endregion

        #region On Model Creating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product
            modelBuilder.Entity<ProductModel>()
                .Property(p => p.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ProductModel>()
                .Property(p => p.Discount)
                .HasPrecision(18, 2);

            // Order Details
            modelBuilder.Entity<OrderDetailsModel>()
                .Property(p => p.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderDetailsModel>()
                .Property(p => p.Discount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderDetailsModel>()
                .Property(p => p.Tax)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderDetailsModel>()
                .Property(p => p.NetAmount)
                .HasPrecision(18, 2);

            // Order
            modelBuilder.Entity<OrderModel>()
                .Property(p => p.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderModel>()
                .Property(p => p.TotalDiscount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderModel>()
                .Property(p => p.TotalTax)
                .HasPrecision(18, 2);
        }
        #endregion
    }
}