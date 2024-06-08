using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;     // 현재 상태 값
    public float maxValue;     // 최대 상태 값
    public float startValue;   // 시작할 때 상태 값
    public float passiveValue; // 패시브 상태 변화량 (예: 초당 체력 회복량)
    public Image uiBar;       // UI 바 이미지

    private void Start()
    {
        curValue = startValue; // 시작할 때 현재 상태 값 설정
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage(); // UI 바 업데이트
    }

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue); // 상태 값 증가
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f); // 상태 값 감소
    }

    public float GetPercentage()
    {
        return curValue / maxValue; // 상태 값의 백분율 반환
    }
}
