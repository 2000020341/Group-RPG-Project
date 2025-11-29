using UnityEngine;

[CreateAssetMenu(fileName = "MoveTemplate", menuName = "Scriptable Objects/MoveTemplate")]
public class MoveTemplate : ScriptableObject
{
    public int attack;
    public int speed;
    public int accuracy;
    public int manaCost;
    public enum MoveTypes {Fire, Water, Air, Earth, Physical};
    public MoveTypes type;
    [TextArea] public string moveDescription;
}
