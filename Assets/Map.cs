using UnityEngine;

public class Map : MonoBehaviour
{
    private void Start()
    {
        for(var i = 0; i < 8; i++) {
            PoolManager.Instance.Spawn("Tree");
            PoolManager.Instance.Spawn("Rock");
        }
    }

    private void Update()
    {
    }
}