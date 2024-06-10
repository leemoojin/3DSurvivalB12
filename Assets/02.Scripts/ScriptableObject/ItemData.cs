using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������� ������ �����ϴ� ������
public enum ItemType
{
    Equipable,    // ��� ������
    Consumable,   // �Һ� ������
    Resource      // �ڿ� ������
}

// �Һ� �������� ������ �����ϴ� ������
public enum ConsumableType
{
    Health,   // ü�� ȸ�� ������
    Hunger    // ������ ȸ�� ������
}

// �Һ� �������� �����͸� ��� Ŭ����
[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type; // �Һ� �������� ����
    public float value;         // �Һ� �������� ������Ű�� ��
}

// ScriptableObject�� ��ӹ޾� ������ �����͸� �����ϴ� Ŭ����
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;  // �������� ǥ�� �̸�
    public string description;  // �������� ����
    public ItemType type;       // �������� ����
    public Sprite icon;         // �������� ������
    public GameObject dropPrefabs; // �������� ������� �� �����Ǵ� ������

    [Header("Stacking")]
    public bool canStack;         // �������� ���� �������� ����
    public int maxStackAmount;    // �ִ� ���� ����

    [Header("Consumable")]
    public ItemDataConsumable[] consumables; // �Һ� ������ ������ �迭

    [Header("Equip")]
    public GameObject equipPrefabs; // ��� �������� �������� �� �����Ǵ� ������

}