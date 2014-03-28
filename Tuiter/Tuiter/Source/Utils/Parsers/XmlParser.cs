namespace Tuiter.Source.Utils.Parsers
{
    using System.Data;
    using System.IO;
    using System.Xml;
    using Data;

    internal sealed class XmlParser : IParser
    {
        public void Parse(Stream stream, TuiterDS set)
        {
            using (var reader = new XmlTextReader(stream))
            {
                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.Element) 
                        continue;
                    
                    var name = reader.Name;

                    switch (name)
                    {
                        case "user":
                            var subtree = reader.ReadSubtree();
                            subtree.ReadToDescendant("id");
                            ParseUser(set, subtree, subtree.ReadString());
                            break;
                        case "status":
                            ParseStatus(set, reader.ReadSubtree());
                            break;
                        case "direct_message":
                            ParseMessage(set, reader.ReadSubtree());
                            break;
                    }
                }
            }
        }

        private static void ParseUser(DataSet set, XmlReader reader, string id)
        {
            using (var table = set.Tables["user"])
            {
                if (table.Select("id = '" + id + "'").Length == 0)
                {
                    var row = table.NewRow();
                    row["id"] = id;
                    string name;

                    while (reader.Read())
                    {
                        if (reader.NodeType != XmlNodeType.Element) 
                            continue;

                        name = reader.Name;
                        switch (name)
                        {
                            case "screen_name":
                                row[name] = reader.ReadString();

                                break;
                            case "profile_image_url":
                                var value = reader.ReadString();
                                using (var image = set.Tables["image"]) 
                                {
                                    if (image.Select(string.Format("{0} = '{1}'", name, value)).Length == 0)
                                        image.Rows.Add(table.NewRow()[name] = value);
                                }
                                row[name] = value;

                                break;
                            case "status":
                                ShredSubtree(reader.ReadSubtree());

                                break;
                        }
                    }
                    table.Rows.Add(row);
                }
                else 
                    ShredSubtree(reader);
            }
        }

        private static void ParseStatus(DataSet set, XmlReader reader)
        {
            using (var table = set.Tables["status"])
            {
                var row = table.NewRow();
                string name;

                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.Element) 
                        continue;

                    name = reader.Name;
                    switch (name)
                    {
                        case "user":
                            var subtree = reader.ReadSubtree();
                            subtree.ReadToDescendant("id");

                            var value = subtree.ReadString();

                            ParseUser(set, subtree, value);
                            row["sender_id"] = value;

                            break;
                        case "created_at":
                            row[name] = Date.ToDate(reader.ReadString());

                            break;
                        case "id":
                        case "text":
                            row[name] = reader.ReadString();

                            break;
                    }
                }
                if (table.Select("id = '" + row["id"] + "'").Length == 0)
                    table.Rows.Add(row);
            }
        }

        private static void ParseMessage(DataSet set, XmlReader reader)
        {
            using (var table = set.Tables["direct_message"])
            {
                reader.ReadToDescendant("id");
                var id = reader.ReadString();

                if (table.Select("id = '" + id + "'").Length == 0)
                {
                    var row = table.NewRow();
                    row["id"] = id;

                    string name;

                    while (reader.Read())
                    {
                        if (reader.NodeType != XmlNodeType.Element) 
                            continue;

                        name = reader.Name;
                        switch (name)
                        {
                            case "text":
                            case "sender_id":
                            case "recipient_id":
                                row[name] = reader.ReadString();

                                break;
                            case "sender":
                            case "recipient":
                                var subtree = reader.ReadSubtree();
                                subtree.ReadToDescendant("id");

                                ParseUser(set, subtree, subtree.ReadString());

                                break;
                            case "created_at":
                                row[name] = Date.ToDate(reader.ReadString());

                                break;
                        }
                    }
                    table.Rows.Add(row);
                }
                else ShredSubtree(reader);
            }
        }

        private static void ShredSubtree(XmlReader reader)
        {
            while (reader.Read()) ;
        }
    }
}