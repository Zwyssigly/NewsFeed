using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace NewsFeed
{
    /// <summary>
    /// implements NewsReader to read an Atom feed
    /// </summary>
    public class AtomReader : NewsReader
    {
        protected override IEnumerator Read(XmlReader reader)
        {
            List<NewsItem> items = new List<NewsItem>();

            if (reader.IsStartElement("feed"))
            {


                //channel.version = reader.GetAttribute("version");

                reader.ReadStartElement();

                while (reader.IsStartElement())
                {
                    switch (reader.Name.ToLower())
                    {
                        case "title":
                            channel.title = MonoHttp.HttpUtility.HtmlDecode(reader.ReadElementString());
                            break;
                        case "link":
                            if (reader.GetAttribute("rel") == "alternate")
                                channel.link = reader.GetAttribute("href");
                            reader.Skip();
                            break;
                        case "subtitle":
                            channel.description = MonoHttp.HttpUtility.HtmlDecode(reader.ReadElementString());
                            break;
                        case "entry":

                            NewsItem item = new NewsItem();

                            reader.ReadStartElement();

                            while (reader.IsStartElement())
                            {
                                switch (reader.Name.ToLower())
                                {
                                    case "title":
                                        item.title = MonoHttp.HttpUtility.HtmlDecode(reader.ReadElementString());
                                        break;
                                    //case "content":
                                    case "summary":
                                        item.description = MonoHttp.HttpUtility.HtmlDecode(reader.ReadElementString());
                                        break;
                                    case "link":
                                        if (reader.GetAttribute("rel") == "alternate")
                                            item.link = reader.GetAttribute("href");
                                        reader.Skip();
                                        break;
                                    case "published":
                                        string dateString = reader.ReadElementString();
                                        item.pubDate = DateTime.Parse(dateString, DateTimeFormatInfo.InvariantInfo);
                                        break;
                                    case "author":
                                        reader.ReadStartElement();
                                        while (reader.IsStartElement())
                                        {
                                            switch (reader.Name.ToLower())
                                            {
                                                case "name":
                                                    item.author = MonoHttp.HttpUtility.HtmlDecode(reader.ReadElementString());
                                                    break;
                                                default:
                                                    reader.Skip();
                                                    break;
                                            }
                                        }
                                        reader.ReadEndElement();
                                        break;
                                    //case "comments":
                                    //    item.comments = reader.ReadElementString();
                                    //    break;
                                    case "id":
                                        item.guid = reader.ReadElementString();
                                        break;
                                    //case "category":
                                    //    item.category = reader.ReadElementString();
                                    //    break;
                                    default:
                                        reader.Skip();
                                        break;
                                }
                            }
                            reader.ReadEndElement();

                            items.Add(item);

                            yield return null;

                            break;
                        default:
                            reader.Skip();
                            break;
                    }
                }

                reader.ReadEndElement();

                channel.items = items.ToArray();
            }
            else
            {
                // probably it isn't an Atom feed at all
                SetError("<feed> as root element expected");
            }
        }
    }
}
