using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MakaleBLL;
using MakaleEntities;
using _01_PROJE_MVC.Models;
using _01_PROJE_MVC.Filter;

namespace _01_PROJE_MVC.Controllers
{
    [Exc]
    public class MakalesController : Controller
    {
        MakaleYonet my = new MakaleYonet();
        KategoriYonet ky = new KategoriYonet();
        BegeniYonet by = new BegeniYonet();

        // GET: Makales
        [Auth]
        public ActionResult Index()
        {
            if (SessionUser.Login != null)
            {
                return View(my.Listele().Where(x => x.Kullanici.ID == SessionUser.Login.ID));
            }
            return View(my.Listele());
        }

        // GET: Makales/Details/5
        [Auth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = my.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        // GET: Makales/Create
        [Auth]
        public ActionResult Create()
        {
            ViewBag.Kategori = new SelectList(CacheHelper.KategoriCache(), "ID", "Baslik");   // ky.Listele() gördüğümüz her yere CacheHelper.KategoriCache() yazdık, performans artırmak için.
            return View();
        }

        // POST: Makales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Makale makale)
        {
            ModelState.Remove("Kategori.Baslik");
            ModelState.Remove("DegistirenKullanici");
            ModelState.Remove("Kategori.DegistirenKullanici");
            ViewBag.kategori = new SelectList(CacheHelper.KategoriCache(), "ID", "Baslik", makale.Kategori.ID);
            if (ModelState.IsValid)
            {
                makale.Kullanici = SessionUser.Login;
                makale.Kategori = ky.KategoriBul(makale.Kategori.ID);
                MakaleBLL_Sonuc<Makale> sonuc = my.makaleEkle(makale);
                if (sonuc.hatalar.Count > 0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(makale);
                }

                return RedirectToAction("Index");
            }

            return View(makale);
        }

        // GET: Makales/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            Makale makale = my.MakaleBul(id.Value);

            ViewBag.kategori = new SelectList(CacheHelper.KategoriCache(), "ID", "Baslik", makale.Kategori.ID);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        // POST: Makales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Makale makale)
        {
            ViewBag.Kategori = new SelectList(CacheHelper.KategoriCache(), "ID", "Baslik", makale.Kategori.ID);
            ModelState.Remove("Kategori.Baslik");
            ModelState.Remove("DegistirenKullanici");
            ModelState.Remove("Kategori.DegistirenKullanici");
            if (ModelState.IsValid)
            {
                makale.Kategori = ky.KategoriBul(makale.Kategori.ID);
                MakaleBLL_Sonuc<Makale> sonuc = my.MakaleUpdate(makale);
                if (sonuc.hatalar.Count > 0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(makale);
                }

                my.MakaleUpdate(makale);
                return RedirectToAction("Index");
            }

            return View(makale);
        }

        // GET: Makales/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = my.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        // POST: Makales/Delete/5
        [Auth]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            my.MakaleSil(id);
            return RedirectToAction("Index");
        }

        [Auth]
        [HttpPost]
        public ActionResult MakaleGetir(int[] mid)
        {
            // mid = 1, 5, 9, 12, 15, 35
            // select * from begeni where kullanici_id = 5 and makale_id in (1, 5, 9, 12, 15, 35)
            BegeniYonet by = new BegeniYonet();
            List<int> mliste = null;
            if (SessionUser.Login != null)
            {
                mliste = by.Liste().Where(
                    x => x.Kullanici.ID == SessionUser.Login.ID && mid.Contains(
                    x.Makale.ID)).Select(
                    x => x.Makale.ID).ToList();
            }

            return Json(new { liste = mliste });
        }

        [Auth]
        [HttpPost]
        public ActionResult MakaleBegen(int makaleid, bool begeni)
        {
            Begeni like = this.by.BegeniBul(makaleid, SessionUser.Login.ID);
            Makale makale = this.my.MakaleBul(makaleid);
            int sonuc = 0;

            if (like != null && begeni == false)
            {
                sonuc = this.by.BegeniSil(like);
            }
            else if (like == null && begeni == true)
            {
                sonuc = this.by.BegeniEkle(new Begeni()
                {
                    Kullanici = SessionUser.Login,
                    Makale = makale
                }); ;
            }

            if (sonuc > 0)
            {
                if (begeni)
                {
                    makale.BegeniSayisi++;
                }
                else
                {
                    makale.BegeniSayisi--;
                }
                this.my.MakaleUpdate(makale);

                return Json(new { hata = false, begenisayisi = makale.BegeniSayisi });
            }
            else
            {
                return Json(new { hata = true, begenisayisi = makale.BegeniSayisi });
            }
        }

        
        public ActionResult MakaleGoster(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = my.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialPageMakaleGoster", makale);
        }


    }
}
