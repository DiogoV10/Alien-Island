using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIconChange : MonoBehaviour
{
    public SkillSelectionUI skillSelectionUI;

    public void OnPickUpObjectButtonClick()
    {
        skillSelectionUI.ChangeCurrentSkillIcon(0);
    }
    public void OnDecoyButtonClick()
    {
        skillSelectionUI.ChangeCurrentSkillIcon(2); 
    }

    public void OnGasButtonClick()
    {
        skillSelectionUI.ChangeCurrentSkillIcon(1);
    }

}
