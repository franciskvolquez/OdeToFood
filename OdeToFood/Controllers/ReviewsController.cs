using OdeToFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OdeToFood.Controllers
{
    public class ReviewsController : Controller
    {
        OdeToFoodContext _context = new OdeToFoodContext();

        // GET: Reviews
        public ActionResult Index([Bind(Prefix="id")]int restaurantId)
        {
            var restaurant = _context.Restaurants.Find(restaurantId);

            if(restaurant == null)
            {
                return HttpNotFound();
            }

            return View(restaurant);
        }

        [HttpGet]
        public ActionResult Create(int restaurantId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(RestaurantReview review)
        {
            if(ModelState.IsValid)
            {
                _context.Reviews.Add(review);
                _context.SaveChanges();
                return RedirectToAction("Index", new {restaurantId = review.RestaurantId });
            }

            return View(review);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = _context.Reviews.Find(id);
            
            if(model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(RestaurantReview review)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(review).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index", new { id = review.RestaurantId });
            }

            return View(review);
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
