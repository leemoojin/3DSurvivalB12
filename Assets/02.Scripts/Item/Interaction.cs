using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// ��ȣ �ۿ� ������ ������Ʈ�� ���� �������̽�
public interface IInteractable
{
    public string GetInteractPrompt(); // ��ȣ �ۿ� ������Ʈ ��ȯ
    public void OnInteract();          // ��ȣ �ۿ� �� ȣ��� �Լ�
}

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;     // ��ȣ �ۿ� üũ ����
    private float lastCheckTime;        // ���������� üũ�� �ð�
    public float maxCheckDistance;      // ��ȣ �ۿ� �Ÿ�
    public LayerMask layerMask;         // ��ȣ �ۿ� ���̾� ����ũ

    public GameObject curInteractGameObject;  // ���� ��ȣ �ۿ� ���� ���� ������Ʈ
    private IInteractable curInteractable;     // ���� ��ȣ �ۿ� ������ �������̽�

    public TextMeshProUGUI promptText;  // ��ȣ �ۿ� ������Ʈ�� ǥ���� �ؽ�Ʈ �޽� ����
    private Camera camera;              // ���� ī�޶�

    void Start()
    {
        camera = Camera.main;
    }

    // �� �����Ӹ��� ����Ǵ� �Լ�
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); // ī�޶� �ü��� ȭ�� �߾����κ��� �߻�
            RaycastHit hit;

            // ����ĳ��Ʈ�� ���� ��ȣ �ۿ� ����� üũ
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    // ���� ��ȣ �ۿ� ��� ������Ʈ
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();                    
                    SetPromptText(); // ������Ʈ �ؽ�Ʈ ���� - �ӽ� �ּ�ó��**
                }
            }
            else
            {
                // ��ȣ �ۿ� ����� ������ �ʱ�ȭ
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false); // ������Ʈ �ؽ�Ʈ ��Ȱ��ȭ
            }
        }
    }

    // ��ȣ �ۿ� ������Ʈ �ؽ�Ʈ ����
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true); // ������Ʈ �ؽ�Ʈ Ȱ��ȭ
        promptText.text = curInteractable.GetInteractPrompt(); // ���ͷ��ͺ�κ��� ������Ʈ �ؽ�Ʈ ������ ����
    }

    // ��ȣ �ۿ� �Է� ó��
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            // ��ȣ �ۿ� ���� �� ���� ��ȣ �ۿ� ���� ���ͷ��ͺ� �ʱ�ȭ �� ������Ʈ �ؽ�Ʈ ��Ȱ��ȭ
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
