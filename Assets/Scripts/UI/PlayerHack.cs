
using System;
using System.Collections;
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
            //UpdateSequenceUI();
            Debug.Log($"Valid partial: {HackController.IsValidPartialSequence()}");
        }
        if (Inputs.HackConfirm && HackController.SequenceLength > 0 || HackController.SequenceLength >=8)
        {
            string ability = HackController.CheckSequence();
            
            if (ability != null)
            {
                Debug.Log($"Ability matched: {ability}");

                HackController.StopHackingTimer();
                StartCoroutine(ChangeArrowUIColour(Color.green, comboValidSeconds));
                HackController.inputTimer = HackController.inputTimeLimit;
                HackController.timerBar.value = HackController.inputTimeLimit;

            }
            else if (HackController.SequenceLength > 0 && HackController.IsValidPartialSequence())
            {
                Debug.Log($"Incomplete sequence");
                StartCoroutine(ChangeArrowUIColour(Color.yellow, comboPartialSeconds));
            }  
            else
            {
                Debug.Log("Invalid combination");
                StartCoroutine(ChangeArrowUIColour(Color.red, comboInvalidSeconds));
                
            }
            
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

    

    public IEnumerator ChangeArrowUIColour(Color validateColor, float time)
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

        var currentTarget = HackController.currentTarget;
        if(currentTarget == null) return;
        //var grid = comboListContainer.GetComponent<GridLayoutGroup>();
        foreach (var combo in currentTarget.Data.combinations)
        {
            //grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            //grid.constraintCount = combo.sequence.Length;
            var rowObject = Instantiate(comboRowPrefab,comboListContainer);
           
            var row = rowObject.GetComponent<HackComboRow>();
            row.AbilityName.text = combo.abilityName;

            foreach (var sequence in combo.sequence)
            {
                AddArrowToContainerUI(sequence.ToString(), row.ArrowContainer);
            }
        }
    }
    
}