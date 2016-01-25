using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls.WebParts;
using NHibernate.Linq;
using SimpleBlog.Infrastructure;
using SimpleBlog.Models;
using SimpleBlog.ViewModels;

namespace SimpleBlog.Controllers
{
    public class PostsController : Controller
    {
        private const int PostsPerPage = 5;

        public ActionResult Index(int page = 1)
        {
            var baseQuery = Database.Session.Query<Post>()
                .Where(a => a.DeletedAt == null)
                .OrderByDescending(a => a.CreatedAt);

            var totalCount = baseQuery.Count();
            var postIds = baseQuery.Skip((page - 1) * PostsPerPage)
                .Take(PostsPerPage)
                .Select(a => a.Id)
                .ToArray();

            var currentPostsPage = baseQuery.Where(a => postIds.Contains(a.Id))
                .FetchMany(a => a.Tags)
                .Fetch(a => a.User)
                .ToList();

            return View(new PostsIndex
            {
                Posts = new PagedData<Post>(currentPostsPage, totalCount, page, PostsPerPage)
            });
        }

        public ActionResult Tag(string idAndSlug, int page = 1)
        {
            var parts = SeperateIdAndSlug(idAndSlug);
            if (parts == null)
                return HttpNotFound();

            var tag = Database.Session.Load<Tag>(parts.Item1);
            if (tag == null)
                return HttpNotFound();

            if (!tag.Slug.Equals(parts.Item2, StringComparison.CurrentCultureIgnoreCase))
                return RedirectToRoutePermanent("Post", new { Id = parts.Item1, tag.Slug });

            var totalCount = tag.Posts.Count;
            var postIds = tag.Posts
                .OrderByDescending(a => a.CreatedAt)
                .Where(a => a.DeletedAt == null)
                .Skip((page - 1)*PostsPerPage)
                .Take(PostsPerPage)
                .Select(a => a.Id)
                .ToArray();

            var currentPostsPage = Database.Session.Query<Post>()
                .OrderByDescending(a => a.CreatedAt)
                .Where(a => postIds.Contains(a.Id))
                .FetchMany(a => a.Tags)
                .Fetch(a => a.User)
                .ToList();

            return View(new PostsTag
            {
                Tag = tag,
                Posts = new PagedData<Post>(currentPostsPage, totalCount, page, PostsPerPage)
            });
        }

        public ActionResult Show(string idAndSlug)
        {
            var parts = SeperateIdAndSlug(idAndSlug);
            if (parts == null)
                return HttpNotFound();

            var post = Database.Session.Load<Post>(parts.Item1);
            if (post == null || post.IsDeleted)
                return HttpNotFound();

            if (!post.Slug.Equals(parts.Item2, StringComparison.CurrentCultureIgnoreCase))
                return RedirectToRoutePermanent("Post", new {Id = parts.Item1, post.Slug});

            return View(new PostsShow
            {
                Post = post
            });
        }

        private System.Tuple<int, string> SeperateIdAndSlug(string idAndSlug)
        {
            var matches = Regex.Match(idAndSlug, @"^(\d+)\-(.*)?$");
            if (!matches.Success)
                return null;

            var id = int.Parse(matches.Result("$1"));
            var slug = matches.Result("$2");

            return Tuple.Create(id, slug);
        }
    }
}