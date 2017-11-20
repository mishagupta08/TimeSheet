using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeSheet.Models
{
    public class User
    {
        public string id { get; set; }
        public string username { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string password { get; set; }
        public string cpassword { get; set; }
        public string role_id { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string phone { get; set; }
        public string dob { get; set; }
        public string sex { get; set; }
        public string joining_date { get; set; }
        public string marital_status { get; set; }
        public string anniversary_date { get; set; }
        public string work_handled { get; set; }
        public string department_id { get; set; }
        public string remark { get; set; }
        public string profile_image { get; set; }
        public object complaint_category_id { get; set; }
        public object complaint_type_id { get; set; }
        public string device_token { get; set; }
        public string device_type { get; set; }
        public string otp { get; set; }
        public string otp_expire { get; set; }
        public string otp_attempt { get; set; }
        public string mobile_verify { get; set; }
        public string status { get; set; }
        public string created { get; set; }
        public string name { get; set; }
        public string role_name { get; set; }
        public string department { get; set; }
    }

    public class RootObject
    {
        public string code { get; set; }
        public string message { get; set; }
        public User User { get; set; }
        public List<User> employeeList { get; set; }        
    }

    
}