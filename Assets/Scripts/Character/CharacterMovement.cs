using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 3;
    [SerializeField] private float rotationSpeed = 300;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float groundCheckOffset = 0.01f;

    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    private Animator _animator;
    private InputManager _inputManager;

    private bool _isInLadder = false;

    private static readonly int XMove = Animator.StringToHash("xMove");
    private static readonly int YMove = Animator.StringToHash("yMove");
    
    public bool isGround = false;
    public bool isLanding = false;
    public bool isMoving = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        if (InputManager.Instance)
        {
            _inputManager = InputManager.Instance;
        }
    }

    private void Update()
    {
        CheckGround();
        MoveForward(_inputManager.moveForward);
        MoveHorizontal(_inputManager.moveHorizontal);
        Rotate(_inputManager.cameraHorizontalMovement);
        Jump(_inputManager.isJump);
        UpdateStatusVariables();
        UpdateAnimationValue();
    }

    private void CheckGround()
    {
        RaycastHit hit;
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground");
        var result = Physics.Raycast(
            transform.position, 
            -transform.up, 
            out hit, 
            _collider.height * 0.5f + groundCheckOffset,
            layerMask);
        isLanding = result && !isGround;
        isGround = result;
    }

    private void MoveForward(float value)
    {
        if (_isInLadder)
        {
            ClimbLadder(value);
        }
        else
        {
            _rigidbody.position += movementSpeed * value * Time.deltaTime * transform.forward;
        }
    }

    private void MoveHorizontal(float value)
    {
        _rigidbody.position += movementSpeed * value * Time.deltaTime * transform.right;
    }

    private void Rotate(float value)
    {
        
        _rigidbody.rotation = Quaternion.Euler(_rigidbody.rotation.eulerAngles + rotationSpeed * value * Time.deltaTime * Vector3.up);
    }

    private void Jump(bool value)
    {
        if (!value) return;
        if (!isGround) return;
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ClimbLadder(float value)
    {
        _rigidbody.position += movementSpeed * value * Time.deltaTime * transform.up;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            _isInLadder = true;
            _rigidbody.useGravity = !_isInLadder;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            _isInLadder = false;
            _rigidbody.useGravity = !_isInLadder;
        }
    }

    private void UpdateAnimationValue()
    {
        _animator.SetFloat(XMove, _inputManager.moveHorizontal);
        _animator.SetFloat(YMove, _inputManager.moveForward);
    }
    
    private void UpdateStatusVariables()
    {
        isMoving = Mathf.Abs(_inputManager.moveHorizontal) >  0 || Mathf.Abs(_inputManager.moveForward) >  0;
    }
}
