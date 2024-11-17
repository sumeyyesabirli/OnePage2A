using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnePage2ADataAccess.Contexts;

namespace OnePage2AClient.Controllers
{
    //[Authorize(Roles = "admin,editor")]
    public class BaseController : Controller
    {
        protected readonly DbContext2A _context;
        protected readonly IWebHostEnvironment _webHostEnvironment;

        public BaseController(DbContext2A context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
    }
}
