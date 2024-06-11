using UnityEngine;

public class SingletonDestroyable<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T s_instance;

    public static T Instance
    {
        get
        {
            if (s_instance != null) return s_instance;

            var go = GameObject.Find(typeof(T).Name);
            
            var prefabPath = "Prefabs/" + typeof(T).Name;
            var prefab = Resources.Load<GameObject>(prefabPath);

            // 씬 내에 찾고자 하는 (이 싱글톤 클래스를 상속 받고 있는 스크립트가 붙어 있는) 오브젝트가
            if (go != null) // 있다면 
            {
                s_instance = go.GetComponent<T>(); // 해당 오브젝트의 컴포넌트 받아옴 
            }
            else if (prefab != null) // 프리팹으로 돼 있다면
            {
                // 씬에 생성하고 해당 프리팹 오브젝트의 컴포넌트를 불러와 (싱글톤) 인스턴스에 저장한 후 이름을 새로 지정
                var singletonObject = Instantiate(prefab);  
                s_instance = singletonObject.GetComponent<T>();
                singletonObject.name = typeof(T) + " (Singleton)";
            }
            else // 씬에도 없고 프리팹으로도 안 돼 있다면 
            {
                go = new GameObject(typeof(T).Name);    // T 타입의 이름으로 게임 오브젝트를 새로 생성 
                s_instance = go.AddComponent<T>();  // 해당 오브젝트에 T 타입의 컴포넌트를 추가한 후 (싱글톤) 인스턴스에 저장
            }

            return s_instance;  // (싱글톤) 인스턴스 반환
        }
    }
}