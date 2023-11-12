using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{
    private SpriteRenderer backGroundSpriteRenderer;
    private TextMeshPro textMeshPro;


    private void Awake()
    {
        backGroundSpriteRenderer = transform.Find("BackGround").GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        Setup("Hello world!!! jsxbcnuecbwjcwoedhweondoioia\nshxuihciuweufwhefcuihewfuhjs\ndhu99wehdfuwehfdiowhe89dfhoidn");
    }

    private void Setup(string text)
    {
        textMeshPro.SetText(text);
        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues(false);

        Vector2 padding = new Vector2(7f, 4f); 
        backGroundSpriteRenderer.size = textSize + padding;
        textMeshPro.fontSize = 30f;


        backGroundSpriteRenderer.transform.localPosition = new Vector3(backGroundSpriteRenderer.size.x / 2, 0f);
    }
}
