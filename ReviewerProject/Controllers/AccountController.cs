using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReviewerProject.Models;
using ReviewerProject.ViewModels;

namespace ReviewerProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationContext _applicationContext;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationContext applicationContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationContext = applicationContext;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { FirstName = model.FirstName, UserName = model.Email, LastName = model.LastName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Password", "Incorrect either login or password");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string id, string searchResult)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Profile", new { id = _userManager.GetUserId(User) });
                }
                return RedirectToAction("Index", "Home");
            }

            if (Request.Query.ContainsKey("searchResult") && string.IsNullOrEmpty(searchResult))
            {
                return RedirectToAction("Profile", new { id });
            }

            var reviews = _applicationContext.Reviews.Join(_applicationContext.ReviewingTypes, t => t.ReviewingTypeKey, r => r.Id, (t, r) => new ReviewViewModel
            {
                Id = t.Id,
                UserKey = t.UserKey,
                Name = t.Name,
                Rating = t.Rating,
                Description = t.Description,
                ReviewingType = r.Name,
                CreatedDate = t.CreatedDate,
                WasUpdated = t.WasUpdated
            }).Where(u => u.UserKey == user.Id).ToList();
            if (reviews.IsNullOrEmpty())
            {

            }
            else
            {
                reviews = reviews.OrderByDescending(r => r.CreatedDate).ToList();
                if (!String.IsNullOrEmpty(searchResult))
                {
                    reviews = reviews.Where(r => r.Name.ToLower().Contains(searchResult.ToLower()) || r.Description.ToLower().Contains(searchResult.ToLower())).ToList();
                }
                user.Reviews = reviews;
            }
            ProfileViewModel profile = new ProfileViewModel(user, searchResult);
            return View(profile);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ProfileEdit()
        {
            User user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel editUserVM = new EditUserViewModel { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName };
            return View(editUserVM);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileEdit(EditUserViewModel model)
        {
            User user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            if (user.Id != model.Id)
            {
                return RedirectToAction("Index", "Home");
            }
            if (model.FirstName.IsNullOrEmpty())
            {
                ModelState.AddModelError("FirstName", "First name must contain something");
            }
            else if (model.FirstName.Length < 5 || model.FirstName.Length > 50)
            {
                ModelState.AddModelError("FirstName", "Length of the first name must be between 5 and 50");
            }
            if (model.Password.IsNullOrEmpty())
            {
                ModelState.AddModelError("Password", "Fill the password");
            }
            else
            {
                var passwordMatchResult = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!passwordMatchResult)
                {
                    ModelState.AddModelError("Password", "Wrong password");
                }
            }
            if (ModelState.IsValid)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile", "Account", new { Id = user.Id });

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ProfileDelete(EditUserViewModel model)
        {
            User user = await _userManager.FindByIdAsync(model.Id);
            if (model.Password.IsNullOrEmpty())
            {
                ModelState.AddModelError("Password", "Fill the password");
            }
            else
            {
                var passwordMatchResult = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!passwordMatchResult)
                {
                    ModelState.AddModelError("Password", "Wrong password");
                }
            }
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    IdentityResult result = await _userManager.DeleteAsync(user);
                }
                return RedirectToAction("Index", "Home");
            }

            return View("ProfileEdit", model);
        }
    }

}
