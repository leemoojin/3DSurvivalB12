using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int Hp; // 바위의 체력. 0 이 되면 파괴됨

    [SerializeField]
    private int DestroyTime; // 파괴된 바위의 파편들의 생명 (이 시간이 지나면 Destroy)

    [SerializeField]
    private SphereCollider Col; // 구체 콜라이더. 바위가 깨지면 비활성화.

    [SerializeField]
    private GameObject GoRock;  // 일반 바위 오브젝트. 평소에 활성화, 바위가 깨지면 비활성화
    [SerializeField]
    private GameObject GoDebris;  // 깨진 바위 오브젝트. 평소에 비활성화, 바위가 깨지면 활성화

    public void Mining()
    {
        Hp--;
        if (Hp <= 0)
            Destruction();
    }

    private void Destruction()
    {
        Col.enabled = false;
        Destroy(GoRock);

        GoDebris.SetActive(true);
        Destroy(GoDebris, DestroyTime);
    }
}