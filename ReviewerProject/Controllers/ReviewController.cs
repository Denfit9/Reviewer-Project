using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReviewerProject.Models;
using ReviewerProject.ViewModels;

namespace ReviewerProject.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationContext _applicationContext;
        private readonly UserManager<User> _userManager;

        public ReviewController(ApplicationContext applicationContext, UserManager<User> userManager)
        {
            _applicationContext = applicationContext;
            _userManager = userManager;
        }

        public bool IsReallyChanged(Review review, EditReviewViewModel viewModel)
        {
            if(review.Name != viewModel.Name)
            {
                return true;
            }
            if(review.Description != viewModel.Description)
            {
                return true;
            }
            if(review.ReviewingTypeKey != viewModel.ReviewingTypeKey)
            {
                return true;
            }
            if(review.Rating != viewModel.Rating)
            {
                return true;
            }
            return false;
        }

        [Route("Review")]
        public async Task<IActionResult> Review(Guid id)
        {
            var review = await _applicationContext.Reviews.FindAsync(id);
            review.ReviewingType = await _applicationContext.ReviewingTypes.FindAsync(review.ReviewingTypeKey);
            ReviewExactViewModel reviewExactViewModel = new ReviewExactViewModel
            {
                Id = review.Id,
                Name = review.Name,
                Description = review.Description,
                Rating = review.Rating,
                ReviewingTypeKey = review.ReviewingTypeKey,
                ReviewingType = review.ReviewingType,
                UserKey = review.UserKey,
                CreatedDate = review.CreatedDate,
                WasUpdated = review.WasUpdated,
                User = await _userManager.FindByIdAsync(review.UserKey)
            };
            return View(reviewExactViewModel);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ReviewAdd()
        {
            AddReviewViewModel addReviewViewModel = new AddReviewViewModel { ReviewingTypes = _applicationContext.ReviewingTypes.ToList(), Ratings = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] };
            return View(addReviewViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ReviewAdd(AddReviewViewModel addReviewViewModel)
        {
            User user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            if (addReviewViewModel.Name.IsNullOrEmpty())
            {
                ModelState.AddModelError("Name", "Title must contain something");
            }
            if (addReviewViewModel.Description.IsNullOrEmpty())
            {
                ModelState.AddModelError("Description", "Desctiption must contain some information");
            }
            if (addReviewViewModel.ReviewingTypeKey == null)
            {
                ModelState.AddModelError("ReviewingTypeKey", "Choose type of an object of the review");
            }
            if (addReviewViewModel.Rating == null)
            {
                ModelState.AddModelError("Rating", "Choose final rating");
            }
            if (ModelState.IsValid)
            {
                Review review = new Review
                {
                    Name = addReviewViewModel.Name,
                    Description = addReviewViewModel.Description,
                    Rating = (int)addReviewViewModel.Rating,
                    ReviewingTypeKey = (Guid)addReviewViewModel.ReviewingTypeKey,
                    ReviewingType = await _applicationContext.ReviewingTypes.FindAsync((Guid)addReviewViewModel.ReviewingTypeKey),
                    UserKey = user.Id,
                    User = user,
                    CreatedDate = DateTime.UtcNow
                };
                if (review != null)
                {
                    await _applicationContext.AddAsync(review);
                    await _applicationContext.SaveChangesAsync();
                }
                return RedirectToAction("Profile", "Account", new { Id = user.Id });
            }

            return View(addReviewViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ReviewEdit(Guid id)
        {
            var exactReview = await _applicationContext.Reviews.FindAsync(id);
            EditReviewViewModel editReviewViewModel = new EditReviewViewModel
            {
                Name = exactReview.Name,
                Description = exactReview.Description,
                Id = exactReview.Id,
                Rating = exactReview.Rating,
                ReviewingTypeKey = exactReview.ReviewingTypeKey,
                ReviewingTypes = _applicationContext.ReviewingTypes.ToList(),
                Ratings = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
            };
            return View(editReviewViewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReviewEdit(EditReviewViewModel editReviewViewModel)
        {
            User user = await _userManager.GetUserAsync(User);
            Review review = await _applicationContext.Reviews.FindAsync(editReviewViewModel.Id);
            if (user == null)
            {
                return NotFound();
            }
            if (editReviewViewModel.Name.IsNullOrEmpty())
            {
                ModelState.AddModelError("Name", "Title must contain something");
            }
            if (editReviewViewModel.Description.IsNullOrEmpty())
            {
                ModelState.AddModelError("Description", "Desctiption must contain some information");
            }
            if (editReviewViewModel.ReviewingTypeKey == null)
            {
                ModelState.AddModelError("ReviewingTypeKey", "Choose type of an object of the review");
            }
            if (editReviewViewModel.Rating == null)
            {
                ModelState.AddModelError("Rating", "Choose final rating");
            }
            if (ModelState.IsValid && review!=null)
            {
                if (user.Id == review.UserKey)
                {
                    if(IsReallyChanged(review, editReviewViewModel))
                    {
                        review.WasUpdated = true;
                    }
                    review.Name = editReviewViewModel.Name;
                    review.Description = editReviewViewModel.Description;
                    review.Rating = (int)editReviewViewModel.Rating;
                    review.ReviewingTypeKey = (Guid)editReviewViewModel.ReviewingTypeKey;
                    review.ReviewingType = await _applicationContext.ReviewingTypes.FindAsync((Guid)editReviewViewModel.ReviewingTypeKey);
                    review.CreatedDate = DateTime.UtcNow;
                    _applicationContext.SaveChanges();
                    return RedirectToAction("Profile", "Account", new { Id = user.Id });
                }
            }
            return View(editReviewViewModel);
        }

        [Authorize]
        public async Task<IActionResult> ReviewDelete(Guid id)
        {
            User user = await _userManager.GetUserAsync(User);
            Review review = await _applicationContext.Reviews.FindAsync(id);
            if (user != null)
            {
                if (review.UserKey == user.Id)
                {
                    _applicationContext.Reviews.Remove(review);
                    await _applicationContext.SaveChangesAsync();
                }
                return RedirectToAction("Profile", "Account", new { Id = user.Id });
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
