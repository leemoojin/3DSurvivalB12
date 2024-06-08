using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image image;       // ���� ǥ�ø� ���� �̹��� UI
    public float flashSpeed;  // ���� ǥ�ð� ������� �ӵ�

    private Coroutine coroutine; // �ڷ�ƾ ����

    private void Start()
    {
        // �÷��̾��� ���¿��� ���ظ� �޾��� �� Flash �޼��� ȣ��
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;
    }

    // ���� ǥ�ø� ȭ�鿡 �÷����ϴ� �޼���
    public void Flash()
    {
        // ���� ���� ���� �ڷ�ƾ�� ������ ����
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        // �̹��� Ȱ��ȭ �� ���� ����
        image.enabled = true;
        image.color = new Color(1f, 105f / 255f, 105f / 255f); // ���������� ����
        // ���� ǥ�ø� ������� �ϴ� �ڷ�ƾ ����
        coroutine = StartCoroutine(FadeAway());
    }

    // ���� ǥ�ð� ������ ������� �ϴ� �ڷ�ƾ
    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f; // ���� ���� ��
        float a = startAlpha;    // ���� ���� ��

        // ���� ���� 0�� �� ������ �ݺ�
        while (a > 0.0f)
        {
            // ���� ���� ���ҽ�Ŵ
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            // �̹����� ����� ���� �� ����
            image.color = new Color(1f, 105f / 255f, 105f / 255f, a); // �������� ���� ���� ������ ����
            yield return null;
        }

        // �ڷ�ƾ�� ������ �̹��� ��Ȱ��ȭ
        image.enabled = false;
    }
}
