using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillOrUpgradeOptionUI : MonoBehaviour
{


    public static SkillOrUpgradeOptionUI Instance { get; private set; }


    [SerializeField] private Button upgradeTreeButton;
    [SerializeField] private Button skillSelectionButton;

    private Color normalColor = Color.black;
    private Color selectedColor = Color.grey;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        upgradeTreeButton.onClick.AddListener(() =>
        {
            SkillSelectionUI.Instance.Hide();
            UpgradeTreeUI.Instance.Show();

            SetButtonSelected(upgradeTreeButton);
            SetButtonNormal(skillSelectionButton);
        });

        skillSelectionButton.onClick.AddListener(() =>
        {
            UpgradeTreeUI.Instance.Hide();
            SkillSelectionUI.Instance.Show();

            SetButtonSelected(skillSelectionButton);
            SetButtonNormal(upgradeTreeButton);
        });

        Hide();
    }

    private void SetButtonSelected(Button button)
    {
        button.GetComponent<Image>().color = selectedColor;
    }

    private void SetButtonNormal(Button button)
    {
        button.GetComponent<Image>().color = normalColor;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        UpgradeTreeUI.Instance.Show();

        SetButtonSelected(upgradeTreeButton);
        SetButtonNormal(skillSelectionButton);
    }

    public void Hide()
    {
        SkillSelectionUI.Instance.Hide();
        UpgradeTreeUI.Instance.Hide();

        gameObject.SetActive(false);
    }


}
