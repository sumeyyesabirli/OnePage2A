using Microsoft.AspNetCore.Mvc;
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
            var users = await _userService.GetPagedUsersAsync(page, pageSize);
            var totalUserCount = await _userService.GetTotalUserCountAsync();

            ViewBag.TotalPages = (int)Math.Ceiling(totalUserCount / (double)pageSize);
            ViewBag.CurrentPage = page;

            return View(users);
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
