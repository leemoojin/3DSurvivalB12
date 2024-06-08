using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public string displayName;
    public int health;
    public float walkSpeed;
    public float runSpeed;

    // 드롭할 아이템 목록 - 추후 수정**
    //public ItemData[] dropOnDeath;

    [Header("Nav")]
    public float viewRadius;    
    public float viewAngle;
    public float toPlayerDistance;

    [Header("Wandering")]
    public float roamRadius;
    public float roamTime;
    public Vector3 startPosition;
    public bool isDayTimeMode = true;


    public float minWanderDistance;
    public float maxWanderDistance;

    [Header("UI")]
    public float meshResolution;

    [Header("Combat")]
    public int damage;
    public float attackRate;
    public float attackDistance;
    public float lastAttackTime;

    [Header("Invade")]
    public Transform Target;
    public bool isInvadeArrive = false;


    private void Awake()
    {

        startPosition = transform.position;
     
    }


    private void Update()
    {
        // 플레이어와의 거리 - 추후 수정**
        //toPlayerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);

    }



}
