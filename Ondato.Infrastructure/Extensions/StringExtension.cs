namespace Ondato.Infrastructure.Extensions
{
  public static class StringExtension
  {
    public static bool IsNullOrEmpty(this string str)
    {
      return string.IsNullOrEmpty(str);
    }

    public static bool IsNullOrWhiteSpace(this string str)
    {
      return string.IsNullOrWhiteSpace(str);
    }

    public static int ConvertToInt(this string value, int defaultValue)
    {
      int result;
      if (int.TryParse(value, out result))
        return result;

      return defaultValue;
    }

    public static string ToLowerCaseKey(this string key)
    {
      var keyList = key.Split('.');
      for (var i = 0; i < keyList.Length; i++)
      {
        keyList[i] = char.ToLowerInvariant(keyList[i][0]) + keyList[i].Substring(1);
      }

      return string.Join('.', keyList);
    }
  }
}
