using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO characterData;
    public AttackData_SO attackData;

    #region Read from Data SO
    public int MaxHealth
    {
        get { return characterData != null ? characterData.maxHealth: 0; }
        set { if(characterData!=null) characterData.maxHealth = value;}
    }

    public int CurrentHealth
    {
        get { return characterData != null ? characterData.currentHealth : 0; }
        set { if (characterData != null) characterData.currentHealth = value; }
    }

    public int BaseDefense
    {
        get { return characterData != null ? characterData.baseDefense : 0; }
        set { if (characterData != null) characterData.baseDefense = value; }
    }

    public int CurrentDefense
    {
        get { return characterData != null ? characterData.currentDefense : 0; }
        set { if (characterData != null) characterData.currentDefense = value; }
    }
    #endregion
}
