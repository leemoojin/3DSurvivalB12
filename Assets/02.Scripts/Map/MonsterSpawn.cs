using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner instance;

    public GameObject[] MonsterPrefabs; // 몬스터 프리팹 배열

    [SerializeField]
    private int MonsterGenCount;
    
    private readonly Queue<GameObject> _mQueue = new();
    
    [SerializeField]
    private float XPos;
    [SerializeField]
    private float ZPos;
    private Vector3 _randomVector;

    private int _monsterRandomIndex;

    private void Start()
    {
        instance = this;
        var random = new System.Random();


        for (var i = 0; i < MonsterGenCount; i++)
        {
            _monsterRandomIndex = random.Next(0, MonsterPrefabs.Length);
            var tObject = Instantiate(MonsterPrefabs[_monsterRandomIndex], gameObject.transform);
            _mQueue.Enqueue(tObject);
            tObject.SetActive(false);
        }

        StartCoroutine(MonsterSpawn());
    }

    public void InsertQueue(GameObject pObject)
    {
        _mQueue.Enqueue(pObject);
        pObject.SetActive(false);
    }

    private GameObject GetQueue()
    {
        var tObject = _mQueue.Dequeue();
        tObject.SetActive(true);

        return tObject;
    }

    private IEnumerator MonsterSpawn()
    {
        while (true)
        {
            if (_mQueue.Count != 0)
            {
                XPos = Random.Range(-46, -74);
                ZPos = Random.Range(46, 74);
                _randomVector = new Vector3(XPos, 0.0f, ZPos);
                var tObject = GetQueue();
                tObject.transform.position = gameObject.transform.position + _randomVector;
            }
            yield return new WaitForSeconds(1f);
        }
    }

}