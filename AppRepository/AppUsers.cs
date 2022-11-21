using TMC.EntitesInterfaces.AppModels;
using TMC.EntitesInterfaces.DBEntities;
using System;
using System.Collections.Generic;
using TMC.DBConnections;
using TMC.Models;

namespace TMC.AppRepository
{
    public static class AppUsers
    {
        public static registerloginUserViewModel Save(registerloginUserViewModel obj)
        {
            if (!string.IsNullOrEmpty(obj.Email) && !string.IsNullOrEmpty(obj.ContactNumber) && !string.IsNullOrEmpty(obj.UserName))
            {
                //verify if the email is unique
                var emailCnt = new TMCDBContext().fn_ValidateUniqueEmailID(obj.Email.Trim());
                if (emailCnt > 0)
                {
                    obj.UserStatus = false;
                    obj.validationMessage = "Email-ID already registered.";
                }
                else
                {
                    //verify if the contact number is unique
                    var numberCnt = new TMCDBContext().fn_ValidateUniqueContactNumber(obj.ContactNumber);
                    if (numberCnt > 0)
                    {
                        obj.UserStatus = false;
                        obj.validationMessage = "Contact number already registered.";
                    }
                    else
                    {
                        //finally register the user under "Viewer" role
                        obj = new TMCDBContext().fn_SaveUser(obj);
                        if (obj.UserStatus)
                        {
                            obj.validationMessage = "You're successfully registered, please log-in.";
                        }
                        else
                            obj.validationMessage = "Something went wrong, please try again.";
                    }
                }
            }
            else
            {
                obj.UserStatus = false;
                obj.validationMessage = "Email/Contact Number/Name can't be left empty.";
            }
            return obj;
        }

        public static registerloginUserViewModel GetUserByEmail(string Email)
        {
            registerloginUserViewModel user = null;
            if (!string.IsNullOrEmpty(Email))
            {
                var accObj = new Tbl_AccountMaster();
                accObj = new TMCDBContext().fn_GetUserByEmail(Email.Trim());
                if (accObj != null)
                {
                    user = new registerloginUserViewModel();
                    user.Email = accObj.Email;
                    user.ContactNumber = accObj.ContactNumber;
                    user.Password = accObj.UserPassword;
                    user.UserName = accObj.UserName;
                    user.Token = accObj.Token;
                    user.TokenExpireDate = accObj.TokenExpireDate;                    
                }
            }
            return user;
        }

        public static registerloginUserViewModel GetUserByID(string ID)
        {
            registerloginUserViewModel user = null;
            if (!string.IsNullOrEmpty(ID))
            {
                var accObj = new AccountMaster();
                accObj = new TMCDBContext().fn_GetUserByID(Convert.ToInt32(ID));
                if (accObj != null)
                {
                    user = new registerloginUserViewModel();
                    user.Email = accObj.Email;
                    user.ContactNumber = accObj.ContactNumber;
                  //  user.Password = accObj.p;
                    user.UserName = accObj.UserName;
                    user.Token = accObj.Token;
                    user.TokenExpireDate = accObj.TokenExpireDate;
                }
            }
            return user;
        }

        public static registerloginUserViewModel FindUserByEmail(string Email)
        {
            registerloginUserViewModel user = null;
            if (!string.IsNullOrEmpty(Email))
            {
                var accObj = new Tbl_AccountMaster();
                accObj = new TMCDBContext().fn_GetUserByEmail(Email.Trim());
                if (accObj != null && accObj.Email == Email)
                {                    
                    user = new registerloginUserViewModel();
                    user.Email = accObj.Email;
                    user.ContactNumber = accObj.ContactNumber;
                    user.Password = accObj.UserPassword;
                    user.UserName = accObj.UserName;
                    user.Token = Guid.NewGuid().ToString();
                    user.TokenExpireDate = DateTime.Now.AddDays(1);
                    var saveObj = new TMCDBContext().fn_UpdateUser(user);
                    user.UserStatus = saveObj.UserStatus;
                    
                }
            }
            return user;
        }

