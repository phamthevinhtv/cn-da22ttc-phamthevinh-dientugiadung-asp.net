using System.Text.Json;

public static class SessionHelper
{
    public static void SetObject<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T? GetObject<T>(this ISession session, string key)
    {
        var json = session.GetString(key);
        if (string.IsNullOrEmpty(json)) return default;

        try
        {
            return JsonSerializer.Deserialize<T>(json);
        }
        catch
        {
            session.Remove(key);
            return default;
        }
    }

}
