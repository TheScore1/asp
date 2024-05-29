namespace Core.Extensions;

public static class StringExtensions
{
    public static bool IsInRange(this string str, int minLength, int maxLength) =>
        str.Length >= minLength && str.Length <= maxLength;
}