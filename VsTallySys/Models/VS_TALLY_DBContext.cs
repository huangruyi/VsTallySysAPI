using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace VsTallySys.Models
{
    public partial class VS_TALLY_DBContext : DbContext
    {
        public VS_TALLY_DBContext()
        {
        }

        public VS_TALLY_DBContext(DbContextOptions<VS_TALLY_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<VsIncomeDetail> VsIncomeDetail { get; set; }
        public virtual DbSet<VsLiquidityDetail> VsLiquidityDetail { get; set; }
        public virtual DbSet<VsSpendingDetail> VsSpendingDetail { get; set; }
        public virtual DbSet<VsStorageDetail> VsStorageDetail { get; set; }
        public virtual DbSet<VsSysApiModule> VsSysApiModule { get; set; }
        public virtual DbSet<VsSysModule> VsSysModule { get; set; }
        public virtual DbSet<VsSysPower> VsSysPower { get; set; }
        public virtual DbSet<VsSysSecure> VsSysSecure { get; set; }
        public virtual DbSet<VsSysSecureUser> VsSysSecureUser { get; set; }
        public virtual DbSet<VsSysUser> VsSysUser { get; set; }
        public virtual DbSet<VsTallyType> VsTallyType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // 获取appsettings.json配置信息
                var config = new ConfigurationBuilder()
                                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json")
                                .Build();
                // 获取数据库连接字符串
                string conn = config.GetConnectionString("SqlConn");
                //连接数据库
                optionsBuilder.UseSqlServer(conn);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<VsIncomeDetail>(entity =>
            {
                entity.ToTable("VS_INCOME_DETAIL");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.DTime)
                    .HasColumnName("D_TIME")
                    .HasColumnType("date");

                entity.Property(e => e.FRmb).HasColumnName("F_RMB");

                entity.Property(e => e.SCode)
                    .IsRequired()
                    .HasColumnName("S_CODE")
                    .HasMaxLength(50);

                entity.Property(e => e.SDesc).HasColumnName("S_DESC");

                entity.Property(e => e.SOwner)
                    .IsRequired()
                    .HasColumnName("S_OWNER")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<VsLiquidityDetail>(entity =>
            {
                entity.ToTable("VS_LIQUIDITY_DETAIL");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.DTime)
                    .HasColumnName("D_TIME")
                    .HasColumnType("date");

                entity.Property(e => e.FRmb).HasColumnName("F_RMB");

                entity.Property(e => e.SCode)
                    .IsRequired()
                    .HasColumnName("S_CODE")
                    .HasMaxLength(50);

                entity.Property(e => e.SDesc).HasColumnName("S_DESC");

                entity.Property(e => e.SOwner)
                    .IsRequired()
                    .HasColumnName("S_OWNER")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<VsSpendingDetail>(entity =>
            {
                entity.ToTable("VS_SPENDING_DETAIL");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.DTime)
                    .HasColumnName("D_TIME")
                    .HasColumnType("date");

                entity.Property(e => e.FRmb).HasColumnName("F_RMB");

                entity.Property(e => e.SCode)
                    .IsRequired()
                    .HasColumnName("S_CODE")
                    .HasMaxLength(50);

                entity.Property(e => e.SDesc).HasColumnName("S_DESC");

                entity.Property(e => e.SName)
                    .IsRequired()
                    .HasColumnName("S_NAME")
                    .HasMaxLength(50);

                entity.Property(e => e.SOwner)
                    .IsRequired()
                    .HasColumnName("S_OWNER")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<VsStorageDetail>(entity =>
            {
                entity.ToTable("VS_STORAGE_DETAIL");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.DTime)
                    .HasColumnName("D_TIME")
                    .HasColumnType("date");

                entity.Property(e => e.FRmb).HasColumnName("F_RMB");

                entity.Property(e => e.IOperation).HasColumnName("I_OPERATION");

                entity.Property(e => e.SCode)
                    .IsRequired()
                    .HasColumnName("S_CODE")
                    .HasMaxLength(50);

                entity.Property(e => e.SDesc).HasColumnName("S_DESC");

                entity.Property(e => e.SOwner)
                    .IsRequired()
                    .HasColumnName("S_OWNER")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<VsSysApiModule>(entity =>
            {
                entity.ToTable("VS_SYS_API_MODULE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.SAction)
                    .IsRequired()
                    .HasColumnName("S_ACTION")
                    .HasMaxLength(50);

                entity.Property(e => e.SController)
                    .IsRequired()
                    .HasColumnName("S_CONTROLLER")
                    .HasMaxLength(50);

                entity.Property(e => e.SLinkurl)
                    .IsRequired()
                    .HasColumnName("S_LINKURL")
                    .HasMaxLength(50);

                entity.Property(e => e.SModuleid)
                    .IsRequired()
                    .HasColumnName("S_MODULEID")
                    .HasMaxLength(50);

                entity.Property(e => e.SName)
                    .IsRequired()
                    .HasColumnName("S_NAME")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<VsSysModule>(entity =>
            {
                entity.ToTable("VS_SYS_MODULE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.BIsshow).HasColumnName("B_ISSHOW");

                entity.Property(e => e.ILevel).HasColumnName("I_LEVEL");

                entity.Property(e => e.SIcon)
                    .HasColumnName("S_ICON")
                    .HasMaxLength(50);

                entity.Property(e => e.SLinkurl)
                    .HasColumnName("S_LINKURL")
                    .HasMaxLength(50);

                entity.Property(e => e.SName)
                    .IsRequired()
                    .HasColumnName("S_NAME")
                    .HasMaxLength(50);

                entity.Property(e => e.SParentid)
                    .IsRequired()
                    .HasColumnName("S_PARENTID")
                    .HasMaxLength(50);

                entity.Property(e => e.IOrder).HasColumnName("I_ORDER");
            });

            modelBuilder.Entity<VsSysPower>(entity =>
            {
                entity.ToTable("VS_SYS_POWER");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.BIsdeleted).HasColumnName("B_ISDELETED");

                entity.Property(e => e.SApimoduleid)
                    .IsRequired()
                    .HasColumnName("S_APIMODULEID")
                    .HasMaxLength(50);

                entity.Property(e => e.SModuleid)
                    .IsRequired()
                    .HasColumnName("S_MODULEID")
                    .HasMaxLength(50);

                entity.Property(e => e.SUserid)
                    .IsRequired()
                    .HasColumnName("S_USERID")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<VsSysSecure>(entity =>
            {
                entity.ToTable("VS_SYS_SECURE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.SQuestion)
                    .IsRequired()
                    .HasColumnName("S_QUESTION")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<VsSysSecureUser>(entity =>
            {
                entity.ToTable("VS_SYS_SECURE_USER");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.SAnswer)
                    .IsRequired()
                    .HasColumnName("S_ANSWER");

                entity.Property(e => e.SQuestionid)
                    .IsRequired()
                    .HasColumnName("S_QUESTIONID")
                    .HasMaxLength(50);

                entity.Property(e => e.SUserid)
                    .IsRequired()
                    .HasColumnName("S_USERID")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<VsSysUser>(entity =>
            {
                entity.HasKey(e => e.SUsername)
                    .HasName("PK_VS_SYS_USER_1");

                entity.ToTable("VS_SYS_USER");

                entity.Property(e => e.SUsername)
                    .HasColumnName("S_USERNAME")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.DCreatetime)
                    .HasColumnName("D_CREATETIME")
                    .HasColumnType("date");

                entity.Property(e => e.DLastlogin)
                    .HasColumnName("D_LASTLOGIN")
                    .HasColumnType("date");

                entity.Property(e => e.DUpdatetime)
                    .HasColumnName("D_UPDATETIME")
                    .HasColumnType("date");

                entity.Property(e => e.SDesc).HasColumnName("S_DESC");

                entity.Property(e => e.SLogo).HasColumnName("S_LOGO");

                entity.Property(e => e.SName)
                    .HasColumnName("S_NAME")
                    .HasMaxLength(50);

                entity.Property(e => e.SPassword)
                    .IsRequired()
                    .HasColumnName("S_PASSWORD")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<VsTallyType>(entity =>
            {
                entity.ToTable("VS_TALLY_TYPE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.SCode)
                    .IsRequired()
                    .HasColumnName("S_CODE")
                    .HasMaxLength(50);

                entity.Property(e => e.SDesc).HasColumnName("S_DESC");

                entity.Property(e => e.SName)
                    .IsRequired()
                    .HasColumnName("S_NAME")
                    .HasMaxLength(50);
            });
        }
    }
}
