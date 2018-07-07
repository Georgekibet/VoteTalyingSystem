using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using vts.Shared.Entities.Master;
using vts.Shared.Repository;
using vts.Shared.Services;
using vts.Web.Helpers;
using vts.Web.Models;
using Vts.WebLib.ViewModelBuilders.PoliticalPartyViewModelBuilder;
using Vts.WebLib.ViewModels;

namespace vts.Web.Controllers.UI
{
    [Authorize]
    [AccessDeniedAuthorize(Roles = "Admin, Manager")]
    public class PoliticalPartyController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IPoliticalPartyRepository _politicalPartyRepository;
        private IPoliticalPartyViewModelBuilder _politicalPartyViewModelBuilder;

        public PoliticalPartyController(IPoliticalPartyRepository politicalPartyRepository, IPoliticalPartyViewModelBuilder politicalPartyViewModelBuilder)
        {
            _politicalPartyRepository = politicalPartyRepository;
            _politicalPartyViewModelBuilder = politicalPartyViewModelBuilder;
        }

        // GET: PoliticalParty
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name" : "";
            ViewBag.CodeSortParm = String.IsNullOrEmpty(sortOrder) ? "Code" : "";
            ViewBag.StatusSortParm = String.IsNullOrEmpty(sortOrder) ? "Status" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                searchString = searchString.ToLower();
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var politicalPartyList = _politicalPartyRepository.GetAll().ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                politicalPartyList = new List<PoliticalParty>(politicalPartyList.Where(r => r.Name.ToLower().Contains(searchString)
                || r.Code.ToString().ToLower().Contains(searchString)
                || r.Status.ToString().ToLower().Contains(searchString)
                || r.DateCreated.ToString().ToLower().Contains(searchString)));
            }
            switch (sortOrder)
            {
                case "Name":
                    politicalPartyList = new List<PoliticalParty>(politicalPartyList.OrderBy(s => s.Name));
                    break;

                case "Code":
                    politicalPartyList = new List<PoliticalParty>(politicalPartyList.OrderBy(s => s.Code));
                    break;

                case "Status":
                    politicalPartyList = new List<PoliticalParty>(politicalPartyList.OrderBy(s => s.Status));
                    break;

                case "Date":
                    politicalPartyList = new List<PoliticalParty>(politicalPartyList.OrderBy(s => s.DateCreated));
                    break;

                case "date_desc":
                    politicalPartyList = new List<PoliticalParty>(politicalPartyList.OrderByDescending(s => s.DateCreated));
                    break;

                default:
                    politicalPartyList = new List<PoliticalParty>(politicalPartyList.OrderBy(s => s.Name));
                    break;
            }

            ViewBag.AlertMessage = TempData["Msg"] ?? "";
            ViewBag.AlertType = TempData["Alrt"] ?? "";

            int pageSize = 30;
            int pageNumber = (page ?? 1);
            return View(politicalPartyList.ToPagedList(pageNumber, pageSize));
        }

        // GET: PoliticalParty/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PoliticalParty politicalParty = db.PoliticalParties.Find(id);
            if (politicalParty == null)
            {
                return HttpNotFound();
            }
            return View(politicalParty);
        }

        // GET: PoliticalParty/Create
        public ActionResult Add()
        {
            DisplayConfigurationMessage();
            return View(new PoliticalPartyViewModel());
        }

        // POST: PoliticalParty/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(PoliticalPartyViewModel ppvm)
        {
            try
            {
                ppvm.PoliticalParty.Id = Guid.NewGuid();
                _politicalPartyViewModelBuilder.Save(ppvm);
                TempData["Msg"] = "PoliticalParty added successfully";
                TempData["Alrt"] = "alert-success";
                return RedirectToAction("Index");
            }
            catch (DomainValidationException dve)
            {
                ViewBag.AlertMessage = dve.Message;
                ViewBag.AlertType = "alert-danger";
            }
            catch (Exception ex)
            {
                ViewBag.AlertMessage = ex.Message;
                ViewBag.AlertType = "alert-danger";
            }
            return View(ppvm);
        }

        // GET: PoliticalParty/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PoliticalParty politicalParty = db.PoliticalParties.Find(id);
            if (politicalParty == null)
            {
                return HttpNotFound();
            }
            return View(politicalParty);
        }

        // POST: PoliticalParty/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Code,TotalRegisteredVoters,DateCreated,DateLastUpdated,Status")] PoliticalParty politicalParty)
        {
            if (ModelState.IsValid)
            {
                db.Entry(politicalParty).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(politicalParty);
        }

        // GET: PoliticalParty/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PoliticalParty politicalParty = db.PoliticalParties.Find(id);
            if (politicalParty == null)
            {
                return HttpNotFound();
            }
            return View(politicalParty);
        }

        // POST: PoliticalParty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PoliticalParty politicalParty = db.PoliticalParties.Find(id);
            db.PoliticalParties.Remove(politicalParty);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AccessDeniedAuthorize(Roles = "Admin")]
        public ActionResult Import()
        {
            DisplayConfigurationMessage();
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Helpers

        private void DisplayConfigurationMessage()
        {
            ViewBag.AlertMessage = TempData["Msg"] ?? "";
            ViewBag.AlertType = TempData["Alrt"] ?? "";
        }

        #endregion Helpers
    }
}