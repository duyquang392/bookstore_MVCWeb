﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList;
using System.Configuration;
using Common;

namespace Model.Dao
{
    public class UserDao
    {
        OnlineShopDbContext db = null;
        public UserDao()
        {
            db = new OnlineShopDbContext();
        }

        public long Insert(User entity)
        {
            if (entity.GroupID == null)
            {
                entity.GroupID = "CUSTOMER";
            }
            entity.CreatedDate = DateTime.Now;
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        //public long InsertForFacebook(User entity)
        //{
        //    var user = db.Users.SingleOrDefault(x => x.UserName == entity.UserName);
        //    if (user == null)
        //    {
        //        entity.GroupID = "CUSTOMER";
        //        entity.Status = true;
        //        db.Users.Add(entity);
        //        db.SaveChanges();
        //        return entity.ID;
        //    }
        //    else
        //    {
        //        return user.ID;
        //    }

        //}
        public bool Update(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.ID);
                user.UserName = entity.UserName;
                user.Name = entity.Name;
                if (!string.IsNullOrEmpty(entity.Password))
                {
                    user.Password = entity.Password;
                }
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.Phone = entity.Phone;
                user.ModifiedDate = DateTime.Now;
                user.ModifiedBy = entity.ModifiedBy;
                if (entity.GroupID == null) { 
                    user.GroupID = "CUSTOMER";
                }
                else
                {
                    user.GroupID = entity.GroupID;
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                //logging
                return false;
            }

        }
        public List<User> ListAll()
        {
            return db.Users.Where(x => x.Status == true).ToList();
        }

        public IEnumerable<User> ListAllPaging(string searchString,ref int totalRecord, int page, int pageSize, string groupID)
        {
            IQueryable<User> model = db.Users;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.UserName.Contains(searchString) || x.Name.Contains(searchString));
            }
            if (groupID != "")
            {
                model = model.Where(x => (x.GroupID == groupID));
            }
            totalRecord = model.Count();
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public User GetById(string userName)
        {
            return db.Users.SingleOrDefault(x => x.UserName == userName);
        }
        public User ViewDetail(int id)
        {
            return db.Users.Find(id);
        }

        public User ViewCustomerDetail(int id)
        { 
            return db.Users.Find(id);
        }
        public User ViewDetailCustomer(int id)
        {
            User user = db.Users.Find(id);
            if (user.GroupID == "CUSTOMER")
            {
                return user;
            }
            else {
                return new User();
            }
        }
        public User ViewDetailMember(int id)
        {
            User user = db.Users.Find(id);
            if (user.GroupID == "MEMBER")
            {
                return user;
            }
            else
            {
                return new User();
            }
        }
        public List<string> GetListCredential(string userName)
        {
            var user = db.Users.Single(x => x.UserName == userName);
            var data = (from a in db.Credentials
                        join b in db.UserGroups on a.UserGroupID equals b.ID
                        join c in db.Roles on a.RoleID equals c.ID
                        where b.ID == user.GroupID
                        select new
                        {
                            RoleID = a.RoleID,
                            UserGroupID = a.UserGroupID
                        }).AsEnumerable().Select(x => new Credential()
                        {
                            RoleID = x.RoleID,
                            UserGroupID = x.UserGroupID
                        });
            return data.Select(x => x.RoleID).ToList();

        }

        public int LoginAdmin(string userName, string passWord, bool isLoginAdmin = false)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (isLoginAdmin == true)
                {
                    if (result.GroupID == CommonConstants.ADMIN_GROUP || result.GroupID == CommonConstants.MEMBER_GROUP )
                    {
                        if (result.Status == false)
                        {
                            return -1;
                        }
                        else
                        {
                            if (result.Password == passWord)
                                return 1;
                            else
                                return -2;
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    if (result.Status == false)
                    {
                        return -1;
                    }
                    else
                    {
                        if (result.Password == passWord)
                            return 1;
                        else
                            return -2;
                    }
                }
            }
        }
        public int Login(string userName, string passWord, bool isLoginAdmin = false)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                //if (isLoginAdmin == true)
                //{
                if (result.GroupID == CommonConstants.CUSTOMER_GROUP)
                {
                    if (result.Status == false)
                    {
                        return -1;
                    }
                    else
                    {
                        if (result.Password == passWord)
                            return 1;
                        else
                            return -2;
                    }
                }
                else
                {
                    return -3;
                }
                //}
                //else
                //{
                //    if (result.Status == false)
                //    {
                //        return -1;
                //    }
                //    else
                //    {
                //        if (result.Password == passWord)
                //            return 1;
                //        else
                //            return -2;
                //    }
                //}
            }
        }

        public bool ChangeStatus(long id)
        {
            var user = db.Users.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool CheckUserName(string userName)
        {
            return db.Users.Count(x => x.UserName == userName) > 0;
        }
        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }

        public int CountCustomerInMonth(int month, int year)
        {
            IQueryable<User> model = db.Users;
            model = model.Where(x => x.CreatedDate.Month == month && x.CreatedDate.Year == year);
            model = model.Where(x => x.GroupID == "CUSTOMER");
            int number = model.Count();
            return number;
        }
    }
}
