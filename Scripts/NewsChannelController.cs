using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;

namespace NewsFeed
{
    /// <summary>
    /// Controller to show a news channel in the game
    /// </summary>
    public class NewsChannelController : MonoBehaviour
    {
        /// <summary>
        /// All NewsItemController will be added to this UI element as a child 
        /// </summary>
        public RectTransform contentTransform;

        /// <summary>
        /// the url of the RSS or Atom feed that will be showed
        /// </summary>
        public string url;
        /// <summary>
        /// does the url refer to a RSS or Atom feed?
        /// </summary>
        public NewsFeedType feedType = NewsFeedType.Rss;

        /// <summary>
        /// prefab wich is used to show a NewsItem. It must have added the component NewsItemController
        /// </summary>
        public GameObject itemPrefab;
        /// <summary>
        /// how the items are ordered
        /// </summary>
        public NewsItemSortOrder itemsSortOrder = NewsItemSortOrder.ByPubDate;

        /// <summary>
        /// if its's greater than 1, the feed will automatically updated by this interval (in seconds)
        /// </summary>
        public float refreshInterval = 0f;
        /// <summary>
        /// refers to a GameObject that will be automatically deactivated, once the content is loaded
        /// </summary>
        public GameObject loadingText;

        /// <summary>
        /// If its greater than zero, description/summary will be truncated to set length in consideration of words' endings.
        /// </summary>
        public int descMaxLength = 0;

        /// <summary>
        /// If its true, the description will be strip away all html tags and entities
        /// </summary>
        public bool descStripHtml = true;

        /// <summary>
        /// this format will be used to format the publication date, formats are the same like in DateTime.ToString method
        /// </summary>
        public string pubDateFormat = "g";


        private NewsChannel channel;
        private bool isLoading;

        private float timeSinceUpdate = 0;

        void Start()
        {
            UpdateChannel();
        }

        
        void Update()
        {
            if (!isLoading && refreshInterval > 1f)
            {
                timeSinceUpdate += Time.deltaTime;
                if (timeSinceUpdate > refreshInterval)
                    UpdateChannel();
            }
        }

        /// <summary>
        /// whole news channel will be updated in a coroutine
        /// </summary>
        public void UpdateChannel()
        {
            if (isLoading)
                return;

            BeginUpdating();
            StartCoroutine(ReadAsync());
        }

        /// <summary>
        /// gets invoked before the channels gets updated
        /// </summary>
        void BeginUpdating()
        {
            isLoading = true;
        }

        /// <summary>
        /// gets invoked after the channels and UI elements are updated
        /// </summary>
        void EndUpdating()
        {
            isLoading = false;
            timeSinceUpdate = 0;

            if (loadingText != null)
                loadingText.SetActive(channel == null);
        }

        /// <summary>
        /// updates each NewsItemController and creates/destroys instances if needed
        /// </summary>
        void UpdateUIContent()
        {
            int itemsCount = channel == null ? 0 : channel.items.Length;

            // if there are to many NewsItemControllers instantiated
            while (contentTransform.childCount > itemsCount)
            {
                GameObject obj = contentTransform.GetChild(contentTransform.childCount-1).gameObject;
                DestroyImmediate(obj);
            }

            // if there are to few NewsItemController instantiated
            while (contentTransform.childCount < itemsCount)
            {
                GameObject itemObj = Instantiate(itemPrefab);
                RectTransform itemTrsf = itemObj.GetComponent<RectTransform>();
                itemTrsf.SetParent(contentTransform, false);
            }

            // update the NewsItemControllers with the new RSS items or Atom entries
            if (itemsCount > 0)
            {
                NewsItem[] items = channel.GetItems(itemsSortOrder);
                for (int i = 0; i < itemsCount; i++)
                {
                    GameObject itemObj = contentTransform.GetChild(i).gameObject;
                    NewsItemController itemCtrl = itemObj.GetComponent<NewsItemController>();
                    itemCtrl.item = items[i];
                    itemCtrl.UpdateContent();
                }
            }
        }

        /// <summary>
        /// coroutine to update news channel
        /// </summary>
        /// <returns></returns>
        IEnumerator ReadAsync()
        {
            WWW www = new WWW(url);
            yield return www;

            if (www.error != null)
            {
                Debug.Log(www.error);
                channel = null;
            }
            else
            {
                string xml = www.text;
                NewsReader reader = NewsReader.FromType(feedType);
                yield return reader.ReadAsync(xml);

                if (reader.error != null)
                {
                    Debug.Log(reader.error);
                    channel = null;
                }
                else 
                    channel = reader.channel;
            }

            UpdateUIContent();
            EndUpdating();
        }

        internal string FormatDesc(string input)
        {
            if (descStripHtml)
                input = Regex.Replace(input, "(<.*?>|&#?[a-z0-9]*?;)", string.Empty);

            if (descMaxLength > 0 && input.Length > descMaxLength)
            {
                int lastWhite = input.LastIndexOfAny(new char[] { ' ', '\t', '\n', '\r' }, descMaxLength - 3);
                if (lastWhite > 0)
                    input = input.Substring(0, lastWhite).TrimEnd() + "...";
                else
                    input = input.Substring(0, descMaxLength - 3).TrimEnd() + "...";
            }

            return input;
        }

        internal string FormatPubDate(DateTime pubDate)
        {
            if (pubDate == DateTime.MinValue)
                return string.Empty;
            return pubDate.ToString(pubDateFormat);
        }
    }
}
