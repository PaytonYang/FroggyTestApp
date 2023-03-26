using CommunityToolkit.Mvvm.ComponentModel;
using Froggy.Database;
using FroggyTest.WPF.Models.DBSchema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FroggyTest.WPF.Models;

public partial class UserModel : ObservableValidator
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "User Name is required")]
    private string _userName = "";
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [MinLength(5)]
    private string _password = "";
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [EmailAddress(ErrorMessage = "Wrong email address format")]
    [Required(ErrorMessage = "Email is required")]
    private string _email = "";

    public Authority AuthorityLevel { get; set; } = Authority.Registered;

    public void RevalidateProperty()
    {
        this.ValidateAllProperties();
    }
    
    public static async Task AddUser(IDapperDB db, UserModel user)
    {
        try
        {
            UserTable user_table = new UserTable
            {
                UserName = user.UserName,
                UserEmail = user.Email,
                UserPassword = user.Password,
                AuthorityId = (int)user.AuthorityLevel
            };
            await db.Insert("UserTable", new[] { "UserName", "UserPassword", "UserEmail", "AuthorityId" }, user_table);
        }
        catch { throw; }
    }

    public static async Task<UserModel?> CheckUserExitsted(IDapperDB db, UserModel user)
    {
        try
        {
            var results = await db.QueryAsync<UserTable>("UserTable", $"UserName='{user.UserName}' AND UserPassword='{user.Password}'");
            if (results.Count() > 0)
            {
                UserTable user_table = results.First();
                return new UserModel { UserName = user_table.UserName, Email = user_table.UserEmail, Password = user_table.UserPassword, AuthorityLevel = (Authority)user_table.AuthorityId };
            }
            else
            {
                return null;
            }
        }
        catch { throw; }
    }

    public enum Authority
    {
        Guest = 0,
        Registered,
        Admin
    }
}
