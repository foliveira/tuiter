namespace Tuiter.Source.Twitter
{
    using System;
    using System.Net;
    using Utils.Web;

    internal enum HttpVerbs
    {
        Post,
        Get
    }

    internal static class Methods
    {
        internal static void BeginGetUserDirectMessages(Account account, string sinceId, AsyncCallback cb)
        {
            var url = String.Format("http://twitter.com/direct_messages.xml?{0}",
                           sinceId.Length == 0 ? "count=25" : "since_id=" + sinceId);
            var request = MakeRequest(account, HttpVerbs.Get, url);

            request.BeginGetResponse(cb, request);
        }

        internal static void BeginGetFriendsTimeline(Account account, string sinceId, AsyncCallback cb)
        {
            var url = string.Format("http://twitter.com/statuses/friends_timeline.xml?{0}",
                                    sinceId.Length == 0 ? "count=25" : "since_id=" + sinceId);
            var request = MakeRequest(account, HttpVerbs.Get, url);

            request.BeginGetResponse(cb, request);
        }

        internal static void BeginPostUpdate(Account account, string text, string answerTo, AsyncCallback cb)
        {
            var url = string.Format("http://twitter.com/statuses/update.xml?status={0}{1}",
                                    HttpUtilities.UrlEncode(text),
                                    (answerTo.Length != 0 && !answerTo.Equals(account.Username)
                                         ? "&in_reply_to_status_id=" + answerTo
                                         : string.Empty));
            var request = MakeRequest(account, HttpVerbs.Post, url);
            request.BeginGetResponse(cb, request);
        }

        internal static void BeginPostDirectMessages(Account account, string recipient, string text, AsyncCallback cb)
        {
            var url = string.Format("http://twitter.com/direct_messages/new.xml?user={0}&text={1}", recipient,
                                    HttpUtilities.UrlEncode(text));
            var request = MakeRequest(account, HttpVerbs.Post, url);
            request.BeginGetResponse(cb, request);
        }

        internal static HttpWebRequest MakeRequest(Account account, HttpVerbs verb, string url)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);

            if(account != null)
            {
                request.Credentials = new NetworkCredential(account.Username, account.Password);
                request.PreAuthenticate = true;
                request.Headers.Add("Authorization", string.Format("Basic {0}", account));
            }

            if (verb.Equals(HttpVerbs.Get))
                request.Method = "GET";
            else
            {
                ServicePointManager.Expect100Continue = false;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("X-Twitter-Client", "Tuiter");
            }

            request.Timeout = 30000;

            return request;
        }
    }
}