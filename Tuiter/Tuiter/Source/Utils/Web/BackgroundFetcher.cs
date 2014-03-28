namespace Tuiter.Source.Utils.Web
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.IO;
    
    using Twitter;

    internal sealed class BackgroundFetcher
    {
        private readonly Dictionary<string, Action<object>> _dictionary;
        private readonly ManualResetEvent _mre;
        private readonly Queue<string> _queue;
        private readonly Thread _thread;
        private volatile bool _running;

        internal BackgroundFetcher()
        {
            _thread = new Thread(() =>
                                     {
                                         while (_running)
                                         {
                                             if (_queue.Count == 0)
                                             {
                                                 _mre.WaitOne();
                                             }

                                             Download(Dequeue());
                                         }
                                     })
                          {
                              Priority = ThreadPriority.Lowest
                          };

            _queue = new Queue<string>();
            _dictionary = new Dictionary<string, Action<object>>();
            _mre = new ManualResetEvent(false);
            _running = true;
        }

        internal void Start()
        {
            _thread.Start();
        }

        internal void Stop()
        {
            _running = false;
        }

        internal void Enqueue(string url, Action<object> callback)
        {
            if (!_dictionary.ContainsKey(url))
            {
                _dictionary.Add(url, null);
                Enqueue(url);
            }
            _dictionary[url] += callback;
        }

        private void Enqueue(string url)
        {
            lock (_queue)
            {
                _queue.Enqueue(url);

                if (_queue.Count == 1)
                    _mre.Set();
            }
        }

        private string Dequeue()
        {
            lock (_queue)
            {
                if (_queue.Count == 0)
                    return null;

                if (_queue.Count == 1)
                    _mre.Reset();

                return _queue.Dequeue();
            }
        }

        private void Download(string url)
        {
            try
            {
                var request = Methods.MakeRequest(null, HttpVerbs.Get, url);

                using (var stream = request.GetResponse().GetResponseStream())
                using (var ms = new MemoryStream())
                {
                    var buffer = new byte[1024*4];

                    while (true)
                    {
                        var read = stream.Read(buffer, 0,
                                               buffer.Length);
                        if (read <= 0)
                            break;
                        ms.Write(buffer, 0, read);
                    }
                    _dictionary[url](ms.ToArray());
                    _dictionary.Remove(url);
                }
            }
            catch(Exception e)
            {
                Enqueue(url);

                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
    }
}