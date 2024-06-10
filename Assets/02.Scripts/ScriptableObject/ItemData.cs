using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템의 종류를 정의하는 열거형
public enum ItemType
{
    Equipable,    // 장비 아이템
    Consumable,   // 소비 아이템
    Resource      // 자원 아이템
}

// 소비 아이템의 종류를 정의하는 열거형
public enum ConsumableType
{
    Health,   // 체력 회복 아이템
    Hunger    // 포만감 회복 아이템
}

// 소비 아이템의 데이터를 담는 클래스
[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type; // 소비 아이템의 종류
    public float value;         // 소비 아이템이 증가시키는 값
}

// ScriptableObject를 상속받아 아이템 데이터를 정의하는 클래스
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;  // 아이템의 표시 이름
    public string description;  // 아이템의 설명
    public ItemType type;       // 아이템의 종류
    public Sprite icon;         // 아이템의 아이콘
    public GameObject dropPrefabs; // 아이템을 드롭했을 때 생성되는 프리팹

    [Header("Stacking")]
    public bool canStack;         // 아이템이 스택 가능한지 여부
    public int maxStackAmount;    // 최대 스택 개수

    [Header("Consumable")]
    public ItemDataConsumable[] consumables; // 소비 아이템 데이터 배열

    [Header("Equip")]
    public GameObject equipPrefabs; // 장비 아이템을 장착했을 때 생성되는 프리팹

}