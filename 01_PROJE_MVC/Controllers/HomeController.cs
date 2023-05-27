using MakaleBLL;
using MakaleCommon;
using MakaleEntities;
using MakaleEntities.viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _01_PROJE_MVC.Models;
using System.Data.Entity;
using _01_PROJE_MVC.Filter;

namespace _01_PROJE_MVC.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        // GET: Home
        MakaleYonet my = new MakaleYonet();
        KategoriYonet ky = new KategoriYonet();
        KullaniciYonet kul_y = new KullaniciYonet();
        BegeniYonet be_y = new BegeniYonet();
        public ActionResult Index()
        {
            Test test = new Test();
            //test.EkleTest();
            //test.UpdateTest();
            //test.DeleteTest();
            //test.YorumTest();
            //int i = 1;
            //int s = i / 0;
            

            return View("Index", my.Listele().Where(x => x.Taslak == false).OrderByDescending(x => x.BegeniSayisi).ToList());
        }

        public ActionResult Kategori(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadGateway);
            }

            Kategori kat = ky.KategoriBul(id.Value);

            return View("Index", my.Listele().Where(x => x.Taslak == false).OrderByDescending(x => x.BegeniSayisi).ToList());
        }

        public ActionResult EnBegenilenler()
        {
            return View("Index", my.Listele().Where(x => x.Taslak == false).OrderByDescending(x => x.BegeniSayisi).ToList());
        }

        public ActionResult EnSonYazilanlar()
        {
            return View("Index", my.Listele().OrderByDescending(x => x.DegistirmeTarihi).ToList());
        }

        public ActionResult Hakkimizde()
        {
            return View();
        }

        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(loginModel model)
        {
            if (ModelState.IsValid)
            {
                MakaleBLL_Sonuc<Kullanici> sonuc = kul_y.loginKontrol(model);

                if (sonuc.hatalar.Count > 0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }
                Session["login"] = sonuc.nesne;
                Uygulama.login = sonuc.nesne.KullaniciAdi;
                return RedirectToAction("Index");
            }

            return View(model);
        }

        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////


        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {

            // kullanoco adı ve mail varmı kontrolu
            if (ModelState.IsValid)
            {
                MakaleBLL_Sonuc<Kullanici> sonuc = kul_y.KullaniciKaydet(model);

                if (sonuc.hatalar.Count > 0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }
                else
                {
                    return RedirectToAction("KayitBasarili");
                }
            }

            return View(model);
        }


        public ActionResult KayitBasarili()
        {
            return View();
        }

        public ActionResult HesapAktiflestir(Guid id)
        {
            MakaleBLL_Sonuc<Kullanici> sonuc = kul_y.ActivateUser(id);

            if (sonuc.hatalar.Count > 0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("ActivateError");
            }
            return View();

        }

        public ActionResult Cikis()
        {
            //Session["login"] = null;  aşağıdaki de olur
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult Error()
        {

            List<string> errors = new List<string>();

            if (TempData["hatalar"] != null)
            {
                ViewBag.hatalar = TempData["hatalar"];
            }
            else
            {
                ViewBag.hatalar = errors;
            }

            return View();
        }

        [Auth]
        public ActionResult Profil()
        {
            Kullanici kullanici = SessionUser.Login as Kullanici;

            MakaleBLL_Sonuc<Kullanici> sonuc = kul_y.KullaniciBul(kullanici.ID);

            if (sonuc.hatalar.Count > 0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("Error");
            }

            return View(sonuc.nesne);
        }

        [Auth]
        public ActionResult ProfilDegistir()
        {
            Kullanici kullanici = SessionUser.Login as Kullanici;
            MakaleBLL_Sonuc<Kullanici> sonuc = kul_y.KullaniciBul(kullanici.ID);

            if (sonuc.hatalar.Count > 0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("Error");
            }

            return View(sonuc.nesne);

        }

        [Auth]
        [HttpPost]
        public ActionResult ProfilDegistir(Kullanici model, HttpPostedFileBase profilResim)
        {
            ModelState.Remove("DegistirenKullanici");

            if (ModelState.IsValid)
            {
                if (profilResim != null &&
                    (profilResim.ContentType == "image/jpg" ||
                    profilResim.ContentType == "image/jpeg" ||
                    profilResim.ContentType == "image/png"))
                {
                    string dosya = $"user_{model.ID}.{profilResim.ContentType.Split('/')[1]}";
                    profilResim.SaveAs(Server.MapPath($"~/img/{dosya}"));
                    model.ProfilResimDosyaAdi = dosya;
                }
                Uygulama.login = model.KullaniciAdi;

                MakaleBLL_Sonuc<Kullanici> sonuc = kul_y.KullaniciUpdate(model);
                if (sonuc.hatalar.Count > 0)
                {
                    //TempData["hatalar"] = sonuc.hatalar;
                    //return RedirectToAction("Error");
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }
                SessionUser.Login = sonuc.nesne;
                return RedirectToAction("Profil");
            }
            else
            {
            return View(model);

            }

        }

        [Auth]
        public ActionResult ProfilSil()
        {
            Kullanici kullanici = SessionUser.Login as Kullanici;

            MakaleBLL_Sonuc<Kullanici> sonuc = kul_y.KullaniciSil(kullanici.ID);

            if (sonuc.hatalar.Count > 0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("Error");
            }

            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult YetkisizErisim()
        {
            return View();  
        }

        public ActionResult HataliIslem()
        {
            return View();
        }

        [Auth]
        public ActionResult Begendiklerim()
        {
            var query = be_y.ListQueryable().Include("Kullanici").Include("Makale").Where(
                x => x.Kullanici.ID == SessionUser.Login.ID).Select(
                x => x.Makale).Include("Kategori").Include("Kullanici").OrderByDescending(
                x => x.DegistirmeTarihi);

            return View("Index", query.ToList());  
        }
    }
}