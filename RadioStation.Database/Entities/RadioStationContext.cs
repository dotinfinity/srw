namespace RadioStation.Database.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RadioStationContext : DbContext
    {
        public RadioStationContext()
            : base("name=RadioStation")
        {
        }

        public virtual DbSet<Song> Songs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
