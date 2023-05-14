namespace AnnosWebAPI.User
{
    public class UserCredential
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel()
            {
                UserName = "gm-admin",
                EmailAddress = "gm@admin.se",
                Password = "Hejsan123#",
                GivenName = "Ghazanfar",
                SurName = "Mahmood",
                Role = "Admin",
            },

            new UserModel()
            {
                UserName = "gm-user",
                EmailAddress = "gm@user.se",
                Password = "Hejsan123#",
                GivenName = "Ghazanfar",
                SurName = "Mahmood",
                Role = "User",
            }
        };
    }
}
