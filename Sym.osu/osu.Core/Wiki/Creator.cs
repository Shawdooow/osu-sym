#region usings

using osu.Game.Users;

#endregion

namespace osu.Core.Wiki
{
    public class Creator : User
    {
        public string Quote;

        public static Creator Shawdooow => new Creator
        {
            Username = "Shawdooow",
            Id = 7726082,
            AvatarUrl = "https://a.ppy.sh/7726082?1543067532.jpeg",
            CoverUrl = "https://assets.ppy.sh/user-profile-covers/7726082/0a39dd05012a6a03746f41f404eba82b12e5699315472ce67bd24d4763100265.jpeg",
            Country = new Country
            {
                FullName = "United States",
                FlagName = "US",
            },
            Quote = "I didn't make this, I just pretend to!",
        };
    }
}
