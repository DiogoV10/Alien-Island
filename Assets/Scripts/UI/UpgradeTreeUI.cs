using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTreeUI : MonoBehaviour
{

    public static UpgradeTreeUI Instance { get; private set; }


    [System.Serializable]
    public class UpgradeButtonData
    {
        public Button button;
        public PlayerUpgrades.UpgradeType upgradeType;
    }


    [Header("Materials")]
    [SerializeField] private Material upgradeLockedMaterial;
    [SerializeField] private Material upgradeUnlockableMaterial;

    [Header("Buttons")]
    [SerializeField] private List<UpgradeButtonData> buttonList;
    
    
    private List<UpgradeButton> upgradeButtonList;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        upgradeButtonList = new List<UpgradeButton>();

        foreach (UpgradeButtonData buttonData in buttonList)
        {
            upgradeButtonList.Add(new UpgradeButton(buttonData.button, buttonData.upgradeType, upgradeUnlockableMaterial, upgradeLockedMaterial));
        }

        UpdateVisuals();

        PlayerUpgrades.Instance.OnUpgradeUnlocked += PlayerUpgrades_OnUpgradeUnlocked;

        Hide();
    }

    private void PlayerUpgrades_OnUpgradeUnlocked(object sender, PlayerUpgrades.OnUpgradeUnlockedEventArgs e)
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        foreach (UpgradeButton upgradeButton in upgradeButtonList)
        {
            upgradeButton.UpdateVisual();
        }
    }

    private class UpgradeButton
    {
        private Button button;
        private PlayerUpgrades.UpgradeType upgradeType;
        private Material upgradeUnlockableMaterial;
        private Material upgradeLockedMaterial;

        public UpgradeButton(Button button, PlayerUpgrades.UpgradeType upgradeType, Material upgradeUnlockableMaterial, Material upgradeLockedMaterial)
        {
            this.button = button;
            this.upgradeType = upgradeType;
            this.upgradeUnlockableMaterial = upgradeUnlockableMaterial;
            this.upgradeLockedMaterial = upgradeLockedMaterial;

            button.onClick.AddListener(() =>
            {
                if (!PlayerUpgrades.Instance.TryUnlockUpgrade(upgradeType))
                {
                    TooltipUI.Instance.Show("Cannot unlock " + upgradeType + "!");
                }
            });
        }

        public void UpdateVisual()
        {
            if (PlayerUpgrades.Instance.IsUpgradeUnlocked(upgradeType))
            {
                button.image.material = null;
            }
            else
            {
                if (PlayerUpgrades.Instance.CanUnlock(upgradeType))
                {
                    button.image.material = upgradeUnlockableMaterial;
                }
                else
                {
                    button.image.material = upgradeLockedMaterial;
                }
            }
        }
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
