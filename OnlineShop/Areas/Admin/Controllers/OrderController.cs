using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
using OnlineShop.Common;
using PagedList;
using OnlineShop.Areas.Admin.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Admin/Order
        public ActionResult Index(string searchString="", int page = 1, int pageSize = 10)
        {
            int totalRecord = 0;
            int totalPage = 0;
            var dao = new OrderDao();
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
            SetViewBagCustomer();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                var dao = new OrderDao();
                long id = dao.Insert(order);
                if (id > 0)
                {
                    SetAlert("Thêm đơn hàng thành công", "success");
                    return RedirectToAction("Index", "Order");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm đơn hàng không thành công");
                }
                SetViewBagCustomer();
            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var order = new OrderDao().ViewDetail(id);
            SetViewBagCustomer(order.CustomerID);
            return View(order);
        }

        [HttpPost]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                var dao = new OrderDao();
                var result = dao.Update(order);
                if (result)
                {
                    SetAlert("Cập nhật đơn hàng thành công.", "success");
                    return RedirectToAction("Index", "Order");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật đơn hàng không thành công.");
                }
                SetViewBagCustomer(order.CustomerID);
            }
            return View("Index");
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new OrderDao().Delete(id);
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            var orderModel = new OrdelModel();
            orderModel.ModelOrder = new OrderDao().ViewDetail(id);
            orderModel.ModelOrderDetail = new OrderDetailDao().ListAll(id);
            orderModel.ModelProduct = new OrderDetailDao().ListAllBook(orderModel.ModelOrderDetail);
            return View(orderModel);
        }
        public void SetViewBagCustomer(long? selectedID = null)
        {
            var dao = new UserDao();
            ViewBag.CustomerID = new SelectList(dao.ListAll(), "ID", "Name", selectedID);
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new OrderDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}