using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHackController : MonoBehaviour
{
    [SerializeField] PlayerController player;

    public bool hasMatch;

    public float inputTimeLimit = 5f;
    public float inputTimer;

    public Slider timerBar;
    [SerializeField] List<string> playerSequence = new List<string>();
    public int SequenceLength => playerSequence.Count;

    public string CurrentSequence => string.Join("", playerSequence);
    public bool isTimerRunning;

    public HackableObject currentTarget;   
    
    public void Awake()
    {
        SetCurrentTarget();
        SetUpHackingTimer();
    }
    public void AddDirection(string direction)
    {
        if (direction == null) return;

        playerSequence.Add(direction);
                    
        //Debug.Log(string.Join(" ",playerSequence));

    }

 
    public void ClearSequence()
    {
        playerSequence.Clear();
    }

    public bool IsValidPartialSequence()
    {
        string currentSequence = string.Join("",playerSequence);

        foreach (var combo in currentTarget.Data.combinations)
        {
            if (combo.sequence.StartsWith(currentSequence))
            {
                return true;
            }
        }
        return false;
    }

    public string CheckSequence()
    {
        string currentSequence = string.Join("", playerSequence);

        foreach (var combo in currentTarget.Data.combinations)
        {
            if(combo.sequence.Equals(currentSequence))
            {
                ClearSequence();
                return  combo.abilityName;
            }
        }

        return null;
    }
    
    public void SetUpHackingTimer()
    {
        inputTimer = inputTimeLimit;
        timerBar.maxValue = inputTimeLimit;
        timerBar.minValue = 0.0f;

    }
    public void StartHackingTimer()
    {
        if(!isTimerRunning) return;

        inputTimer -= Time.deltaTime;
        timerBar.value = inputTimer;

        if(inputTimer <= 0.0f) 
        { 
            inputTimer = 0;
            timerBar.value = 0;

            player.Hack.StartCoroutine(player.Hack.ChangeSequenceArrowUIColour(Color.darkRed, player.Hack.comboInvalidSeconds));
            player.Hack.StartCoroutine(player.Hack.ChangeBackgroundColour(player.Hack.sequenceBG, Color.darkRed, player.Hack.comboInvalidSeconds));
            player.Hack.StartCoroutine(player.Hack.ChangeBackgroundColour(player.Hack.combosBG, Color.darkRed, player.Hack.comboInvalidSeconds));
            ClearSequence();
            StopHackingTimer();
            inputTimer = inputTimeLimit;
            timerBar.value = inputTimeLimit;
            player.Hack.ResetComboList();
        }
        
    }
    public void StopHackingTimer()
    {
        isTimerRunning = false;
    }

    public void BeginSequence()
    {
        inputTimer = inputTimeLimit;
        timerBar.value = inputTimeLimit;
        isTimerRunning = true;
    }
    public void SetCurrentTarget()
    {
        Transform target = player.TargetFndr.currentTarget;

        if (target == null)
        {
            currentTarget = null;
            return;
        }

        currentTarget = target?.GetComponentInParent<HackableObject>();
        var dic = currentTarget.Data;
        Debug.Log(currentTarget);
    }


}