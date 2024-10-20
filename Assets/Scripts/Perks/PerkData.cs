using UnityEngine;

// ScriptableObject to store the data of each perk
[CreateAssetMenu(fileName = "New Perk", menuName = "Perk/Create New Perk")]
public class PerkData : ScriptableObject
{
    public string perkName;      // Name of the perk
    public int cost;             // Cost of the perk in the shop
    public ShapeType shapeType;  // Shape of the perk (Triangle, Circle, Square)
    public string description;   // Description of the perk's effect

    // Define the perk's unique effect with a string key (used in a more complex system)
    public string effectKey;
}
