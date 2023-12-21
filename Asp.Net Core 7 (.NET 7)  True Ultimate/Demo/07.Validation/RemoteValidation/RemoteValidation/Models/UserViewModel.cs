

namespace RemoteValidation.Models
{
    public class UserViewModel
    {
        [Remote(action: "IsUsernameValid", controller: "User")] // remote attribute handle validation by specigy action(validation action method) and controller to
        public string Username { get; set; }
    }
}
