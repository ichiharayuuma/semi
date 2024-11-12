using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    public int[] sw = new int[3];
    private int[] swOld = new int[3];
    public int[] vol = new int[2];

    [SerializeField] private float moveValue = 0.01f;
    [SerializeField] private float jumpPower = 1f;
    [SerializeField] private float runRatio = 2f;

    private Rigidbody rb;
    private float Horizontal = 0.0f;
    private float Vertical = 0.0f;
    private bool Jump = false;
    private bool Run = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        debugText.text = "debug";

        sw[0] = sw[1] = sw[2] = 1;
        swOld[0] = swOld[1] = swOld[2] = 1;
        vol[0] = vol[1] = 2400;
    }

    void Update()
    {
        //var current = Keyboard.current;
        //if (current == null)
        //{
        //    return;
        //}

        // âEà⁄ìÆ
        //if(current.rightArrowKey.isPressed)
        //{
        //    this.transform.position += new Vector3(moveValue, 0, 0);
        //}

        //// ç∂à⁄ìÆ
        //if(current.leftArrowKey.isPressed)
        //{
        //    this.transform.position += new Vector3(-moveValue, 0, 0);
        //}

        if (sw[0] == 0 && swOld[0] == 1)
        {
            Jump = true;
        }
        else
        {
            Jump = false;
        }


        Run = false;
        Vector2 moveDirection = new(0, 0);
        // xé≤
        if (vol[0] < 1500)
        {
            moveDirection.x = -1;
            if (vol[0] < 800)
            {
                Run = true;
            }
        } else if (vol[0] > 3000)
        {
            moveDirection.x = 1;
            if (vol[0] > 3600)
            {
                Run = true;
            }
        }
        // yé≤
        if (vol[1] < 1500)
        {
            moveDirection.y = -1;
            if (vol[1] < 1000)
            {
                Run = true;
            }
        } else if (vol[1] > 3000)
        { 
            moveDirection.y = 1;
            if (vol[1] > 3600)
            {
                Run = true;
            }
        }
        moveDirection.Normalize();
        Horizontal= moveDirection.x;
        Vertical= moveDirection.y;


        string str = string.Format("vol:{0} {1}, sw:{2} {3} {4}", vol[0], vol[1], sw[0], sw[1], sw[2]);
        debugText.text = str;

        for(int i = 0; i < 3; i++)
        {
            swOld[i] = sw[i];
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new(0, 0, 0);
        Vector3 front = this.transform.forward.normalized;
        Vector3 right = this.transform.right.normalized;
        movement += moveValue * Vertical * front;
        movement += Horizontal * moveValue * right;
        if(Run)
        {
            movement *= runRatio;
        }
        //Vector3 movement = new Vector3(Horizontal * moveValue, 0, Vertical * moveValue);
        if (Jump)
        {
            movement.y = jumpPower;
        }
        else
        {
            movement.y = 0;
        }
        rb.AddForce(movement, ForceMode.VelocityChange);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        Horizontal = moveInput.x;
        Vertical = moveInput.y;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Jump = true;
        }
        else
        {
            Jump= false;
        }
    }
}
