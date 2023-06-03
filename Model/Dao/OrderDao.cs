using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class OrderDao
    {
        OnlineShopDbContext db = null;
        public OrderDao()
        {
            db = new OnlineShopDbContext();
        }
        public long Insert(Order order)
        {
            order.CreatedDate = DateTime.Now;
            db.Orders.Add(order);
            db.SaveChanges();
            return order.ID;
        }
        public List<Order> ListAll()
        {
            return db.Orders.Where(x => x.Status == true).ToList();
        }
        public IEnumerable<Order> ListAllPaging(string searchString, ref int totalRecord, int page, int pageSize)
        {
            IQueryable<Order> model = db.Orders;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.ShipName.Contains(searchString));
            }
            totalRecord = model.Count();
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public Order ViewDetail(int id)
        {
            return db.Orders.Find(id);
        }
        public bool Update(Order entity)
        {
            try
            {
                var order = db.Orders.Find(entity.ID);
                order.CustomerID = entity.CustomerID;
                order.Status = entity.Status;
                order.ModifiedDate = DateTime.Now;
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
                var order = db.Orders.Find(id);
                db.Orders.Remove(order);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool ChangeStatus(long id)
        {
            var order = db.Orders.Find(id);
            order.Status = !order.Status;
            db.SaveChanges();
            return order.Status;
        }
        public int CountOrderInMonth(int month, int year)
        {
            
            IQueryable<Order> model = db.Orders;
            model = model.Where(x => x.CreatedDate.Month == month && x.CreatedDate.Year == year);
            int number = model.Count();
            return number;
        }

        public long TotalSaleInMonth(int month, int year)
        {
            IQueryable<Order> model = db.Orders;
            model = model.Where(x => x.CreatedDate.Month == month && x.CreatedDate.Year == year);
            long total = 0;
            foreach (var item in model)
            {
                total += item.Total;
            }
            return total;
        }

        public IEnumerable<Order> OrdersInMonth(int month, int year)
        {
            IQueryable<Order> model = db.Orders;
            model = model.Where(x => x.CreatedDate.Month == month && x.CreatedDate.Year == year);
            return model;
        }
    }
}
