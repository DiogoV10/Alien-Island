using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    [SerializeField] private string header;
    [Multiline()]
    [SerializeField] private string content;
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!TooltipInfo.Instance) return;

        StartCoroutine(Delay());

        TooltipInfo.Instance.SetText(content, header);
        TooltipInfo.Instance.Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!TooltipInfo.Instance) return;

        StopCoroutine(Delay());
        TooltipInfo.Instance.Hide();
    }

    private IEnumerator Delay()
    {
        float delay = .5f;
        yield return new WaitForSeconds(delay);
    }


}
