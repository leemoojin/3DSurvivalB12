using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCraftItem : MonoBehaviour, IDamagable
{
    // Start is called before the first frame update
    public int health = 15;

    private void Update()
    {
        DestroyWall();
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health < 0 ) health = 0;
        
    }

    private void DestroyWall()
    {
        if (health <= 0)
        {
            Debug.Log("º® ÆÄ±«");
            Destroy(gameObject);

        }
    }

}
