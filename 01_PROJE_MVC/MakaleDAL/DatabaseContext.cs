using MakaleEntities;
using System;
using System.Data.Entity;
using System.Linq;

namespace MakaleDAL
{
    public class DatabaseContext : DbContext
    {
 
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Makale> Makaleler { get; set; }
        public DbSet<Yorum> Yorumlar { get; set; }
        public DbSet<Begeni> Begeniler { get; set; }

        public DatabaseContext()
        {
            //haz�rlad���m�z veri taban�n� burdan tetikleyim veritaban�na kaydettiriyoruz
            Database.SetInitializer(new VeriTaban�Olusturucu());
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Kategori>().HasMany(m => m.Makaleler).WithRequired(x => x.Kategori).WillCascadeOnDelete(true);

        //    modelBuilder.Entity<Makale>().HasMany(m => m.Yorumlar).WithRequired(x => x.Makale).WillCascadeOnDelete(true);

        //    modelBuilder.Entity<Makale>().HasMany(m => m.Begeniler).WithRequired(x => x.Makale).WillCascadeOnDelete(true);
        //}

    }

}