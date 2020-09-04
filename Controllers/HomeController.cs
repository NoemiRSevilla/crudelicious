using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using crudelicious.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

// Other using statements
namespace HomeController.Controllers
{
    public class HomeController : Controller
    {
        private crudeliciousContext dbContext;

        // here we can "inject" our context service into the constructor
        public HomeController(crudeliciousContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            List<Dish> AllDishes = dbContext.Dishes.OrderBy(dishes => dishes.CreatedAt).ToList();
            return View("Index", AllDishes);
        }

        [HttpPost("submit")]
        public IActionResult Submit(Dish newDish)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(newDish);
                dbContext.SaveChanges();
                return RedirectToAction("Show", newDish);
            }
            else
            {
                return View("New");
            }
        }

        [HttpGet("new")]
        public IActionResult New()
        {
            return View("New");
        }

        [HttpGet("{DishId}")]
        public IActionResult Show(int DishId)
        {
            Dish selectDish = dbContext.Dishes.FirstOrDefault(dish => dish.DishId == DishId);
            return View(selectDish);
        }
        
        [HttpGet("edit/{DishId}")]
        public IActionResult Edit(int DishId)
        {
            Dish retrievedDish = dbContext.Dishes.FirstOrDefault(dish => dish.DishId == DishId);
            HttpContext.Session.SetString("Name",retrievedDish.Name);
            HttpContext.Session.SetString("Chef", retrievedDish.Chef);
            HttpContext.Session.SetInt32("Calories", retrievedDish.Calories);
            HttpContext.Session.SetInt32("Tastiness", retrievedDish.Tastiness);
            HttpContext.Session.SetString("Description", retrievedDish.Description);
            ViewBag.Name = HttpContext.Session.GetString("Name");
            ViewBag.Chef = HttpContext.Session.GetString("Chef");
            ViewBag.Calories = HttpContext.Session.GetInt32("Calories");
            ViewBag.Tastiness = HttpContext.Session.GetInt32("Tastiness");
            ViewBag.Name = HttpContext.Session.GetString("Name");
            ViewBag.Description = HttpContext.Session.GetString("Description");
            return View("Edit", retrievedDish);
        }

        [HttpPost("/edit/{DishId}/submit")]
        public IActionResult EditSubmit(Dish updatedDish, int DishId)
        {
            Dish retrievedDish = dbContext.Dishes.FirstOrDefault(dish => dish.DishId == DishId);
            if (ModelState.IsValid)
            {
                retrievedDish.Name = updatedDish.Name;
                retrievedDish.Chef = updatedDish.Chef;
                retrievedDish.Tastiness =  updatedDish.Tastiness;
                retrievedDish.Calories = updatedDish.Calories;
                retrievedDish.Description = updatedDish.Description;
                retrievedDish.UpdatedAt = DateTime.Now;
                dbContext.SaveChanges();
                return Redirect ($"/{updatedDish.DishId}");
            }
            else
            {
                return View("Edit");
            }
        }

        [HttpGet("delete/{DishId}")]
        public IActionResult Delete (int DishId)
        {
            Dish retrievedDish = dbContext.Dishes.FirstOrDefault(dish => dish.DishId == DishId);
            dbContext.Dishes.Remove(retrievedDish);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}