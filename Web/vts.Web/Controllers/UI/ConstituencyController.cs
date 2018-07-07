using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using vts.Shared.Services;
using vts.Web.Helpers;
using vts.Web.Models;
using Vts.WebLib.ViewModelBuilders.ConstituencyViewModelBuilders;
using Vts.WebLib.ViewModels;

namespace vts.Web.Controllers.UI
{
    [Authorize]
    [AccessDeniedAuthorize(Roles = "Admin, Manager")]
    public class ConstituencyController : Controller
    {
        private IConstituencyViewModelBuilder _constituencyViewModelBuilder;

        public ConstituencyController(IConstituencyViewModelBuilder constituencyViewModelBuilder)
        {
            _constituencyViewModelBuilder = constituencyViewModelBuilder;
        }

        // GET: Constituency
        public ActionResult Index(string searchString, bool showInactive = false, int itemsperpage = 10, int page = 1)
        {
            DisplayConfigurationMessage();
            ViewModelBase.ItemsPerPage = itemsperpage;

            ViewBag.showInactive = showInactive;
            ViewBag.SearchString = searchString;

            int currentPageIndex = page - 1 < 0 ? 0 : page - 1;
            int take = itemsperpage;
            int skip = take * currentPageIndex;
            var query = new QueryStandard()
            {
                IncludeDeactivated = showInactive,
                Skip = skip,
                Take = take,
                Description = searchString
            };
            var cvmList = _constituencyViewModelBuilder.Query(query);

            var constituencyList = cvmList.Data;

            return View(constituencyList.ToPagedList(page, itemsperpage));
        }

        // GET: Constituency/Details/5
        public ActionResult Details(Guid id)
        {
            if (id == Guid.Empty)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                var cvm = _constituencyViewModelBuilder.Get(id);
                return View(cvm);
            }
            catch (Exception ex)
            {

                ViewBag.msg = ex.Message;
                return View();
            }
        }

        // GET: Constituency/Create
        public ActionResult Add()
        {
            ViewBag.RegionList = _constituencyViewModelBuilder.Counties();
            DisplayConfigurationMessage();
            return View(new ConstituencyViewModel());
        }

        // POST: Constituency/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ConstituencyViewModel cvm)
        {
            cvm.Constituency.County.Name = _constituencyViewModelBuilder.Counties()[cvm.Constituency.County.Id];
            try
            {
                cvm.Constituency.Id = Guid.NewGuid();
                cvm.Constituency.County = cvm.Constituency.County;
                _constituencyViewModelBuilder.Save(cvm);
                TempData["Msg"] = "Constituency successfully added";
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
            ViewBag.RegionList = _constituencyViewModelBuilder.Counties();
            return View(cvm);
        }

        // GET: Constituency/Edit/5
        public ActionResult Edit(Guid id)
        {
            DisplayConfigurationMessage();
            ViewBag.RegionList = _constituencyViewModelBuilder.Counties();
            try
            {

                var cvm = _constituencyViewModelBuilder.Get(id);
                return View(cvm);
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex.Message;
                return View();
            }
        }

        // POST: Constituency/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ConstituencyViewModel cvm)
        {
            cvm.Constituency.County.Name = _constituencyViewModelBuilder.Counties()[cvm.Constituency.County.Id];
            try
            {
                cvm.Constituency.County = cvm.Constituency.County;
                _constituencyViewModelBuilder.Save(cvm);
                TempData["Msg"] = "Constituency successfully edited";
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
            ViewBag.RegionList = _constituencyViewModelBuilder.Counties();
            return View();
        }

        // GET: Constituency/Delete/5
        public ActionResult Delete(Guid id)
        {
            DisplayConfigurationMessage();
            if (id == Guid.Empty)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                var cvm = _constituencyViewModelBuilder.Get(id);
                return View(cvm);
            }
            catch (Exception ex)
            {

                ViewBag.msg = ex.Message;
                return View();
            }
        }

        // POST: Constituency/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                _constituencyViewModelBuilder.SetAsDeleted(id);
                TempData["Msg"] = "Constituency Successfully deleted";
                TempData["Alrt"] = "alert-success";
            }
            catch (DomainValidationException dve)
            {
                TempData["Msg"] = dve.Message;
                TempData["Alrt"] = "alert-danger";
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                TempData["Alrt"] = "alert-danger";


            }

            return RedirectToAction("Index");
        }

        public ActionResult Activate(Guid id)
        {
            try
            {
                _constituencyViewModelBuilder.SetActive(id);
                TempData["Msg"] = "Constituency Successfully Activated";
                TempData["Alrt"] = "alert-success";
            }
            catch (DomainValidationException dve)
            {
                TempData["Msg"] = dve.Message;
                TempData["Alrt"] = "alert-danger";
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                TempData["Alrt"] = "alert-danger";


            }
            return RedirectToAction("Index");
        }

        [AccessDeniedAuthorize(Roles = "Admin")]
        public ActionResult Import()
        {

            DisplayConfigurationMessage();
            return View();
        }

        #region Helpers
        private void DisplayConfigurationMessage()
        {
            ViewBag.AlertMessage = TempData["Msg"] ?? "";
            ViewBag.AlertType = TempData["Alrt"] ?? "";
        }
        #endregion
    }
}
