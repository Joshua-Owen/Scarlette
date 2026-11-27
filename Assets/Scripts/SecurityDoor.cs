using System.Collections.Generic;
using UnityEngine;

public class SecurityDoor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Transform door;
    [SerializeField] float openDistance = 3f;
    [SerializeField] float openSpeed = 2f;
    List<HackCombination> combinations;
    Vector3 closedPosition;
    Vector3 openPosition;
    bool isOpening;

    void Start()
    {
        if (door == null)
        {
            Debug.LogError("Door Transform has not been assigned", this);
            return;
        }

        
        combinations = GetComponent<HackableObject>().Data.combinations;
        closedPosition = door.localPosition;
        openPosition = closedPosition + Vector3.right * openDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpening)
        {
            door.localPosition = Vector3.MoveTowards(door.localPosition, openPosition, openSpeed * Time.deltaTime);
        }
    }
    public void UnlockDoor()
    {
        if (isOpening) return;

        Debug.Log("Door is Unlocked");

        isOpening = true;

        //combinations.RemoveAt(combinations.FindIndex(combo => combo.abilityName == "Open Door"));

    }
}
