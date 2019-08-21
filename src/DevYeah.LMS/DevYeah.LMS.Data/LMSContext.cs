using DevYeah.LMS.Models;
using Microsoft.EntityFrameworkCore;


namespace DevYeah.LMS.Data
{
    public partial class LMSContext : DbContext
    {
        public LMSContext()
        {
        }

        public LMSContext(DbContextOptions<LMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountCourse> AccountCourse { get; set; }
        public virtual DbSet<Avatar> Avatar { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<CourseCategory> CourseCategory { get; set; }
        public virtual DbSet<FileRepo> FileRepo { get; set; }
        public virtual DbSet<OperationHistory> OperationHistory { get; set; }
        public virtual DbSet<PracticeRepo> PracticeRepo { get; set; }
        public virtual DbSet<QuizRepo> QuizRepo { get; set; }
        public virtual DbSet<Resource> Resource { get; set; }
        public virtual DbSet<SystemErrors> SystemErrors { get; set; }
        public virtual DbSet<Topic> Topic { get; set; }
        public virtual DbSet<TopicProgress> TopicProgress { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }
        public virtual DbSet<VideoRepo> VideoRepo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("Idx_Email")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AccountCourse>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.CourseId })
                    .HasName("PK__AccountC__580F72BCECC44C8A");

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(sysutcdatetime())");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountCourse)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountCourse_Account");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.AccountCourse)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountCourse_Course");
            });

            modelBuilder.Entity<Avatar>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AvatarUrl)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Dimension)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.MimeType)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.UserProfile)
                    .WithMany(p => p.Avatar)
                    .HasForeignKey(d => d.UserProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Avatar_UserProfile");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Icon)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Overview)
                    .IsRequired()
                    .HasMaxLength(800);

                entity.Property(e => e.ScreenCast)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasDefaultValueSql("('https://res.cloudinary.com/funfood/image/upload/v1565220505/courses/default_cover.gif')");

                entity.HasOne(d => d.Instructor)
                    .WithMany(p => p.Course)
                    .HasForeignKey(d => d.InstructorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Course_Account");
            });

            modelBuilder.Entity<CourseCategory>(entity =>
            {
                entity.HasKey(e => new { e.CourseId, e.CategoryId })
                    .HasName("PK__CourseCa__68BDE20737D3C555");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CourseCategory)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseCategory_Category");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseCategory)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseCategory_Course");
            });

            modelBuilder.Entity<FileRepo>(entity =>
            {
                entity.HasIndex(e => e.ResourceId)
                    .HasName("UNIQ_FK_FileRepo")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Resource)
                    .WithOne(p => p.FileRepo)
                    .HasForeignKey<FileRepo>(d => d.ResourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FileRepo_Resource");
            });

            modelBuilder.Entity<OperationHistory>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ColumnName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ColumnValue).HasMaxLength(4000);

                entity.Property(e => e.CreatedTime).HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Operator)
                    .WithMany(p => p.OperationHistory)
                    .HasForeignKey(d => d.OperatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OperationHistory_Account");
            });

            modelBuilder.Entity<PracticeRepo>(entity =>
            {
                entity.HasIndex(e => e.ResourceId)
                    .HasName("UNIQ_FK_PracticeRepo")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CodeTemplate).HasMaxLength(1000);

                entity.Property(e => e.CorrectResult).HasMaxLength(1000);

                entity.Property(e => e.HelpMessage).HasMaxLength(1000);

                entity.Property(e => e.Instruction)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Resource)
                    .WithOne(p => p.PracticeRepo)
                    .HasForeignKey<PracticeRepo>(d => d.ResourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PracticeRepo_Resource");
            });

            modelBuilder.Entity<QuizRepo>(entity =>
            {
                entity.HasIndex(e => e.ResourceId)
                    .HasName("UNIQ_FK_QuizRepo")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Answer)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Option)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Resource)
                    .WithOne(p => p.QuizRepo)
                    .HasForeignKey<QuizRepo>(d => d.ResourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuizRepo_Resource");
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.Resource)
                    .HasForeignKey(d => d.TopicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Resource_Topic");
            });

            modelBuilder.Entity<SystemErrors>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CallerFilePath).HasMaxLength(500);

                entity.Property(e => e.CallerMemberName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.Exception)
                    .IsRequired()
                    .HasMaxLength(4000);
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Topic)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Topic_Course");
            });

            modelBuilder.Entity<TopicProgress>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.TopicId })
                    .HasName("PK__TopicPro__F4BF455391CC99EA");

                entity.Property(e => e.StartTime).HasDefaultValueSql("(sysutcdatetime())");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.TopicProgress)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TopicProgress_Account");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.TopicProgress)
                    .HasForeignKey(d => d.TopicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TopicProgress_Topic");
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasIndex(e => e.AccountId)
                    .HasName("UNIQ_FK_UserProfile")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Bio).HasMaxLength(500);

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.FullName).HasMaxLength(256);

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.RecoveryEmail)
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithOne(p => p.UserProfile)
                    .HasForeignKey<UserProfile>(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProfile_Account");
            });

            modelBuilder.Entity<VideoRepo>(entity =>
            {
                entity.HasIndex(e => e.ResourceId)
                    .HasName("UNIQ_FK_VideoRepo")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.MimeType)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Resource)
                    .WithOne(p => p.VideoRepo)
                    .HasForeignKey<VideoRepo>(d => d.ResourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VideoRepo_Resource");
            });
        }
    }
}
