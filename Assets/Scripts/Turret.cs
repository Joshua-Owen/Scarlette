using UnityEngine;
using System.Collections.Generic;
 
public class Turret : MonoBehaviour
{
    [SerializeField] GameObject turret;
    List<HackCombination> combinations;
    bool isDisabled;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (turret == null)
        {
            Debug.LogError("Door Transform has not been assigned", this);
            return;
        }
        combinations = GetComponent<HackableObject>().Data.combinations;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableTurret()
    {
        if(isDisabled) return;
        Debug.Log($"Turret Disabled");
        isDisabled = true;
        //combinations.RemoveAt(combinations.FindIndex(combo => combo.abilityName == "Open Door"));

    }
}
