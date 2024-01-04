using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSaveOrLoadUI : MonoBehaviour
{


    [SerializeField] private Button loadButton;
    [SerializeField] private Button saveButton;


    private void Start()
    {
        loadButton.onClick.AddListener(() =>
        {
            Player.Instance.LoadPlayer();
        });

        saveButton.onClick.AddListener(() =>
        {
            Player.Instance.SavePlayer();
        });
    }
}
