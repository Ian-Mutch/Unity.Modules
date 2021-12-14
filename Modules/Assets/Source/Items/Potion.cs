using Modules.Inventory;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPotion", menuName = "Inventory/Items/Potion")]
public class Potion : ItemBase
{
    public float Percentage => _percentage;

    [SerializeField]
    private float _percentage;
}
