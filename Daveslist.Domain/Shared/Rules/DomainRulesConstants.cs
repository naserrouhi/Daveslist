namespace Daveslist.Domain.Shared.Constants;

public static class DomainRulesConstants
{
    public static class Listing
    {
        public const int MaximumPictureOfListing = 10;
        public const int MaxContentLength = 5000;
        public const int MaxReplyLength = 255;
        public const int MaxTitleLength = 255;
        public const int MaxPictureUrlLength = 1024;
    }

    public static class Category
    {
        public const int MaxNameLength = 128;
    }

    public static class PrivateMessage
    {
        public const int MaxContentLength = 1000;
    }

    public static class User
    {
        public const int MaxFirstNameLength = 100;
        public const int MaxLastNameLength = 100;
    }
}
