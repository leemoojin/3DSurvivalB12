using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondion : MonoBehaviour
{
    public Condition Hp;
    public Condition Stamina;
    // Start is called before the first frame update
    void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
