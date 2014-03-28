namespace Tuiter.Source.UI.Forms
{
    using System;
    using System.Data;
    using System.Data.SqlServerCe;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Threading;
    using Controls;
    using Twitter;
    using Data;
    using Utils.Parsers;

    internal partial class TuiterForm : Form
    {
        private readonly string _dbPath;
        private readonly Assembly _assembly;
        private readonly IParser _parser;
        private TuiterAdapter _adapter;

        private Send _dialog;
        private TweetsView _backContainer;
        private TweetsView _frontContainer;
        private Control _lMenu;
        private Control _rMenu;
        private bool _status;
        private string _user;
        private Account _account;

        internal TuiterForm()
        {
            _assembly = Assembly.GetExecutingAssembly();
            _dbPath = Path.GetDirectoryName(_assembly.GetName().CodeBase) + @"\Tuiter.DB.sdf";
            _parser = new XmlParser();

            InitializeComponent();
            FillAccountData();

            Closing += Exit;       
            _status = true;

            ShowLogin();
        }

        private void Exit(object sender, EventArgs e)
        {
            _banner.Wait();
            _adapter.Exit();
            _banner.Ready();

            Application.Exit();
        }

        private void ShowLogin()
        {
            var login = new LoginForm();

            login.Credentials += account =>
                                     {
                                         _banner.Wait();

                                         try
                                         {

                                             CheckAuthentication(account);
                                             login.Close();
                                         }
                                         catch (WebException e)
                                         {
                                             _banner.Ready(e.Message);
                                             ShowLogin();
                                         }
                                     };

            login.ShowDialog();
        }

        private void CheckAuthentication(Account account)
        {
            var request = Methods.MakeRequest(account, HttpVerbs.Get,
                                              "http://twitter.com/account/verify_credentials.xml");
            var stream = request.GetResponse().GetResponseStream();
            _parser.Parse(stream, _dataSet);

            _user = account.Username;
            var res = _dataSet.Tables["user"].Select(String.Format("screen_name = '{0}'", _user))[0];
            using (var table = _dataSet.Tables["account"])
            {
                if (table.Select(String.Format("screen_name = '{0}'", _user)).Length == 0)
                    table.Rows.Add(table.NewRow()["screen_name"] = _user);
            }

            _user = res["id"] as string;

            InitMenus();

            FillTwitterData();

            _account = account;
            _banner.Ready("Status");
        }

        internal void SwitchContainer()
        {
            if (InvokeRequired)
                BeginInvoke(new Action(SwitchContainer));
            else
            {
                _frontContainer = Interlocked.Exchange(ref _backContainer, _frontContainer);
                _backContainer.Visible = false;
                _frontContainer.Visible = true;
            }
        }

        private void ScreenClick(object sender, EventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new EventHandler(ScreenClick), sender, e);
            else
            {
                var x = MousePosition.X;

                if (x < _lMenu.Width)
                {
                    _lMenu.Visible = true;
                    _rMenu.Visible = false;

                    return;
                }
                
                if (x > Width - _rMenu.Width)
                {
                    _rMenu.Visible = true;
                    _lMenu.Visible = false;

                    return;
                }

                _lMenu.Visible = false;
                _rMenu.Visible = false;
                _frontContainer.Select(new Point(MousePosition.X, MousePosition.Y));
            }
        }

        private void InitMenus()
        {
            if (InvokeRequired)
                BeginInvoke(new Action(InitMenus));
            else
            {
                _adapter = new TuiterAdapter(_dataSet);
                _lMenu = new Control();
                _rMenu = new Control();
                _backContainer = new TweetsView();
                _frontContainer = new TweetsView();

                SuspendLayout();

                //
                // _adapter
                //
                _adapter.Status += _backContainer.TweetCreated;
                _adapter.Message += _frontContainer.TweetCreated;

                //
                // _backContainer
                //
                _backContainer.AutoScroll = false;
                _backContainer.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                _backContainer.Click += ScreenClick;
                _backContainer.Size = Size;

                //
                // _frontContainer
                //
                _frontContainer.AutoScroll = false;
                _frontContainer.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                _frontContainer.Click += ScreenClick;
                _frontContainer.Size = Size;

                // 
                // _lMenu
                // 
                
                /*CHECK FRIENDS STATUS*/
                var image = Properties.Resources.user_timeline;
                var pictureBox = new PictureBox {Image = image, Size = image.Size, Location = new Point(0, 0)};
                pictureBox.Click += RefreshFriendsTimeline;
                _lMenu.Controls.Add(pictureBox);
                /*CHECK MESSAGES*/
                image = Properties.Resources.show_messages;
                pictureBox = new PictureBox {Image = image, Size = image.Size, Location = new Point(0, 48)};
                pictureBox.Click += RefreshMessages;
                _lMenu.Controls.Add(pictureBox);
                /*EXIT*/
                image = Properties.Resources.exit;
                pictureBox = new PictureBox { Image = image, Size = image.Size, Location = new Point(0, 96) };
                pictureBox.Click += Exit;
                _lMenu.Controls.Add(pictureBox);

                // 
                // _rMenu
                // 
                /*NEW STATUS*/
                image = Properties.Resources.status_post;
                pictureBox = new PictureBox { Image = image, Size = image.Size, Location = new Point(0, 0) };
                pictureBox.Click += PostStatus;
                _rMenu.Controls.Add(pictureBox);
                /*NEW STATUS REPLY*/
                image = Properties.Resources.status_reply;
                pictureBox = new PictureBox {Image = image, Size = image.Size, Location = new Point(0, 48)};
                pictureBox.Click += StatusReply;
                _rMenu.Controls.Add(pictureBox);
                /*SEND MESSAGE*/
                image = Properties.Resources.message_post;
                pictureBox = new PictureBox { Image = image, Size = image.Size, Location = new Point(0, 96) };
                pictureBox.Click += PostMessage;
                _rMenu.Controls.Add(pictureBox);
                /*NEW MESSAGE REPLY*/
                image = Properties.Resources.message_reply;
                pictureBox = new PictureBox {Image = image, Size = image.Size, Location = new Point(0, 144)};
                pictureBox.Click += MessageReply;
                _rMenu.Controls.Add(pictureBox);


                // Menus display
                _lMenu.Size = new Size(48, 144);
                _lMenu.Location = new Point(0, Height / 2 - _lMenu.Height / 2);
                _lMenu.Visible = false;

                _rMenu.Size = new Size(48, 192);
                _rMenu.Location = new Point(Width - _rMenu.Width, Height/2 - _rMenu.Height/2);
                _rMenu.Visible = false;

                // 
                // Controls
                //
                Controls.Add(_frontContainer);
                Controls.Add(_backContainer);

                _backContainer.BringToFront();
                _frontContainer.BringToFront();

                Controls.Add(_rMenu);
                Controls.Add(_lMenu);

                _lMenu.BringToFront();
                _rMenu.BringToFront();

                _banner.BringToFront();


                new Scroll(_frontContainer);
                new Scroll(_backContainer);
                
                SwitchContainer();

                ResumeLayout(false);
            }
        }

        private void RefreshFriendsTimeline(object sender, EventArgs e)
        {
            if (!_status)
            {
                SwitchContainer();
                _status = true;
            }
            else
            {
                _frontContainer.Location = new Point(0, 0);
                _banner.Wait();
                Methods.BeginGetFriendsTimeline(_account, _dataSet.Tables["status"].Compute("MAX(id)", "").ToString(),
                                                ConsumeDataStream);
            }
        }

        private void RefreshMessages(object sender, EventArgs e)
        {
            if (_status)
            {
                SwitchContainer();
                _status = false;
            }
            else
            {
                _frontContainer.Location = new Point(0, 0);
                _banner.Wait();
                Methods.BeginGetUserDirectMessages(_account,
                                                   _dataSet.Tables["direct_message"].Compute("MAX(id)", "").ToString(),
                                                   ConsumeDataStream);
            }
        }

        private void PostMessage(object sender, EventArgs e)
        {
            SendMessage(false);
        }

        private void PostStatus(object sender, EventArgs e)
        {
            SendStatus(false);
        }

        private void StatusReply(object sender, EventArgs e)
        {
            if (_frontContainer.Selected != null)
                SendStatus(true);
            else
                MessageBox.Show("Select someone to reply.");
        }

        private void MessageReply(object sender, EventArgs e)
        {
            if (_frontContainer.Selected != null)
                SendMessage(true);
            else
            {
                MessageBox.Show("Select someone to message.");
            }
        }

        private void SendStatus(bool reply)
        {
            if (!reply)
                _dialog = new Send(_account.Username, null);
            else
            {
                var item = _frontContainer.Selected as VisualTweet;
                _dialog = new Send(item.Tweet["screen_name"] as String,
                                   String.Format("@{0} ", item.Tweet["screen_name"]));
            }

            _dialog.Message += (name, msg) =>
                                   {
                                       _banner.Wait();
                                       Methods.BeginPostUpdate(_account, msg, name, CheckPostSuccess);
                                       _dialog.Close();
                                   };
            _dialog.Text += " Status";
            _dialog.ShowDialog();
        }

        private void SendMessage(bool reply)
        {
            if (!reply)
                _dialog = new Send();
            else
            {
                var item = _frontContainer.Selected as VisualTweet;
                _dialog = new Send(null, item.Tweet["screen_name"] as string);
            }

            _dialog.Message += (name, msg) =>
                                   {
                                       _banner.Wait();
                                       Methods.BeginPostDirectMessages(_account, name, msg, CheckPostSuccess);
                                       _dialog.Close();
                                   };
            
            _dialog.Text += " Message";
            _dialog.ShowDialog();
        }

        private void ConsumeDataStream(IAsyncResult ar)
        {
            var request = ar.AsyncState as WebRequest;

            try
            {
                using(var response = request.EndGetResponse(ar))
                using(var stream = response.GetResponseStream())
                {
                    _parser.Parse(stream, _dataSet);
                    _banner.Ready();
                }
            }
            catch (WebException e)
            {
                _banner.Ready(e.Message);
            }
        }

        private void CheckPostSuccess(IAsyncResult ar)
        {
            var request = ar.AsyncState as WebRequest;
            if (request == null)
            {
                _banner.Ready("Cannot send tweet!");
                return;
            }

            try
            {
                using(request.EndGetResponse(ar))
                    _banner.Ready("Success!");
            }
            catch (WebException e)
            {
                _banner.Ready(e.Message);
            }
        }

        private void FillAccountData()
        {
            using (var connection = new SqlCeConnection(@"Data Source=" + _dbPath))
            {
                connection.Open();
                FillTable(connection, "account", "SELECT * FROM \"account\"");
                connection.Close();
            }
        }

        private void FillTwitterData()
        {
            using (var connection = new SqlCeConnection(@"Data Source=" + _dbPath))
            {
                connection.Open();
                FillTable(connection, "user", "SELECT * FROM \"user\"");
                FillTable(connection, "image", "SELECT * FROM \"image\"");
                FillTable(connection, "status", "SELECT TOP(25) * FROM \"status\"");
                FillTable(connection, "direct-message",
                          String.Format("SELECT TOP(25) * FROM \"direct_message\" WHERE recipient_id = '{0}'", _user));

                connection.Close();
            }
        }

        private void FillTable(SqlCeConnection connection, string table, string selection)
        {
            var dataAdapter = new SqlCeDataAdapter(selection, connection);
            dataAdapter.FillSchema(_dataSet, SchemaType.Source, table);
            dataAdapter.Fill(_dataSet, table);
        }
    }
}