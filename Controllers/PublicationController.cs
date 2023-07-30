using IdeaExchange.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdeaExchange.Controllers
{
    public class PublicationController : Controller
    {
        public IdeaExchangeContext _dbContext;

        private readonly UserManager<ApplicationUser> _userManager;

        public PublicationController(IdeaExchangeContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [Authorize]
        [Route("createpublication")]
        public IActionResult CreatePublication()
        {
             return View();
        }

    [Authorize]
    [HttpPost]
    [Route("publication/post")]
     public async Task<IActionResult> Post(Publication basic)
     {
       if (ModelState.IsValid)
        {
          string title = basic.Title;
          string content = basic.Content;
          DateTime? date = basic.Date;

          var author = await _userManager.GetUserAsync(User);

                    if (author == null)
                    {
                        return BadRequest(new { Error = "User not found or not authenticated." });
                    }

                    var publication = new Publication
                    {
                        Title = title,
                        Content = content,
                        Date = DateTime.Now,
                        Author = author,
                        AuthorId = author.Id
                    };

                    _dbContext.Publications.Add(publication);
                    _dbContext.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var errorMessages = ModelState.Values
                     .SelectMany(v => v.Errors)
                     .Select(e => e.ErrorMessage)
                     .ToList();
                    return BadRequest(new { Errors = errorMessages });
                }
            }


        public IActionResult Details(int id)
        {
            var publication = _dbContext.Publications.Find(id);

            if (publication == null)
            {
                return NotFound();
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == publication.AuthorId);
            
            if (user != null)
            {
                publication.AuthorName = user.UserName;
            }

            return View(publication);
        }
    }
    }
