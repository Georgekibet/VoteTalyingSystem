using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using vts.Data.Repository.MasterData;
using vts.Shared.Entities.Master;
using vts.Shared.Repository;
using vts.Web.Helpers;
using vts.Web.Models;
using PagedList;
using vts.Core.Import.CsvHelpers;
using vts.Core.Import.Services;
using vts.Core.Shared.Entities.Master;
using vts.Shared.Services;
using Vts.WebLib.ViewModelBuilders.RegionViewModelBuilders;
using Vts.WebLib.ViewModels;

namespace vts.Web.Controllers.UI
{
    [Authorize]
    [AccessDeniedAuthorize(Roles = "Admin, Manager")]
    public class RegionController : Controller
    {
        
        private IRegionImportService _regionImportService;

        private IRegionViewModelBuilder _regionViewModelBuilder;

        public RegionController(IRegionImportService regionImportService, IRegionViewModelBuilder regionViewModelBuilder)
        {
            
            _regionImportService = regionImportService;
            _regionViewModelBuilder = regionViewModelBuilder;
        }

        // GET: Regions
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
            var rvmList = _regionViewModelBuilder.Query(query);

            var regionList = rvmList.Data;

            return View(regionList.ToPagedList(page, itemsperpage));
        }

        // GET: Regions/Details/5
        public ActionResult Details(Guid id)
        {
            if (id == Guid.Empty)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                var rvm = _regionViewModelBuilder.Get(id);
                return View(rvm);
            }
            catch (Exception ex)
            {

                ViewBag.msg = ex.Message;
                return View();
            }
        }

        // GET: Regions/Create
        public ActionResult Add()
        {
            DisplayConfigurationMessage();
            return View(new RegionViewModel());
        }

        // POST: Regions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(RegionViewModel rvm)
        {

            //rvm.County.Region.Name = _countyViewModelBuilder.Regions()[rvm.Region.Region.Id];
            try
            {
                rvm.Region.Id = Guid.NewGuid();
                _regionViewModelBuilder.Save(rvm);
                TempData["Msg"] = "Region successfully added";
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
            return View(rvm);
        }

        // GET: Regions/Edit/5
        public ActionResult Edit(Guid id)
        {
            DisplayConfigurationMessage();
            try
            {
                var rvm = _regionViewModelBuilder.Get(id);
                return View(rvm);
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex.Message;
                return View();
            }
        }

        // POST: Regions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RegionViewModel rvm)
        {
            
            try
            {
                _regionViewModelBuilder.Save(rvm);
                TempData["Msg"] = "Region successfully edited";
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
            return View();
        }

        // GET: Regions/Delete/5
        public ActionResult Delete(Guid id)
        {
            DisplayConfigurationMessage();
            if (id == Guid.Empty)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                var rvm = _regionViewModelBuilder.Get(id);
                return View(rvm);
            }
            catch (Exception ex)
            {

                ViewBag.msg = ex.Message;
                return View();
            }
        }

        // POST: Regions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                _regionViewModelBuilder.SetAsDeleted(id);
                TempData["Msg"] = "Region Successfully deleted";
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
                _regionViewModelBuilder.SetActive(id);
                TempData["Msg"] = "Region Successfully Activated";
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

        [AccessDeniedAuthorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import(HttpPostedFileBase file)
        {
           
            if (file != null)
            {
                try
                {
                    if (file.ContentLength > 0)
                    {
                        var fileName = string.Concat((object)Guid.NewGuid().ToString(), ".csv");
                        var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);

                        file.SaveAs(path);
                        var importResults = new RegionCsvReadWriteHelper().ReadCsv(path);
                        var imported = importResults.Imported.Count;
                        var ignored = importResults.Ignored.Count;
                        var totalRecords = importResults.TotalRecords;
                        var result = _regionImportService.Process(importResults.Imported);
                        

                        ViewBag.AlertMessage = $"File uploaded Successfully. Total records: {totalRecords}. Invalid Records: {ignored}. Saved Records: {result.Imported}. Existing Records. {result.NotImported}.";
                        ViewBag.AlertType = "alert-success";


                    }
                    else
                    {
                        ViewBag.AlertMessage = "Uploaded File is empty. No records imported";
                        ViewBag.AlertType = "alert-info";
                    }

                }
                catch (Exception ex)
                {
                    ViewBag.AlertMessage = "Exception occured! Message:" + ex.InnerException.Message;
                    ViewBag.AlertType = "alert-warning";
                }

            }
            else
            {
                ViewBag.AlertMessage = "Please select a CSV file to upload";
                ViewBag.AlertType = "alert-warning";
            }

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
