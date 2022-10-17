using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;

    void Start()
    {
        
    }

    void Update()
    {

    }

    void onMove(InputValue value) 
    {
        moveInput = value.Get<Vector2>();
    }
}
