using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _01_PROJE_MVC.Filter;
using MakaleBLL;
using MakaleEntities;

namespace _01_PROJE_MVC.Controllers
{
    [Exc]
    [Auth]
    [AuthAdmin]
    public class KullaniciController : Controller
    {     
        KullaniciYonet ky = new KullaniciYonet();
        MakaleBLL_Sonuc<Kullanici> sonuc = new MakaleBLL_Sonuc<Kullanici>();
        // GET: Kullanici
        public ActionResult Index()
        {
            return View(ky.KullaniciListesi());
        }

        // GET: Kullanici/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sonuc = ky.KullaniciBul(id.Value);
            if (sonuc.nesne == null)
            {
                return HttpNotFound();
            }
            return View(sonuc.nesne);
        }

        // GET: Kullanici/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kullanici/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] 
        [ValidateAntiForgeryToken]
        public ActionResult Create(Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                ky.KullaniciKaydet(kullanici);
                return RedirectToAction("Index");
            }

            return View(kullanici);
        }

        // GET: Kullanici/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sonuc = ky.KullaniciBul(id.Value);
            if (sonuc.nesne == null)
            {
                return HttpNotFound();
            }
            return View(sonuc.nesne);
        }

        // POST: Kullanici/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                sonuc = ky.KullaniciUpdate(kullanici);
                return RedirectToAction("Index");
            }
            return View(kullanici);
        }

        // GET: Kullanici/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sonuc = ky.KullaniciBul(id.Value);
            if (sonuc.nesne == null)
            {
                return HttpNotFound();
            }
            return View(sonuc.nesne);
        }

        // POST: Kullanici/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sonuc = ky.KullaniciSil(id);
            return RedirectToAction("Index");
        }

    }
}
