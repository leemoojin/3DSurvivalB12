using UnityEngine;

[System.Serializable]
public class CraftingMaterial
{
    public ItemData item; // 아이템 데이터
    public int quantity;  // 필요한 수량
}

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public ItemData craftedItem;          // 제작될 아이템 데이터
    public CraftingMaterial[] requiredMaterials; // 필요한 재료 배열
}
