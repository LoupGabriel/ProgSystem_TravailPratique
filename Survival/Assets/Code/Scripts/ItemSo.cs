using UnityEngine;


[CreateAssetMenu(menuName = "Collectable/NewItem")]
public class ItemSo : ScriptableObject
{
    public string Name;
    public string Id;

    public EItemType type;

    public int amount;
    public Sprite icon;

}


public enum EItemType
{
    FOOD,
    STAMINA,
    HEALTH
}
