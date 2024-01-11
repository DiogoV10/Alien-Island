using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{


    public static TooltipUI Instance { get; private set; }


    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private RectTransform backgroundRectTransform;
    [SerializeField] private TextMeshProUGUI text;


    private RectTransform rectTransform;
    private float showTimer;


    private void Awake()
    {
        Instance = this;

        rectTransform = GetComponent<RectTransform>();

        Hide();
    }

    private void SetText(string tooltipText)
    {
        text.SetText(tooltipText);
        text.ForceMeshUpdate();

        Vector2 textSize = text.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8, 8);

        backgroundRectTransform.sizeDelta = textSize + paddingSize;
    }

    private void Update()
    {
        SetAnchoredPosition();

        showTimer -= Time.deltaTime;
        if (showTimer <= 0)
        {
            Hide();
        }
    }

    private void SetAnchoredPosition()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }

        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }

        anchoredPosition.x += 5;
        anchoredPosition.y += 5;

        rectTransform.anchoredPosition = anchoredPosition;
    }

    public void Show(string tooltipText, float showTimerMax = 2f)
    {
        gameObject.SetActive(true);
        SetAnchoredPosition();
        SetText(tooltipText);
        showTimer = showTimerMax;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


}
