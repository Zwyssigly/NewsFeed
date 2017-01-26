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
    /// implements NewsReader to read a RSS feed.
    /// </summary>
    public class RssReader : NewsReader
    {
        protected override IEnumerator Read(XmlReader reader)
        {
            List<NewsItem> items = new List<NewsItem>();

            if (reader.IsStartElement("rss"))
            {
                channel.version = reader.GetAttribute("version");

                reader.ReadStartElement();

                while (reader.IsStartElement())
                {
                    if (reader.IsStartElement("channel"))
                    {
                        reader.ReadStartElement();

                        while (reader.IsStartElement())
                        {
                            switch (reader.Name.ToLower())
                            {
                                case "title":
                                    channel.title = MonoHttp.HttpUtility.HtmlDecode(reader.ReadElementString());
                                    break;
                                case "link":
                                    channel.link = reader.ReadElementString();
                                    break;
                                case "description":
                                    channel.description = MonoHttp.HttpUtility.HtmlDecode(reader.ReadElementString());
                                    break;
                                case "item":

                                    NewsItem item = new NewsItem();

                                    reader.ReadStartElement();

                                    while (reader.IsStartElement())
                                    {
                                        switch (reader.Name.ToLower())
                                        {
                                            case "title":
                                                item.title = MonoHttp.HttpUtility.HtmlDecode(reader.ReadElementString());
                                                break;
                                            case "description":
                                                item.description = MonoHttp.HttpUtility.HtmlDecode(reader.ReadElementString());
                                                break;
                                            case "link":
                                                item.link = reader.ReadElementString();
                                                break;
                                            case "pubdate":
                                                string dateString = reader.ReadElementString();
                                                item.pubDate = DateTime.Parse(dateString);//, "ddd, dd MMM yyyy HH:mm:ss zzz", DateTimeFormatInfo.InvariantInfo);
                                                break;
                                            case "dc:creator":
                                            case "author":
                                                item.author = MonoHttp.HttpUtility.HtmlDecode(reader.ReadElementString());
                                                break;
                                            case "guid":
                                                item.guid = reader.ReadElementString();
                                                break;
                                            case "category":
                                                item.category = reader.ReadElementString();
                                                break;
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
                    }
                    else reader.Skip();
                }

                reader.ReadEndElement();

                channel.items = items.ToArray();
            }
            else
            {
                // probably it isnt a RSS feed document
                SetError("<rss> as root element expected");
            }
        }
    }
}
