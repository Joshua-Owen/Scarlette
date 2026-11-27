
using UnityEngine;

public class HackableObject : MonoBehaviour
{

    [SerializeField] HackData hackData;
    public HackData Data => hackData;


    public void ExecuteAbiliity(string abilityName)
    {
        switch (abilityName)
        {
            case "Open Door":
                GetComponent<SecurityDoor>()?.UnlockDoor();
                break;
            case "Disable Turret":
                GetComponent<Turret>()?.DisableTurret();
                break;
            case "Overload":
                Debug.Log("Overloading generator");
                break;
            default:
                Debug.LogWarning($"Unknown ability: {abilityName}");
                break;

        }
    }
}
