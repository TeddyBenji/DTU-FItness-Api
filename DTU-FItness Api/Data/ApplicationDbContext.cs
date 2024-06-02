using Microsoft.EntityFrameworkCore;
using DtuFitnessApi.Models;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
     
    public DbSet<ClubModel> Clubs { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<ClubMember> ClubMembers { get; set; }
    public DbSet<ExerciseLog> ExerciseLogs { get; set; }
    public DbSet<ExerciseModel> Exercises { get; set; }
    public DbSet<Metric> Metrics { get; set; }
    public DbSet<ExerciseMetric> ExerciseMetrics { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<UserNotification> UserNotifications { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ClubModel>(entity =>
    {
        entity.HasKey(e => e.ClubID); 

        entity.ToTable("clubs");
        entity.Property(e => e.ClubID).ValueGeneratedOnAdd(); 
        entity.Property(e => e.ClubName)
            .IsRequired()
            .HasMaxLength(255);

        entity.Property(e => e.Description)
            .HasMaxLength(1000);

        entity.Property(e => e.OwnerUserId)
            .IsRequired();

        entity.Property(e => e.CreationDate).IsRequired();

        entity.HasMany(c => c.ClubMembers)
        .WithOne(cm => cm.Club)
        .HasForeignKey(cm => cm.ClubId)
        .OnDelete(DeleteBehavior.Cascade);

    });

          modelBuilder.Entity<UserProfile>(entity =>
{
    entity.HasKey(e => e.IdentityUserID); 

    entity.ToTable("user_profiles"); 
    
    entity.Property(e => e.IdentityUserID)
          .IsRequired(); 

    entity.Property(e => e.Username)
          .IsRequired()
          .HasMaxLength(255);

    entity.Property(e => e.Email)
          .IsRequired()
          .HasMaxLength(255);


});

        modelBuilder.Entity<ClubMember>(entity =>
        {
            entity.HasKey(e => e.ClubMemberId);
         
            entity.HasOne(cm => cm.Club)
                .WithMany(c => c.ClubMembers)
                .HasForeignKey(cm => cm.ClubId)
                .OnDelete(DeleteBehavior.Cascade); 
            
            entity.HasOne(cm => cm.UserProfile)
                .WithMany(up => up.ClubMembers)
                .HasForeignKey(cm => cm.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.ToTable("clubmembers");
        });

        modelBuilder.Entity<ExerciseLog>(entity =>
    {
        entity.ToTable("exercise_logs");
        entity.HasKey(e => e.LogID);
        entity.Property(e => e.ExerciseDate).IsRequired();
        entity.HasOne(d => d.UserProfile).WithMany(p => p.ExerciseLogs).HasForeignKey(d => d.UserID);
        entity.HasOne(d => d.ExerciseModel) 
          .WithMany(p => p.ExerciseLogs)
          .HasForeignKey(d => d.ExerciseID);
    });

    modelBuilder.Entity<ExerciseModel>(entity =>
    {
        entity.ToTable("exercises");
        entity.HasKey(e => e.ExerciseID);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
    });

    modelBuilder.Entity<Metric>(entity =>
    {
        entity.ToTable("metrics");
        entity.HasKey(e => e.MetricID);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
    });

    modelBuilder.Entity<ExerciseMetric>(entity =>
{
    entity.ToTable("exercise_metrics"); 
    entity.HasKey(e => e.ExerciseMetricID);
    entity.Property(e => e.Value).IsRequired();
    
    entity.HasOne(d => d.ExerciseLog)
          .WithMany(p => p.ExerciseMetrics) 
          .HasForeignKey(d => d.ExerciseLogID);

    entity.HasOne(d => d.Metric)
          .WithMany(p => p.ExerciseMetrics)
          .HasForeignKey(d => d.MetricID);
});

modelBuilder.Entity<Notification>(entity =>
{
    entity.ToTable("notifications");
    entity.HasKey(n => n.NotificationID);

    entity.Property(n => n.Message).HasColumnType("TEXT");

    
    entity.HasOne(n => n.Event)
          .WithMany(e => e.Notifications)
          .HasForeignKey(n => n.EventID);

    
    entity.HasMany(n => n.UserNotifications)
          .WithOne(un => un.Notification)
          .HasForeignKey(un => un.NotificationID);
});

modelBuilder.Entity<Event>(entity =>
{
    entity.ToTable("events");
    entity.HasKey(e => e.EventID);

    entity.Property(e => e.ClubID).IsRequired();
    entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
    entity.Property(e => e.Description).HasColumnType("TEXT");
    entity.Property(e => e.EventDate).IsRequired();

   
    entity.HasOne(e => e.Club)
          .WithMany(c => c.Events)
          .HasForeignKey(e => e.ClubID);

    
    entity.HasMany(e => e.Notifications)
          .WithOne(n => n.Event)
          .HasForeignKey(n => n.EventID);
});

modelBuilder.Entity<UserNotification>(entity =>
{
    entity.ToTable("user_notifications"); 
    entity.HasKey(un => un.UserNotificationID);
    
    entity.Property(un => un.IsRead).IsRequired();

    entity.HasOne(un => un.Notification)
          .WithMany(n => n.UserNotifications) 
          .HasForeignKey(un => un.NotificationID);

    entity.HasOne(un => un.User)
          .WithMany(up => up.UserNotifications) 
          .HasForeignKey(un => un.IdentityUserID); 
});



    }
}

