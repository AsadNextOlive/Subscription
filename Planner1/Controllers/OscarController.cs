using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planner1.Data;
using Planner1.Models;

namespace Planner1.Controllers
{
    public class OscarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OscarController(ApplicationDbContext context)
        {
            _context = context;
        } 

        public IActionResult Index()
        {
            var userEmail = HttpContext.Session.GetString("OscarUserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                //return RedirectToAction("SignIn", "PurchasedSignIn");
                return RedirectToAction("OscarSignIn", "Oscar");
            }
            return View();
        }

        public IActionResult SubscriptionExpired()
        {
            return View();
        }

        public async Task<IActionResult> OscarSignIn() 
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> OscarSignIn(OscarSignIn oscarSignIn)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Register.FirstOrDefaultAsync(x=>x.Email == oscarSignIn.Email);
                if (existingUser == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Email");
                }
                var pwd=await _context.Register.FirstOrDefaultAsync(x=>x.Password == oscarSignIn.Password);
                if (pwd == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Password");
                }
                if (existingUser != null && pwd != null)
                {
                    //check if user has a valid Subscription in PlanPurchased
                    var userSubscription = await _context.PlanPurchased
                        .Where(p=>p.Email == oscarSignIn.Email)
                        .OrderByDescending(p=>p.ExpiredDate)
                        .FirstOrDefaultAsync();

                    if (userSubscription != null && userSubscription.ExpiredDate>DateTime.Now)
                    {
                        //Storing email into the Session
                        HttpContext.Session.SetString("OscarUserEmail", oscarSignIn.Email);
                        return RedirectToAction("Index", "Oscar");
                    }
                    else
                    {
                        return RedirectToAction("SubscriptionExpired", "Oscar");
                    }

                }
            }
            return View(oscarSignIn);
        }

        public async Task<IActionResult> OscarSignOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("OscarSignIn", "Oscar");
        }
    }
}
