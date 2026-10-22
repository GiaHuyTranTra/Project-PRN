using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SportCenterManament_PRN.DataAccess.Entities;

public partial class SportsCenterDbContext : DbContext
{
    public SportsCenterDbContext()
    {
    }

    public SportsCenterDbContext(DbContextOptions<SportsCenterDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<CenterManager> CenterManagers { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<ClassChangeEvent> ClassChangeEvents { get; set; }

    public virtual DbSet<ClassRegistration> ClassRegistrations { get; set; }

    public virtual DbSet<ClassSession> ClassSessions { get; set; }

    public virtual DbSet<Coach> Coaches { get; set; }

    public virtual DbSet<CoachDiscipline> CoachDisciplines { get; set; }

    public virtual DbSet<Discipline> Disciplines { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<MemberSubscription> MemberSubscriptions { get; set; }

    public virtual DbSet<MembershipInvoice> MembershipInvoices { get; set; }

    public virtual DbSet<MembershipPackage> MembershipPackages { get; set; }

    public virtual DbSet<MembershipPackageSport> MembershipPackageSports { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<PasswordChangeOtp> PasswordChangeOtps { get; set; }

    public virtual DbSet<Receptionist> Receptionists { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            try
            {
                string basePath = AppContext.BaseDirectory;
                Microsoft.Extensions.Configuration.IConfigurationRoot configuration = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                string? connectionString = Microsoft.Extensions.Configuration.ConfigurationExtensions.GetConnectionString(configuration, "SportsCenterConnection");
                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    optionsBuilder.UseSqlServer(connectionString);
                    return;
                }
            }
            catch
            {
            }

            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS01;Database=SportsCenterManagement;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC07E4264B6E");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Phone, "UQ_Account_Phone")
                .IsUnique()
                .HasFilter("([Phone] IS NOT NULL)");

            entity.HasIndex(e => e.Email, "UQ__Account__A9D105343F8562DD").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Role");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuditLog__3214EC07CCB0C4E6");

            entity.ToTable("AuditLog");

            entity.HasIndex(e => new { e.ClassId, e.CreatedAt }, "IX_AuditLog_Class_Time");

            entity.HasIndex(e => new { e.SessionId, e.CreatedAt }, "IX_AuditLog_Session_Time");

            entity.Property(e => e.Id)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.AccountId)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.EntityId)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.EntityType)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_AuditLog_Account");

            entity.HasOne(d => d.Class).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK_AuditLog_Class");

