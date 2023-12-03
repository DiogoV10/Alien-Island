using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{

    public Transform CreateChatBubble(Transform parent, Vector3 localPosition, string _text)
    {
        currentText = _text;
        text = currentText;
        Transform chatBubbleTransform = Instantiate(chatBubble, parent);
        chatBubbleTransform.localPosition = localPosition;

        chatBubbleTransform.GetComponent<ChatBubble>().Setup(text);
        //chatBubble.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        

        //chatBubbleTransform.LookAt(dialogueCam.transform, Vector3.left);
        return chatBubbleTransform;
    }

    

    [SerializeField] Transform chatBubble;
    private SpriteRenderer backGroundSpriteRenderer;
    private TextMeshPro textMeshProText, textMeshProEnter;
    float textSpeed = 0.05f;
    public string text = "";
    public string currentText = "";

    public string pressButtonText;
    

    private void Awake()
    {
        backGroundSpriteRenderer = transform.Find("BackGround").GetComponent<SpriteRenderer>();
        textMeshProText = transform.Find("Text").GetComponent<TextMeshPro>();
        textMeshProEnter = transform.Find("Press Enter").GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        pressButtonText = "Press Enter";
    }

    private void Update()
    {
        if (text != currentText)
        {
            ChangeChatBubbleText(text);
            currentText = text;
        }
    }

    void ChangeChatBubbleText(string text)
    {
        StopAllCoroutines();
        textMeshProText.SetText("");
        textMeshProEnter.SetText(pressButtonText);
        StartCoroutine(TypeLine(text));
    }

    public void Setup(string text)
    {
        textMeshProText.SetText("");
        StartCoroutine(TypeLine(text));
        textMeshProText.fontSize = 28f;
        textMeshProText.ForceMeshUpdate();
        Vector2 textSize = new Vector2(65f, 15f); //textMeshPro.GetRenderedValues(false);
        textMeshProEnter.transform.localPosition = new Vector2(50, -3.5f);
        Debug.Log(textSize);
        

        Vector2 padding = new Vector2(0f, 5f); 
        backGroundSpriteRenderer.size = textSize + padding;
        Vector3 offset = new Vector3(-10f, 0f);
        


        backGroundSpriteRenderer.transform.localPosition = new Vector3(backGroundSpriteRenderer.size.x / 2, 0f) + offset;
    }

    IEnumerator TypeLine(string text)
    {
        foreach (char c in text.ToCharArray())
        {
            textMeshProText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
