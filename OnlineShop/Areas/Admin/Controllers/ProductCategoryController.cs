using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        // GET: Admin/ProductCategory
        public ActionResult Index(string searchString="", int page = 1, int pageSize = 5)
        {
            int totalRecord = 0;
            int totalPage = 0;
            var dao = new ProductCategoryDao();
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
        public ActionResult Create(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductCategoryDao();
                var session = (UserLogin)Session[OnlineShop.Common.CommonConstants.ADMIN_SESSION];
                productCategory.CreatedBy = session.UserName;
                long id = dao.Insert(productCategory);
                if (id > 0)
                {
                    SetAlert("Thêm Thể loại thành công", "success");
                    return RedirectToAction("Index", "ProductCategory");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm user không thành công");
                }
            }
            return View(productCategory);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var productCategory = new ProductCategoryDao().ViewDetail(id);
            return View(productCategory);
        }
        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductCategoryDao();
                var session = (UserLogin)Session[OnlineShop.Common.CommonConstants.USER_SESSION];
                productCategory.ModifiedBy = session.UserName;
                var result = dao.Update(productCategory);
                if (result)
                {
                    SetAlert("Sửa thể loại thành công", "success");
                    return RedirectToAction("Index", "ProductCategory");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật thể loại không thành công");
                }
            }
            return View(productCategory);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            bool result = new ProductCategoryDao().Delete(id);
            if (result)
            {
                SetAlert("Xóa thể loại thành công", "success");
                return RedirectToAction("Index", "ProductCategory");
            }
            else
            {
                SetAlert("Xóa không thành công. Còn sách thuộc thể loại này", "error");
                return RedirectToAction("Index", "ProductCategory");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            ProductCategory productCategory = new ProductCategoryDao().ViewDetail(id);
            return View(productCategory);
        }
        public JsonResult ChangeStatus(long id)
        {
            var result = new ProductCategoryDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}