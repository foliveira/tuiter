namespace Tuiter.Source.Twitter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class Tweet
    {
        private readonly Dictionary<string, object> _dictionary;

        internal event Action<string, object> ValueChanged;

        internal Tweet()
        {
            _dictionary = new Dictionary<string, object>
                              {
                                  {"id", null},
                                  {"screen_name", null},
                                  {"text", null},
                                  {"created_at", null},
                                  {"image", null}
                              };
        }

        internal object this[string key]
        {
            set
            {
                lock (this)
                {
                    if (!_dictionary.ContainsKey(key)) 
                        return;

                    _dictionary[key] = value;

                    NotifyChanges(key, value);
                }
            }
            get
            {
                lock (this)
                    return _dictionary[key];
            }
        }

        internal string[] GetProperties()
        {
            return _dictionary.Keys.ToArray();
        }

        private void NotifyChanges(string key, object value)
        {
            var changed = ValueChanged;

            if (changed != null)
                changed(key, value);
        }
    }
}