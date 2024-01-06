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

    [SerializeField] private Slider _SkillON;
    [SerializeField] private Image currentSkillIcon;
    public Sprite newPickUpObjectIcon;
    public Sprite newDecoyIcon;
    public Sprite newGasIcon;


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
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ChangeCurrentSkillIcon(int skillIndex)
    {
        switch (skillIndex)
        {
            case 0:
                currentSkillIcon.sprite = newPickUpObjectIcon;
                break;
            case 1:
                currentSkillIcon.sprite = newGasIcon;
                break;
            case 2:
                currentSkillIcon.sprite = newDecoyIcon;
                break;
            default:
                break;
        }
        _SkillON.gameObject.SetActive(true);
    }
}
