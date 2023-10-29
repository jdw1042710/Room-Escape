using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public float moveForward = 0;
    public float moveHorizontal = 0;
    public float cameraVerticalMovement = 0;
    public float cameraHorizontalMovement = 0;
    public bool isJump = false;
    public bool isInteract = false;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    private void Update()
    {
        moveForward = Input.GetAxis("Vertical");
        moveHorizontal = Input.GetAxis("Horizontal");
        cameraVerticalMovement = Input.GetAxis("Mouse Y");
        cameraHorizontalMovement = Input.GetAxis("Mouse X");
        isJump = Input.GetKeyDown(KeyCode.Space);
        isInteract = Input.GetKeyDown(KeyCode.E);
        //Debug.Log($"{cameraHorizontalMovement} {cameraVerticalMovement}");
    }
}
