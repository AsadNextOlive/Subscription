using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planner1.Data;
using Planner1.Models;
using System.Threading.Tasks;

namespace Planner1.Controllers
{
    public class PlansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //check if email is stored in the session
            var userEmail = HttpContext.Session.GetString("PlansUserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("PlansSignIn", "Plans");
            }

            var ListOfPlans = _context.Plans.ToList();
            return View(ListOfPlans);
        }

        //Clearing Session
        public IActionResult SignOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("PlansSignIn", "Plans");
        }

        
        public async Task<IActionResult> PlansSignIn()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> PlansSignIn(PlansSignIn plansSignIn)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Register.FirstOrDefaultAsync(x => x.Email == plansSignIn.Email);
                if (existingUser == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Username");
                }
                var Pwd = await _context.Register.FirstOrDefaultAsync(x => x.Email == plansSignIn.Email && x.Password == plansSignIn.Password);
                if (Pwd == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Password");
                }
                if (existingUser != null && Pwd != null)
                {
                    //Storing Email into the Session
                    HttpContext.Session.SetString("PlansUserEmail", plansSignIn.Email);
                    return RedirectToAction("Index", "Plans");
                }
            }
            return View(plansSignIn);
        }

        [HttpPost]
        public async Task<IActionResult> BuyNow(string planName, int price, int duration)
        {
            //check if user is Authendicated
            var userEmail = HttpContext.Session.GetString("PlansUserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("PlansSignIn", "Plans");
            }

            //Create a PurchasedPlan object with the purchase details
            var planPurchased = new PlanPurchased
            {
                Email = userEmail,
                PlanName = planName,
                Price = price,
                Duration = duration,
                PurchasedDate = DateTime.Now,
                ExpiredDate = DateTime.Now.AddMinutes(duration)
            };

            //Add the purchase to the database
            _context.PlanPurchased.Add(planPurchased);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Plans");
        }
    }
}
