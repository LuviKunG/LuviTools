using System;
using System.Text;
using System.Collections.Generic;
using WebSocketSharp;
using System.Collections;

namespace LuviKunG.Web.Socket
{
    public sealed class UriQuery : IEnumerable<KeyValuePair<string, string>>, IEnumerable
    {
        private Dictionary<string, string> queries;

        public UriQuery()
        {
            queries = new Dictionary<string, string>();
        }

        public UriQuery(string query) : this()
        {
            string queryString = Uri.UnescapeDataString(query);
            if (queryString.Contains('?'))
                queryString = queryString.Substring(queryString.IndexOf('?') + 1);
            foreach (string kv in queryString.Split('&'))
            {
                string[] singlePair = kv.Split('=');
                if (singlePair.Length == 2)
                    queries.Add(singlePair[0], singlePair[1]);
                else
                    queries.Add(singlePair[0], string.Empty);
            }
        }

        public UriQuery(UriQuery query) : this()
        {
            foreach (var kv in query)
                queries.Add(kv.Key, kv.Value);
        }

        public string this[string key] => queries[key];

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() { return queries.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return queries.GetEnumerator(); }

        public bool HasKey(string key)
        {
            return queries.ContainsKey(key);
        }

        public bool RemoveKey(string key)
        {
            return queries.Remove(key);
        }

        public void SetKey(string key, string value)
        {
            if (queries.ContainsKey(key))
                queries[key] = value;
            else
                queries.Add(key, value);
        }

        public void SetKey(string key, int value)
        {
            SetKey(key, value.ToString("D0"));
        }

        public void SetKey(string key, bool value)
        {
            SetKey(key, value ? "true" : "false");
        }

        public string ToQueryString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('?');
            bool next = false;
            foreach (var kv in queries)
            {
                if (next)
                    sb.Append('&');
                sb.Append(Uri.EscapeDataString(kv.Key));
                sb.Append('=');
                sb.Append(Uri.EscapeDataString(kv.Value));
                next = true;
            }
            return sb.ToString();
        }
    }
}