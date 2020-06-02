using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RedditViewerApp.Models;

namespace RedditViewerApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            List<Post> posts = Post.GetRecentPosts("aww");
            return View(posts);
        }
        
        [HttpPost]
        public IActionResult Index(string type, int limit, string query)
        {
            List<Post> posts;
            
            if (!string.IsNullOrEmpty(query))
            {
                posts = 
                    Post.GetRecentPosts("aww", "search", limit, query, type);
            }
            else
            {
                posts = Post.GetRecentPosts("aww", type, limit);
            }

            
            return View(posts);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
