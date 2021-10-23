using EntitesInterfaces.AppModels;
using EntitesInterfaces.DBEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace TMC.DBConnections
{
    public class TMCDBContext : DbContext
    {
        public TMCDBContext() : base() { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Tbl_AccountMaster> Tbl_AccountMaster { get; set; }
        public DbSet<Tbl_RoleMaster> Tbl_RoleMaster { get; set; }
        public DbSet<Tbl_TeamMaster> Tbl_TeamMaster { get; set; }

        public AccountMaster fn_GetUserByID(int ID)
        {
            var respObj = new AccountMaster();
            try
            {
                //using (var context = new TMCDBContext())
                //{

                //}
                respObj = this.Tbl_AccountMaster.Where(x => x.ID == ID).Select(y => new AccountMaster()
                {
                    ContactNumber = y.ContactNumber,
                    DateCreated = y.DateCreated.ToString("dddd, dd MMMM yyyy"),
                    Email = y.Email,
                    ID = y.ID,
                    UserName = y.UserName,
                    UserStatus = y.UserStatus,
                    Role = this.Tbl_RoleMaster.Where(x => x.ID == y.RoleID).Select(y => y.RoleName).FirstOrDefault()
                }).FirstOrDefault();
            }
            catch (Exception ex) { respObj = new AccountMaster(); }
            return respObj;
        }

        public AccountMaster fn_GetUserByContactNumber(int MobNo)
        {
            var respObj = new AccountMaster();
            try
            {
                respObj = this.Tbl_AccountMaster.Where(x => x.ContactNumber == MobNo).Select(y => new AccountMaster()
                {
                    ContactNumber = y.ContactNumber,
                    DateCreated = y.DateCreated.ToString("dddd, dd MMMM yyyy"),
                    Email = y.Email,
                    ID = y.ID,
                    UserName = y.UserName,
                    UserStatus = y.UserStatus,
                    Role = this.Tbl_RoleMaster.Where(x => x.ID == y.RoleID).Select(y => y.RoleName).FirstOrDefault()
                }).FirstOrDefault();
            }
            catch (Exception ex) { respObj = new AccountMaster(); }
            return respObj;
        }

        public AccountMaster fn_GetUserByEmail(string Email)
        {
            var respObj = new AccountMaster();
            try
            {
                respObj = this.Tbl_AccountMaster.Where(x => x.Email == Email).Select(y => new AccountMaster()
                {
                    ContactNumber = y.ContactNumber,
                    DateCreated = y.DateCreated.ToString("dddd, dd MMMM yyyy"),
                    Email = y.Email,
                    ID = y.ID,
                    UserName = y.UserName,
                    UserStatus = y.UserStatus,
                    Role = this.Tbl_RoleMaster.Where(x => x.ID == y.RoleID).Select(y => y.RoleName).FirstOrDefault()
                }).FirstOrDefault();
            }
            catch (Exception ex) { respObj = new AccountMaster(); }
            return respObj;
        }
    }
}
