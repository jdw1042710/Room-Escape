using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 3000;
    
    private InputManager _inputManager;

    private float _rotationX;

    private void Start()
    {
        if (InputManager.Instance)
        {
            _inputManager = InputManager.Instance;
        }
        else
        {
            Debug.Log($"참조 개망함");
        }
    }

    void Update()
    {
        MoveVertical(_inputManager.cameraVerticalMovement);
    }

    private void MoveVertical(float value)
    {
        value *= -1;

        _rotationX += value * movementSpeed * Time.deltaTime;
        _rotationX = Mathf.Clamp(_rotationX, -90, 90);
        transform.localRotation = Quaternion.Euler(Vector3.right * _rotationX);
    }
}
