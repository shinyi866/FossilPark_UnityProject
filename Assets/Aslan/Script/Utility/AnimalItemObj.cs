using UnityEngine;

[System.Serializable]
public class Item
{
    public Sprite MainImage;
    public Sprite NameImage;
    public string text;
}

[CreateAssetMenu(fileName = "AnimalItemObj", menuName = "ScriptableObjects/AnimalItemObj", order = 0)]
public class AnimalItemObj : ScriptableObject
{
    [SerializeField]
    public Item[] AnimalItems;

    [SerializeField]
    public Item[] DinosaurlItems;

}
