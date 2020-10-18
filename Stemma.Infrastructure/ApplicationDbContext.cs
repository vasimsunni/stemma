using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Stemma.Core;

namespace Stemma.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
            //ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        }

        #region Entity
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<FileUpload> FileUploads { get; set; }
        public DbSet<Bin> Bin { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonSpouse> PersonSpouses { get; set; }
        public DbSet<SpouseRelation> SpouseRelations { get; set; }
        public DbSet<Surname> Surnames { get; set; }
        public DbSet<GalleryType> GalleryTypes { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<GalleryPerson> GalleryPeople { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);			

            builder.Entity<ApplicationUser>(b =>
            {
                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "1",
                Name = "Super Admin",
                NormalizedName = "Super Admin"
            });

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "2",
                Name = "Admin",
                NormalizedName = "Admin"
            });

            var hasher = new PasswordHasher<ApplicationUser>();
            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = "1",
                UserName = "vasim",
                NormalizedUserName = "Vasim",
                FirstName = "Vasim",
                LastName = "Sunni",
                IsActive = true,
                Email = "vasimsunni@timeline.ae",
                NormalizedEmail = "vasimsunni@timeline.ae",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "P@ssword@1"),
                SecurityStamp = Guid.NewGuid().ToString("D")
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "1",
                UserId = "1"
            });

            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = "2",
                UserName = "safer",
                NormalizedUserName = "safer",
                FirstName = "Safer",
                LastName = "Sunni",
                IsActive = true,
                Email = "beinyors@gmail.com",
                NormalizedEmail = "beinyors@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "P@ssword@1"),
                SecurityStamp = Guid.NewGuid().ToString("D")
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "2",
                UserId = "2"
            });

            builder.Entity<Administrator>().HasData(new Administrator
            {
                AdminId = 1,
                IdentityUserIdf = "1",
                FirstName = "Vasim",
                LastName = "Sunni",
                IsActive = true,
                Email = "vasimsunni@timeline.ae",
                ContactNumbers = "971 56 825 2376"
            });

            builder.Entity<Administrator>().HasData(new Administrator
            {
                AdminId = 2,
                IdentityUserIdf = "2",
                FirstName = "Safer",
                LastName = "Sunni",
                IsActive = true,
                Email = "beinyors@gmail.com",
                ContactNumbers = "971 56 825 2376"
            });
        }
    }
}
