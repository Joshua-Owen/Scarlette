using UnityEngine;

public class RotateReticle : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 2f;
    RectTransform rect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rect.Rotate( new Vector3(0f, 0f, rotateSpeed));
    }
}