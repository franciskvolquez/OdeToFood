using OdeToFood.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace OdeToFood.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private OdeToFoodContext _context = new OdeToFoodContext();

        public ActionResult Autocomplete(string term)
        {
            var model = _context.Restaurants
                .Where(r => r.Name.StartsWith(term))
                .Take(10)
                .Select(r => new { label = r.Name });

           return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Index(string searchTerm = null, int page =1 )
        {
            var model = _context.Restaurants
                .OrderByDescending(r => r.Reviews.Average(review => review.Rating))
                .Where(r => searchTerm == null || r.Name.StartsWith(searchTerm))
                .Select(r => new RestaurantListVM
                {
                    Id = r.Id,
                    Name = r.Name,
                    City = r.City,
                    Country = r.Country,
                    CountOfReviews = r.Reviews.Count()
                }).ToPagedList(page, 10); 

            if(Request.IsAjaxRequest())
            {
                return PartialView("_Restaurants", model);
            }

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if(_context != null)
            {
                _context.Dispose();
            }

            base.Dispose(disposing);    
        }
    }
}