using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller; // 플레이어 컨트롤러
    public PlayerCondition condition;    // 플레이어 상태
    public Equipment equip;
    public ItemData itemData;   // 현재 플레이어가 가진 아이템 데이터
    public Action addItem;      // 아이템을 추가할 때 호출될 액션

    public Transform dropPosition; // 아이템을 버릴 위치

    // 게임 오브젝트가 생성될 때 호출되는 메서드
    private void Awake()
    {
        // CharacterManager의 Player 속성에 현재 플레이어 설정
        CharacterManager.Instance.Player = this;

        // 필요한 컴포넌트들을 참조
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        equip = GetComponent<Equipment>();
    }
}
