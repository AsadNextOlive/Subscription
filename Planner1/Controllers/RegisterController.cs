using Microsoft.AspNetCore.Mvc;
using Planner1.Data;

namespace Planner1.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;
        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var registerList=_context.Register.ToList();
            return View(registerList);
        }
    }
}
