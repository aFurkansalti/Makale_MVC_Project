using MakaleCommon;
using MakaleDAL;
using MakaleEntities;
using MakaleEntities.viewmodel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleBLL
{
    public class KullaniciYonet
    {
        Repository<Kullanici> rep_kul = new Repository<Kullanici>();
        MakaleBLL_Sonuc<Kullanici> sonuc = new MakaleBLL_Sonuc<Kullanici>();
        public MakaleBLL_Sonuc<Kullanici> ActivateUser(Guid id)
        {
            MakaleBLL_Sonuc<Kullanici> sonuc = new MakaleBLL_Sonuc<Kullanici>();
            sonuc.nesne = rep_kul.Find(x => x.AktifGuid == id);

            if (sonuc.nesne!=null)
            {
                if (sonuc.nesne.Aktif)
                {
                    sonuc.hatalar.Add("kullanıcı zaten daha önce kaydedilmiş");
                }
                else
                {
                    sonuc.nesne.Aktif = true;
                    rep_kul.Update(sonuc.nesne);
                }
            }
            else
            {
                sonuc.hatalar.Add("Aktifleştirelecek kullanıcı bulunamadı");
            }
            return sonuc;
        }

        public MakaleBLL_Sonuc<Kullanici> KullaniciBul(int ID)
        {
            MakaleBLL_Sonuc<Kullanici> sonuc = new MakaleBLL_Sonuc<Kullanici>();
            sonuc.nesne = rep_kul.Find(x => x.ID == ID);
            
            if (sonuc.nesne == null)
            {
                sonuc.hatalar.Add("Kullanıcı bulunamadı");
            }

            return sonuc;
        }

        public MakaleBLL_Sonuc<Kullanici> KullaniciKaydet(RegisterModel model)
        {
            MakaleBLL_Sonuc<Kullanici> sonuc = new MakaleBLL_Sonuc<Kullanici>();

            //sonuc.nesne = rep_kul.Find(x => x.KullaniciAdi == model.KullaniciAdi || x.Email == model.Email);

            //if (sonuc.nesne != null)
            //{
            //    if (sonuc.nesne.KullaniciAdi == model.KullaniciAdi)
            //    {
            //        sonuc.hatalar.Add("kullanıcı adı sistemde kayıtlı");
            //    }
            //    if (sonuc.nesne.Email == model.Email)
            //    {
            //        sonuc.hatalar.Add("bu mail adresi kayıtlı");
            //    }
            //}
            Kullanici nesne = new Kullanici();

            nesne.Email = model.Email;
            nesne.KullaniciAdi = model.KullaniciAdi;

            sonuc = KullaniciKontrol(nesne);

            if (sonuc.hatalar.Count > 0)
            {
                sonuc.nesne = nesne;
                return sonuc;
            }
            else
            {
                int islemsonuc = rep_kul.Insert(new Kullanici()
                {
                    KullaniciAdi = model.KullaniciAdi,
                    Email = model.Email,
                    Sifre = model.Sifre,
                    AktifGuid = Guid.NewGuid(),
                    ProfilResimDosyaAdi = "user_8.jpg"

                });
                if (islemsonuc > 0)
                {
                    sonuc.nesne = rep_kul.Find(x => x.KullaniciAdi == model.KullaniciAdi && x.Email == model.Email);
                    // buradaki insert yani yeni kullanıcı kaydı başarılı olduğunda 

                    // aktivasyon maili
                    string siteurl = ConfigHelper.Get<string>("SiteRootUri");
                    string aktivateurl = $"{siteurl}/Home/HesapAktiflestir/{sonuc.nesne.AktifGuid}";

                    string body = $"Merhaba " +
                        $"{sonuc.nesne.Adi} " +
                        $"{sonuc.nesne.SoyAdi} " +
                        $"<br /> Hesapınızı aktifleştirmek için <a href='{aktivateurl}' target='_blank'>tıklayınız</a> ";

                    MailHelper.SendMail(body,sonuc.nesne.Email,"Hesap Aktifleştirme");

                    
                }
            
                return sonuc;
            }
        }

            

        public MakaleBLL_Sonuc<Kullanici> KullaniciUpdate(Kullanici model)
        {
            sonuc = KullaniciKontrol(model);
            //if (kullanici != null && kullanici.ID != model.ID)
            //{
            //    if (kullanici.Email == model.Email)
            //    {
            //        sonuc.hatalar.Add("Bu email adresi zaten kayıtlı.");
            //    }

            //    if (kullanici.KullaniciAdi == model.KullaniciAdi)
            //    {
            //        sonuc.hatalar.Add("Bu kullanıcı adı zaten kayıtlı.");
            //    }
            //} 
            if (sonuc.hatalar.Count > 0)
            {
                sonuc.nesne = model;
                return sonuc;
            }
            else
            {
                sonuc.nesne = rep_kul.Find(x => x.ID == model.ID);

                sonuc.nesne.Adi = model.Adi;
                sonuc.nesne.SoyAdi = model.SoyAdi;
                sonuc.nesne.Email = model.Email;
                sonuc.nesne.KullaniciAdi = model.KullaniciAdi;
                sonuc.nesne.Sifre = model.Sifre;
                sonuc.nesne.ProfilResimDosyaAdi = model.ProfilResimDosyaAdi;

                if(rep_kul.Update(sonuc.nesne) < 1)
                {
                    sonuc.hatalar.Add("Profil bilgileri güncellenemedi");
                }
            }

            return sonuc;
        }

        public MakaleBLL_Sonuc<Kullanici> KullaniciKontrol(Kullanici kullanici)
        {
            Kullanici k1 = rep_kul.Find(x => x.Email == kullanici.Email);

            Kullanici k2 = rep_kul.Find(x => x.KullaniciAdi == kullanici.KullaniciAdi);

            if (k1 != null && k1.ID != kullanici.ID)
            {
                sonuc.hatalar.Add("Bu email adresi kayıtlı");
            }

            if (k2 != null && k2.ID!= kullanici.ID)
            {
                sonuc.hatalar.Add("Bu kullanıcı adı kayıtlı");
            }

            return sonuc;
        }

        public MakaleBLL_Sonuc<Kullanici> loginKontrol(loginModel model)
        {
            MakaleBLL_Sonuc<Kullanici> sonuc = new MakaleBLL_Sonuc<Kullanici>();

            sonuc.nesne = rep_kul.Find(x => x.KullaniciAdi == model.KullaniciAdi && x.Sifre == model.Sifre);

            if (sonuc.nesne == null)
            {
                sonuc.hatalar.Add("Kullanıcı Adı Yada Sifre hatalı");
            }
            else
            {
                if (!sonuc.nesne.Aktif)
                {
                    sonuc.hatalar.Add("kullanıcı aktifleştirmemiştir, lütfen e-postanızdan aktivasyon linkinden hesabınız aktif ediniz.");
                }
            }
            return sonuc;
        }

        public MakaleBLL_Sonuc<Kullanici> KullaniciSil(int id)
        {
            Kullanici kullanici = rep_kul.Find(x => x.ID == id);
            if (kullanici != null)
            {
                if (rep_kul.Delete(kullanici) < 1)
                {
                    sonuc.hatalar.Add("Kullanıcı silinemedi");
                }
            }
            else
            {
                sonuc.hatalar.Add("Kullanıcı bulunamadı");
            }

            return sonuc;
        }

        public List<Kullanici> KullaniciListesi()
        {
            return rep_kul.Liste();
        }

        public void KullaniciKaydet(Kullanici kullanici)
        {

        }

        
    }
}
