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
    public class AuthorDao
    {
        OnlineShopDbContext db = null;
        public AuthorDao()
        {
            db = new OnlineShopDbContext();
        }
        public long Insert(Author author)
        {
            author.MetaTitle = StringHelper.ToUnsignString(author.Name);
            author.CreatedDate = DateTime.Now;
            db.Authors.Add(author);
            db.SaveChanges();
            return author.ID;
        }

        public List<Author> ListAll()
        {
            //var lst = db.Database.SqlQuery<Author>("select au.ID as id,  au.Name as Name,au.MetaTitle as MetaTitle, au.ParentID as ParentID, au.DisplayOrder as DisplayOrder, au.DisplayOrder as DisplayOrder,  au.SeoTitle as SeoTitle,  au.CreatedDate as CreatedDate,  au.CreatedBy as CreatedBy,  au.ModifiedDate as ModifiedDate,  au.ModifiedBy as ModifiedBy,  au.MetaKeywords as MetaKeywords,  au.MetaDescriptions as MetaDescriptions,  au.Status as Status" +
            //    " from Author au ").ToList();
            //return lst;
            return db.Authors.Where(x => x.Status == true).ToList();
        }
        public IEnumerable<Author> ListAllPaging(string searchString, ref int totalRecord, int page, int pageSize)
        {
            IQueryable<Author> model = db.Authors;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }
            totalRecord = model.Count();
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public Author ViewDetail(int id)
        {
            return db.Authors.Find(id);
        }
        public bool Update(Author entity)
        {
            try
            {
                var author = db.Authors.Find(entity.ID);
                author.Name = entity.Name;
                author.YearOfBirth = entity.YearOfBirth;
                author.SeoTitle = entity.SeoTitle;
                author.Status = entity.Status;
                author.ModifiedDate = DateTime.Now;
                author.ModifiedBy = entity.ModifiedBy;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                //logging
                return false;
            }

        }
        public bool Delete(int id)
        {
            try
            {
                var author = db.Authors.Find(id);

                IQueryable<Product> model = db.Products;
                model = model.Where(x => x.AuthorID == author.ID);
                if (model.Count() == 0)
                {
                    db.Authors.Remove(author);
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
            var author = db.Authors.Find(id);
            author.Status = !author.Status;
            db.SaveChanges();
            return author.Status;
        }
    }
}
