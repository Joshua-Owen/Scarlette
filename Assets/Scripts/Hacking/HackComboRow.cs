using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HackComboRow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI abilityName;
    public TextMeshProUGUI AbilityName => abilityName;
    [SerializeField] Transform arrowContainer;
    public Transform ArrowContainer => arrowContainer;
    public string Sequence { get; private set;}

    public void SetSequence(string sequence)
    {
        Sequence = sequence;
    } 
    public void HighlightArrow(int index, Color colour)
    {
        if (index < 0 || index >= ArrowContainer.childCount) return;

        Image arrow = ArrowContainer.GetChild(index).GetComponent<Image>();
        
        arrow.color = colour;
    }
}