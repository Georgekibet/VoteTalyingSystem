using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using PagedList;
using vts.Core.Import;
using vts.Core.Import.models;
using vts.Data.Context;
using vts.Shared.Entities.Master;
using vts.Shared.Repository;
using vts.Shared.Services;
using vts.Web.Helpers;
using vts.Web.Models;
using Vts.WebLib.ViewModelBuilders.CountyViewModelBuilders;
using Vts.WebLib.ViewModels.CountyViewModels;

namespace vts.Web.Controllers.UI
{
    [Authorize]
    [AccessDeniedAuthorize(Roles = "Admin, Manager")]
    public class CountyController : Controller
    {
      
        private ICountyViewModelBuilder _countyViewModelBuilder;


        public CountyController( ICountyViewModelBuilder countyViewModelBuilder)
        {
            
            _countyViewModelBuilder = countyViewModelBuilder;
        }

        // GET: Counties
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
            var cvmList = _countyViewModelBuilder.Query(query);

            var countyList = cvmList.Data;

            return View(countyList.ToPagedList(page, itemsperpage));

        }

        // GET: Counties/Details/5
        public ActionResult Details(Guid id)
        {
            if (id == Guid.Empty)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                var cvm = _countyViewModelBuilder.Get(id);
                return View(cvm);
            }
            catch (Exception ex)
            {

                ViewBag.msg = ex.Message;
                return View();
            }

        }

        // GET: Counties/Create
        public ActionResult Add()
        {
            ViewBag.RegionList = _countyViewModelBuilder.Regions();
            DisplayConfigurationMessage();
            return View(new CountyViewModel());
        }

        // POST: Counties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CountyViewModel cvm)
        {
            cvm.County.Region.Name = _countyViewModelBuilder.Regions()[cvm.County.Region.Id];
            try
            {
                cvm.County.Id = Guid.NewGuid();
                cvm.County.Region = cvm.County.Region;
                _countyViewModelBuilder.Save(cvm);
                TempData["Msg"] = "County successfully added";
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
            ViewBag.RegionList = _countyViewModelBuilder.Regions();
            return View(cvm);
        }

        // GET: Counties/Edit/5
        public ActionResult Edit(Guid id)
        {
            DisplayConfigurationMessage();
            ViewBag.RegionList = _countyViewModelBuilder.Regions();
            try
            {

                var cvm = _countyViewModelBuilder.Get(id);
                return View(cvm);
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex.Message;
                return View();
            }

        }

        // POST: Counties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CountyViewModel cvm)
        {
            cvm.County.Region.Name = _countyViewModelBuilder.Regions()[cvm.County.Region.Id];
            try
            {
                cvm.County.Region = cvm.County.Region;
                _countyViewModelBuilder.Save(cvm);
                TempData["Msg"] = "County successfully edited";
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
            ViewBag.RegionList = _countyViewModelBuilder.Regions();
            return View();
        }

        // GET: Counties/Delete/5
        public ActionResult Delete(Guid id)
        {
            DisplayConfigurationMessage();
            if (id == Guid.Empty)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                var cvm = _countyViewModelBuilder.Get(id);
                return View(cvm);
            }
            catch (Exception ex)
            {

                ViewBag.msg = ex.Message;
                return View();
            }
        }

        // POST: Counties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                _countyViewModelBuilder.SetAsDeleted(id);
                TempData["Msg"] = "County Successfully deleted";
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
                _countyViewModelBuilder.SetActive(id);
                TempData["Msg"] = "County Successfully Activated";
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
