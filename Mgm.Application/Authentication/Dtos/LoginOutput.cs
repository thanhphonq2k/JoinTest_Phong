namespace Mgm.Authentication.Dtos
{
    public class LoginOutput
    {
        public string AccessToken { get; set; }
        public int UserId { get; set; }
        public string RoleList { get; set; }
    }
}
