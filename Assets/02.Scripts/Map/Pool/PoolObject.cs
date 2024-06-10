using UnityEngine;
using KeyType = System.String;

[DisallowMultipleComponent]
public class PoolObject : MonoBehaviour
{
    public KeyType key;

    /// <summary> 게임오브젝트 복제 </summary>
    public PoolObject Clone()
    {
        var go = Instantiate(gameObject);
        if (!go.TryGetComponent(out PoolObject po))
            po = go.AddComponent<PoolObject>();
        go.SetActive(false);

        return po;
    }

    /// <summary> 게임오브젝트 활성화 </summary>
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    /// <summary> 게임오브젝트 비활성화 </summary>
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}