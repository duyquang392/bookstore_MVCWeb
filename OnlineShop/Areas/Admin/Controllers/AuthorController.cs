using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
using OnlineShop.Common;
using PagedList;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class AuthorController : BaseController
    {
        // GET: Admin/Author
        public ActionResult Index(string searchString="", int page = 1, int pageSize = 5)
        {
            int totalRecord = 0;
            int totalPage = 0;
            var dao = new AuthorDao();
            var list = dao.ListAllPaging(searchString, ref totalRecord, page, pageSize);
            ViewBag.SearchString = searchString;
            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            int maxPage = 5;
            totalPage = (int)Math.Ceiling((double)totalRecord / (double)pageSize);
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Author author)
        {
            if (ModelState.IsValid)
            {
                var dao = new AuthorDao();
                var session = (UserLogin)Session[OnlineShop.Common.CommonConstants.ADMIN_SESSION];
                author.CreatedBy = session.UserName;
                long id = dao.Insert(author);
                if (id > 0)
                {
                    SetAlert("Thêm tác giả thành công", "success");
                    return RedirectToAction("Index", "Author");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm tác giả không thành công");
                }
            }
            return View(author);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var author = new AuthorDao().ViewDetail(id);
            return View(author);
        }

        [HttpPost]
        public ActionResult Edit(Author author)
        {
            if (ModelState.IsValid)
            {
                var dao = new AuthorDao();
                var session = (UserLogin)Session[OnlineShop.Common.CommonConstants.ADMIN_SESSION];
                author.ModifiedBy = session.UserName;
                var result = dao.Update(author);
                if (result)
                {
                    SetAlert("Sửa Tác giả thành công", "success");
                    return RedirectToAction("Index", "Author");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật Tác giả không thành công");
                }
            }
            return View(author);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            bool result = new AuthorDao().Delete(id);
            if (result)
            {
                SetAlert("Xóa Tác giả thành công", "success");
                return RedirectToAction("Index", "Author");
            }
            else
            {
                SetAlert("Xóa không thành công. Còn sách của tác giả này", "error");
                return RedirectToAction("Index", "Author");
            }
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            Author author = new AuthorDao().ViewDetail(id);
            return View(author);
        }
        [HttpPost]
        public ActionResult ChangeStatus(long id)
        {
            var result = new AuthorDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}