using UnityEngine;

// �� �����ۿ� ���� ������ ��� Ŭ����
[System.Serializable]
public class Craft
{
    public string craftName; // ������ �̸�
    public GameObject go_prefab; // ���� ������ ������
    public GameObject go_PreviewPrefab; // �̸� ���⿡ ���� ������
}

// ���� �޴����� �����ϴ� Ŭ����
public class CraftManual : MonoBehaviour
{
    private bool isActivated = false; // CraftManual UI�� Ȱ��ȭ ���¸� ��Ÿ��
    private bool isPreviewActivated = false; // �̸� ������ Ȱ��ȭ ���¸� ��Ÿ��
    private PlayerController controller;

    [SerializeField]
    private GameObject go_BaseUI; // �⺻ ���̽� UI

    [SerializeField]
    private Craft[] craft_fire; // �� �ǿ� �ִ� ���Ե��� ��Ÿ���� �迭

    private GameObject go_Preview; // �̸� ���� �������� ������ ����
    private GameObject go_Prefab; // ���� ������ �������� ������ ���� 

    [SerializeField]
    private Transform tf_Player; // �÷��̾��� ��ġ�� ��Ÿ��

    private RaycastHit hitInfo; // ����ĳ��Ʈ�� ���� ���� ������ ������ ����
    [SerializeField]
    private LayerMask layerMask; // ����ĳ��Ʈ���� �浹 �˻��� ���̾� ����ũ
    [SerializeField]
    private float range; // ����ĳ��Ʈ�� �ִ� �Ÿ�

    private void Start()
    {
        controller = FindObjectOfType<PlayerController>(); // PlayerController �ν��Ͻ��� ã��
        if (controller != null)
        {
            controller.craft += Toggle; // PlayerController�� craft �̺�Ʈ�� Toggle �޼��带 ����
        }
    }

    // ���� Ŭ�� �� ȣ��Ǵ� �޼���
    public void SlotClick(int _slotNumber)
    {
        // UI�� ����
        Window(false); // UI�� ����
        // ���� ī�޶��� ��ġ�� ������� �̸����� �������� ������
        go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, Camera.main.transform.position, Quaternion.identity);
        go_Prefab = craft_fire[_slotNumber].go_prefab;
        isPreviewActivated = true;
    }



    void Update()
    {
        if (isPreviewActivated)
        {
            PreviewPositionUpdate(); // �̸������� ��ġ�� ������Ʈ��
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Build(); // ���带 ������
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel(); // ��Ҹ� ������
        }
    }

    // �̸������� ��ġ�� ������Ʈ�ϴ� �޼���
    private void PreviewPositionUpdate()
    {
        // �÷��̾��� �þ� �������� ����ĳ��Ʈ�� �߻��Ͽ� �̸����� �������� ��ġ�� ������
        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;
                go_Preview.transform.position = _location;
            }
        }
    }

    // ���带 �����ϴ� �޼���
    private void Build()
    {
        if (isPreviewActivated)
        {
            Instantiate(go_Prefab, hitInfo.point, Quaternion.identity);
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
            // UI�� �ٽ� Ȱ��ȭ��
            Window(true);
        }
    }

    // â�� ���ų� �ݴ� �޼���
    private void Window(bool toggle)
    {
        if (!toggle)
        {
            if (go_BaseUI.activeSelf) // UI�� Ȱ��ȭ�Ǿ� ���� ���� ȭ�� ���� ����
            {
                controller.ToggleCursor(false);
            }
        }

        if (isActivated)
        {
            OpenWindow();
        }
        else
        {
            CloseWindow();
        }
    }


    // â�� ���� ���� �޼���
    private void OpenWindow()
    {
        isActivated = true;
        go_BaseUI.SetActive(true); // UI�� Ȱ��ȭ��
    }

    // â�� �ݱ� ���� �޼���
    private void CloseWindow()
    {
        isActivated = false;
        go_BaseUI.SetActive(false); // UI�� ��Ȱ��ȭ��
    }

    // ��Ҹ� �����ϴ� �޼���
    private void Cancel()
    {
        if (isPreviewActivated)
        {
            Destroy(go_Preview); // �̸����⸦ ������
        }

        isActivated = false;
        isPreviewActivated = false;

        go_Preview = null;
        go_Prefab = null;

        go_BaseUI.SetActive(false); // UI�� ��
    }

    // UI�� ����ϴ� �޼���
    public void Toggle()
    {
        Debug.Log("toggle open");
        if (go_BaseUI.activeInHierarchy)
        {
            go_BaseUI.SetActive(false); // UI�� ��Ȱ��ȭ��
        }
        else
        {
            go_BaseUI.SetActive(true); // UI�� Ȱ��ȭ��
        }
    }

}
