using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebComment.Data
{
    public class SqlDbContext : IdentityDbContext<User>
    {
        public SqlDbContext()
            : base("SqlDbContext")
        {
            //this.Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// WEB COMMENT
        /// </summary>

        public virtual DbSet<CM_TrangThai> CM_TrangThai { get; set; }
        public virtual DbSet<CM_LoaiComment> CM_LoaiComment { get; set; }
        public virtual DbSet<CM_Comment> CM_Comment { get; set; }
        public virtual DbSet<CM_NhanVienTiepNhanComment> CM_NhanVienTiepNhanComment { get; set; }
        public virtual DbSet<CM_ReplyComment> CM_ReplyComment { get; set; }
        public virtual DbSet<CM_Satisfy> CM_Satisfy { get; set; }
        public virtual DbSet<CM_ThongTinKhachHang> CM_ThongTinKhachHang { get; set; }
        public virtual DbSet<CM_NVQuanLyDanhGia> CM_NVQuanLyDanhGia { get; set; }
        public virtual DbSet<CM_UserLoginHistory> CM_UserLoginHistory { get; set; }
        public virtual DbSet<CM_LichLamViec> CM_LichLamViec { get; set; }
        
        #region WEBADMIN
        //public DbSet<User> Users { get; set; } 
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserInRole> UserInRoles { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserLoginToken> UserLoginTokens { get; set; }
        public DbSet<ActionGroup> ActionGroups { get; set; }
        public DbSet<Action> Actions { get; set; }
        public DbSet<ActionActionGroupRef> ActionActionGroupRefs { get; set; }
        public DbSet<ActionRoleRef> ActionRoleRefs { get; set; }
        public DbSet<ActionUserRef> ActionUserRefs { get; set; }
        public DbSet<Company> Companies { get; set; }
        public virtual DbSet<DM_CommonData> DM_CommonData { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<M_VnPostVta> M_VnpostVta { get; set; }
        public virtual DbSet<O_Order> O_Order { get; set; }
        public virtual DbSet<O_OrderDetail> O_OrderDetail { get; set; }
        public virtual DbSet<O_OrderHistory> O_OrderHistory { get; set; }
        public virtual DbSet<O_OrderType> O_OrderType { get; set; }
        public virtual DbSet<O_OrderCondition> O_OrderCondition { get; set; }
        public virtual DbSet<P_Category> P_Category { get; set; }
        public virtual DbSet<P_Happycare> P_Happycare { get; set; }
        public virtual DbSet<P_Product> P_Product { get; set; }
        public virtual DbSet<P_ProductImage> P_ProductImage { get; set; }
        public virtual DbSet<P_ProductColor> P_ProductColor { get; set; }

        public virtual DbSet<A_Affiliate_branch> A_Affiliate_branch { get; set; }
        public virtual DbSet<A_Business> A_Business { get; set; }
        public virtual DbSet<A_Business_campaign> A_Business_campaign { get; set; }
        public virtual DbSet<A_Business_products> A_Business_products { get; set; }
        public virtual DbSet<DM_StoreVTA> DM_StoreVTA { get; set; }
        public virtual DbSet<P_Banner> P_Banner { get; set; }
        public virtual DbSet<P_Position> P_Position { get; set; }
        public virtual DbSet<M_AffiliateVta> M_AffiliateVta { get; set; }
        public virtual DbSet<O_Payment_deferred> O_Payment_deferred { get; set; }
        public virtual DbSet<Log_Working> Log_Working { get; set; }
         public virtual DbSet<DM_Menu> DM_Menu { get; set; }
         public virtual DbSet<B_Business> B_Business { get; set; }
         public virtual DbSet<B_BusinessProducts> B_BusinessProducts { get; set; }
         public virtual DbSet<B_Order> B_Order { get; set; }
         public virtual DbSet<B_OrderDetail> B_OrderDetail { get; set; }
         public virtual DbSet<DM_Tragop> DM_Tragop { get; set; }
         public virtual DbSet<DM_District> DM_District { get; set; }
         public virtual DbSet<DM_Province> DM_Province { get; set; }
         public virtual DbSet<DM_Ward> DM_Ward { get; set; }
         public virtual DbSet<P_Sponsor> P_Sponsor { get; set; }
         public virtual DbSet<P_SponsorProduct> P_SponsorProduct { get; set; }
         public virtual DbSet<Cauhinhtragop> Cauhinhtragop { get; set; }
         public virtual DbSet<B_KhachHangDoanhNghiep> B_KhachHangDoanhNghiep { get; set; }
         public virtual DbSet<B_OrderCondition> B_OrderCondition { get; set; }
        public virtual DbSet<AppNotificationHistory> AppNotificationHistory { get; set; }
        public virtual DbSet<AppNotificationHistoryDetail> AppNotificationHistoryDetail { get; set; }
        public virtual DbSet<B_Lienhe> B_Lienhe { get; set; }
        public virtual DbSet<VTA_QuaySo_Event> VTA_QuaySo_Event { get; set; }
        public virtual DbSet<VTA_QuaySo_KetQua> VTA_QuaySo_KetQua { get; set; }
        public virtual DbSet<VTA_QuaySo_Khachhang> VTA_QuaySo_Khachhang { get; set; }
        public virtual DbSet<VTA_QuaySo_Quatang> VTA_QuaySo_Quatang { get; set; }
        public virtual DbSet<AppEmployee> AppEmployee { get; set; }
        public virtual DbSet<AppTokenFailed> AppTokenFailed { get; set; }
        public virtual DbSet<AppDevice> AppDevice { get; set; }
        public virtual DbSet<Log_AdminCp> Log_AdminCp { get; set; }
        #endregion

       

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }


}
