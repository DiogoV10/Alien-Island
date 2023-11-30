using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillOrUpgradeOptionUI : MonoBehaviour
{


    public static SkillOrUpgradeOptionUI Instance { get; private set; }


    [SerializeField] private Button upgradeTreeButton;
    [SerializeField] private Button skillSelectionButton;


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
        });

        skillSelectionButton.onClick.AddListener(() =>
        {
            UpgradeTreeUI.Instance.Hide();
            SkillSelectionUI.Instance.Show();
        });

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        UpgradeTreeUI.Instance.Show();
    }

    public void Hide()
    {
        SkillSelectionUI.Instance.Hide();
        UpgradeTreeUI.Instance.Hide();

        gameObject.SetActive(false);
    }


}
