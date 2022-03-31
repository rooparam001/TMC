using EntitesInterfaces.AppModels;
using EntitesInterfaces.DBEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMC.Models;

namespace TMC.DBConnections
{
    public class TMCDBContext : DbContext
    {
        public TMCDBContext() : base() { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ChatServiceContactModel>(
            eb =>
            {
                eb.HasNoKey();
            });
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
        public DbSet<TBL_PLAYSMASTER> TBL_PLAYSMASTER { get; set; }
        public DbSet<TBL_SLIDERMASTER> TBL_SLIDERMASTER { get; set; }
        public DbSet<TBL_GENREMASTER> TBL_GENREMASTER { get; set; }
        public DbSet<TBL_LANGUAGEMASTER> TBL_LANGUAGEMASTER { get; set; }
        public DbSet<TBL_DIRECTORMASTER> TBL_DIRECTORMASTER { get; set; }
        public DbSet<TBL_CITYMASTER> TBL_CITYMASTER { get; set; }
        public DbSet<TBL_ENQUIRYMASTER> TBL_ENQUIRYMASTER { get; set; }
        public DbSet<TBL_PROFILESMASTER> TBL_PROFILESMASTER { get; set; }
        public DbSet<TBL_GIVEAWAYMASTER> TBL_GIVEAWAYMASTER { get; set; }
        public DbSet<TBL_HOMEPAGESETTINGS> TBL_HOMEPAGESETTINGS { get; set; }
        public DbSet<TBL_CHATMESSAGEMASTER> TBL_CHATMESSAGEMASTER { get; set; }
        public DbSet<TBL_CHATGROUPMASTER> TBL_CHATGROUPMASTER { get; set; }
        public DbSet<ChatServiceContactModel> ChatServiceContactModel { get; set; }

        public TBL_CHATGROUPMASTER fn_CheckChatGroupExists(int partyID1, int partyID2)
        {
            var respObj = new TBL_CHATGROUPMASTER();
            try
            {
                using (var context = new TMCDBContext())
                {
                    respObj = context.TBL_CHATGROUPMASTER.Where(x => (x.HOSTID == partyID1 && x.PARTYID == partyID2) || (x.HOSTID == partyID2 && x.PARTYID == partyID1)).FirstOrDefault();
                }
            }
            catch { respObj = new TBL_CHATGROUPMASTER(); }
            return respObj;
        }

        public TBL_CHATGROUPMASTER fn_SaveChatGroup(TBL_CHATGROUPMASTER obj)
        {
            try
            {
                var obj1 = fn_CheckChatGroupExists(partyID1: obj.HOSTID, partyID2: obj.PARTYID);
                if (obj1 != null)
                {
                    if (obj1.ID > 0)
                    {
                        return obj1;
                    }
                }
                using (var context = new TMCDBContext())
                {
                    context.TBL_CHATGROUPMASTER.Add(obj);
                    context.SaveChanges();
                }
            }
            catch { obj = new TBL_CHATGROUPMASTER(); }
            return obj;
        }
        public bool fn_SaveChat(TBL_CHATMESSAGEMASTER obj)
        {
            var respObj = false;
            try
            {
                using (var context = new TMCDBContext())
                {
                    context.TBL_CHATMESSAGEMASTER.Add(obj);
                    context.SaveChanges();
                }
            }
            catch { respObj = false; }
            return respObj;
        }

        public List<ChatServiceContactModel> fn_GetAllChatContactsByUserID(int ID)
        {
            var respObj = new List<ChatServiceContactModel>();
            try
            {
                using (var context = new TMCDBContext())
                {
                    respObj = context.ChatServiceContactModel.FromSqlRaw<ChatServiceContactModel>("EXEC USP_GETCHATLIST_HOMEPAGE @USERID=" + ID).ToList();
                    /*var chatcontactlist = context.TBL_CHATGROUPMASTER.Where(x => x.HOSTID == ID || x.PARTYID == ID.ToString()).ToList();
                    if (chatcontactlist != null)
                    {
                        if (chatcontactlist.Count > 0)
                        {
                            objList = (List<ChatServiceContactModel>)(from main in chatcontactlist
                                                                      join temp in context.TBL_PROFILESMASTER.Where(x => x.ISENABLE)
                                                                      on main.HOSTID equals temp.ID
                                                                      select new ChatServiceContactModel()
                                                                      {
                                                                          ContactID = temp.ID,
                                                                          ContactName = temp.USERTITLE,
                                                                          ContactPic = temp.PROFILEPICTURE,
                                                                          GroupID = main.ID,
                                                                          LastDateTime = context.TBL_CHATMESSAGEMASTER.Where(x => x.CHATMASTERID == main.ID).OrderByDescending(x => x.DATECREATED).Select(y => y.DATECREATED.ToString("dddd, dd MMMM yyyy")).FirstOrDefault()
                                                                      });

                            objList.AddRange(from main in chatcontactlist
                                             join temp in context.TBL_PROFILESMASTER.Where(x => x.ISENABLE)
                                             on main.PARTYID equals temp.ID.ToString()
                                             select new ChatServiceContactModel()
                                             {
                                                 ContactID = temp.ID,
                                                 ContactName = temp.USERTITLE,
                                                 ContactPic = temp.PROFILEPICTURE,
                                                 GroupID = main.ID,
                                                 LastDateTime = context.TBL_CHATMESSAGEMASTER.Where(x => x.CHATMASTERID == main.ID).OrderByDescending(x => x.DATECREATED).Select(y => y.DATECREATED.ToString("dddd, dd MMMM yyyy")).FirstOrDefault()
                                             });

                            respObj = objList.Distinct().ToList();
                        }
                    }*/

                }
            }
            catch (Exception ex) { respObj = new List<ChatServiceContactModel>(); }
            return respObj;
        }

        public List<ChatServiceMessageListModel> fn_GetAllChatByGroupID(int GroupID, int HostID, int LastMsgID = 0)
        {
            var respObj = new List<ChatServiceMessageListModel>();
            try
            {
                using (var context = new TMCDBContext())
                {
                    respObj = context.TBL_CHATMESSAGEMASTER.Where(x => x.CHATMASTERID == GroupID && (LastMsgID == 0 ? true : x.ID > LastMsgID)).Select(y => new ChatServiceMessageListModel()
                    {
                        GroupID = GroupID,
                        SenderID = y.SENDERID,
                        ChatMessage = y.CHATMESSAGE,
                        isSenderSelfAccount = (y.SENDERID == HostID ? true : false),
                        DateCreated = y.DATECREATED.ToString("HH:mm"),
                        MsgID = y.ID
                    }).ToList();
                }
            }
            catch { respObj = new List<ChatServiceMessageListModel>(); }
            return respObj;
        }

        public Tbl_RoleMaster fn_SaveRole(string objName)
        {
            var resp = new Tbl_RoleMaster()
            {
                DateCreated = DateTime.Now,
                RoleName = objName.Trim().ToUpper()
            };
            try
            {
                using (var context = new TMCDBContext())
                {
                    if (context.Tbl_RoleMaster.Where(x => x.RoleName == resp.RoleName).ToList().Count > 0)
                    {
                        resp = context.Tbl_RoleMaster.Where(x => x.RoleName == resp.RoleName).FirstOrDefault();
                    }
                    else
                    {
                        context.Tbl_RoleMaster.Add(resp);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { resp = new Tbl_RoleMaster(); }
            return resp;
        }
        public List<Tbl_RoleMaster> fn_GetAllRoles()
        {
            var resp = new List<Tbl_RoleMaster>();
            try
            {
                using (var context = new TMCDBContext())
                {
                    resp = context.Tbl_RoleMaster.ToList();
                }
            }
            catch (Exception ex) { resp = new List<Tbl_RoleMaster>(); }
            return resp;
        }
        public string fn_GetRoleByID(int ID)
        {
            var resp = "";
            try
            {
                using (var context = new TMCDBContext())
                {
                    resp = context.Tbl_RoleMaster.Where(x => x.ID == ID).FirstOrDefault().RoleName;
                }
            }
            catch (Exception ex) { resp = ""; }
            return resp;
        }
        public AccountMaster fn_GetUserByID(int ID)
        {
            var respObj = new AccountMaster();
            try
            {
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
        public registerloginUserViewModel fn_SaveUser(registerloginUserViewModel obj)
        {
            var tblObj = new Tbl_AccountMaster()
            {
                ContactNumber = obj.ContactNumber,
                DateCreated = DateTime.Now,
                Email = obj.Email.Trim(),
                RoleID = (string.IsNullOrEmpty(obj.userrole) ? this.fn_SaveRole("Viewer").ID : this.fn_SaveRole(obj.userrole).ID),
                UserName = obj.UserName.Trim(),
                UserPassword = obj.Password.Trim(),
                UserStatus = true
            };
            try
            {
                using (var context = new TMCDBContext())
                {
                    context.Tbl_AccountMaster.Add(tblObj);
                    context.SaveChanges();
                    obj.UserStatus = true;
                }
            }
            catch (Exception ex) { obj = new registerloginUserViewModel() { UserStatus = false }; }
            return obj;
        }
        public int fn_ValidateUniqueEmailID(string Email)
        {
            var respObj = 0;
            try
            {
                respObj = this.Tbl_AccountMaster.Where(x => x.Email == Email).Count();
            }
            catch (Exception ex) { respObj = 0; }
            return respObj;
        }
        public int fn_ValidateUniqueContactNumber(string MobNo)
        {
            var respObj = 0;
            try
            {
                respObj = this.Tbl_AccountMaster.Where(x => x.ContactNumber == MobNo).Count();
            }
            catch (Exception ex) { respObj = 0; }
            return respObj;
        }
        public AccountMaster fn_GetUserByContactNumber(string MobNo)
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
        public Tbl_AccountMaster fn_GetUserByEmail(string Email)
        {
            var respObj = new Tbl_AccountMaster();
            try
            {
                respObj = this.Tbl_AccountMaster.Where(x => x.Email == Email).FirstOrDefault();
                if (respObj == null)
                    respObj = new Tbl_AccountMaster();
            }
            catch (Exception ex) { respObj = new Tbl_AccountMaster(); }
            return respObj;
        }
        public TBL_PLAYSMASTER fn_GetSinglePlayByID(int ID)
        {
            return this.TBL_PLAYSMASTER.Where(x => x.ID == ID && x.ISENABLE).FirstOrDefault();
        }
        public TBL_GENREMASTER fn_GetSingleGenreByID(int ID)
        {
            return this.TBL_GENREMASTER.Where(x => x.ID == ID).FirstOrDefault();
        }
        public TBL_LANGUAGEMASTER fn_GetSingleLanguageByID(int ID)
        {
            return this.TBL_LANGUAGEMASTER.Where(x => x.ID == ID).FirstOrDefault();
        }
        public TBL_CITYMASTER fn_GetSingleCityByID(int ID)
        {
            return this.TBL_CITYMASTER.Where(x => x.ID == ID).FirstOrDefault();
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
                respList = this.TBL_UPCOMINGPLAYS.Where(x => x.ISENABLE).OrderBy(y => y.SERIALORDER).ToList();
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
        public TBL_PLAYSMASTER fn_SavePlay(TBL_PLAYSMASTER obj)
        {
            try
            {
                using (var context = new TMCDBContext())
                {
                    if (obj.ID == 0)
                        context.TBL_PLAYSMASTER.Add(obj);
                    else
                        context.TBL_PLAYSMASTER.Update(obj);

                    context.SaveChanges();
                }
            }
            catch (Exception ex) { }
            return obj;
        }
        public bool fn_DeletePlay(int objID)
        {
            var resp = false;
            try
            {
                var relationObj = new TBL_PLAYSMASTER();
                relationObj = this.TBL_PLAYSMASTER.Where(x => x.ID == objID).FirstOrDefault();
                if (relationObj != null)
                    if (relationObj.ID > 0)
                    {
                        relationObj.ISENABLE = false;
                        this.TBL_PLAYSMASTER.Update(relationObj);
                        this.SaveChanges();
                        resp = true;
                    }
            }
            catch { resp = false; }
            return resp;
        }
        public int fn_SaveGenre(string obj)
        {
            int resp = 0;
            try
            {
                if (this.TBL_GENREMASTER.Where(x => x.Genre.ToLower() == obj.ToLower()).Count() == 0)
                {
                    this.TBL_GENREMASTER.Add(new TBL_GENREMASTER()
                    {
                        DateCreated = DateTime.Now,
                        Genre = obj.ToString()
                    });
                    this.SaveChanges();
                }
                return this.TBL_GENREMASTER.Where(x => x.Genre.ToLower() == obj.ToLower()).Select(y => y.ID).FirstOrDefault();
            }
            catch { resp = 0; }
            return resp;
        }
        public int fn_SaveLanguage(string obj)
        {
            int resp = 0;
            try
            {
                if (this.TBL_LANGUAGEMASTER.Where(x => x.LANGUAGEVAL.ToLower() == obj.ToLower()).Count() == 0)
                {
                    this.TBL_LANGUAGEMASTER.Add(new TBL_LANGUAGEMASTER()
                    {
                        DateCreated = DateTime.Now,
                        LANGUAGEVAL = obj.ToString()
                    });
                    this.SaveChanges();
                }
                return this.TBL_LANGUAGEMASTER.Where(x => x.LANGUAGEVAL.ToLower() == obj.ToLower()).Select(y => y.ID).FirstOrDefault();
            }
            catch { resp = 0; }
            return resp;
        }
        public int fn_SaveCity(string obj)
        {
            int resp = 0;
            try
            {
                if (this.TBL_CITYMASTER.Where(x => x.CITY.ToLower() == obj.ToLower()).Count() == 0)
                {
                    this.TBL_CITYMASTER.Add(new TBL_CITYMASTER()
                    {
                        DATECREATED = DateTime.Now,
                        CITY = obj.ToString()
                    });
                    this.SaveChanges();
                }
                return this.TBL_CITYMASTER.Where(x => x.CITY.ToLower() == obj.ToLower()).Select(y => y.ID).FirstOrDefault();
            }
            catch { resp = 0; }
            return resp;
        }
        public List<TBL_GENREMASTER> fn_GetAllGenres()
        {
            var outputObj = new List<TBL_GENREMASTER>();
            try
            {
                outputObj = this.TBL_GENREMASTER.ToList();
            }
            catch { outputObj = new List<TBL_GENREMASTER>(); }
            return outputObj;
        }
        public List<TBL_LANGUAGEMASTER> fn_GetAllLanguages()
        {
            var outputObj = new List<TBL_LANGUAGEMASTER>();
            try
            {
                outputObj = this.TBL_LANGUAGEMASTER.ToList();
            }
            catch { outputObj = new List<TBL_LANGUAGEMASTER>(); }
            return outputObj;
        }
        public List<TBL_CITYMASTER> fn_GetAllCities()
        {
            var outputObj = new List<TBL_CITYMASTER>();
            try
            {
                outputObj = this.TBL_CITYMASTER.ToList();
            }
            catch { outputObj = new List<TBL_CITYMASTER>(); }
            return outputObj;
        }
        public List<TBL_PLAYSMASTER> fn_GetAllExistingPlays()
        {
            var respList = new List<TBL_PLAYSMASTER>();
            try
            {
                respList = this.TBL_PLAYSMASTER.Where(x => x.ISENABLE).OrderBy(y => y.ID).ToList();
            }
            catch (Exception ex) { respList = new List<TBL_PLAYSMASTER>(); }
            return respList;
        }
        public bool fn_DeleteAllExistingPlays()
        {
            var resp = false;
            try
            {
                var playList = new List<TBL_PLAYSMASTER>();
                playList = this.TBL_PLAYSMASTER.Where(x => x.ISENABLE).ToList();
                foreach (var currPlay in playList)
                {
                    currPlay.ISENABLE = false;
                    this.TBL_PLAYSMASTER.Update(currPlay);
                    this.SaveChanges();
                }

                resp = true;
            }
            catch { resp = false; }
            return resp;
        }
        public bool fn_SaveSlider(TBL_SLIDERMASTER obj)
        {
            var resp = false;
            try
            {
                using (var context = new TMCDBContext())
                {
                    context.TBL_SLIDERMASTER.Add(obj);
                    context.SaveChanges();
                }
                resp = true;
            }
            catch (Exception ex) { resp = false; }
            return resp;
        }
        public bool fn_DelSlider(int ObjID, bool isObjectID = true)
        {
            var resp = false;
            try
            {
                var context = new TMCDBContext();
                //context.TBL_SLIDERMASTER.RemoveRange(context.TBL_SLIDERMASTER.Where(x => x.OBJECTID == ObjID && x.OBJECTTYPE == (int)SliderObjectType.Plays).ToList());
                if (isObjectID)
                    context.TBL_SLIDERMASTER.RemoveRange(context.TBL_SLIDERMASTER.Where(x => x.OBJECTID == ObjID).ToList());
                else
                    context.TBL_SLIDERMASTER.RemoveRange(context.TBL_SLIDERMASTER.Where(x => x.ID == ObjID).ToList());
                context.SaveChanges();
                resp = true;
            }
            catch (Exception ex) { resp = false; }
            return resp;
        }
        public sliderListViewModel fn_GetSlider(int ObjID)
        {
            var resp = new sliderListViewModel();
            try
            {
                var context = new TMCDBContext();
                resp.lst = context.TBL_SLIDERMASTER.Where(x => x.OBJECTTYPE == (int)SliderObjectType.MainPage).Select(x => new sliderViewModel()
                {
                    ID = x.ID,
                    SliderImgURL = x.OBJECTURL
                }).ToList();
            }
            catch (Exception ex) { resp = new sliderListViewModel(); }
            return resp;
        }
        public bool fn_SaveDirectors(TBL_DIRECTORMASTER obj)
        {
            var resp = false;
            try
            {
                using (var context = new TMCDBContext())
                {
                    obj.ISENABLE = true;
                    context.TBL_DIRECTORMASTER.Add(obj);
                    context.SaveChanges();
                }
                resp = true;
            }
            catch (Exception ex) { resp = false; }
            return resp;
        }
        public bool fn_DeleteDirectors(int objID)
        {
            var resp = false;
            try
            {
                var relationObj = new TBL_DIRECTORMASTER();
                relationObj = this.TBL_DIRECTORMASTER.Where(x => x.ID == objID && x.ISENABLE).FirstOrDefault();
                if (relationObj != null)
                    if (relationObj.ID > 0)
                    {
                        relationObj.ISENABLE = false;
                        this.TBL_DIRECTORMASTER.Update(relationObj);
                        this.SaveChanges();
                        resp = true;
                    }
            }
            catch { resp = false; }
            return resp;
        }
        public List<TBL_DIRECTORMASTER> fn_getallDirectors(int ID = 0)
        {
            var outputObj = new List<TBL_DIRECTORMASTER>();
            try
            {
                outputObj = this.TBL_DIRECTORMASTER.Where(x => (ID == 0 ? true : x.ID == ID) && x.ISENABLE).ToList();
            }
            catch { outputObj = new List<TBL_DIRECTORMASTER>(); }
            return outputObj;
        }
        public bool fn_SaveEnquiry(TBL_ENQUIRYMASTER obj)
        {
            var resp = false;
            try
            {
                using (var context = new TMCDBContext())
                {
                    context.TBL_ENQUIRYMASTER.Add(obj);
                    context.SaveChanges();
                }
                resp = true;
            }
            catch (Exception ex) { resp = false; }
            return resp;
        }
        public List<TBL_ENQUIRYMASTER> fn_getallUnSeenEnquiries(int ID = 0)
        {
            var outputObj = new List<TBL_ENQUIRYMASTER>();
            try
            {
                outputObj = this.TBL_ENQUIRYMASTER.Where(x => (ID == 0 ? true : x.ID == ID) && !x.SEENSTATUS).ToList();
            }
            catch { outputObj = new List<TBL_ENQUIRYMASTER>(); }
            return outputObj;
        }
        public bool fn_SaveProfiles(TBL_PROFILESMASTER obj)
        {
            var resp = false;
            try
            {
                using (var context = new TMCDBContext())
                {
                    context.TBL_PROFILESMASTER.Add(obj);
                    context.SaveChanges();
                }
                resp = true;
            }
            catch (Exception ex) { resp = false; }
            return resp;
        }
        public bool fn_ProfileVerifyifExists(TBL_PROFILESMASTER obj)
        {
            var resp = false;
            try
            {
                using (var context = new TMCDBContext())
                {
                    resp = (context.TBL_PROFILESMASTER.Where(x => x.ISENABLE && x.USERROLE == obj.USERROLE && x.USERTITLE == obj.USERTITLE)).ToList().Count > 0 ? true : false;
                }
            }
            catch (Exception ex) { resp = false; }
            return resp;
        }
        public bool fn_DeleteProfile(int objID)
        {
            var resp = false;
            try
            {
                var relationObj = new TBL_PROFILESMASTER();
                relationObj = this.TBL_PROFILESMASTER.Where(x => x.ID == objID && x.ISENABLE).FirstOrDefault();
                if (relationObj != null)
                    if (relationObj.ID > 0)
                    {
                        relationObj.ISENABLE = false;
                        this.TBL_PROFILESMASTER.Update(relationObj);
                        this.SaveChanges();
                        resp = true;
                    }
            }
            catch { resp = false; }
            return resp;
        }
        public List<TBL_PROFILESMASTER> fn_getallProfiles(int ID = 0)
        {
            var outputObj = new List<TBL_PROFILESMASTER>();
            try
            {
                outputObj = this.TBL_PROFILESMASTER.Where(x => (ID == 0 ? true : x.ID == ID) && x.ISENABLE).ToList();
            }
            catch (Exception ex) { outputObj = new List<TBL_PROFILESMASTER>(); }
            return outputObj;
        }
        public TBL_GIVEAWAYMASTER fn_SaveGiveaway(TBL_GIVEAWAYMASTER obj)
        {
            try
            {
                using (var context = new TMCDBContext())
                {
                    if (obj.ID > 0)
                    {
                        context.TBL_GIVEAWAYMASTER.Update(obj);
                        context.SaveChanges();
                    }
                    else
                    {
                        context.TBL_GIVEAWAYMASTER.Add(obj);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { obj.ID = 0; }
            return obj;
        }
        public List<TBL_GIVEAWAYMASTER> fn_getallGiveaways(int ID = 0, int city = 0, string searchTxt = "", int isPDF = 0)
        {
            var respObj = new List<TBL_GIVEAWAYMASTER>();
            try
            {
                using (var context = new TMCDBContext())
                {
                    respObj = context.TBL_GIVEAWAYMASTER.Where(x => (isPDF == 0 ? true : (isPDF == 1 ? x.ISPDF : !x.ISPDF)) && (ID > 0 ? x.ID == ID : true) && (city == 0 ? true : x.CITY == city) && (string.IsNullOrEmpty(searchTxt) ? true : x.OBJTITLE.Contains(searchTxt)) && x.ISENABLE).ToList();
                }
            }
            catch { respObj = new List<TBL_GIVEAWAYMASTER>(); }
            return respObj;
        }
        public bool fn_DeleteGiveAway(int objID)
        {
            var resp = false;
            try
            {
                var relationObj = new TBL_GIVEAWAYMASTER();
                relationObj = this.TBL_GIVEAWAYMASTER.Where(x => x.ID == objID && x.ISENABLE).FirstOrDefault();
                if (relationObj != null)
                    if (relationObj.ID > 0)
                    {
                        relationObj.ISENABLE = false;
                        this.TBL_GIVEAWAYMASTER.Update(relationObj);
                        this.SaveChanges();
                        resp = true;
                    }
            }
            catch { resp = false; }
            return resp;
        }
        public bool fn_AcceptGiveAway(int objID)
        {
            var resp = false;
            try
            {
                var relationObj = new TBL_GIVEAWAYMASTER();
                relationObj = this.TBL_GIVEAWAYMASTER.Where(x => x.ID == objID && x.ISENABLE).FirstOrDefault();
                if (relationObj != null)
                    if (relationObj.ID > 0)
                    {
                        relationObj.ISACCEPTED = true;
                        this.TBL_GIVEAWAYMASTER.Update(relationObj);
                        this.SaveChanges();
                        resp = true;
                    }
            }
            catch { resp = false; }
            return resp;
        }

        public TBL_HOMEPAGESETTINGS fn_SaveHomePageSetting(TBL_HOMEPAGESETTINGS obj)
        {
            try
            {
                using (var context = new TMCDBContext())
                {
                    obj.ISENABLE = true;
                    context.TBL_HOMEPAGESETTINGS.Add(obj);
                    context.SaveChanges();
                }
            }
            catch (Exception ex) { obj = new TBL_HOMEPAGESETTINGS(); }
            return obj;
        }
        public bool fn_DeleteHomePageSetting(int objID)
        {
            var resp = false;
            try
            {
                var relationObj = new TBL_HOMEPAGESETTINGS();
                relationObj = this.TBL_HOMEPAGESETTINGS.Where(x => x.ID == objID && x.ISENABLE).FirstOrDefault();
                if (relationObj != null)
                    if (relationObj.ID > 0)
                    {
                        relationObj.ISENABLE = false;
                        this.TBL_HOMEPAGESETTINGS.Update(relationObj);
                        this.SaveChanges();
                        resp = true;
                    }
            }
            catch { resp = false; }
            return resp;
        }
    }
}
