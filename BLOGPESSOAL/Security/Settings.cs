namespace BLOGPESSOAL.Security
{
    public class Settings
    {
        private static string secret = "97a7cb2272bfaacba1a521de1f3138b2109e3de4c68e8db879bc2845d804d02f";

        public static string Secret { get => secret; set => secret = value; }

    }
}
