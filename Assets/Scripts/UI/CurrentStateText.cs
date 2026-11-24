using TMPro;
using UnityEngine;

public class CurrentStateText : MonoBehaviour
{
    [SerializeField] PlayerStateMachine playerStateMachine;
    public PlayerStateMachine StateMachine => playerStateMachine;
    TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"current state: {StateMachine.currentState.ToString()}";
    }
}
