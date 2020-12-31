using Ecommerce_Web_App.Models.Data;
using Ecommerce_Web_App.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce_Web_App.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            // Declare list of PageVM
            List<PageVM> PagesList;

            using (Db db = new Db())
            {
                // Init the list
                PagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }

            // Return view with list
            return View(PagesList);
        }
    }
}