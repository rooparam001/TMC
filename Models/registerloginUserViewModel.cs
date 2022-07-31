using System;

namespace TMC.Models
{
    public class registerloginUserViewModel
    {
        public string UserName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool UserStatus { get; set; }
        public string validationMessage { get; set; }
        public string userrole { get; set; }
        public string Token { get; set; }    
        public DateTime TokenExpireDate { get; set; }
    }
}
