using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveEffect : MonoBehaviour
{

    [SerializeField] DeliveryQuest deliveryQuest;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            deliveryQuest.QuestEnd();
            Destroy(gameObject);
        }
    }

    public void GetDeliveryQuest(DeliveryQuest _quest)
    {
        deliveryQuest = _quest;
    }
}
