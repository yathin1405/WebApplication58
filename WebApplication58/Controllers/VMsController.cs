using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication58.Models;


namespace WebApplication58.Controllers
{
    public class VMsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: VMs
        public ActionResult Index()
        {
            return View(db.VMs.ToList());
        }
        [ChildActionOnly]
        public ActionResult PersonalP()
        {
            return PartialView("PersonalData");
        }
        [ChildActionOnly]
        public ActionResult WalkinP()
        {
            ViewBag.RoomNo = new SelectList(db.Rooms, "RoomNo", "RoomNo");
            ViewBag.RoomTypeId = new SelectList(db.roomTypes, "RoomTypeId", "Type");
            ViewBag.ID = new SelectList(db.personalData, "ID", "Email");

            return PartialView("WalkinReserve");
        }

        [ChildActionOnly]
        public ActionResult JoinP()
        {
            List<VM> vm = new List<VM>();

            var list = (from p in db.personalData
                        join w in db.walkReserve on p.ID equals w.ID
                        join r in db.Rooms on w.RoomNo equals r.RoomNo
                        join rt in db.roomTypes on w.RoomTypeId equals rt.RoomTypeId
                        select new
                        {
                            p.Email,
                            p.Tel,

                            w.CheckinDate,
                            w.CheckoutDate,
                            w.NumDays,
                            w.NumGuests,
                            w.TotalAmount,

                            r.RoomNo,
                            rt.Type,

                        }).ToList();
            foreach (var x in list)
            {
                VM v = new VM();


                v.Email = x.Email;
                v.Tel = x.Tel;
                v.CheckinDate = x.CheckinDate;
                v.CheckoutDate = x.CheckoutDate;
                v.NumDays = x.NumDays;
                v.NumGuests = x.NumGuests;
                v.TotalAmount = x.TotalAmount;
                v.RoomNo = x.RoomNo;
                v.Type = x.Type;

                vm.Add(v);
            }
            return PartialView("Join", vm);
        }


        [HttpPost]
        public ActionResult CreatePersonal(PersonalData personalData)
        {
            PersonalData p = new PersonalData()
            {
                Email = personalData.Email,
                Tel = personalData.Tel,
            };
            db.personalData.Add(p);
            db.SaveChanges();
            return View("Display");
        }

        [HttpPost]
        public ActionResult CreateWalkin(WalkinReserve walkin)
        {
            WalkinReserve w = new WalkinReserve()
            {
                CheckinDate = walkin.CheckinDate,
                CheckoutDate = walkin.CheckoutDate,
                NumGuests = walkin.NumGuests,
                NumDays = walkin.NumDays,
                TotalAmount = walkin.CalcBasicPrice(),
                ID = walkin.ID,
                RoomNo = walkin.RoomNo,
                RoomTypeId = walkin.RoomTypeId,
            };
            db.walkReserve.Add(w);
            db.SaveChanges();
            return View("Display");
        }

        public ActionResult Display()
        {
            return View();
        }


        // GET: VMs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VM vM = db.VMs.Find(id);
            if (vM == null)
            {
                return HttpNotFound();
            }
            return View(vM);
        }

        // GET: VMs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VMK,Email,Tel,CheckinDate,CheckoutDate,NumGuests,NumDays,TotalAmount,RoomNo,Type")] VM vM)
        {
            if (ModelState.IsValid)
            {
                db.VMs.Add(vM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vM);
        }

        // GET: VMs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VM vM = db.VMs.Find(id);
            if (vM == null)
            {
                return HttpNotFound();
            }
            return View(vM);
        }

        // POST: VMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VMK,Email,Tel,CheckinDate,CheckoutDate,NumGuests,NumDays,TotalAmount,RoomNo,Type")] VM vM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vM);
        }

        // GET: VMs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VM vM = db.VMs.Find(id);
            if (vM == null)
            {
                return HttpNotFound();
            }
            return View(vM);
        }

        // POST: VMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VM vM = db.VMs.Find(id);
            db.VMs.Remove(vM);
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