        public static registerloginUserViewModel ResetPassword(registerloginUserViewModel user, string token, string password)
        {
            registerloginUserViewModel saveObj = null;
            if (!string.IsNullOrEmpty(password))
            {
                var accObj = new Tbl_AccountMaster();
                accObj = new TMCDBContext().fn_GetUserByEmail(user.Email.Trim());
                if (accObj != null && accObj.Email == user.Email && accObj.Token == token)
                {
                    user.Password = password;
                    user.Token = "";
                    user.TokenExpireDate = DateTime.MinValue;
                    saveObj = new TMCDBContext().fn_UpdateUser(user);
                    if(saveObj.UserStatus)
                    {
                        saveObj.validationMessage = "Password updated successfully";
                    }
                    else
                    {
                        saveObj.validationMessage = "Password has not been updated";
                    }
                }
                else
                {
                    saveObj = new registerloginUserViewModel()
                    {
                        UserStatus = false,
                        validationMessage = "Password has not been updated"
                    };
                }
            }
            return saveObj;
        }

        public static registerloginUserViewModel ChangePassword(registerloginUserViewModel user, string password)
        {
            registerloginUserViewModel saveObj = null;
            if (!string.IsNullOrEmpty(password))
            {
                var accObj = new Tbl_AccountMaster();
                accObj = new TMCDBContext().fn_GetUserByEmail(user.Email.Trim());
                if (accObj != null && accObj.Email == user.Email)
                {
                    user.Password = password;
                    user.Token = "";
                    user.TokenExpireDate = DateTime.MinValue;
                    saveObj = new TMCDBContext().fn_UpdateUser(user);
                    if (saveObj.UserStatus)
                    {
                        saveObj.validationMessage = "Password updated successfully";
                    }
                    else
                    {
                        saveObj.validationMessage = "Password has not been updated";
                    }
                }
                else
                {
                    saveObj = new registerloginUserViewModel()
                    {
                        UserStatus = false,
                        validationMessage = "Password has not been updated"
                    };
                }
            }
            return saveObj;
        }
        public static registerloginUserViewModel GetUserByEmailPassword(registerloginUserViewModel obj)
        {

            if (!string.IsNullOrEmpty(obj.Password) && !string.IsNullOrEmpty(obj.Email))
            {
                var accObj = new Tbl_AccountMaster();
                accObj = new TMCDBContext().fn_GetUserByEmail(obj.Email.Trim());
                if (accObj != null)
                {
                    if (!string.IsNullOrEmpty(accObj.UserPassword))
                    {
                        if (accObj.UserPassword.Trim() == obj.Password.Trim())
                        {
                            obj = new registerloginUserViewModel()
                            {
                                UserStatus = true,
                                validationMessage = "User found."
                            };
                        }
                        else
                        {
                            obj = new registerloginUserViewModel()
                            {
                                UserStatus = false,
                                validationMessage = "Invalid User found."
                            };
                        }
                    }
                    else
                    {
                        obj = new registerloginUserViewModel()
                        {
                            UserStatus = false,
                            validationMessage = "Invalid User found."
                        };
                    }
                }
                else
                {
                    obj = new registerloginUserViewModel()
                    {
                        UserStatus = false,
                        validationMessage = "Invalid User found."
                    };
                }
            }
            else
            {
                obj = new registerloginUserViewModel()
                {
                    UserStatus = false,
                    validationMessage = "Email/Contact Number/Name can't be left empty."
                };
            }
            return obj;
        }

        public static AccountMaster fn_GetUserByEmail(string Email)
        {
            var respObj = new AccountMaster();
            try
            {
                var accObj = new Tbl_AccountMaster();
                accObj = new TMCDBContext().fn_GetUserByEmail(Email);
                respObj = new AccountMaster()
                {
                    ContactNumber = accObj.ContactNumber,
                    DateCreated = accObj.DateCreated.ToString("dddd, dd MMMM yyyy"),
                    Email = accObj.Email,
                    ID = accObj.ID,
                    UserName = accObj.UserName,
                    UserStatus = accObj.UserStatus,
                    Role = new TMCDBContext().fn_GetRoleByID(accObj.RoleID)
                };
            }
            catch { respObj = new AccountMaster(); }
            return respObj;
        }

        public static List<Tbl_RoleMaster> fn_GetAllRoles()
        {
            return new TMCDBContext().fn_GetAllRoles();
        }
    }
}
