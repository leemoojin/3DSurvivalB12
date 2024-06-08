using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondion uiCondition;

    Condition Hp { get {  return uiCondition.Hp; } }
    Condition Stamina {  get { return uiCondition.Stamina;} }

    void Update()
    {
        Stamina.Add(Stamina.passiveValue * Time.deltaTime);

        if(Hp.curValue <= 0f)
        {
            Die();
        }

    }
    public void Die()
    {
        Debug.Log("Game over");
    }
}
