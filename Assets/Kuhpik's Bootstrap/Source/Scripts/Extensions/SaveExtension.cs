using UnityEngine;

namespace Kuhpik
{
    public static class SaveExtension
    {
        public static void Save<T>(T value, string key)
        {
            var json = JsonUtility.ToJson(value);
            PlayerPrefs.SetString(key, json);
        }

        public static T Load<T>(string key, T defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                var json = PlayerPrefs.GetString(key);
                return JsonUtility.FromJson<T>(json);
            }

            else
            {
                return defaultValue;
            }
        }
    }
}