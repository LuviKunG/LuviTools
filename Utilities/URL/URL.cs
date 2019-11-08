using System;
using System.Collections.Generic;
using System.Text;

namespace LuviKunG
{
    public class URL
    {
        public class Query
        {
            public Dictionary<string, string> quries { get; private set; }

            public string this[string key] => quries[key];

            public bool hasContent => quries.Count > 0;

            public Query()
            {
                quries = new Dictionary<string, string>();
            }

            public Query(string query) : this()
            {
                string cacheQuery = query;
                if (cacheQuery.Length > 0 && cacheQuery[0] == '?')
                    cacheQuery = cacheQuery.Remove(0, 1);
                string[] cacheQuries = cacheQuery.Split('&');
                for (int i = 0; i < cacheQuries.Length; i++)
                {
                    string[] queryPair = cacheQuries[i].Split('=');
                    if (queryPair.Length > 0)
                    {
                        string key = Uri.UnescapeDataString(queryPair[0]);
                        string value = queryPair.Length > 1 ? Uri.UnescapeDataString(queryPair[1]) : string.Empty;
                        Set(key, value);
                    }
                }
            }

            public bool Contains(string key)
            {
                return quries.ContainsKey(key);
            }

            public void Set(string key, string value)
            {
                if (string.IsNullOrWhiteSpace(key))
                    return;
                if (quries.ContainsKey(key))
                    quries[key] = value;
                else
                    quries.Add(key, value);
            }

            public void Set(string key, int value)
            {
                string stringValue = value.ToString("D0");
                if (quries.ContainsKey(key))
                    quries[key] = stringValue;
                else
                    quries.Add(key, stringValue);
            }

            public void Set(string key, bool value)
            {
                string stringValue = value ? bool.TrueString : bool.FalseString;
                if (quries.ContainsKey(key))
                    quries[key] = stringValue;
                else
                    quries.Add(key, stringValue);
            }

            public string value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    bool hasNext = false;
                    foreach (var pair in quries)
                    {
                        if (hasNext)
                            sb.Append('&');
                        sb.Append(Uri.EscapeDataString(pair.Key));
                        sb.Append('=');
                        sb.Append(Uri.EscapeDataString(pair.Value));
                        hasNext = true;
                    }
                    return sb.ToString();
                }
            }

            public override string ToString() { return value; }
        }

        public class Path
        {
            public List<string> paths { get; private set; }

            public string this[int i] => paths[i];

            public bool isEndRoot
            {
                get
                {
                    if (paths.Count > 0)
                        return !HasExtension(paths[paths.Count - 1]);
                    else
                        return true;
                }
            }

            public bool HasExtension(string value)
            {
                return System.IO.Path.HasExtension(value);
            }

            public Path()
            {
                paths = new List<string>();
            }

            public Path(string path) : this()
            {
                string cachePath = path;
                if (cachePath.Length > 0 && cachePath[0] == '/')
                    cachePath = cachePath.Remove(0, 1);
                string[] cachePaths = cachePath.Split('/');
                for (int i = 0; i < cachePaths.Length; i++)
                    Add(cachePaths[i]);
            }

            public void Add(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;
                paths.Add(value);
            }

            public string value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    if (paths.Count > 0)
                        sb.Append(paths[0]);
                    for (int i = 1; i < paths.Count; i++)
                    {
                        sb.Append('/');
                        sb.Append(paths[i]);
                    }
                    return sb.ToString();
                }
            }

            public override string ToString() { return value; }
        }

        public Uri uri;
        public Query query;
        public Path path;

        public URL(string url)
        {
            uri = new Uri(url);
            query = new Query(uri.Query);
            path = new Path(uri.AbsolutePath);
        }

        public string value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(uri.Scheme);
                sb.Append("://");
                sb.Append(uri.Authority);
                sb.Append('/');
                sb.Append(path.value);
                if (query.hasContent)
                {
                    sb.Append('?');
                    sb.Append(query.value);
                }
                return sb.ToString();
            }
        }

        public override string ToString() { return value; }
    }
}