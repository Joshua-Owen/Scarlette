using Microsoft.Unity.VisualStudio.Editor;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHack : MonoBehaviour
{
    [SerializeField] PlayerInputs playerInputs;
    public PlayerInputs Inputs => playerInputs;
    [SerializeField] TargetFinder targetFinder;
    public TargetFinder Target => targetFinder;
    public GameObject HackUI;
    public bool isHacking;

    void Start()
    {
       
    }

    void Update()
    {
       
       
    }

    public void HackToggle()
    {
        if(Target.currentTarget && Inputs.HackPressed && isHacking == false)
        {
            isHacking = true;
            
        }
        else if((Inputs.HackPressed && isHacking == true) || !Target.currentTarget)
        {
            isHacking = false;
        }
         
            
        //HackUI.SetActive(isHacking);
    }
}