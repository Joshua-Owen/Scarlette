
using System;
using System.Collections;
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
    [SerializeField] Transform sequenceContainer;
    public bool isShowingResult;
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
            AddArrowToUI(direction);
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

    public void AddArrowToUI(string direction )
    {
        int index = GetArrowIndex(direction);

        if(index == -1) throw new Exception("Invalid Directional input");
        
        var currentArrow = Instantiate(arrowIconPrefab,sequenceContainer);
        
        currentArrow.GetComponent<Image>().sprite = arrowSprites[index]; 


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
            ClearSequenceUI();
            HackController.ClearSequence();
            
        }
            
        
    }

    public void ClearSequenceUI()
    {
        foreach (Transform arrow in sequenceContainer)
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
    
}