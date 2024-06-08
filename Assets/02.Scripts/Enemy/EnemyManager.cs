using UnityEngine;


public class EnemyManager : MonoBehaviour
{

    private static EnemyManager _instance;

    public static EnemyManager Instance
    {
        get
        {            
            return _instance;
        }
    }

    //public EnemyInfo enemyInfo;
    public EnemyNav enemyNav;
    
    public Transform dropPosition;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;           
        }

        //enemyInfo = GetComponent<EnemyInfo>();
        enemyNav = GetComponent<EnemyNav>();        
    }
}
