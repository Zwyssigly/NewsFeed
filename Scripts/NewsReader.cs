using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace NewsFeed
{
    /// <summary>
    /// Base class for reading a NewsChannel either RSS or Atom.
    /// </summary>
    public abstract class NewsReader
    {
        /// <summary>
        /// Instantiate a NewsReader using either of its implementations
        /// </summary>
        /// <param name="type">which implemtation will be used</param>
        /// <returns>implementated NewsReader</returns>
        public static NewsReader FromType(NewsFeedType type)
        {
            switch (type)
            {
                case NewsFeedType.Rss:
                    return new RssReader();
                case NewsFeedType.Atom:
                    return new AtomReader();
                default:
                    throw new UnityException("Unkown News Feed Type!");
            }
        }

        /// <summary>
        /// collection of the read news items
        /// </summary>
        public NewsChannel channel { get; private set; }

        /// <summary>
        /// when any error occurred while reading, its saved here 
        /// </summary>
        public string error { get; private set; }

        /// <summary>
        /// Reads the news feed asynchronic
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public IEnumerator ReadAsync(string xml)
        {
            channel = new NewsChannel();

            var reader = new XmlTextReader(new StringReader(xml));
            reader.MoveToContent();

            return Read(reader);
        }

        /// <summary>
        /// implementation can set an error here
        /// </summary>
        /// <param name="error"></param>
        protected void SetError(string error)
        {
            this.error = error;
            channel = null;
        }


        protected abstract IEnumerator Read(XmlReader reader);
    }
}
