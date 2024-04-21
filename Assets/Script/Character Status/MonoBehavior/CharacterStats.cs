using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO characterData;
    public AttackData_SO attackData;

    [HideInInspector]
    public bool isCritical;
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

    #region Character Combat

    // defender is 'this'
    public void TakeDagame(CharacterStats attacker)
    {
        // defense is a number, but damage is calculated using: minDamage,
        // maxDamage, criticalMultiplier,  criticalChance (isCritical);
        int damage = attacker.CurrentDamage() - CurrentDefense;
        // damage could not be negative. 
        damage = Mathf.Max(damage, 0);
        // health could not be negative.
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        if (attacker.isCritical)
        {
            GetComponent<Animator>().SetTrigger("Hit");
        }
        // TODO:Update UI
        // TODO:exp update if enemy died

    }

    int CurrentDamage()
    {
        float coreDamage = Random.Range(attackData.minDamage, attackData.maxDamage);
        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
        }
        Debug.Log(isCritical + " " + coreDamage);
        return (int)coreDamage;
    }
    #endregion
}
