namespace ECR
{
    public static class Constants
    {
        public static class Logging
        {
            public const string DEFAULT_COLOR        = "#e3e3e3";
            public const string INFRASTRUCTURE_COLOR = "#e38d46";
            public const string META_COLOR           = "#e346b4";
            public const string GAMEPLAY_COLOR       = "#4697e3";
            
            public const string INFRASTRUCTURE_REGEX = ".*Infrastructure.*";
            public const string META_REGEX           = ".*Meta.*";
            public const string GAMEPLAY_REGEX       = ".*Gameplay.*";

            public const string MESSAGE_TEMPLATE     = "<b><color={0}>[{1}]:</color></b> {2}";
        }
    }
}