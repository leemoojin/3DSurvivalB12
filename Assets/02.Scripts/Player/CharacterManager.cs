using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance; // �̱��� �ν��Ͻ�
    public static CharacterManager Instance // �̱��� �ν��Ͻ��� �����ϱ� ���� ������Ƽ
    {
        get
        {
            if (_instance == null)
            {
                // �ν��Ͻ��� ������ ���� �����Ͽ� ��ȯ
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }

    // �÷��̾ ���� ������Ƽ
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }
    private Player _player; // �÷��̾� ��ü

    // ���� ������Ʈ�� ������ �� ȣ��Ǵ� �޼���
    private void Awake()
    {
        if (_instance == null)
        {
            // �ν��Ͻ��� ������ ���� ��ü�� �ν��Ͻ��� �����ϰ�, �� ��ȯ �� �ı����� �ʵ��� ����
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                // �̹� �ν��Ͻ��� �����ϸ� ���� ��ü�� �ı��Ͽ� �ߺ� ���� ����
                Destroy(gameObject);
            }
        }
    }
}