            entity.HasOne(d => d.Session).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("FK_AuditLog_Session");
        });

        modelBuilder.Entity<CenterManager>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__CenterMa__349DA5A64807BEB6");

            entity.ToTable("CenterManager");

            entity.Property(e => e.AccountId)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FullName).HasMaxLength(100);

            entity.HasOne(d => d.Account).WithOne(p => p.CenterManager)
                .HasForeignKey<CenterManager>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CenterManager_Account");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.ToTable("Class");

            entity.HasIndex(e => e.DisciplineId, "IX_Class_DisciplineId");

            entity.HasIndex(e => e.NormalizedName, "UQ_Class_NormalizedName_Active")
                .IsUnique()
                .HasFilter("([Status]<>'ARCHIVED')");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Class_CreatedAt");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.NormalizedName)
                .HasMaxLength(100)
                .HasComputedColumnSql("(CONVERT([nvarchar](100),[dbo].[NormalizeFlow2Name]([Name])))", true);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("DRAFT", "DF_Class_Status");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Classes)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_Class_CreatedBy");

            entity.HasOne(d => d.Discipline).WithMany(p => p.Classes)
                .HasForeignKey(d => d.DisciplineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Class_Discipline");
        });

        modelBuilder.Entity<ClassChangeEvent>(entity =>
        {
            entity.ToTable("ClassChangeEvent");

            entity.HasIndex(e => new { e.OperationId, e.SessionId, e.EventType }, "UQ_ClassChangeEvent_Operation").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ActorAccountId)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.EventType)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.ActorAccount).WithMany(p => p.ClassChangeEvents)
                .HasForeignKey(d => d.ActorAccountId)
                .HasConstraintName("FK_ClassChangeEvent_Actor");

            entity.HasOne(d => d.Session).WithMany(p => p.ClassChangeEvents)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassChangeEvent_Session");
        });

        modelBuilder.Entity<ClassRegistration>(entity =>
        {
            entity.ToTable("ClassRegistration");

            entity.HasIndex(e => new { e.MemberId, e.Status, e.SessionId }, "IX_ClassRegistration_Member");

            entity.HasIndex(e => new { e.SessionId, e.Status }, "IX_ClassRegistration_Session");

            entity.HasIndex(e => new { e.SessionId, e.MemberId }, "UQ_ClassRegistration_ActiveBooking")
                .IsUnique()
                .HasFilter("([Status]='BOOKED')");

            entity.Property(e => e.CancelReason).HasMaxLength(500);
            entity.Property(e => e.CanceledBy)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.MemberId)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.RegisteredAt).HasDefaultValueSql("(sysutcdatetime())", "DF_ClassRegistration_RegisteredAt");
            entity.Property(e => e.RegisteredBy)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.RegisteredVia)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("MEMBER", "DF_ClassRegistration_RegisteredVia");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("BOOKED", "DF_ClassRegistration_Status");

            entity.HasOne(d => d.CanceledByNavigation).WithMany(p => p.ClassRegistrationCanceledByNavigations)
                .HasForeignKey(d => d.CanceledBy)
                .HasConstraintName("FK_ClassRegistration_CanceledBy");

            entity.HasOne(d => d.Member).WithMany(p => p.ClassRegistrations)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassRegistration_Member");

            entity.HasOne(d => d.MemberSubscription).WithMany(p => p.ClassRegistrations)
                .HasForeignKey(d => d.MemberSubscriptionId)
                .HasConstraintName("FK_ClassRegistration_Subscription");

            entity.HasOne(d => d.RegisteredByNavigation).WithMany(p => p.ClassRegistrationRegisteredByNavigations)
                .HasForeignKey(d => d.RegisteredBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassRegistration_RegisteredBy");

            entity.HasOne(d => d.Session).WithMany(p => p.ClassRegistrations)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassRegistration_ClassSession");
        });

        modelBuilder.Entity<ClassSession>(entity =>
        {
            entity.ToTable("ClassSession");

            entity.HasIndex(e => new { e.ClassId, e.SessionDate }, "IX_ClassSession_ClassId");

            entity.HasIndex(e => new { e.CoachId, e.SessionDate, e.StartTime }, "IX_ClassSession_Date_Coach").HasFilter("([Status]='SCHEDULED')");

            entity.HasIndex(e => new { e.RoomId, e.SessionDate, e.StartTime }, "IX_ClassSession_Date_Room").HasFilter("([Status]='SCHEDULED')");

            entity.Property(e => e.CancelReason).HasMaxLength(500);
            entity.Property(e => e.CanceledBy)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.CoachId)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_ClassSession_CreatedAt");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.EndTime).HasPrecision(0);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.StartTime).HasPrecision(0);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("SCHEDULED", "DF_ClassSession_Status");

            entity.HasOne(d => d.CanceledByNavigation).WithMany(p => p.ClassSessionCanceledByNavigations)
                .HasForeignKey(d => d.CanceledBy)
                .HasConstraintName("FK_ClassSession_CanceledBy");

            entity.HasOne(d => d.Class).WithMany(p => p.ClassSessions)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassSession_Class");

            entity.HasOne(d => d.Coach).WithMany(p => p.ClassSessions)
                .HasForeignKey(d => d.CoachId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassSession_Coach");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ClassSessionCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_ClassSession_CreatedBy");

            entity.HasOne(d => d.Room).WithMany(p => p.ClassSessions)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassSession_Room");
        });

        modelBuilder.Entity<Coach>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Coach__349DA5A65C9861FD");

            entity.ToTable("Coach");

            entity.HasIndex(e => new { e.CreatedAt, e.AccountId }, "IX_Coach_Pagination").IsDescending();

            entity.Property(e => e.AccountId)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Specialization).HasMaxLength(255);
            entity.Property(e => e.WorkSchedule).HasMaxLength(500);

            entity.HasOne(d => d.Account).WithOne(p => p.Coach)
                .HasForeignKey<Coach>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Coach_Account");
        });

        modelBuilder.Entity<CoachDiscipline>(entity =>
        {
            entity.HasKey(e => new { e.CoachAccountId, e.DisciplineId });

            entity.ToTable("CoachDiscipline");

            entity.HasIndex(e => e.DisciplineId, "IX_CoachDiscipline_DisciplineId");

            entity.Property(e => e.CoachAccountId)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_CoachDiscipline_CreatedAt");

            entity.HasOne(d => d.CoachAccount).WithMany(p => p.CoachDisciplines)
                .HasForeignKey(d => d.CoachAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CoachDiscipline_Coach");

            entity.HasOne(d => d.Discipline).WithMany(p => p.CoachDisciplines)
                .HasForeignKey(d => d.DisciplineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CoachDiscipline_Discipline");
        });

        modelBuilder.Entity<Discipline>(entity =>
        {
            entity.ToTable("Discipline");

            entity.HasIndex(e => e.Name, "UQ_Discipline_Name").IsUnique();

            entity.HasIndex(e => e.NormalizedName, "UQ_Discipline_NormalizedName").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Discipline_CreatedAt");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF_Discipline_IsActive");
            entity.Property(e => e.Name).HasMaxLength(80);
            entity.Property(e => e.NormalizedName)
                .HasMaxLength(80)
                .HasComputedColumnSql("(CONVERT([nvarchar](80),[dbo].[NormalizeFlow2Name]([Name])))", true);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Member__349DA5A67C08C6CC");

            entity.ToTable("Member");

            entity.HasIndex(e => e.MemberCode, "UQ__Member__84CA6377C2C6C2FB").IsUnique();

            entity.Property(e => e.AccountId)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.MemberCode)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithOne(p => p.Member)
                .HasForeignKey<Member>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Member_Account");
        });

        modelBuilder.Entity<MemberSubscription>(entity =>
        {
            entity.ToTable("MemberSubscription");

            entity.HasIndex(e => new { e.MemberId, e.Status }, "IX_MemberSubscription_MemberId_Status");

            entity.HasIndex(e => new { e.MemberId, e.Status, e.EndDate }, "IX_MemberSubscription_Member_Confirmed_EndDate").IsDescending(false, false, true);

            entity.HasIndex(e => e.MemberId, "UQ_MemberSubscription_Pending_Member")
                .IsUnique()
                .HasFilter("([Status]='PENDING_PAYMENT')");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_MemberSubscription_CreatedAt");
            entity.Property(e => e.Kind)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.MemberId)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.PackageName).HasMaxLength(80);
            entity.Property(e => e.PackagePrice).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("PENDING_PAYMENT", "DF_MemberSubscription_Status");
            entity.Property(e => e.SuspensionReason).HasMaxLength(500);

            entity.HasOne(d => d.Member).WithOne(p => p.MemberSubscription)
                .HasForeignKey<MemberSubscription>(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberSubscription_Member");

            entity.HasOne(d => d.Package).WithMany(p => p.MemberSubscriptions)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberSubscription_MembershipPackage");
        });

        modelBuilder.Entity<MembershipInvoice>(entity =>
        {
            entity.ToTable("MembershipInvoice");

            entity.HasIndex(e => new { e.MemberId, e.Status }, "IX_MembershipInvoice_MemberId_Status");

            entity.HasIndex(e => e.InvoiceNumber, "UQ_MembershipInvoice_InvoiceNumber").IsUnique();

            entity.HasIndex(e => e.SubscriptionId, "UQ_MembershipInvoice_SubscriptionId").IsUnique();

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_MembershipInvoice_CreatedAt");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MemberId)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.PaidBy)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("PENDING_PAYMENT", "DF_MembershipInvoice_Status");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MembershipInvoiceCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MembershipInvoice_CreatedBy");

            entity.HasOne(d => d.Member).WithMany(p => p.MembershipInvoices)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MembershipInvoice_Member");

            entity.HasOne(d => d.PaidByNavigation).WithMany(p => p.MembershipInvoicePaidByNavigations)
                .HasForeignKey(d => d.PaidBy)
                .HasConstraintName("FK_MembershipInvoice_PaidBy");

            entity.HasOne(d => d.Subscription).WithOne(p => p.MembershipInvoice)
                .HasForeignKey<MembershipInvoice>(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MembershipInvoice_Subscription");
        });

        modelBuilder.Entity<MembershipPackage>(entity =>
        {
            entity.ToTable("MembershipPackage");

            entity.HasIndex(e => e.Name, "UQ_MembershipPackage_Name").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_MembershipPackage_CreatedAt");
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF_MembershipPackage_IsActive");
            entity.Property(e => e.Name).HasMaxLength(80);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<MembershipPackageSport>(entity =>
        {
            entity.HasKey(e => new { e.PackageId, e.DisciplineId });

            entity.ToTable("MembershipPackageSport");

            entity.HasIndex(e => e.DisciplineId, "IX_MembershipPackageSport_DisciplineId");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_MembershipPackageSport_CreatedAt");

            entity.HasOne(d => d.Discipline).WithMany(p => p.MembershipPackageSports)
                .HasForeignKey(d => d.DisciplineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MembershipPackageSport_Discipline");

            entity.HasOne(d => d.Package).WithMany(p => p.MembershipPackageSports)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MembershipPackageSport_Package");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notification");

            entity.HasIndex(e => new { e.RecipientId, e.ReadAt, e.CreatedAt }, "IX_Notification_Recipient_ReadAt").IsDescending(false, false, true);

            entity.HasIndex(e => new { e.EventId, e.RecipientId }, "UQ_Notification_EventRecipient")
                .IsUnique()
                .HasFilter("([EventId] IS NOT NULL)");

            entity.Property(e => e.Body).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Notification_CreatedAt");
            entity.Property(e => e.EntityId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EntityType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RecipientId)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Event).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK_Notification_Event");

            entity.HasOne(d => d.Recipient).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.RecipientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_Recipient");
        });

        modelBuilder.Entity<PasswordChangeOtp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Password__3214EC07225EE6F9");

            entity.ToTable("PasswordChangeOtp");

            entity.HasIndex(e => e.AccountId, "UQ_PasswordChangeOtp_ActiveAccount")
                .IsUnique()
                .HasFilter("([IsUsed]=(0))");

            entity.Property(e => e.AccountId)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.MaxAttempts).HasDefaultValue(3);
            entity.Property(e => e.OtpHash)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithOne(p => p.PasswordChangeOtp)
                .HasForeignKey<PasswordChangeOtp>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PasswordChangeOtp_Account");
        });

        modelBuilder.Entity<Receptionist>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Receptio__349DA5A6971DB159");

            entity.ToTable("Receptionist");

            entity.Property(e => e.AccountId)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.WorkShift).HasMaxLength(100);

            entity.HasOne(d => d.Account).WithOne(p => p.Receptionist)
                .HasForeignKey<Receptionist>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Receptionist_Account");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.HasIndex(e => e.Name, "UQ__Role__737584F616AD49EB").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.ToTable("Room");

            entity.HasIndex(e => e.NormalizedName, "UQ_Room_NormalizedName").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())", "DF_Room_CreatedAt");
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF_Room_IsActive");
            entity.Property(e => e.Location).HasMaxLength(120);
            entity.Property(e => e.Name).HasMaxLength(80);
            entity.Property(e => e.NormalizedName)
                .HasMaxLength(80)
                .HasComputedColumnSql("(CONVERT([nvarchar](80),[dbo].[NormalizeFlow2Name]([Name])))", true);
            entity.Property(e => e.RoomType).HasMaxLength(20);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
