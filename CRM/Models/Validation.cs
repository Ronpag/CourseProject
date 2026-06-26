using System.Text.RegularExpressions;

namespace CRM;

public static class Validation
{
    public static bool IsEnglish(string text)
    {
        return Regex.IsMatch(text, @"^[A-Za-z0-9]+$");
    }
}