namespace JWTWebApi.Models
{
    public class User
    {
       // public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
    //We WONT store the user to the db 
}
