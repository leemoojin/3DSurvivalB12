using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;     // ���� ���� ��
    public float maxValue;     // �ִ� ���� ��
    public float startValue;   // ������ �� ���� ��
    public float passiveValue; // �нú� ���� ��ȭ�� (��: �ʴ� ü�� ȸ����)
    public Image uiBar;       // UI �� �̹���

    private void Start()
    {
        curValue = startValue; // ������ �� ���� ���� �� ����
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage(); // UI �� ������Ʈ
    }

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue); // ���� �� ����
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f); // ���� �� ����
    }

    public float GetPercentage()
    {
        return curValue / maxValue; // ���� ���� ����� ��ȯ
    }
}
