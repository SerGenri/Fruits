using Fruits.Domain.DB;
using Microsoft.EntityFrameworkCore;

namespace Fruits.DAL.Context
{
	public class FruitDbContext : DbContext
	{
		public FruitDbContext(DbContextOptions<FruitDbContext> options) : base(options)
		{
		}

		public virtual DbSet<FruitsCatalog> FruitsCatalog { get; set; }
		public virtual DbSet<PriceCatalog> PriceCatalog { get; set; }
		public virtual DbSet<ProvidersCatalog> ProvidersCatalog { get; set; }
		public virtual DbSet<Stock> Stock { get; set; }
		public virtual DbSet<StockFruits> StockFruits { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{                   
			//Disabling Delete Cascade
			foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
				         .SelectMany(model => model.GetForeignKeys()))
			{
				foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
			}
		}

	}
}
