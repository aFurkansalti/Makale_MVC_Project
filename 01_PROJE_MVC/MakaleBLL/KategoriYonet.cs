using MakaleDAL;
using MakaleEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleBLL
{
    public class KategoriYonet
    {
        Repository<Kategori> rep_kat = new Repository<Kategori>();
        MakaleBLL_Sonuc<Kategori> sonuc = new MakaleBLL_Sonuc<Kategori>();
        public List<Kategori> Listele()
        {
            return rep_kat.Liste();
        }
        public Kategori KategoriBul(int id)
        {
            return rep_kat.Find(x => x.ID == id);
        }

        public MakaleBLL_Sonuc<Kategori> KategoriEkle(Kategori model)
        {
            sonuc.nesne = rep_kat.Find(x => x.Baslik == model.Baslik);

            if (sonuc.nesne != null)
            {
                sonuc.hatalar.Add("Bu kategori kayıtlı");
                return sonuc;
            }
            else
            {
                if (rep_kat.Insert(model) < 1)
                {
                    sonuc.hatalar.Add("Kategori kaydedilemedi.");
                }
            }
            return sonuc;
        }

        public MakaleBLL_Sonuc<Kategori> KategoriUpdate(Kategori model)
        {
            sonuc.nesne = rep_kat.Find(x => x.ID == model.ID);
            Kategori kategori = rep_kat.Find(x => x.Baslik == model.Baslik && x.ID != model.ID);

            if (sonuc.nesne != null && kategori == null)
            {
                sonuc.nesne.Baslik = model.Baslik;
                sonuc.nesne.Aciklama = model.Aciklama;
                if (rep_kat.Update(sonuc.nesne) < 1)
                {
                    sonuc.hatalar.Add("Kategori güncellenemedi.");
                }
            }
            else
            {
                if (kategori != null)
                {
                    sonuc.hatalar.Add("Bu kategori zaten kayıtlı.");
                }
                else
                {
                    sonuc.hatalar.Add("Kategori bulunamadı.");
                }
            }
            return sonuc;
        }

        public void KategoriSil(int id)
        {
            Kategori kategori = sonuc.nesne = rep_kat.Find(x => x.ID == id);

            Repository<Makale> rep_makale = new Repository<Makale>();
            Repository<Yorum> rep_yorum = new Repository<Yorum>();
            Repository<Begeni> rep_begen = new Repository<Begeni>();    

            //Kategorinin Makalelerini sil
            foreach (var item in kategori.Makaleler.ToList())
            {
                //Makalenin yorumlarını sil
                foreach (Yorum yorum in item.Yorumlar.ToList())
                {
                    
                    rep_yorum.Delete(yorum);
                }
                //Makalenin beğenilerini sil

                foreach (Begeni begen in item.Begeniler.ToList())
                {
                    rep_begen.Delete(begen);
                }

                rep_makale.Delete(item);
            }

            rep_kat.Delete(sonuc.nesne);
        }
    }
}
