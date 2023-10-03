using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{


    [SerializeField] private PlayerCombat playerCombat;


    public void NextAttack()
    {
        playerCombat.NextAttack();
    }


}
