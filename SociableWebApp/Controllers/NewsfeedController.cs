using Microsoft.AspNetCore.Mvc;

namespace SociableWebApp.Controllers
{
    public class NewsfeedController : Controller
    {
        // GET: NewsfeedController
        public ActionResult NewsFeed()
        {
            return View();
        }

        // GET: NewsfeedController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NewsfeedController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NewsfeedController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NewsfeedController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NewsfeedController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NewsfeedController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NewsfeedController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
