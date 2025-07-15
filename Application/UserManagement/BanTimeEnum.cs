namespace Application.UserManagement
{
    public enum BanTimeEnum
    {
        ONEWEEK, THIRTYDAYS, SIXTYDAYS, NINETYDAYS, SIXMONTHS, ONEYEAR, PERMANENT
    }

    public static class BanTimeEnumExtensions
    {
        public static DateTime ToDateTime(this BanTimeEnum banTime)
        {
            return banTime switch
            {
                BanTimeEnum.ONEWEEK => DateTime.UtcNow.AddDays(7),
                BanTimeEnum.THIRTYDAYS => DateTime.UtcNow.AddDays(30),
                BanTimeEnum.SIXTYDAYS => DateTime.UtcNow.AddDays(60),
                BanTimeEnum.NINETYDAYS => DateTime.UtcNow.AddDays(90),
                BanTimeEnum.SIXMONTHS => DateTime.UtcNow.AddMonths(6),
                BanTimeEnum.ONEYEAR => DateTime.UtcNow.AddYears(1),
                BanTimeEnum.PERMANENT => DateTime.MaxValue,
                _ => throw new ArgumentOutOfRangeException(nameof(banTime), $"Invalid ban time: {banTime}")
            };
        }
    }
}