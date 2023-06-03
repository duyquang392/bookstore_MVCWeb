using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class OrderDetailDao
    {
        OnlineShopDbContext db = null;
        public OrderDetailDao()
        {
            db = new OnlineShopDbContext();
        }
        public bool Insert(OrderDetail detail)
        {
            try
            {
                db.OrderDetails.Add(detail);
                db.SaveChanges();

                Product product = db.Products.Find(detail.ProductID);
                product.NumberOfBooks = product.NumberOfBooks - detail.Quantity;
                product.SoldOut = product.SoldOut + detail.Quantity;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;

            }
        }
        public List<OrderDetail> ListAll(long selectedId)
        {
            return db.OrderDetails.Where(x => x.OrderID == selectedId).ToList();
        }
        public List<Product> ListAllBook(List<OrderDetail> data)
        {
            List<Product> book = new List<Product>();
            foreach (var item in data)
                try
                {
                    book.Add(db.Products.Find(item.ProductID));
                }
                catch(Exception)
                {
                    book.Add(new Product());
                }
            return book;
        }
    }
}
