namespace JWTWebApi.Models
{
    public class UserDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }

    //We will only use this to send the data to the server for registering and login the user
}
