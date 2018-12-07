using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Models.ArticlesViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Blog.Models;

namespace Blog.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

       
        public ArticlesController(ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            List<ArticleIndexViewModel> Articles = this.dbContext.Articles.Select(a => new ArticleIndexViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Summury = a.Summury,
                AuthorFullName = a.Author.FullName

            }).ToList();

            return View(Articles);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(Article article)
        {
            string userId = this.userManager.GetUserId(this.User);
            article.AuthorId = userId;

            if (ModelState.IsValid)
            {
                this.dbContext.Articles.Add(article);
                this.dbContext.SaveChanges();


                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Article article = this.dbContext.Articles.Include(a=>a.Author).SingleOrDefault(a=>a.Id == id);


            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }


        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            Article article = this.dbContext.Articles.Find(id);

            string currentUserId = this.userManager.GetUserId(this.User);
            if (article == null)
            {
                return NotFound();
            }

            if (article.AuthorId != currentUserId)
            {
                return Forbid();
            }
            return View(article);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(Article article)
        {

            string currentUserId = this.userManager.GetUserId(this.User);
            if (ModelState.IsValid)
            {
                this.dbContext.Articles.Update(article);
                this.dbContext.SaveChanges();


                return RedirectToAction("Index") ;
            }
            if (article.AuthorId != currentUserId)
            {
                return Forbid();
            }

            return View(article);
        }


        [HttpGet]
        [Authorize]
        public IActionResult Delete(int id)
        {
            Article article = this.dbContext.Articles.Find(id);
            string currentUserId = this.userManager.GetUserId(this.User);
            if (article == null)
            {
                return NotFound();
            }
            if (article.AuthorId != currentUserId)
            {
                return Forbid();
            }
            return View(article);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(Article article)
        {
            bool isArticleExisting = this.dbContext.Articles.Any(a => a.Id == article.Id);
            string currentUserId = this.userManager.GetUserId(this.User);
            if (isArticleExisting == false)
            {
                return NotFound();
            }
            if (article.AuthorId != currentUserId)
            {
                return Forbid();
            }
            this.dbContext.Articles.Remove(article);
            this.dbContext.SaveChanges();


            return RedirectToAction("Index");
        }
    }
}
