using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeSheet.Models;

namespace TimeSheet.Repository
{
    public class LoginApi
    {
        public async Task<UserDetail> LoginWithApi(LoginModel loginModel)
        {
            var loginDetail = "{\"Username\":\"UsernameValue\",\"Password\":\"PasswordValue\",\"Devicetoken\":\"asasd\",\"Devicetype\":\"asdasd\"}";
            loginDetail = loginDetail.Replace("UsernameValue", loginModel.username).Replace("PasswordValue", loginModel.password);

            using (var httpClient = new HttpClient())
            {
                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                var httpContent = new StringContent(loginDetail, Encoding.UTF8, "application/json");
                //httpClient.DefaultRequestHeaders.Add("authKey", "E65280E1-0FFD-44FC-87FC-FC6DE2BC5E5B");

                // Do the actual request and await the response
                var httpResponse = await httpClient.PostAsync("http://trustapi.mycityapp.in/customer_care/web_service/login", httpContent);

                // If the response contains content we want to read it!
                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<RootObject>(responseContent);

                    /*Map login api user to user detail*/
                    UserDetail user = null;

                    if (result != null && result.message != null && result.message.Contains("success") && result.User != null)
                    {
                        user = new UserDetail();
                        user.Name = result.User.first_name + " " + result.User.last_name;
                        user.EmailId = result.User.email;
                        user.EmployeeId = result.User.id;
                        user.Username = result.User.username;
                        user.Password = result.User.password;
                        user.RoleId = result.User.role_id;
                    }

                    // From here on you could deserialize the ResponseContent back again to a concrete C# type using Json.Net
                    return user;
                }
            }

            return null;
        }

        public async Task<List<User>> EmployeeListApi()
        {
            using (var httpClient = new HttpClient())
            {
                // Do the actual request and await the response
                var httpResponse = await httpClient.GetAsync("http://trustapi.mycityapp.in/customer_care/web_service/employeeList");

                // If the response contains content we want to read it!
                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<RootObject>(responseContent);

                    // From here on you could deserialize the ResponseContent back again to a concrete C# type using Json.Net
                    return result.employeeList;
                }
            }

            return null;
        }
    }
}