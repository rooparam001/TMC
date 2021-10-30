using EntitesInterfaces.AppModels;
using EntitesInterfaces.DBEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string c = Directory.GetCurrentDirectory();
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(c).AddJsonFile("appsettings.json").Build();
            string connectionStringIs = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionStringIs))
            {
                throw new InvalidOperationException("Could not find connection string.");
            }

            optionsBuilder.UseSqlServer(connectionString: connectionStringIs);
        }

        public DbSet<Tbl_AccountMaster> Tbl_AccountMaster { get; set; }
        public DbSet<Tbl_RoleMaster> Tbl_RoleMaster { get; set; }
        public DbSet<Tbl_TeamMaster> Tbl_TeamMaster { get; set; }
        public DbSet<TBL_UPCOMINGPLAYS> TBL_UPCOMINGPLAYS { get; set; }

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
        public bool fn_SaveUpcomingPlay(TBL_UPCOMINGPLAYS obj)
        {
            var resp = false;
            try
            {
                using (var context = new TMCDBContext())
                {
                    context.TBL_UPCOMINGPLAYS.Add(obj);
                    context.SaveChanges();
                }
                //this.TBL_UPCOMINGPLAYS.Add(obj);
                //this.SaveChanges();
                resp = true;
            }
            catch (Exception ex) { resp = false; }
            return resp;
        }
        public bool fn_DeleteUpcomingPlay(int objID)
        {
            var resp = false;
            try
            {
                var relationObj = new TBL_UPCOMINGPLAYS();
                relationObj = this.TBL_UPCOMINGPLAYS.Where(x => x.ID == objID).FirstOrDefault();
                if (relationObj != null)
                    if (relationObj.ID > 0)
                    {
                        relationObj.ISENABLE = false;
                        this.TBL_UPCOMINGPLAYS.Update(relationObj);
                        this.SaveChanges();
                        resp = true;
                    }
            }
            catch { resp = false; }
            return resp;
        }
        public List<TBL_UPCOMINGPLAYS> fn_GetAllPlays()
        {
            var respList = new List<TBL_UPCOMINGPLAYS>();
            try
            {
                respList = this.TBL_UPCOMINGPLAYS.Where(x => x.ISENABLE).ToList();
            }
            catch { respList = new List<TBL_UPCOMINGPLAYS>(); }
            return respList;
        }
        public bool fn_DeleteAllPlays()
        {
            var resp = false;
            try
            {
                var playList = new List<TBL_UPCOMINGPLAYS>();
                playList = this.TBL_UPCOMINGPLAYS.Where(x => x.ISENABLE).ToList();
                foreach (var currPlay in playList)
                {
                    currPlay.ISENABLE = false;
                    this.TBL_UPCOMINGPLAYS.Update(currPlay);
                    this.SaveChanges();
                }

                resp = true;
            }
            catch { resp = false; }
            return resp;
        }
    }
}
