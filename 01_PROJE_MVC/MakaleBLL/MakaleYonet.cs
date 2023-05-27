using MakaleDAL;
using MakaleEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleBLL
{
    public class MakaleYonet
    {
        Repository<Makale> rep_makale = new Repository<Makale>();
        MakaleBLL_Sonuc<Makale> sonuc = new MakaleBLL_Sonuc<Makale>();

        public List<Makale> Listele()
        {
            return rep_makale.Liste();
        }

        public IQueryable<Makale> ListQueryable()
        {
            return rep_makale.ListQueryable();
        }

        public Makale MakaleBul(int id)
        {
            return rep_makale.Find(x => x.ID == id);
        }

        public MakaleBLL_Sonuc<Makale> makaleEkle(Makale makale)
        {
            sonuc.nesne = rep_makale.Find(x => x.Baslik == makale.Baslik && x.Kategori.ID == makale.Kategori.ID);

            if (sonuc.nesne != null)
            {
                sonuc.hatalar.Add("Bu makale kayıtlı");
            }
            else
            {
                if (rep_makale.Insert(makale) < 1)
                {
                    sonuc.hatalar.Add("Makale eklenemedi");
                }
            }

            return sonuc;
        }

        public MakaleBLL_Sonuc<Makale> MakaleSil(int id)
        {
            sonuc.nesne = rep_makale.Find(x => x.ID == id);

            if(sonuc.nesne != null)
            {
                if(rep_makale.Delete(sonuc.nesne) < 1)
                {
                    sonuc.hatalar.Add("Makale silinemedi");
                }
            }
            else
            {
                sonuc.hatalar.Add("Makale bulunamadı");
            }

            return sonuc;
        }

        public MakaleBLL_Sonuc<Makale> MakaleUpdate(Makale makale)
        {
            Makale nesne = rep_makale.Find(x => x.Baslik == makale.Baslik && x.Kategori.ID == makale.Kategori.ID && x.ID != makale.ID);

            if (nesne != null)
            {
                sonuc.hatalar.Add("Bu makale kayıtlı");
            }
            else
            {
                sonuc.nesne = rep_makale.Find(x => x.ID == makale.ID);
                sonuc.nesne.Kategori = makale.Kategori;
                sonuc.nesne.Baslik = makale.Baslik;
                sonuc.nesne.Icerik = makale.Icerik;
                sonuc.nesne.Taslak = makale.Taslak;
                if (rep_makale.Update(sonuc.nesne) < 1)
                {
                    sonuc.hatalar.Add("Makale güncellenemedi");
                }
            }
            return sonuc;
        }
    }
}
