using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class SlideController : BaseController
    {
        // GET: Admin/Slide
        public ActionResult Index(string searchString = "", int page = 1, int pageSize = 5)
        {
            int totalRecord = 0;
            int totalPage = 0;
            var dao = new SlideDao();
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

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Slide slide)
        {
            if (ModelState.IsValid)
            {
                var dao = new SlideDao();
                var session = (UserLogin)Session[OnlineShop.Common.CommonConstants.ADMIN_SESSION];
                slide.CreatedBy = session.UserName;
                long id = dao.Insert(slide);
                if (id > 0)
                {
                    SetAlert("Thêm slide thành công", "success");
                    return RedirectToAction("Index", "Slide");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm slide không thành công");
                    return RedirectToAction("Create", "Slide");
                }
            }
            return View(slide);
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var slide = new SlideDao().ViewDetail(id);
            return View(slide);
        }

        public ActionResult ChangeStatus(long id)
        {
            var result = new SlideDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        [HttpPost]
        public ActionResult Edit(Slide slide)
        {
            if (ModelState.IsValid)
            {
                var dao = new SlideDao();
                var session = (UserLogin)Session[OnlineShop.Common.CommonConstants.ADMIN_SESSION];
                slide.ModifiedBy = session.UserName;
                var result = dao.Update(slide);
                if (result)
                {
                    SetAlert("Sửa Slide thành công", "success");
                    return RedirectToAction("Index", "Slide");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật slide không thành công");
                }
            }
            return View(slide);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new SlideDao().Delete(id);
            return RedirectToAction("Index");
        }
    }
}