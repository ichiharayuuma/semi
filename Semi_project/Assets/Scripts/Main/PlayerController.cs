using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    [Header("à⁄ìÆån")]
    [SerializeField] private float MaxSpeed = 10;
    [SerializeField] private float MinSpeed = 2;
    [SerializeField] private float HorizontalSpeed = 2;
    [SerializeField] private float RollingDistance = 1;
    [SerializeField] private float RollingCoolTime = 0.8f;
    [SerializeField] private GameObject player;

    [Header("ÉJÉÅÉâån")]
    [Tooltip("Virtual Camera Ç™Ç¬Ç¢ÇƒÇ¢Ç≠ì_")]
    [SerializeField] private GameObject FollowAt;
    [Tooltip("Virtual Camera Ç™å©ÇÈì_")]
    [SerializeField] private GameObject LookAt;
    [Tooltip("Follow At Ç™Ç«ÇÍÇ≠ÇÁÇ¢ç∂âEÇ…êUÇÍÇÈÇ©")]
    [SerializeField] private float FollowRatio = 1;
    [Tooltip("Follow At Ç™Ç«ÇÍÇ≠ÇÁÇ¢íxÇÍÇÈÇ©")]
    [SerializeField] private float FollowDelay = 5;
    [Tooltip("Look At Ç™Ç«ÇÍÇ≠ÇÁÇ¢ç∂âEÇ…êUÇÍÇÈÇ©")]
    [SerializeField] private float PanRatio = 1;
    [SerializeField] private CinemachineVirtualCamera resultCamera;
    [SerializeField] private CinemachineVirtualCamera StartCamera;

    [Header("class")]
    [SerializeField] private PointManager pointManager;
    [SerializeField] private ControllerManager controllerManager;

    [Header("VFX")]
    [SerializeField] private VisualEffect RStrip;
    [SerializeField] private VisualEffect LStrip;
    [SerializeField] private VisualEffect Damage;

    private float ForewardVelocity = 0;
    private float HorizontalVelocity = 0;
    private float LRBlend = 0;
    private float LRBlendGoal = 0;
    private bool enable = false;
    private bool isRolling = false;

    public int[] acc = new int[3];
    public int[] gyro = new int[3];
    public int resi;

    Rigidbody rb;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        ForewardVelocity = MinSpeed;

        player.transform.rotation = Quaternion.AngleAxis(30, player.transform.right);

        resultCamera.Priority = 0;
        StartCamera.Priority = 100;

        RStrip.SendEvent("OnPlay");
        LStrip.SendEvent("OnPlay");
    }

    public void OnStart()
    {
        enable = true;
        StartCamera.Priority = 0;
        controllerManager.peltier = 1; // if start, set hot
    }

    // Update is called once per frame
    void Update()
    {
        var key = Keyboard.current;
        if(key.wKey.isPressed)
        {
            Go(100);
        }
        else
        {
            Go(0);
        }

        if(key.aKey.isPressed)
        {
            LR(-100);
        }
        else if(key.dKey.isPressed)
        {
            LR(100);
        }
        else
        {
            LR(0);
        }

        // ------------  Controller  ------------------
        // horizontal
        var factor = acc[1];
        var d = factor > 0 ? 1 : -1;
        factor = factor * factor * d;
        LR((float)(Mathf.Clamp(factor, -10000, 10000) * 0.01));

        // vertical
        factor = resi;
        factor -= 220;
        factor *= 2;
        Go(Mathf.Clamp(factor, 0, 100));

        // rolling
        // var roll = acc[1] > acc[2] ? gyro[1] : gyro[2];
        var roll = Mathf.Abs(gyro[1]) + Mathf.Abs(gyro[2]);
        // Debug.Log(roll);
        if(roll > 400 && !isRolling)
        {
            // Debug.Log("Rolling!");
            isRolling = true;
            Rolling(gyro[2] < 0);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(HorizontalVelocity, 0, ForewardVelocity), ForceMode.VelocityChange);

        if (LRBlend != LRBlendGoal)
        {
            var diff = (LRBlendGoal - LRBlend) * 0.1f;
            if (diff < 0.1f)
            {
                LRBlend = LRBlendGoal;
            }
            else
            {
                LRBlend = LRBlend + diff;
            }
            animator.SetFloat("LRBlend", LRBlend);
        }
    }

    void LR(float x)    // -100 ~ 100
    {
        if (!enable) return;

        x = Mathf.Clamp(x, -100, 100);
        HorizontalVelocity = x * HorizontalSpeed * 0.01f;

        // change LookAt position
        var Lpos = LookAt.transform.localPosition;
        Lpos.x = x * 0.01f * PanRatio;
        LookAt.transform.localPosition = Lpos;

        // change FollowAt position
        var Fpos = FollowAt.transform.localPosition;
        Fpos.x = x * 0.01f * FollowRatio;
        FollowAt.transform.localPosition = Fpos;

        // change charactor rotation
        player.transform.localRotation = Quaternion.AngleAxis(x * 0.1f * -1, player.transform.forward);

        // set animator
        // animator.SetFloat("LRBlend", (x + 100) * 0.005f);
        LRBlendGoal = (x) * 0.01f;
    }

    void Go(float x)    // 0 ~ 100
    {
        if (!enable) return;

        x = Mathf.Clamp(x, 0, 100);
        var foreward = Mathf.Lerp(MinSpeed, MaxSpeed, x * 0.01f);
        ForewardVelocity = foreward;

        // delay follow at
        var Fpos = FollowAt.transform.localPosition;
        Fpos.z = x * 0.01f * FollowDelay * -1;
        FollowAt.transform.localPosition = Fpos;

        // set animator
        var speed = (x + 1) * 0.01f;
        animator.SetFloat("SpeedBlend", speed);
    }

    void Rolling(bool isRight)
    {
        if (!enable) return;

        // player ÇÃà íuÇà⁄ìÆ
        var Ppos = player.transform.position;
        if(isRight)
        {
            Ppos.x += RollingDistance;
            animator.SetTrigger("RightRoll");
        }
        else
        {
            Ppos.x -= RollingDistance;
            animator.SetTrigger("LeftRoll");
        }

        player.transform.position = Ppos;

        DOVirtual.DelayedCall(RollingCoolTime, () =>
        {
            enableRoll();
        });
    }

    private void enableRoll()
    {
        isRolling = false;
        // Debug.Log("Enable Rolling!");
    }

    public void Result()
    {
        animator.SetTrigger("End");
        resultCamera.Priority = 100;
        ForewardVelocity = 0;
        HorizontalVelocity = 0;
        enable = false;
        rb.AddForce(Vector3.zero);

        controllerManager.peltier = 0; // set cool

        RStrip.SendEvent("OnStop");
        LStrip.SendEvent("OnStop");
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Object")
        {
            pointManager.TakeDamage();
            controllerManager.peltier = 0; // if damaged, set cool
            DOVirtual.DelayedCall(4, () =>
            {
                controllerManager.peltier = 1; // after if return to hot
            });
            Damage.SendEvent("OnPlay");
        }
    }
}
