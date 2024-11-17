using Microsoft.AspNetCore.Mvc;
using OnePage2ABussiness.AboutUs.Models;
using OnePage2ABussiness.Users.Abstract;

namespace OnePage2AClient.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;

            // Fetch paginated users
            var users = await _userService.GetPagedUsersAsync(page, pageSize);

            // Fetch total user count for pagination
            int totalUsers = await _userService.GetTotalUserCountAsync();

            // Convert the result to a List to satisfy the view
            var paginatedUsers = users.ToList();

            // Set ViewBag properties for pagination
            ViewBag.TotalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);
            ViewBag.CurrentPage = page;

            // Pass the paginated user list to the view
            return View(paginatedUsers);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetActiveUser(int selectedUserId)
        {
            await _userService.SetActiveUserAsync(selectedUserId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
