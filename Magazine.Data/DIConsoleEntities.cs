namespace Magazine.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Configuration;
    using System.Data.Common;

    public partial class DIConsoleEntities : DbContext
    {
        private static string connString = ConnectionString();
        public DIConsoleEntities() : base(connString)//base("name=DIConsoleEntities")
        {
        }

        public virtual DbSet<Journal> Journals { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        private static string ConnectionString()
        {
            string rootPath = System.Environment.CurrentDirectory.Replace(@"MagazineConsoleApp\bin\Debug", @"Magazine.Data");
            //string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DIConsoleEntities"].ToString();
            string connectionString = "data source=(LocalDB)\\MSSQLLocalDB;attachdbfilename=|DataDirectory|\\App_Data\\dbJournalEvents.mdf;integrated security=True;connect timeout=30;MultipleActiveResultSets=True;App=EntityFramework";
            connectionString = connectionString.Replace("|DataDirectory|", rootPath);
            return connectionString;
        }
    }
}
