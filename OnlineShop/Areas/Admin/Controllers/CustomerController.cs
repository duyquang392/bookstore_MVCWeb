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
    public class CustomerController : BaseController
    {
        // GET: Admin/Customer
        public ActionResult Index(string searchString="", int page = 1, int pageSize = 10)
        {
            string groupID = "CUSTOMER";
            int totalRecord = 0;
            int totalPage = 0;
            var dao = new UserDao();
            var list = dao.ListAllPaging(searchString, ref totalRecord, page, pageSize, groupID);
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
        
        public ActionResult Edit(int id)
        {
            var user = new UserDao().ViewDetailCustomer(id);
            if (user == null)
            {
                RedirectToAction("Index", "Customer"); 
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();

                var encryptedMd5Pas = Encryptor.MD5Hash(user.Password);
                user.Password = encryptedMd5Pas;
                var session = (UserLogin)Session[OnlineShop.Common.CommonConstants.ADMIN_SESSION];
                user.CreatedBy = session.UserName;
                User result = dao.GetById(user.UserName);
                if (result == null)
                {
                    long id = dao.Insert(user);
                    if (id > 0)
                    {
                        SetAlert("Thêm khách hàng thành công", "success");
                        return RedirectToAction("Index", "Customer");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Thêm không thành công. Tài khoản đã tồn tại.");
                    return View();
                }
            }
            return View(user);
        }
        [HttpPost]
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
                var session = (UserLogin)Session[OnlineShop.Common.CommonConstants.USER_SESSION];
                user.ModifiedBy = session.UserName;
                User result = dao.GetById(user.UserName);
                if (result == null)
                {
                    var res = dao.Update(user);
                    SetAlert("Cập nhật khách hàng thành công", "success");
                    return RedirectToAction("Index", "Customer");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật khách hàng không thành công");
                }
            }
            return View(user);
        }
        public ActionResult Details(int id)
        {
            User user = new UserDao().ViewCustomerDetail(id);
            return View(user);
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new UserDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}