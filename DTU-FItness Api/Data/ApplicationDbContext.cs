using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<SimpleModel> SimpleTable { get; set; }
    public DbSet<ClubModel> Clubs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the table name for SimpleModel to match the actual table name in the database
        modelBuilder.Entity<SimpleModel>().ToTable("simple_table");

        modelBuilder.Entity<ClubModel>(entity =>
    {
        entity.HasKey(e => e.ClubID); // Explicitly setting ClubId as the primary key

        entity.ToTable("clubs");
        entity.Property(e => e.ClubID).ValueGeneratedOnAdd(); // Assuming ClubId is auto-generated
        entity.Property(e => e.ClubName)
            .IsRequired()
            .HasMaxLength(255);

        entity.Property(e => e.Description)
            .HasMaxLength(1000);

        entity.Property(e => e.OwnerUserId)
            .IsRequired();

        entity.Property(e => e.CreationDate).IsRequired();
        
        });



    }
}

