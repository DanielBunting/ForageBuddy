using System.Text;

namespace ForageBuddy
{
    public static class StringExtensions
    {

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '-')
                    sb.Append(c);
            return sb.ToString();
        }

        public static string ToTitleCase(this string str)
        {
            var cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }
    }
}
