
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHack : MonoBehaviour
{
    [SerializeField] PlayerInputs playerInputs;
    public PlayerInputs Inputs => playerInputs;
    [SerializeField] TargetFinder targetFinder;
    public TargetFinder Target => targetFinder;
    [SerializeField] PlayerHackController playerHackController;
    public PlayerHackController HackController => playerHackController;
    public GameObject HackUI;
    //[SerializeField] TextMeshProUGUI comboListUI;
    //public bool isHacking;
    public float comboValidSeconds = .8f;
    public float comboInvalidSeconds = 1.5f;
    public float comboPartialSeconds = 1f;
    Coroutine highlightRoutine;
    int ComboListLength;
    readonly List<HackComboRow> candidateComboRows = new List<HackComboRow>();

    [SerializeField] Sprite[] arrowSprites = new Sprite[4];
    [SerializeField] GameObject arrowIconPrefab;
    public Transform sequenceContainer;
    public Transform comboListContainer;
    public bool isShowingResult;
    [SerializeField] GameObject comboRowPrefab;

    void Update()
    {
        
    }   


    public void HandleHackValidation()
    {
        if(isShowingResult) return;
        string direction = HackInputHandler();
        if(direction != null)
        {
            if (!HackController.isTimerRunning)
            {
                HackController.BeginSequence();
            }
         
            HackController.AddDirection(direction);
            AddArrowToContainerUI(direction, sequenceContainer);
            UpdateComboListHightlight();
            //UpdateSequenceUI();
            Debug.Log($"Valid partial: {HackController.IsValidPartialSequence()}");
        }
        if (HackController.SequenceLength > 0 && (Inputs.HackConfirm || HackController.SequenceLength >= 8))
        {
            string ability = HackController.CheckSequence();
            
            if (ability != null)
            {
                Debug.Log($"Ability matched: {ability}");

                HackController.StopHackingTimer();
                StartCoroutine(ChangeSequenceArrowUIColour(Color.green, comboValidSeconds));
                    //FindComboRow(ability);
                StartCoroutine(ChangeComboListArrowUIColour(ability));
                HackController.inputTimer = HackController.inputTimeLimit;
                HackController.timerBar.value = HackController.inputTimeLimit;
                ResetComboList();
            }
            else if (HackController.SequenceLength > 0 && HackController.IsValidPartialSequence())
            {
                Debug.Log($"Incomplete sequence");
                UpdateComboListHightlightPartial();
                StartCoroutine(ChangeSequenceArrowUIColour(Color.yellow, comboPartialSeconds));
            }  
            else
            {
                Debug.Log("Invalid combination");
                StartCoroutine(ChangeSequenceArrowUIColour(Color.red, comboInvalidSeconds));
                ResetComboList();
            }                   
            //ClearSequenceUI(comboListContainer);
            
            //ShowComboListUI();
        }
    }
    /*
    public void UpdateSequenceUI()
    {
        comboListUI.text = HackController.CurrentSequence;
    }
    */

    public int GetArrowIndex (string direction)
    {
        return direction switch
        {
            "^" => 0,
            "v" => 1,
            "<" => 2,
            ">" => 3,
            _  => -1
        };
    }

    public void AddArrowToContainerUI(string direction, Transform container )
    {
        int index = GetArrowIndex(direction);

        if(index == -1) throw new Exception("Invalid Directional input");
        
        var currentArrow = Instantiate(arrowIconPrefab,container);
        
        currentArrow.GetComponent<Image>().sprite = arrowSprites[index]; 

        if(container.Equals(comboListContainer))
        {
            currentArrow.GetComponent<Image>().color = Color.skyBlue;
        }
    }

    

    public IEnumerator ChangeSequenceArrowUIColour(Color validateColor, float time)
    {
        isShowingResult = true;

        foreach (Transform arrow in sequenceContainer )
        {
            arrow.GetComponent<Image>().color = validateColor;
        }
        yield return new WaitForSeconds(time);
        
        isShowingResult = false;
       
        if (validateColor != Color.yellow)
        {
            ClearSequenceUI(sequenceContainer);
            HackController.ClearSequence();
            
        }    
    }

    public IEnumerator ChangeComboListArrowUIColour(string ability)
    {
        var originalColour = Color.white;
        foreach (Transform comboRowTransform in comboListContainer)
        {
            var ComboRowAbilityName = comboRowTransform.GetComponent<HackComboRow>().AbilityName;
            var comboListArrows = comboRowTransform.GetComponent<HackComboRow>().ArrowContainer;
            Debug.Log($"name: {ComboRowAbilityName} | arrows list: {comboListArrows}");
            if(ComboRowAbilityName.text.Equals(ability))
            {
                ComboRowAbilityName.color = Color.softBlue;
                Debug.Log("doo be doo");
                foreach (Transform arrowTransform in comboListArrows)
                {
                    Debug.Log($"{arrowTransform}");
                    //originalColour = arrowTransform.GetComponent<Image>().color;
                    arrowTransform.GetComponent<Image>().color =  Color.softBlue;
                    
                }

                yield return new WaitForSeconds(comboValidSeconds);

                ComboRowAbilityName.color = originalColour;
                foreach (Transform arrowTransform in comboListArrows)
                {
                    arrowTransform.GetComponent<Image>().color =  originalColour;
                }

                yield break;
            }

        }
    }

public void ResetComboList()
{
    candidateComboRows.Clear();

    foreach (Transform comboRowTransform in comboListContainer)
    {
        HackComboRow row = comboRowTransform.GetComponent<HackComboRow>();

        if (row == null) continue;

        foreach (Transform arrow in row.ArrowContainer)
        {
            arrow.GetComponent<Image>().color = Color.white;
        }
    }
}
    public void UpdateComboListHightlight()
    {
        string currentSequence = HackController.CurrentSequence;
        int arrowIndex = currentSequence.Length - 1;

        if (HackController.SequenceLength == 1)
        {
            candidateComboRows.Clear();
            foreach (Transform comboRowTransform in comboListContainer)
            {
                HackComboRow row = comboRowTransform.GetComponent<HackComboRow>();
                if (row != null)
                {
                    candidateComboRows.Add(row);
                }
            }
        }

        for (int i = candidateComboRows.Count - 1; i >= 0; i--)
        {
            HackComboRow row = candidateComboRows[i];
            if (row.Sequence.StartsWith(currentSequence, StringComparison.Ordinal))
            {
          
                    
                row.HighlightArrow(arrowIndex, Color.lightBlue);
                
            }
        
            else
            {
                row.HighlightArrow(arrowIndex, Color.red);
                candidateComboRows.RemoveAt(i);
            }
        }
    }


     public void UpdateComboListHightlightPartial()
    {
        string currentSequence = HackController.CurrentSequence;
        int arrowIndex = currentSequence.Length - 1;
        foreach (Transform comboRowTransform in comboListContainer)
            {
                HackComboRow row = comboRowTransform.GetComponent<HackComboRow>();
                if (row != null)
                {
                    candidateComboRows.Add(row);
                }
            
        }

        for (int i = candidateComboRows.Count - 1; i >= 0; i--)
        {
            HackComboRow row = candidateComboRows[i];
            if (row.Sequence.StartsWith(currentSequence, StringComparison.Ordinal))
            {
          
                    
                row.HighlightArrow(arrowIndex, Color.yellow);
                
            }
        
            else
            {
                candidateComboRows.RemoveAt(i);
            }
        }
    }
    public void ClearSequenceUI(Transform container)
    {
        foreach (Transform arrow in container)
        {
            Destroy(arrow.gameObject);
        }
    }

  
    string HackInputHandler()
    {
        Vector2 direction = Inputs.HackInput;


        if (direction == Vector2.zero) return null;
        if (Mathf.Abs(direction.x) > 0 && Mathf.Abs(direction.y) > 0) return null;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? ">" : "<";
        }

        return direction.y > 0 ? "^" : "v";

    }

    public void ShowComboListUI()
    {
        //ClearSequenceUI(comboListContainer);
        candidateComboRows.Clear();

        var currentTarget = HackController.currentTarget;
        if(currentTarget == null) return;
        //var grid = comboListContainer.GetComponent<GridLayoutGroup>();
        foreach (var combo in currentTarget.Data.combinations)
        {
            //grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            //grid.constraintCount = combo.sequence.Length;
            var rowObject = Instantiate(comboRowPrefab,comboListContainer);
           
            var row = rowObject.GetComponent<HackComboRow>();
            row.SetSequence(combo.sequence);
            row.AbilityName.text = combo.abilityName;
            candidateComboRows.Add(row);

            foreach (var sequence in combo.sequence)
            {
                AddArrowToContainerUI(sequence.ToString(), row.ArrowContainer);
            }
        }
    }

    public void FindComboRow(string ability)
    {
        var test = HackController.currentTarget.Data.combinations;

        foreach(var comboRow in test)
        {

            if (ability.Equals(comboRow.abilityName))
            {
                
            }
            else
            {
                throw new Exception("No matching ability found");
            }
            
        }
    }
    
}