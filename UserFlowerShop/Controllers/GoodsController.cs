using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UserFlowerShop.Models;

namespace UserFlowerShop.Controllers
{
    public class GoodsController : Controller
    {
        private FlowerEntities db = new FlowerEntities();

        // GET: Goods
        public ActionResult Index()
        {
            var goods = db.Goods.Include(g => g.Categories).Include(g => g.Manufacturer);
            return View(goods.ToList());
        }

        // GET: Goods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goods goods = db.Goods.Find(id);
            if (goods == null)
            {
                return HttpNotFound();
            }
            return View(goods);
        }

        // GET: Goods/Create
        public ActionResult Create()
        {
            ViewBag.IdCategory = new SelectList(db.Categories, "ID", "Name");
            ViewBag.IdManufacturer = new SelectList(db.Manufacturer, "ID", "Manufacturer1");
            return View();
        }

        // POST: Goods/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IdManufacturer,IdCategory,Name,Description,Price,Quantity,Availability,Image")] Goods goods)
        {
            if (ModelState.IsValid)
            {
                db.Goods.Add(goods);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCategory = new SelectList(db.Categories, "ID", "Name", goods.IdCategory);
            ViewBag.IdManufacturer = new SelectList(db.Manufacturer, "ID", "Manufacturer1", goods.IdManufacturer);
            return View(goods);
        }

        // GET: Goods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goods goods = db.Goods.Find(id);
            if (goods == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCategory = new SelectList(db.Categories, "ID", "Name", goods.IdCategory);
            ViewBag.IdManufacturer = new SelectList(db.Manufacturer, "ID", "Manufacturer1", goods.IdManufacturer);
            return View(goods);
        }

        // POST: Goods/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IdManufacturer,IdCategory,Name,Description,Price,Quantity,Availability,Image")] Goods goods)
        {
            if (ModelState.IsValid)
            {
                db.Entry(goods).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCategory = new SelectList(db.Categories, "ID", "Name", goods.IdCategory);
            ViewBag.IdManufacturer = new SelectList(db.Manufacturer, "ID", "Manufacturer1", goods.IdManufacturer);
            return View(goods);
        }

        // GET: Goods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goods goods = db.Goods.Find(id);
            if (goods == null)
            {
                return HttpNotFound();
            }
            return View(goods);
        }

        // POST: Goods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Goods goods = db.Goods.Find(id);
            db.Goods.Remove(goods);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
