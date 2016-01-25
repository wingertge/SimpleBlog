using System.Linq;
using System.Web.Mvc;
using NHibernate.Linq;
using SimpleBlog.Models;
using SimpleBlog.ViewModels;

namespace SimpleBlog.Controllers
{
    [ChildActionOnly]
    public class LayoutController : Controller
    {
        public ActionResult Sidebar()
        {
            return View(new LayoutSidebar
            {
                IsLoggedIn = Auth.User != null,
                Username = Auth.User != null ? Auth.User.Username : "",
                IsAdmin = User.IsInRole("admin"),
                Tags = Database.Session.Query<Tag>().Select(a => new 
                {
                    a.Id,
                    a.Name,
                    a.Slug,
                    PostCount = a.Posts.Count
                }).Where(a => a.PostCount > 0)
                .OrderByDescending(a => a.PostCount)
                .Select(a => new SidebarTag(a.Id, a.Name, a.Slug, a.PostCount))
                .ToList()
            });
        }
    }
}