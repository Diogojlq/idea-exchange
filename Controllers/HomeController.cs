using IdeaExchange.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdeaExchange.Controllers
{
    public class HomeController : Controller
    {
        public IdeaExchangeContext _dbContext;

        public HomeController(IdeaExchangeContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("/")]
        public IActionResult Index()
        {
            List<Publication> publications = _dbContext.Publications.ToList();

            foreach (var publication in publications)
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Id == publication.AuthorId);

                if (user != null)
                {
                    publication.AuthorName = user.UserName;
                }
            }
            return View(publications);
        }

    }
}
