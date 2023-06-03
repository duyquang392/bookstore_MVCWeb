using Model.EF;
using Model.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Model.Dao
{
    public class ProductDao
    {
        OnlineShopDbContext db = null;
        public ProductDao()
        {
            db = new OnlineShopDbContext();
            //int total = 0;
        }
        public long Insert(Product entity)
        {
            entity.CreatedDate = DateTime.Now;
            entity.MetaTitle = StringHelper.ToUnsignString(entity.Name);
            if (entity.PromotionPrice == null || entity.PromotionPrice == 0)
            {
                entity.PromotionPrice = entity.Price;
            }
            db.Products.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public bool Delete(int id)
        {
            try
            {
                var product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(Product entity)
        {
            try
            {
                var product = db.Products.Find(entity.ID);
                product.Name = entity.Name;
                product.Description = entity.Description;
                product.Image = entity.Image;
                product.AuthorID = entity.AuthorID;
                product.CategoryID = entity.CategoryID;
                product.PublishingCompanyID = entity.CategoryID;
                product.Price = entity.Price;
                product.NumberOfBooks = entity.NumberOfBooks;
                product.NumberOfPages = entity.NumberOfPages;
                product.Detail = entity.Detail;
                product.MetaTitle = StringHelper.ToUnsignString(entity.Name);
                if (entity.PromotionPrice == null || entity.PromotionPrice == 0)
                {
                    product.PromotionPrice = entity.Price;
                }
                product.Status = entity.Status;
                product.ModifiedDate = DateTime.Now;
                product.ModifiedBy = entity.ModifiedBy;

                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }

        }

        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }
        public List<string> ListAuthorName(string keyword)
        {
            return db.Authors.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }
        public IEnumerable<Product> ListAllPaging(string searchString, string soldout, ref int totalRecord, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Name.Contains(searchString));
            }
            totalRecord = model.Count();
            if (soldout == "banchay")
            {
                return model.OrderByDescending(x => x.SoldOut).ToPagedList(page, pageSize);
            }
            else if (soldout == "banit")
            {
                return model.OrderBy(x => x.SoldOut).ToPagedList(page, pageSize);
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public List<ProductViewModel> ListByCategoryId(long categoryID, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            //totalRecord = db.Products.Where(x => x.CategoryID == categoryID).Count();
            var model = (from a in db.Products
                         join b in db.ProductCategories
                         on a.CategoryID equals b.ID
                         where a.CategoryID == categoryID
                         select new
                         {
                             CateMetaTitle = b.MetaTitle,
                             CateName = b.Name,
                             CreatedDate = a.CreatedDate,
                             ID = a.ID,
                             Images = a.Image,
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Price = a.Price,
                             PromotionPrice = a.PromotionPrice,
                             NumberOfBooks = a.NumberOfBooks
                         }).AsEnumerable().Select(x => new ProductViewModel()
                         {
                             CateMetaTitle = x.MetaTitle,
                             CateName = x.Name,
                             CreatedDate = x.CreatedDate,
                             ID = x.ID,
                             Images = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price,
                             PromotionPrice = (decimal)x.PromotionPrice,
                             NumberOfBooks = x.NumberOfBooks
                         });
            totalRecord = model.Count();
            return model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        public List<ProductViewModel> Search(string keyword, ref int totalRecord, decimal min = 0, decimal max = 999999999999999, int pageIndex = 1, int pageSize = 2)
        {
            //totalRecord = db.Products.Where(x => x.Name.Contains(keyword)).Count();
            var model = (from a in db.Products
                         join b in db.ProductCategories
                         on a.CategoryID equals b.ID
                         join c in db.Authors
                         on a.AuthorID equals c.ID
                         where ((a.Name.Contains(keyword) || c.Name.Contains(keyword)) && a.PromotionPrice >= min && a.PromotionPrice <= max)
                         select new
                         {
                             CateMetaTitle = b.MetaTitle,
                             CateName = b.Name,
                             CreatedDate = a.CreatedDate,
                             ID = a.ID,
                             Images = a.Image,
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Price = a.Price,
                             PromotionPrice = a.PromotionPrice,
                             NumberOfBooks = a.NumberOfBooks,
                             authorName = c.Name
                         }).AsEnumerable().Select(x => new ProductViewModel()
                         {
                             CateMetaTitle = x.MetaTitle,
                             CateName = x.Name,
                             CreatedDate = x.CreatedDate,
                             ID = x.ID,
                             Images = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price,
                             PromotionPrice = (decimal)x.PromotionPrice,
                             NumberOfBooks = x.NumberOfBooks,
                             authorName = x.Name
                         });
            totalRecord = model.Count();

            return model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<ProductViewModel> Show(ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            //totalRecord = db.Products.Count();
            var model = (from a in db.Products
                         join b in db.ProductCategories
                         on a.CategoryID equals b.ID
                         select new
                         {
                             CateMetaTitle = b.MetaTitle,
                             CateName = b.Name,
                             CreatedDate = a.CreatedDate,
                             ID = a.ID,
                             Images = a.Image,
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Price = a.Price,
                             PromotionPrice = a.PromotionPrice,
                             NumberOfBooks = a.NumberOfBooks
                         }).AsEnumerable().Select(x => new ProductViewModel()
                         {
                             CateMetaTitle = x.MetaTitle,
                             CateName = x.Name,
                             CreatedDate = x.CreatedDate,
                             ID = x.ID,
                             Images = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price,
                             PromotionPrice = (decimal)x.PromotionPrice,
                             NumberOfBooks = x.NumberOfBooks
                         });
            totalRecord = model.Count();

            return model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(); ;
        }
        /// <summary>
        /// List feature product
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        public List<Product> ListRelatedProducts(long productId)
        {
            var product = db.Products.Find(productId);
            return db.Products.Where(x => x.ID != productId && x.CategoryID == product.CategoryID).ToList();
        }

        public void UpdateImages(long productId, string images)
        {
            var product = db.Products.Find(productId);
            product.MoreImages = images;
            db.SaveChanges();
        }
        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }

        public string ViewDetailCategory(long id)
        {
            string category = "";
            try
            {
                category = db.ProductCategories.Find(db.Products.Find(id).CategoryID).Name;
                return category;

            }
            catch (Exception ex)
            {
                category = "error";
                return category;

            }
        }
        public string ViewDetailAuthor(long id)
        {
            string author = "";
            try
            {
                author = db.Authors.Find(db.Products.Find(id).AuthorID).Name;
            }
            catch (Exception ex)
            {
                author = "error";
            }
            return author;
        }

        public string ViewDetailPublishingCompany(long id)
        {
            string publishingCompany = "";
            try
            {
                publishingCompany = db.PublishingCompanies.Find(db.Products.Find(id).PublishingCompanyID).Name;
            }
            catch (Exception ex)
            {
                publishingCompany = "error";
            }
            return publishingCompany;
        }

        public bool ChangeStatus(long id)
        {
            var product = db.Products.Find(id);
            product.Status = !product.Status;
            db.SaveChanges();
            return product.Status;
        }

    }
}
