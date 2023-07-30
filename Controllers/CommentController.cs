using IdeaExchange.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IdeaExchange.Migrations;

namespace IdeaExchange.Controllers
{
    public class CommentController : Controller
    {
        private readonly IdeaExchangeContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentController(IdeaExchangeContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;

        }

        [Authorize]
        [Route("/Comment")]
        public IActionResult Comment()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [Route("comment/postcomment")]
        public async Task<IActionResult> PostComment(Comment comment)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                var userId = user.Id;

                var newComment = new Comment
                {
                    Content = comment.Content,
                    UserId = userId,
                    PublicationId = comment.PublicationId,
                    Date = DateTime.Now
                };

                _dbContext.Comments.Add(newComment);
                _dbContext.SaveChanges();

                return RedirectToAction("Details", "Publication", new { id = comment.PublicationId });
            }
            else
            {
                Log.Warning("Model invalid");
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new { Errors = errorMessages });
            }
        }
    }
}

