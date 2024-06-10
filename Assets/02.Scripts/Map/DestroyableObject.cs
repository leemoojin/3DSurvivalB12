using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField]
    private int Hp; // 오브젝트의 체력. 0 이 되면 파괴됨

    [SerializeField]
    private int DestroyTime; // 파괴된 오브젝트의 파편들의 생명 (이 시간이 지나면 Destroy)

    [SerializeField]
    private SphereCollider Col; // 구체 콜라이더. 오브젝트가 깨지면 비활성화.

    [SerializeField]
    private GameObject Go;  // 일반 오브젝트. 평소에 활성화, 오브젝트가 깨지면 비활성화
    [SerializeField]
    private GameObject GoDebris;  // 깨진 오브젝트. 평소에 비활성화, 오브젝트가 깨지면 활성화

    public void Mining()
    {
        Hp--;
        if (Hp <= 0)
            Destruction();
    }

    private void Destruction()
    {
        Col.enabled = false;
        Destroy(Go);

        GoDebris.SetActive(true);
        Destroy(GoDebris, DestroyTime);
    }
}