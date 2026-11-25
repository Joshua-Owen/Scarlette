using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hack Data", menuName = "Hacking/Hack Data")]
public class HackData : ScriptableObject
{
    public List<HackCombination> combinations;
}

[System.Serializable]
public class HackCombination
{
    public string sequence;
    public string abilityName;
}
