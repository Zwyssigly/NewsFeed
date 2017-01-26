using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewsFeed
{
    /// <summary>
    /// how the news items are ordered
    /// </summary>
    public enum NewsItemSortOrder
    {
        /// <summary>
        /// it's ordered like in the RSS or Atom document
        /// </summary>
        None,
        /// <summary>
        /// its ordered by the items' publication date
        /// </summary>
        ByPubDate,
    }
}
