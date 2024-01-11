using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipInfo : MonoBehaviour
{


    public static TooltipInfo Instance { get; private set; }


    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private TextMeshProUGUI headerField;
    [SerializeField] private TextMeshProUGUI contentField;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private int characterWrapLimit;


    private RectTransform rectTransform;


    private void Awake()
    {
        Instance = this;

        rectTransform = GetComponent<RectTransform>();

        Hide();
    }

    private void Update()
    {
        SetAnchoredPosition();
    }

    private void SetAnchoredPosition()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

        if (anchoredPosition.x + rectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - rectTransform.rect.width;
        }

        if (anchoredPosition.y + rectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - rectTransform.rect.height;
        }

        anchoredPosition.x += 5;
        anchoredPosition.y += 5;

        rectTransform.anchoredPosition = anchoredPosition;
    }

    public void SetText(string content = "", string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        SetAnchoredPosition();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


}
