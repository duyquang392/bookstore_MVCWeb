using Common;
using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class PublishingCompanyDao
    {
        OnlineShopDbContext db = null;
        public PublishingCompanyDao()
        {
            db = new OnlineShopDbContext();
        }
        public long Insert(PublishingCompany publishingCompany)
        {
            publishingCompany.CreatedDate = DateTime.Now;
            publishingCompany.MetaTitle = StringHelper.ToUnsignString(publishingCompany.Name);
            db.PublishingCompanies.Add(publishingCompany);
            db.SaveChanges();
            return publishingCompany.ID;
        }
        public List<PublishingCompany> ListAll()
        {
            return db.PublishingCompanies.Where(x => x.Status == true).ToList();
        }
        public IEnumerable<PublishingCompany> ListAllPaging(string searchString,ref int totalRecord, int page, int pageSize)
        {
            IQueryable<PublishingCompany> model = db.PublishingCompanies;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }
            totalRecord = model.Count();
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public PublishingCompany ViewDetail(int id)
        {
            return db.PublishingCompanies.Find(id);
        }
        public bool Update(PublishingCompany entity)
        {
            try
            {
                var publishingCompany = db.PublishingCompanies.Find(entity.ID);
                publishingCompany.Name = entity.Name;
                publishingCompany.SeoTitle = entity.SeoTitle;
                publishingCompany.MetaTitle = StringHelper.ToUnsignString(entity.Name);
                publishingCompany.Status = entity.Status;
                publishingCompany.ModifiedDate = DateTime.Now;
                publishingCompany.ModifiedBy = entity.ModifiedBy;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {

                var publishingCompany = db.PublishingCompanies.Find(id);

                IQueryable<Product> model = db.Products;
                model = model.Where(x => x.PublishingCompanyID == publishingCompany.ID);
                if (model.Count() == 0)
                {
                    db.PublishingCompanies.Remove(publishingCompany);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool ChangeStatus(long id)
        {
            var publishingCompany = db.PublishingCompanies.Find(id);
            publishingCompany.Status = !publishingCompany.Status;
            db.SaveChanges();
            return publishingCompany.Status;
        }   
    }
}
