using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Borrower> Borrowers { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.FirstName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(a => a.LastName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(a => a.Biography)
                      .HasMaxLength(1000);

                entity.Ignore(a => a.FullName);

                entity.HasMany(a => a.Books)
                      .WithOne(b => b.Author)
                      .HasForeignKey(b => b.AuthorId)
                      .OnDelete(DeleteBehavior.Restrict); 
            });
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.Property(b => b.Title)
                      .IsRequired()
                      .HasMaxLength(300);

                entity.Property(b => b.ISBN)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.HasIndex(b => b.ISBN).IsUnique();
                entity.Property(b => b.Genre)
                      .HasMaxLength(100);

                entity.Property(b => b.IsAvailable).HasDefaultValue(true).ValueGeneratedNever();

                entity.Property(b => b.IsAvailable)
                      .HasDefaultValue(true);

                entity.HasMany(b => b.BorrowRecords)
                      .WithOne(r => r.Book)
                      .HasForeignKey(r => r.BookId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Borrower ──────────────────────────────────────────────────
            modelBuilder.Entity<Borrower>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.Property(b => b.Name)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(b => b.Email)
                      .IsRequired()
                      .HasMaxLength(250);

                entity.HasIndex(b => b.Email).IsUnique();

                entity.Property(b => b.PhoneNumber)
                      .HasMaxLength(20);

                // One Borrower --> Many BorrowRecords
                entity.HasMany(b => b.BorrowRecords)
                      .WithOne(r => r.Borrower)
                      .HasForeignKey(r => r.BorrowerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<BorrowRecord>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Ignore(r => r.IsReturned);
            });
        }
    }
}
