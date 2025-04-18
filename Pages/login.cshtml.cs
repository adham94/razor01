using Factory;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model;


namespace OfflineFirstRazor.Pages
{
    public class loginModel : PageModel
    {
        public string Message { get; set; }
        public string Domain { get; set; }

        public loginModel()
        {
            Domain = DomainHelper.GetPCDomainName();
        }

        public void OnGet()
        {
            //Console.WriteLine("aaa");
            Message = " Hey Stranger, welcome to login page";
            if (Request.Query.ContainsKey("err"))
            {
                Message = Request.Query["err"].ToString();
            }
        }

        public async Task OnPost()
        {
            try
            {

                //==============================================================
                //Authentication code
                //===============================================================

                var loginRequest = new LoginModel()
                {
                    Domain = Request.Form["domain"],
                    UserName = Request.Form["username"]
                };
                loginRequest.Password = Request.Form["password"];

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "users.json");
                if (!System.IO.File.Exists(filePath))
                {
                    Message = "User data file not found.";
                    return;
                }

                var jsonData = await System.IO.File.ReadAllTextAsync(filePath);
                var users = System.Text.Json.JsonSerializer.Deserialize<List<UserModel>>(jsonData);

                var matchedUser = users?.FirstOrDefault(u =>
                    u.Domain.Equals(loginRequest.Domain, StringComparison.OrdinalIgnoreCase) &&
                    u.UserName.Equals(loginRequest.UserName, StringComparison.OrdinalIgnoreCase) &&
                    u.Password == loginRequest.GetPasswordAsString());

                if (matchedUser != null)
                {
                    Message = $"Welcome back, {matchedUser.UserName}!";
                    // Add session or redirection logic here
                    // Save login info to session
                    HttpContext.Session.SetString("username", matchedUser.UserName);
                    HttpContext.Session.SetString("domain", matchedUser.Domain);

                    // Redirect to home page
                    Response.Redirect("/home");
                    return;
                }
                else
                {
                    Message = "Invalid username or password.";
                }
            }
            catch (Exception ex)
            {
                Message = $"Login error: {ex.Message}";
            }
        }

    }
}

public class UserModel
{
    public string Domain { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}