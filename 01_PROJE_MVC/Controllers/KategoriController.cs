using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MakaleEntities;

using MakaleBLL;
using _01_PROJE_MVC.Models;
using _01_PROJE_MVC.Filter;

namespace _01_PROJE_MVC.Controllers
{
    [Exc]
    [Auth] // Hepsi için vermiş olduk
    [AuthAdmin]
    public class KategoriController : Controller
    {

        KategoriYonet ky = new KategoriYonet();

        // GET: Kategori
        public ActionResult Index()
        {
            return View(ky.Listele());
        }

        // GET: Kategori/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = ky.KategoriBul(id.Value);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        // GET: Kategori/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kategori/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Kategori kategori)
        {
            ModelState.Remove("DegistirenKullanici");
            if (ModelState.IsValid)
            {
                MakaleBLL_Sonuc<Kategori> sonuc = ky.KategoriEkle(kategori);
                ky.KategoriEkle(kategori);
                if (sonuc.hatalar.Count > 0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View();  
                }
                CacheHelper.CacheTemizle();
                return RedirectToAction("Index");
            }

            return View(kategori);
        }

        // GET: Kategori/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = ky.KategoriBul(id.Value);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        // POST: Kategori/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Kategori kategori)
        {
            ModelState.Remove("DegistirenKullanici");
            if (ModelState.IsValid)
            {
                //db.Entry(kategori).State = EntityState.Modified;
                //db.SaveChanges();
                MakaleBLL_Sonuc<Kategori> sonuc = ky.KategoriEkle(kategori);
                sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                return View("Index");
            }
            CacheHelper.CacheTemizle();
            return View(kategori);
        }

        // GET: Kategori/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = ky.KategoriBul(id.Value);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        // POST: Kategori/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ky.KategoriSil(id);
            CacheHelper.CacheTemizle();
            return RedirectToAction("Index");
        }

       


    }
}
