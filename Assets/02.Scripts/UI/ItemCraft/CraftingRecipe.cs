using UnityEngine;

[System.Serializable]
public class CraftingMaterial
{
    public ItemData item; // ������ ������
    public int quantity;  // �ʿ��� ����
}

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public ItemData craftedItem;          // ���۵� ������ ������
    public CraftingMaterial[] requiredMaterials; // �ʿ��� ��� �迭
}
