using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private Sprite lineSprite;
    [SerializeField] private Sprite lineGlowSprite;
    [SerializeField] private TextMeshProUGUI upgradePointsText;

    [Header("Buttons")]
    [SerializeField] private List<UpgradeButtonData> buttonList;
    [SerializeField] private UpgradeUnlockPath[] upgradeUnlockPathArray;


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
        UpdateUpgradePoints();

        PlayerUpgrades.Instance.OnUpgradeUnlocked += PlayerUpgrades_OnUpgradeUnlocked;
        PlayerUpgrades.Instance.OnUpgradePointsChanged += PlayerUpgrades_OnUpgradePointsChanged;
    }

    private void PlayerUpgrades_OnUpgradePointsChanged(object sender, System.EventArgs e)
    {
        UpdateUpgradePoints();
    }

    private void PlayerUpgrades_OnUpgradeUnlocked(object sender, PlayerUpgrades.OnUpgradeUnlockedEventArgs e)
    {
        UpdateVisuals();
    }

    private void UpdateUpgradePoints() 
    {
        upgradePointsText.SetText(PlayerUpgrades.Instance.GetUpgradePoints().ToString());
    }

    private void UpdateVisuals()
    {
        foreach (UpgradeButton upgradeButton in upgradeButtonList)
        {
            upgradeButton.UpdateVisual();
        }

        foreach (UpgradeUnlockPath upgradeUnlockPath in upgradeUnlockPathArray)
        {
            foreach (Image linkImage in upgradeUnlockPath.linkImageArray)
            {
                linkImage.color = new Color(.5f, .5f, .5f);
                linkImage.sprite = lineSprite;
            }
        }

        foreach (UpgradeUnlockPath upgradeUnlockPath in upgradeUnlockPathArray)
        {
            if (PlayerUpgrades.Instance.IsUpgradeUnlocked(upgradeUnlockPath.upgradeType) || PlayerUpgrades.Instance.CanUnlock(upgradeUnlockPath.upgradeType))
            {
                foreach (Image linkImage in upgradeUnlockPath.linkImageArray)
                {
                    linkImage.color = Color.white;
                    linkImage.sprite = lineGlowSprite;
                }
            }
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
                    TooltipInfo.Instance.Hide();
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

    [System.Serializable]
    public class UpgradeUnlockPath
    {
        public PlayerUpgrades.UpgradeType upgradeType;
        public Image[] linkImageArray;
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
