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
    public class PublishingCompanyController : BaseController
    {
        // GET: Admin/PublishingCompany
        public ActionResult Index(string searchString="", int page = 1, int pageSize = 5)
        {
            int totalRecord = 0;
            int totalPage = 0;
            var dao = new PublishingCompanyDao();
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
        public ActionResult Create(PublishingCompany publishingCompany)
        {
            if (ModelState.IsValid)
            {
                var dao = new PublishingCompanyDao();
                var session = (UserLogin)Session[OnlineShop.Common.CommonConstants.ADMIN_SESSION];
                publishingCompany.CreatedBy = session.UserName;
                long id = dao.Insert(publishingCompany);
                if (id > 0)
                {
                    SetAlert("Thêm nhà xuất bản thành công", "success");
                    return RedirectToAction("Index", "PublishingCompany");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm nhà xuất bản không thành công");
                }
            }
            return View(publishingCompany);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var publishingCompany = new PublishingCompanyDao().ViewDetail(id);
            return View(publishingCompany);
        }

        [HttpPost]
        public ActionResult Edit(PublishingCompany publishingCompany)
        {
            if (ModelState.IsValid)
            {
                var dao = new PublishingCompanyDao();
                var session = (UserLogin)Session[OnlineShop.Common.CommonConstants.USER_SESSION];
                publishingCompany.ModifiedBy = session.UserName;
                var result = dao.Update(publishingCompany);
                if (result)
                {
                    SetAlert("Sửa nhà xuất bản thành công", "success");
                    return RedirectToAction("Index", "PublishingCompany");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật nhà xuất bản không thành công");
                }
            }
            return View(publishingCompany);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            bool result = new PublishingCompanyDao().Delete(id);
            if (result)
            {
                SetAlert("Xóa nhà xuất bản thành công", "success");
                return RedirectToAction("Index", "PublishingCompany");
            }
            else
            {
                SetAlert("Xóa không thành công. Còn sách của nhà xuất bản này", "error");
                return RedirectToAction("Index", "PublishingCompany");
            }
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            PublishingCompany publishingCompany = new PublishingCompanyDao().ViewDetail(id);
            return View(publishingCompany);
        }
        [HttpPost]
        public ActionResult ChangeStatus(long id)
        {
            var result = new PublishingCompanyDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}