using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

namespace NewsFeed
{
    /// <summary>
    /// behaviour that let you click an UI-Text like a link. NewsItemController must be added to one of its parents' transform. 
    /// </summary>
    public class NewsItemLink : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        /// <summary>
        /// color of this text component will change by mouse over, can be null
        /// </summary>
        public Text coloredText;
        /// <summary>
        /// color when mouse is over the text
        /// </summary>
        public Color hoverColor = Color.grey;
        /// <summary>
        /// color of the text when mouse is not over
        /// </summary>
        public Color normalColor = Color.blue;

        public void OnPointerClick(PointerEventData eventData)
        {
            GetComponentInParent<NewsItemController>().OpenLink();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (coloredText != null)
                coloredText.color = hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (coloredText != null)
                coloredText.color = normalColor;
        }

        // Use this for initialization
        void Start()
        {
            if (coloredText != null)
                coloredText.color = normalColor;
        }
    }
}
