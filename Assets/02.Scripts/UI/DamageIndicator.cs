using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image image;       // 피해 표시를 위한 이미지 UI
    public float flashSpeed;  // 피해 표시가 사라지는 속도

    private Coroutine coroutine; // 코루틴 참조

    private void Start()
    {
        // 플레이어의 상태에서 피해를 받았을 때 Flash 메서드 호출
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;
    }

    // 피해 표시를 화면에 플래시하는 메서드
    public void Flash()
    {
        // 현재 실행 중인 코루틴이 있으면 중지
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        // 이미지 활성화 및 색상 설정
        image.enabled = true;
        image.color = new Color(1f, 105f / 255f, 105f / 255f); // 빨간색으로 설정
        // 피해 표시를 사라지게 하는 코루틴 시작
        coroutine = StartCoroutine(FadeAway());
    }

    // 피해 표시가 서서히 사라지게 하는 코루틴
    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f; // 시작 알파 값
        float a = startAlpha;    // 현재 알파 값

        // 알파 값이 0이 될 때까지 반복
        while (a > 0.0f)
        {
            // 알파 값을 감소시킴
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            // 이미지의 색상과 알파 값 설정
            image.color = new Color(1f, 105f / 255f, 105f / 255f, a); // 빨간색과 현재 알파 값으로 설정
            yield return null;
        }

        // 코루틴이 끝나면 이미지 비활성화
        image.enabled = false;
    }
}
