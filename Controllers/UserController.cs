using IdeaExchange.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Design;

namespace IdeaExchange.Controllers
{
    [Route("pages")]
    public class UserController : Controller
    {
        private UserContext _dbContext;

        public UserController(UserContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAllUsers() 
        {
            var users = _dbContext.Users.ToList();
            return Ok(users);
        }
        [HttpPost]
        public IActionResult CreateUser(User basic)
        {
            string? name = basic.Name;
            string? email = basic.Email;
            string? password = basic.Password;

            var user = new User
            {
                Name = name,
                Email = email,
                Password = password
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return RedirectToAction("UsersList");
        }
    }
}
