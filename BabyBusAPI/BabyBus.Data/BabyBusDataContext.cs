using System.Data.Entity;
using BabyBus.Model.Entities.Article;
using BabyBus.Model.Entities.Attendance;
using BabyBus.Model.Entities.FAQ;
using BabyBus.Model.Entities.Login;
using BabyBus.Model.Entities.Main;
using BabyBus.Model.Entities.Other;
using BabyBus.Model.Entities.Relation;
using BabyBus.Model.Entities.Summary;

namespace BabyBus.Data
{
    public class BabyBusDataContext : DbContext
    {
        static BabyBusDataContext()
        {
            System.Data.Entity.Database.SetInitializer(new CreateDatabaseIfNotExists<BabyBusDataContext>());
            //System.Data.Entity.Database.SetInitializer(new TestDbInitializer());
        }

        public BabyBusDataContext()
            : base("name=BabyBusDataContext")
        {
        }

        //Article
        public DbSet<Notice> Notice { get; set; }
        public DbSet<NoticeReaded> NoticeReaded { get; set; }
        public DbSet<Favorite> Favorite { get; set; }
        //FAQ
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Question> Question { get; set; }
        //Login
        public DbSet<Checkout> Checkout { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<CardPassword> CardPassword { get; set; }
        public DbSet<Admin> Admin { get; set; }
        //Main
        public DbSet<Child> Child { get; set; }
        public DbSet<Class> Class { get; set; }
        public DbSet<Kindergarten> Kindergarten { get; set; }
        //Other
        public DbSet<WeChatInfo> WeChatInfo { get; set; }
        //Relation
        public DbSet<NoticeChildRelation> NoticeChildRelation { get; set; }
        public DbSet<ParentChildRelation> ParentChildRelation { get; set; }
        public DbSet<Role> Role { get; set; }
        //Attendance
        public DbSet<AttendanceMaster> AttendanceMaster { get; set; }
        public DbSet<AttendanceDetail> AttendanceDetail { get; set; } 
        //Summary
//        public DbSet<ClassDaySummary> ClassDaySummary { get; set; }
//        public DbSet<ClassMonthSummary> ClassMonthSummary { get; set; } 

        public virtual void Commit()
        {
            base.SaveChanges();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
        public class TestDbInitializer : DropCreateDatabaseAlways<BabyBusDataContext>
        {
            protected override void Seed(BabyBusDataContext context)
            {
                context.User.Add(new User
                {
                    WeChatInfoId = 0,
                    LoginName = "123456",
                    Password = "123456",
                });

                context.User.Add(new User
                {
                    WeChatInfoId = 1,
                    LoginName = "admin",
                    Password = "123456",
                });

                context.Kindergarten.Add(new Kindergarten
                {
                    City = "西安",
                    KindergartenName = "睿莱幼儿园"
                });


                base.Seed(context);
            }
        } 
    }
}
