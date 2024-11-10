using Lection1105EFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Lection1105EFCore.Data;

public partial class GameStoreContext : DbContext
{
    public GameStoreContext()
    {
    }

    public GameStoreContext(DbContextOptions<GameStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    [DbFunction("GetAvgPrice", "dbo")]
    public static int GetAvgPrice(int idCategory) => throw new NotSupportedException();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source = PRSERVER\\SQLEXPRESS; Initial Catalog = ispp3502; User ID = ispp3502; Password = 3502; Trust Server Certificate=True");
        
        //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0BB7A84386");

            entity.ToTable("Category");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.HasDbFunction(() => GetAvgPrice(default))
            .HasName("GetAvgPrice")
            .HasSchema("dbo");

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("PK__Game__2AB897FDE47E5A61");

            entity.ToTable("Game", tb => tb.HasTrigger("trChangedGamesCount"));

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(16, 2)");
            entity.Property(e => e.TotalKeys).HasDefaultValue(100);

            entity.HasOne(d => d.Category).WithMany(p => p.Games)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Game_Category");
        });

        //    modelBuilder.Entity<Category>()
        //        .InsertUsingStoredProcedure("dbo.AddCategory",
        //        spbuilder => spbuilder.HasParameter(propertyName => propertyName.Name)
        //        .HasParameter(p => p.Name)
        //        .HasParameter(p => p.CategoryId)
        //        );

        //    OnModelCreatingPartial(modelBuilder);
        //}

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
