using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{


    public static PlayerUpgrades Instance { get; private set; }


    public event EventHandler OnUpgradePointsChanged;
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
        HealthMax_4,
        Damage_1,
        Damage_2,
        Damage_3,
        Damage_4,
        Stamina_1,
        Stamina_2,
        Stamina_3,
        Stamina_4,
        Cooldown_1,
        Cooldown_2,
        Cooldown_3,
        Cooldown_4,
        Passive_1, 
        Passive_2, 
        Passive_3,
        Passive_4,
    }

    private List<UpgradeType> unlockedUpgradeTypeList;

    private int upgradePoints;


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

    public void AddUpgradePoint()
    {
        upgradePoints++;
        OnUpgradePointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetUpgradePoints()
    {
        return upgradePoints;
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
            case UpgradeType.Damage_4:      return UpgradeType.Damage_3;
            case UpgradeType.HealthMax_2:   return UpgradeType.HealthMax_1;
            case UpgradeType.HealthMax_3:   return UpgradeType.HealthMax_2;
            case UpgradeType.HealthMax_4:   return UpgradeType.HealthMax_3;
            case UpgradeType.Cooldown_2:    return UpgradeType.Cooldown_1;
            case UpgradeType.Cooldown_3:    return UpgradeType.Cooldown_2;
            case UpgradeType.Cooldown_4:    return UpgradeType.Cooldown_3;
            case UpgradeType.Stamina_2:     return UpgradeType.Stamina_1;
            case UpgradeType.Stamina_3:     return UpgradeType.Stamina_2;
            case UpgradeType.Stamina_4:     return UpgradeType.Stamina_3;
        }
        return UpgradeType.None;
    }

    public bool TryUnlockUpgrade(UpgradeType upgradeType)
    {
        if (CanUnlock(upgradeType) && upgradePoints > 0)
        {
            upgradePoints--;
            OnUpgradePointsChanged?.Invoke(this, EventArgs.Empty);
            UnlockUpgrade(upgradeType);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool[] GetUpgradeTypeArray()
    {
        int arrayLength = Enum.GetValues(typeof(UpgradeType)).Length;
        bool[] upgradeArray = new bool[arrayLength];

        foreach (UpgradeType upgradeType in Enum.GetValues(typeof(UpgradeType)))
        {
            upgradeArray[(int)upgradeType] = IsUpgradeUnlocked(upgradeType);
        }

        return upgradeArray;
    }

    public void LoadData(int points)
    {
        upgradePoints = points;
    }


}
