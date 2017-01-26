using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewsFeed
{
    /// <summary>
    /// NewsItem represent a &lt;item&gt; tag in RSS or an &lt;entry&gt; tag in Atom. It saves the data for one news entry.
    /// </summary>
    public class NewsItem
    {
        /// <summary>
        /// title of the news item
        /// </summary>
        public string title;
        /// <summary>
        /// short description of the time (uses tag &lt;summary&gt; in Atom)
        /// </summary>
        public string description;
        /// <summary>
        /// link to the full article
        /// </summary>
        public string link;
        /// <summary>
        /// publication date of the item (uses tag &lt;published&gt; in Atom) 
        /// </summary>
        public DateTime pubDate;
        /// <summary>
        /// name of the author
        /// </summary>
        public string author;
        /// <summary>
        /// unique id of the item, not used yet
        /// </summary>
        public string guid;
        /// <summary>
        /// category of this item, not used  yet
        /// </summary>
        public string category;
    }
}
