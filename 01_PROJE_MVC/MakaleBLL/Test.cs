using MakaleDAL;
using MakaleEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleBLL
{
    public class Test
    {
        Repository<Kullanici> rep_kul = new Repository<Kullanici>();
        public Test()
        {
            //DatabaseContext db= new DatabaseContext();
            //db.Kullanicilar.ToList();

            //database yoksa create eder  'db.Kullanicilar.ToList();' bunu yerine de kullanılabilir, buda aynı görevi görür
            //db.Database.CreateIfNotExists();


            // artık ripository miz var onu kullanacaz
                       
            List<Kullanici> sonuc = rep_kul.Liste();
            List<Kullanici> Liste = rep_kul.Liste(x=>x.Admin==true);

        }


        // test amaçlı bir ekleme yapıyoruz
        public void EkleTest()
        {
            rep_kul.Insert(new Kullanici()
            {
                Adi = "test",
                SoyAdi = "test",
                Admin=false,
                Aktif=false,
                AktifGuid=Guid.NewGuid(),
                Email="test",
                KayitTarihi=DateTime.Now,
                DegistirmeTarihi=DateTime.Now,
                DegistirenKullanici="admin",
                KullaniciAdi="test",
                Sifre="test",
            });
        }
        // update çalışıyormu onu test etmek için
        public void UpdateTest() 
        {
            Kullanici kul = rep_kul.Find(x=>x.KullaniciAdi=="test");

            //kullanıcı adı "test" olan kullanıcının adını ve soyadı "deneme" ile dğiştirdik test amaçlı
            if (kul != null)
            {
                kul.Adi = "deneme";
                kul.SoyAdi = "deneme";
                rep_kul.Update(kul);
            }
        }

        // delete test amaçlı bir tane kullanıcıyı silelim
        public void DeleteTest()
        {
            Kullanici kul = rep_kul.Find(x => x.KullaniciAdi == "test");

            //kullanıcı adı "test" olan kullanıcıyı siler
            if (kul != null)
            {
                rep_kul.Delete(kul);
            }
        }

        Repository<Makale> rep_makale= new Repository<Makale>();
        Repository<Yorum> rep_yorum= new Repository<Yorum>();
        public void YorumTest()
        {
            Makale m = rep_makale.Find(x => x.ID == 2);
            Yorum y=rep_yorum.Find(x=>x.ID == 2);
            Kullanici k = rep_kul.Find(x => x.ID == 1);

            Yorum yorum = new Yorum()
            {
                Text = "deneme yorumu",
                KayitTarihi = DateTime.Now,
                DegistirmeTarihi = DateTime.Now,
                DegistirenKullanici = "admin",
                Kullanici = k,
                Makale = m               
            };
            rep_yorum.Insert(yorum);
        }
    }
}
