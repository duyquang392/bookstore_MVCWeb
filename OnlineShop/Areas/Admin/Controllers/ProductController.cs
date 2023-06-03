using Model.Dao;
using Model.EF;
using OnlineShop.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        // GET: Admin/Content
        public ActionResult Index(string searchString = "", string soldOut = "", int page = 1, int pageSize = 5)
        {
            int totalRecord = 0;
            int totalPage = 0;
            var dao = new ProductDao();
            if (soldOut == "")
            {
                ViewBag.soldOut = "Số lượng bán";
            }
            else if (soldOut == "banchay")
            {
                ViewBag.soldOut = "Bán chạy";
            }
            else
            {
                ViewBag.soldOut = "Bán ít";
            }
            var list = dao.ListAllPaging(searchString, soldOut, ref totalRecord, page, pageSize); 
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
            SetViewBagCategory();
            SetViewBagAuthor();
            SetViewBagPublishingCompany();
            return View();
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var product = new ProductDao().ViewDetail(id);
            SetViewBagCategory(product.CategoryID);
            SetViewBagAuthor(product.AuthorID);
            SetViewBagPublishingCompany(product.PublishingCompanyID);
            return View(product);
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
                var session = (UserLogin)Session[OnlineShop.Common.CommonConstants.ADMIN_SESSION];
                product.CreatedBy = session.UserName;
                long id = dao.Insert(product);
                if (id > 0)
                {
                    SetAlert("Thêm sách thành công", "success");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm sách không thành công");
                    return RedirectToAction("Create", "Product");
                }
            }
            SetViewBagCategory();
            SetViewBagAuthor();
            SetViewBagPublishingCompany();
            return View(product);
        }
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
                var session = (UserLogin)Session[OnlineShop.Common.CommonConstants.ADMIN_SESSION];
                product.ModifiedBy = session.UserName;
                var result = dao.Update(product);
                if (result)
                {
                    SetAlert("Sửa sách thành công", "success");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật sách không thành công");
                }
            }
            SetViewBagAuthor(product.AuthorID);
            SetViewBagCategory(product.CategoryID);
            SetViewBagPublishingCompany(product.PublishingCompanyID);
            return View(product);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new ProductDao().Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var productModel = new ProductModel();
            productModel.ModelProduct = new ProductDao().ViewDetail(id);
            productModel.categoryName = new ProductDao().ViewDetailCategory(id);
            productModel.authorName = new ProductDao().ViewDetailAuthor(id);
            productModel.publishingCompanyName = new ProductDao().ViewDetailPublishingCompany(id);
            return View(productModel);
        }



        [HttpPost]
        public ActionResult ChangeStatus(long id)
        {
            var result = new ProductDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        public JsonResult LoadImages(long id)
        {
            ProductDao dao = new ProductDao();
            var product = dao.ViewDetail(id);
            var images = product.MoreImages;
            XElement xImages = XElement.Parse(images);
            List<string> listImagesReturn = new List<string>();

            foreach (XElement element in xImages.Elements())
            {
                listImagesReturn.Add(element.Value);
            }
            return Json(new
            {
                data = listImagesReturn
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveImages(long id, string images)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var listImages = serializer.Deserialize<List<string>>(images);

            XElement xElement = new XElement("Images");

            foreach (var item in listImages)
            {
                var subStringItem = item.Substring(21);
                xElement.Add(new XElement("Image", subStringItem));
            }
            ProductDao dao = new ProductDao();
            try
            {
                dao.UpdateImages(id, xElement.ToString());
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    status = false
                });
            }

        }
        public void SetViewBagCategory(long? selectedId = null)
        {
            var dao = new ProductCategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
        public void SetViewBagAuthor(long? selectedId = null)
        {
            var dao = new AuthorDao();
            ViewBag.AuthorID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
        public void SetViewBagPublishingCompany(long? selectedId = null)
        {
            var dao = new PublishingCompanyDao();
            ViewBag.PublishingCompanyID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }

    }
}