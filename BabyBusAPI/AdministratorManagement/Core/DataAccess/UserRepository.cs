using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AdministratorManagement.Models;
using BabyBus.AutoModel;

namespace AdministratorManagement.Core.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private  IList<Admin> _users;

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(UserRepository));

        public UserRepository()
        {
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    _users = (from a in db.Admins
                              select a).ToList();

                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message, ex);
            }

        }

        public Admin FindByUsername(string username)
        {
            Admin admin = null;
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    admin = (from a in db.Admins
                              where a.UserName == username
                              select a).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message, ex);
            }

//            var admin = _users.FirstOrDefault(x => x.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase));
            return admin;
        }

        public UV_Teacher FindByUserAppName(string username)
        {
            UV_Teacher user = null;
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    user = (from u in db.UV_Teacher
                            where u.LoginName == username
                            select u).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message, ex);
            }

            return user;
        }

        public Admin FindById(Guid id)
        {
//            Admin admin = null;
//            try
//            {
//                using (var db = new BabyBus_Entities())
//                {
//                    admin = (from a in db.Admins
//                             where a.Guid == id
//                             select a).FirstOrDefault();
//
//                }
//            }
//            catch (Exception ex)
//            {
//                Log.Fatal(ex.Message, ex);
//            }
            
//            return admin;
            return _users.FirstOrDefault(x => x.Guid == id);
        }

        public IEnumerable<Admin> FindAll()
        {
            return _users;
        }

        public void Save(Admin entity)
        {
            try
            {
                using (var db = new BabyBus_Entities())
                {
                    _users.Add(entity);
                    db.Admins.AddOrUpdate(entity);

                    //TODO: can not add, I don't know how to use EF 
                    db.SaveChanges();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                Log.Fatal(dbEx.Message, dbEx);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message, ex);
            }
        }

        public void Update(Admin entity, Guid id)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Admin entity)
        {
            _users.Remove(entity);
        }
    }
}