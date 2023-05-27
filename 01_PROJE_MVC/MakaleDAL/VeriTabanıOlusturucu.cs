using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MakaleEntities;
using System.Data;

namespace MakaleDAL
{
    public class VeriTabanıOlusturucu : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            Kullanici Admin = new Kullanici()
            {
                Adi = "furkan",
                SoyAdi = "saltı",
                Email = "ahmetfurkansalti@hotmail.com",
                Admin = true,
                Aktif = true,
                KullaniciAdi = "athemKurfansal",
                Sifre = "123",
                AktifGuid = Guid.NewGuid(),
                KayitTarihi = DateTime.Now,
                DegistirmeTarihi = DateTime.Now.AddMinutes(5),
                DegistirenKullanici = $"user{1}"
            };
            context.Kullanicilar.Add(Admin);

            for (int i = 1; i < 6; i++)
            {
                Kullanici k = new Kullanici()
                {
                    Adi = FakeData.NameData.GetFirstName(),
                    SoyAdi = FakeData.NameData.GetSurname(),
                    Admin = false,
                    Aktif = true,
                    Email = FakeData.NetworkData.GetEmail(),
                    AktifGuid = Guid.NewGuid(),
                    KullaniciAdi = "user" + i.ToString(),
                    Sifre = $"user{i}",//string metodu kullandık yukardaki ile aynı 'kullaniciAdi' ile aynı yani
                    KayitTarihi = DateTime.Now.AddDays(-1),
                    DegistirmeTarihi = DateTime.Now,
                    DegistirenKullanici = "user" + i.ToString(),
                };
                context.Kullanicilar.Add(k);
            }
            context.SaveChanges();

            //////////////////////////////////// kullanıcı ekleme bitti - buradan sonra ise kategori ekleme yapacaz/////////////////////////
            List<Kullanici> kullanicilar = context.Kullanicilar.ToList();

            for (int i = 0; i < 5; i++)
            {// kategori ekleme
                Kategori kat = new Kategori()
                {
                    Baslik = FakeData.PlaceData.GetStreetName(),
                    Aciklama = FakeData.PlaceData.GetAddress(),
                    KayitTarihi = DateTime.Now,
                    DegistirenKullanici = "apo",
                    DegistirmeTarihi = DateTime.Now,
                };
                context.Kategoriler.Add(kat);

                // makale ekleme ve begeni ekleme
                for (int j = 0; j < 6; j++)
                {
                    Kullanici kul = kullanicilar[FakeData.NumberData.GetNumber(0, 5)];
                    Makale makale = new Makale()
                    {
                        Baslik = FakeData.TextData.GetAlphabetical(5),
                        Icerik = FakeData.TextData.GetSentences(2),
                        Taslak = false,
                        BegeniSayisi = FakeData.NumberData.GetNumber(2, 6),
                        KayitTarihi = DateTime.Now.AddDays(-2),
                        DegistirmeTarihi = DateTime.Now,
                        DegistirenKullanici = kul.KullaniciAdi,
                        Kullanici = kul

                    };

                    kat.Makaleler.Add(makale);

                    //// Yorum ekleme
                    for (int z = 0; z < 3; z++)
                    {
                        Kullanici yorum_kul = kullanicilar[FakeData.NumberData.GetNumber(0, 5)];
                        Yorum yorum = new Yorum()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            KayitTarihi = DateTime.Now.AddDays(z - 1),
                            DegistirmeTarihi = DateTime.Now,
                            DegistirenKullanici = yorum_kul.KullaniciAdi,
                            Kullanici = yorum_kul
                        };
                        makale.Yorumlar.Add(yorum);
                        //altaki yöntemde aynı işlemi yapıyor
                        //yorum.Makale=makale;
                        //context.Yorumlar.Add(yorum);
                    }
                    //begeni sayısı ekleme

                    for (int x = 0; x < makale.BegeniSayisi; x++)
                    {
                        Kullanici begenen_kul = kullanicilar[FakeData.NumberData.GetNumber(0, 5)];
                        Begeni begen = new Begeni()
                        {
                            Kullanici = begenen_kul
                        };
                        makale.Begeniler.Add(begen);
                    }
                }
            }
            context.SaveChanges();
        }
    }
}
