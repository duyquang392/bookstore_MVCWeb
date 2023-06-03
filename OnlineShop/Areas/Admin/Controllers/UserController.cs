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

    public class UserController : BaseController
    {
        // GET: Admin/User
        [HasCredential(RoleID = "VIEW_USER")]

        public ActionResult Index(string searchString="", int page = 1, int pageSize = 5)
        {
            int totalRecord = 0;
            int totalPage = 0;
            var dao = new UserDao();
            var list = dao.ListAllPaging(searchString, ref totalRecord, page, pageSize,"");
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
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(int id)
        {
            var user = new UserDao().ViewDetail(id);
            SetViewBag(user.GroupID);
            return View(user);
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();

                var encryptedMd5Pas = Encryptor.MD5Hash(user.Password);
                user.Password = encryptedMd5Pas;
                User result = dao.GetById(user.UserName);
                if (result == null)
                {
                    var session = (UserLogin)Session[OnlineShop.Common.CommonConstants.ADMIN_SESSION];
                    user.CreatedBy = session.UserName;
                    long id = dao.Insert(user);
                    if (id > 0)
                    {
                        SetAlert("Thêm tài khoản thành công", "success");
                        return RedirectToAction("Index", "User");
                    }  
                }
                else
                {
                    ModelState.AddModelError("", "Thêm không thành công. Tài khoản đã tồn tại.");
                }
            }
            SetViewBag();
            return View(user);
        }
        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (!string.IsNullOrEmpty(user.Password))
                {
                    var encryptedMd5Pas = Encryptor.MD5Hash(user.Password);
                    user.Password = encryptedMd5Pas;
                }

                User result = dao.GetById(user.UserName);
                if (result== null)
                {
                    var session = (UserLogin)Session[OnlineShop.Common.CommonConstants.USER_SESSION];
                    user.ModifiedBy = session.UserName;
                    var res = dao.Update(user);
                    SetAlert("Cập nhật tài khoản thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật tài khoản không thành công");
                    return View();
                }
            }
            SetViewBag(user.GroupID);
            return View(user);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new UserDao().Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatus(long id)
        {
            var result = new UserDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        public void SetViewBag(string selectedId = null)
        {
            var dao = new UserGroupDao();
            ViewBag.GroupID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
    }
}