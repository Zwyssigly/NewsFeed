using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NewsFeed
{
    /// <summary>
    /// represents a collection of news items with additional meta data like description and title of the channel.
    /// </summary>
    public class NewsChannel
    {
        /// <summary>
        /// link to the news channel
        /// </summary>
        public string link;
        /// <summary>
        /// title of the news channel
        /// </summary>
        public string title;
        /// <summary>
        /// items of the news channel
        /// </summary>
        public NewsItem[] items;
        /// <summary>
        /// version of the rss standard
        /// </summary>
        public string version;
        /// <summary>
        /// description of the news channel
        /// </summary>
        public string description;

        public NewsItem[] GetItems(NewsItemSortOrder sortOrder)
        {
            if (sortOrder == NewsItemSortOrder.None)
                return items;

            NewsItem[] result = new NewsItem[items.Length];
            Array.Copy(items, result, items.Length);

            switch (sortOrder)
            {
                case NewsItemSortOrder.ByPubDate:
                    Array.Sort(result, (x, y) => x.pubDate.CompareTo(y.pubDate)*-1);
                    break;
                default:
                    throw new UnityException("Unkown NewsItemSortOrder");
            }
            return result;
        }
    }
}
