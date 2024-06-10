using System.Collections.Generic;
using UnityEngine;
using KeyType = System.String;


/// <summary> 
/// 오브젝트 풀 관리 싱글톤
/// </summary>
[DisallowMultipleComponent]
public class PoolManager : SingletonDestroyable<PoolManager>
{
    [SerializeField]
    private List<PoolObjectData> PoolObjectDataList = new(4);
    
    private Dictionary<KeyType, GameObject> _sampleDict;   // Key - 복제용 오브젝트 원본
    private Dictionary<KeyType, PoolObjectData> _dataDict; // Key - 풀 정보
    private Dictionary<KeyType, Stack<GameObject>> _poolDict;         // Key - 풀
    private Dictionary<GameObject, Stack<GameObject>> _clonePoolDict; // 복제된 게임오브젝트 - 풀
    
    private void Awake()
    {
        Init();
    }
    
    private void Init()
    {
        var len = PoolObjectDataList.Count;
        if (len == 0) return;

        // 1. Dictionary 생성
        _sampleDict    = new Dictionary<KeyType, GameObject>(len);
        _dataDict      = new Dictionary<KeyType, PoolObjectData>(len);
        _poolDict      = new Dictionary<KeyType, Stack<GameObject>>(len);
        _clonePoolDict = new Dictionary<GameObject, Stack<GameObject>>(len * PoolObjectData.INITIAL_COUNT);

        // 2. Data로부터 새로운 Pool 오브젝트 정보 생성
        foreach (var data in PoolObjectDataList)
        {
            RegisterInternal(data);
        }
    }
    
    /// <summary> Pool 데이터로부터 새로운 Pool 오브젝트 정보 등록 </summary>
    private void RegisterInternal(PoolObjectData data)
    {
        // 중복 키는 등록 불가능
        if (_poolDict.ContainsKey(data.Key))
        {
            return;
        }

        // 1. 샘플 게임오브젝트 생성, PoolObject 컴포넌트 존재 확인
        var sample = Instantiate(data.Prefab);
        sample.name = data.Prefab.name;
        sample.SetActive(false);

        // 2. Pool Dictionary에 풀 생성 + 풀에 미리 오브젝트들 만들어 담아놓기
        var pool = new Stack<GameObject>(data.MaxObjectCount);
        for (var i = 0; i < data.InitialObjectCount; i++)
        {
            var clone = Instantiate(data.Prefab);
            clone.SetActive(false);
            pool.Push(clone);

            _clonePoolDict.Add(clone, pool); // Clone-Stack 캐싱
        }

        // 3. 딕셔너리에 추가
        _sampleDict.Add(data.Key, sample);
        _dataDict.Add(data.Key, data);
        _poolDict.Add(data.Key, pool);
    }
    
    /// <summary> 샘플 오브젝트 복제하기 </summary>
    private GameObject CloneFromSample(KeyType key)
    {
        return !_sampleDict.TryGetValue(key, out var sample) ? null : Instantiate(sample);
    }
    
    /// <summary> 풀에서 꺼내오기 </summary>
    public GameObject Spawn(KeyType key)
    {
        // 키가 존재하지 않는 경우 null 리턴
        if (!_poolDict.TryGetValue(key, out var pool))
        {
            return null;
        }

        GameObject go;

        // 1. 풀에 재고가 있는 경우 : 꺼내오기
        if (pool.Count > 0)
        {
            go = pool.Pop();
        }
        // 2. 재고가 없는 경우 샘플로부터 복제
        else
        {
            go = CloneFromSample(key);
            _clonePoolDict.Add(go, pool); // Clone-Stack 캐싱
        }

        go.SetActive(true);
        go.transform.SetParent(null);

        return go;
    }

    /// <summary> 풀에 집어넣기 </summary>
    public void Despawn(GameObject go)
    {
        // 캐싱된 게임오브젝트가 아닌 경우 파괴
        if (!_clonePoolDict.TryGetValue(go, out var pool))
        {
            Destroy(go);
            return;
        }

        // 집어넣기
        go.SetActive(false);
        pool.Push(go);
    }
}