using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectionUI : MonoBehaviour
{
    

    public static SkillSelectionUI Instance { get; private set; }


    [SerializeField] private Button pickUpObject;
    [SerializeField] private Button decoy;
    [SerializeField] private Button gas;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pickUpObject.onClick.AddListener(() =>
        {
            PlayerSkills.Instance.ChangeSkill(0);
        });
        
        decoy.onClick.AddListener(() =>
        {
            PlayerSkills.Instance.ChangeSkill(2);
        });
        
        gas.onClick.AddListener(() =>
        {
            PlayerSkills.Instance.ChangeSkill(1);
        });

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }


}
