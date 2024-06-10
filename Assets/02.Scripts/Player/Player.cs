using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller; // �÷��̾� ��Ʈ�ѷ�
    public PlayerCondition condition;    // �÷��̾� ����
    public Equipment equip;
    public ItemData itemData;   // ���� �÷��̾ ���� ������ ������
    public Action addItem;      // �������� �߰��� �� ȣ��� �׼�

    public Transform dropPosition; // �������� ���� ��ġ

    // ���� ������Ʈ�� ������ �� ȣ��Ǵ� �޼���
    private void Awake()
    {
        // CharacterManager�� Player �Ӽ��� ���� �÷��̾� ����
        CharacterManager.Instance.Player = this;

        // �ʿ��� ������Ʈ���� ����
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        equip = GetComponent<Equipment>();
    }
}
