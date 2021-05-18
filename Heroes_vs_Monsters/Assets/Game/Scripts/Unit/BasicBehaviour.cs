﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBehaviour : MonoBehaviour
{
    public int health = 100;
    protected int currentHealth = 1;
    public int defense = 0;

    public int getCurrentHealth()
    {
        return currentHealth;
    }

    public virtual void takeDamage(int damage)
    {
        int damageTaken = 1;
        if (damage - defense > 0)
        {
            damageTaken = damage - defense;
        }
        currentHealth -= damageTaken;

        if (currentHealth <= 0)
        {
            currentHealth = 100;
            print("BOOOOOOM EL NEXO SE CALLÓ: SILENCI");
        }
    }
}