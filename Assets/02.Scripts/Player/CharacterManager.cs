using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance; // 싱글톤 인스턴스
    public static CharacterManager Instance // 싱글톤 인스턴스에 접근하기 위한 프로퍼티
    {
        get
        {
            if (_instance == null)
            {
                // 인스턴스가 없으면 새로 생성하여 반환
                _instance = new GameObject("CharacerManager").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }

    // 플레이어를 위한 프로퍼티
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }
    private Player _player; // 플레이어 객체

    // 게임 오브젝트가 생성될 때 호출되는 메서드
    private void Awake()
    {
        if (_instance == null)
        {
            // 인스턴스가 없으면 현재 객체를 인스턴스로 설정하고, 씬 전환 시 파괴되지 않도록 설정
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                // 이미 인스턴스가 존재하면 현재 객체를 파괴하여 중복 생성 방지
                Destroy(gameObject);
            }
        }
    }
}
