using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{


    public static PlayerUpgrades Instance { get; private set; }


    public event EventHandler<OnUpgradeUnlockedEventArgs> OnUpgradeUnlocked;
    public class OnUpgradeUnlockedEventArgs : EventArgs
    {
        public UpgradeType upgradeType;
    }


    public enum UpgradeType
    {
        None,
        Katana_UltimateSkill,
        Knife_UltimateSkill,
        Pistol_UltimateSkill,
        Rifle_UltimateSkill,
        IllusionaryDecoy_Skill,
        ObjectControl_Skill,
        ToxicBlast_Skill,
        HealthMax_1,
        HealthMax_2,
        HealthMax_3,
        Damage_1,
        Damage_2,
        Damage_3,
        Stamina_1,
        Stamina_2,
        Stamina_3,
        Cooldown_1,
        Cooldown_2,
        Cooldown_3,
        Passive_1, 
        Passive_2, 
        Passive_3,
        Passive_4,
        Passive_5,
        Passive_6,
        Passive_7,
        Passive_8,
    }

    private List<UpgradeType> unlockedUpgradeTypeList;


    private void Awake()
    {
        Instance = this;

        unlockedUpgradeTypeList = new List<UpgradeType>();
    }

    private void UnlockUpgrade(UpgradeType upgradeType)
    {
        if (!IsUpgradeUnlocked(upgradeType))
        {
            unlockedUpgradeTypeList.Add(upgradeType);
            OnUpgradeUnlocked?.Invoke(this, new OnUpgradeUnlockedEventArgs { upgradeType = upgradeType });
        }
    }

    public bool IsUpgradeUnlocked(UpgradeType upgradeType)
    {
        return unlockedUpgradeTypeList.Contains(upgradeType);
    }

    public bool CanUnlock(UpgradeType upgradeType)
    {
        UpgradeType upgradeRequirement = GetUpgradeRequirement(upgradeType);

        if (upgradeRequirement != UpgradeType.None)
        {
            if (IsUpgradeUnlocked(upgradeRequirement))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    public UpgradeType GetUpgradeRequirement(UpgradeType upgradeType)
    {
        switch (upgradeType) 
        { 
            case UpgradeType.Damage_2:      return UpgradeType.Damage_1;
            case UpgradeType.Damage_3:      return UpgradeType.Damage_2;
            case UpgradeType.HealthMax_2:   return UpgradeType.HealthMax_1;
            case UpgradeType.HealthMax_3:   return UpgradeType.HealthMax_2;
            case UpgradeType.Cooldown_2:    return UpgradeType.Cooldown_1;
            case UpgradeType.Cooldown_3:    return UpgradeType.Cooldown_2;
            case UpgradeType.Stamina_2:     return UpgradeType.Stamina_1;
            case UpgradeType.Stamina_3:     return UpgradeType.Stamina_2;
        }
        return UpgradeType.None;
    }

    public bool TryUnlockUpgrade(UpgradeType upgradeType)
    {
        if (CanUnlock(upgradeType))
        {
            UnlockUpgrade(upgradeType);
            return true;
        }
        else
        {
            return false;
        }
    }


}
