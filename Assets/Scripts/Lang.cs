using GameDevWare.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Automation
{
    public class Lang
    {
        public static Lang instance = new Lang();
        public static LangDescription[] langList;
        public static Dictionary<string, LangDescription> langs = new Dictionary<string, LangDescription>();
        public static event Action onLangChanged;

        public string language;
        public Dictionary<string, string> langEntries = new Dictionary<string, string>();
        public Dictionary<string, KeyValuePair<Regex, string>> langRegexes = new Dictionary<string, KeyValuePair<Regex, string>>();
        public Dictionary<string, string> langStartsWith = new Dictionary<string, string>();

        private static bool inited = false;
        public static void Init()
        {
            if (inited) return;
            TextAsset asset = Resources.Load<TextAsset>("Langs/languages");
            if (asset != null)
            {
                try
                {
                    langList = Json.Deserialize<LangDescription[]>(asset.text);
                    langs.Clear();
                    foreach (LangDescription langDescription in langList)
                    {
                        langs.Add(langDescription.code, langDescription);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to load lang file {asset.name}: {ex}");
                }
            }
            Load();
            inited = true;
        }

        public static void Load(string language = "en")
        {
            instance.langEntries.Clear();
            instance.langRegexes.Clear();
            instance.langStartsWith.Clear();
            if (language != "en")
            {
                Load();
            }
            instance.language = language;

            TextAsset asset = Resources.Load<TextAsset>("Langs/" + language);
            if (asset != null)
            {
                try
                {
                    LoadEntries(Json.Deserialize<Dictionary<string, string>>(asset.text));
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to load lang file {asset.name}: {ex}");
                }
            }
            onLangChanged?.Invoke();
        }

        private static void LoadEntries(Dictionary<string, string> entries)
        {
            foreach (KeyValuePair<string, string> entry in entries)
            {
                string text = entry.Key;
                int num = text.CountChars('*');
                if (num > 0)
                {
                    if (num == 1 && text.EndsWith("*"))
                    {
                        instance.langStartsWith[text.TrimEnd('*')] = entry.Value;
                    }
                    else
                    {
                        Regex key = new Regex("^" + text.Replace("*", "(.*)") + "$", RegexOptions.Compiled);
                        instance.langRegexes[text] = new KeyValuePair<Regex, string>(key, entry.Value);
                    }
                }
                else
                {
                    instance.langEntries[text] = entry.Value;
                }
            }
        }

        public static string GetIfExists(string key, params object[] param)
        {
            if (instance.langEntries.TryGetValue(key, out string value))
            {
                return string.Format(value, param);
            }

            return null;
        }

        public static string GetMatchingIfExists(string key, params object[] param)
        {
            if (instance.langEntries.TryGetValue(key, out string value))
            {
                return value;
            }

            foreach (KeyValuePair<string, string> item in instance.langStartsWith)
            {
                if (StringUtil.FastStartsWith(key, item.Key))
                {
                    return string.Format(item.Value, param);
                }
            }

            foreach (KeyValuePair<Regex, string> value2 in instance.langRegexes.Values)
            {
                if (value2.Key.IsMatch(key))
                {
                    return string.Format(value2.Value, param);
                }
            }

            return null;
        }

        public static string Get(string key, params object[] param)
        {
            return string.Format(GetUnformatted(key), param);
        }

        public static string GetUnformatted(string key)
        {
            if (instance.langEntries.TryGetValue(key, out string value))
            {
                return value;
            }

            return key;
        }

        public static string GetMatching(string key, params object[] param)
        {
            if (instance.langEntries.TryGetValue(key, out string value))
            {
                return string.Format(value, param);
            }

            foreach (KeyValuePair<string, string> item in instance.langStartsWith)
            {
                if (StringUtil.FastStartsWith(key, item.Key))
                {
                    return string.Format(item.Value, param);
                }
            }

            foreach (KeyValuePair<Regex, string> value2 in instance.langRegexes.Values)
            {
                if (value2.Key.IsMatch(key))
                {
                    return string.Format(value2.Value, param);
                }
            }

            return string.Format(key, param);
        }

        public static bool HasTranslation(string key, bool findWildcarded = true)
        {
            if (instance.langEntries.ContainsKey(key))
            {
                return true;
            }

            if (findWildcarded)
            {
                foreach (KeyValuePair<string, string> item in instance.langStartsWith)
                {
                    if (StringUtil.FastStartsWith(key, item.Key))
                    {
                        return true;
                    }
                }

                foreach (KeyValuePair<Regex, string> value in instance.langRegexes.Values)
                {
                    if (value.Key.IsMatch(key))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [Serializable]
        public class LangDescription
        {
            public string code;
            public string name;
            public string icon;
        }
    }
}
