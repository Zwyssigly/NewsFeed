using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

namespace NewsFeed
{
    /// <summary>
    /// Enables a GameObject to show a NewsItem
    /// </summary>
    public class NewsItemController : MonoBehaviour
    {
        /// <summary>
        /// Text component to show the item's title, can be null
        /// </summary>
        public Text titleText;
        /// <summary>
        /// Text component to show the item's decription/summary, can be null
        /// </summary>
        public Text descText;
        /// <summary>
        /// Text component to show the item's publication date, can be null
        /// </summary>
        public Text pubDateText;
        /// <summary>
        /// Text component to show the item's author, can be null
        /// </summary>
        public Text authorText;

        /// <summary>
        /// the NewsItem that is showing in this control
        /// </summary>
        public NewsItem item;

        private NewsChannelController channelController;

        // Use this for initialization
        void Start()
        {
            UpdateContent();
        }



        /// <summary>
        /// update the UI elements with the content of the NewsItem
        /// </summary>
        public void UpdateContent()
        {
            if (item != null)
            {
                if (channelController == null)
                    channelController = GetComponentInParent<NewsChannelController>();

                if (titleText != null)
                    titleText.text = item.title;
                if (descText != null)
                    descText.text = channelController.FormatDesc(item.description);
                if (pubDateText != null)
                    pubDateText.text = channelController.FormatPubDate(item.pubDate);
                if (authorText != null)
                    authorText.text = item.author;
            }
        }

        /// <summary>
        /// opens the link of the NewsItem in a browser, useful for onclick events on buttons
        /// </summary>
        public void OpenLink()
        {
            if (item != null && !string.IsNullOrEmpty(item.link))
            {
                Application.OpenURL(item.link);
            }
        }
    }
}
