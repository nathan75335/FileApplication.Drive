using Blazored.SessionStorage;
using Newtonsoft.Json;
using System.Text;

namespace CoursWork.Drive.ClientBlazor.Extension;

public static class SessionStorageServiceExtension
{
    public static async Task SaveItemEncryptedAsync<T>(this ISessionStorageService session, string key, T item)
    {
        var itemSerialized = JsonConvert.SerializeObject(item);
        var baseJson = Convert.ToBase64String(Encoding.UTF8.GetBytes(itemSerialized));
        await session.SetItemAsync(key, baseJson);
    }

    public static async Task<T?> ReadEncryptedItemAsync<T>(this ISessionStorageService session, string key)
    {
        var baseJson = await session.GetItemAsync<string>(key);
        var item = Encoding.UTF8.GetString(Convert.FromBase64String(baseJson));

        return JsonConvert.DeserializeObject<T>(item);
    }
}
