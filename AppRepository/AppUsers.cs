using EntitesInterfaces.AppModels;
using EntitesInterfaces.DBEntities;
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
    }
}
