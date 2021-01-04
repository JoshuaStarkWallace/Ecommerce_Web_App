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

        #region Index

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

        #endregion

        #region AddPage Methods

        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                // Declare slug
                string slug;

                // Init PageDTO
                PageDTO dto = new PageDTO();

                // DTO Title
                dto.Title = model.Title;

                // Check for and set Slug
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                // Check that Tile and Slug are unique
                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That title of slug already exists.");
                    return View(model);
                }

                // DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;

                // Save DTO
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            // Set the TempData message
            TempData["SuccessMessage"] = "You have added a new page!";

            // Redirect
            return RedirectToAction("AddPage");
        }

        #endregion

        #region EditPage Methods

        // GET: Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            // Declare PageVM
            PageVM model;

            using (Db db = new Db())
            {
                //Get the page
                PageDTO dto = db.Pages.Find(id);

                // Confirm page exists
                if (dto == null)
                {
                    return Content("The page does not exist.");
                }

                // Init PageVM
                model = new PageVM(dto);
            }

            //Return view with model

            return View(model);
        }

        // POST: Admin/Pages/EditPage/id
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                // Get page id
                int id = model.Id;

                // Init slug
                string slug = "home";

                // Get the page
                PageDTO dto = db.Pages.Find(id);

                // DTO the title
                dto.Title = model.Title;

                // Check for slug
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }

                // Make sure title and slug are unique
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) ||
                    db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That title of slug already exist.");
                    return View(model);
                }

                //DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;

                // Save the DTO
                db.SaveChanges();
            }

            // Set TempData message
            TempData["SuccessMessage"] = "You have edited the page!";

            // Redirect
            return RedirectToAction("EditPage");
        }

        #endregion

        #region PageDetails Methods

        // GET: Admin/Pages/PageDetails/id
        public ActionResult PageDetails(int id)
        {
            // Declare PageVM
            PageVM model;

            using (Db db = new Db())
            {
                // Get the page
                PageDTO dto = db.Pages.Find(id);

                // Confirm the page exists
                if (dto == null)
                {
                    return Content("The page does not exist.");
                }

                // Init PageVM
                model = new PageVM(dto);
            }

            // Return view with model
            return View(model);
        }

        #endregion

        #region DeletePage Methods

        // GET: Admin/Pages/DeletePage/id
        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {
                // Get the page
                PageDTO dto = db.Pages.Find(id);

                // Remove the page
                db.Pages.Remove(dto);

                // Save
                db.SaveChanges();
            }

                // Redirect
                return RedirectToAction("Index");
        }

        #endregion
    }
}