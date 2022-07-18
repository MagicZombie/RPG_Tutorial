using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int damage;

    public int maxHP;
    public int currentHP;

    public bool TakeDamage(int dmg) 
    {
        currentHP -= dmg;

        // Check if unit has died and return true or false respectively. 
        if (currentHP <= 0)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    public void Heal(int healAmount) 
    {
        currentHP += healAmount;
        if(currentHP > maxHP) 
        {
            currentHP = maxHP;
        }
    }
}
