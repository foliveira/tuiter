namespace Tuiter.Source.Data
{
    using System;
    using System.Data;
    using Twitter;
    using Utils.Web;
    using TuiterDSTableAdapters;

    internal class TuiterAdapter
    {
        private readonly TuiterDS _set;
        private readonly BackgroundFetcher _backgroundFetcher;

        internal event Action<Tweet> Status;
        internal event Action<Tweet> Message;

        internal TuiterAdapter(TuiterDS set)
        {
            if (set == null)
                throw new NullReferenceException();

            _set = set;
            _backgroundFetcher = new BackgroundFetcher();

            _set.Tables["status"].RowChanged += StatusChanged;
            _set.Tables["direct_message"].RowChanged += MessageChanged;
            _set.Tables["image"].RowChanged += ImageChanged;

            _backgroundFetcher.Start();
        }

        internal void Exit()
        {
            _backgroundFetcher.Stop();

            new accountTableAdapter().Update(_set);
            new imageTableAdapter().Update(_set);
            new userTableAdapter().Update(_set);
            new statusTableAdapter().Update(_set);
            new direct_messageTableAdapter().Update(_set);

            _set.AcceptChanges();
        }

        private void ImageChanged(object sender, DataRowChangeEventArgs e)
        {
            _backgroundFetcher.Enqueue(e.Row["profile_image_url"] as string, o => e.Row["image"] = o);
        }

        private void StatusChanged(object sender, DataRowChangeEventArgs e)
        {
            RowChanged("status", e);
        }

        private void MessageChanged(object sender, DataRowChangeEventArgs e)
        {
            RowChanged("message", e);
        }

        private void RowChanged(string table, DataRowChangeEventArgs e)
        {
            if (e.Action != DataRowAction.Add)
                return;

            var descriptor = Process(e.Row);

            if ("status".Equals(table))
                NotifyStatusChange(descriptor);
            else
                NotifyMessageChanged(descriptor);
        }

        protected Tweet Process(DataRow row)
        {
            var tweet = new Tweet();

            var userRow = _set.Tables["user"].Select(
                String.Format(@"id = '{0}'", row["sender_id"]))[0];

            tweet["id"] = row["id"];
            tweet["screen_name"] = userRow["screen_name"];

            var userImage = _set.Tables["image"].Select(
                String.Format("profile_image_url = '{0}'", userRow["profile_image_url"]))[0];

            tweet["image"] = userImage["image"];
            tweet["created_at"] = (DateTime)row["created_at"];
            tweet["text"] = row["text"];

            if (userImage["image"].Equals(DBNull.Value))
                _backgroundFetcher.Enqueue(userRow["profile_image_url"] as string, o => tweet["image"] = o);

            return tweet;
        }

        private void NotifyStatusChange(Tweet obj)
        {
            var status = Status;

            if (status != null)
                status(obj);
        }

        private void NotifyMessageChanged(Tweet obj)
        {
            var message = Message;

            if (message != null)
                message(obj);
        }
    }
}