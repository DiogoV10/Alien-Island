using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{

    public Transform CreateChatBubble(Transform parent, Vector3 localPosition, string text)
    {
        Transform chatBubbleTransform = Instantiate(chatBubble, parent);
        chatBubbleTransform.localPosition = localPosition;

        chatBubbleTransform.GetComponent<ChatBubble>().Setup(text);
        //chatBubble.localScale = new Vector3(0.05f, 0.05f, 0.05f);

        //chatBubbleTransform.LookAt(dialogueCam.transform, Vector3.left);
        return chatBubbleTransform;
    }

    [SerializeField] Transform chatBubble;
    private SpriteRenderer backGroundSpriteRenderer;
    private TextMeshPro textMeshPro;

    private void Awake()
    {
        backGroundSpriteRenderer = transform.Find("BackGround").GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        //Setup("Hello world!!!ajcxiaodmcokadioji\noxjoadjxpamdopxkaopm\nosdwxkowkmdoxwqokdmx");
    }

    public void Setup(string text)
    {
        textMeshPro.SetText(text);
        textMeshPro.fontSize = 30f;
        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        

        Vector2 padding = new Vector2(0f, 5f); 
        backGroundSpriteRenderer.size = textSize + padding;
        Vector3 offset = new Vector3(-10f, 0f);
        


        backGroundSpriteRenderer.transform.localPosition = new Vector3(backGroundSpriteRenderer.size.x / 2, 0f) + offset;
    }


    //void CreateChatBubble(Transform parent, Vector3 localPosition, string text)
    //{
    //    Transform chatBubbleTransform = Instantiate(chatBubble, parent);
    //    chatBubbleTransform.localPosition = localPosition;

    //    chatBubbleTransform.GetComponent<ChatBubble>().Setup(text);
    //}
}
