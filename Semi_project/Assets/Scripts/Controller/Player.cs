using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    public int[] sw = new int[3];
    public int[] swOld = new int[3];
    public int[] vol = new int[2];

    [SerializeField] private float moveValue = 0.01f;
    [SerializeField] private float jumpPower = 1f;
    [SerializeField] private float runRatio = 2f;

    private Rigidbody rb;
    private float Horizontal = 0.0f;
    private float Vertical = 0.0f;
    private bool Jump = false;
    private bool Run = false;

    public VolToSw LRStick = new VolToSw(1800, 3200);
    public VolToSw UDStick = new VolToSw(1800, 3200);

    public bool[] jklPress = new bool[3];
    public bool[] jklToggle = new bool[3];

    public float startTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        debugText.text = "debug";

        sw[0] = sw[1] = sw[2] = 1;
        swOld[0] = swOld[1] = swOld[2] = 1;
        vol[0] = vol[1] = 2400;

        jklPress[0] = false;
        jklPress[1] = false;
        jklPress[2] = false;
        jklToggle[0] = false;
        jklToggle[1] = false;
        jklToggle[2] = false;
    }

    void Update()
    {
        var current = Keyboard.current;
        if (current == null)
        {
            return;
        }

        // 右移動
        if (current.rightArrowKey.isPressed)
        {
            this.transform.position += new Vector3(moveValue, 0, 0);
        }

        // 左移動
        if (current.leftArrowKey.isPressed)
        {
            this.transform.position += new Vector3(-moveValue, 0, 0);
        }

        // ジャンプ
        if (current.spaceKey.wasPressedThisFrame)
        {
            GetComponent<Rigidbody>().velocity = Vector3.up * jumpPower;
        }

        // ライト
        if (current.jKey.wasPressedThisFrame)
        {
            jklPress[0] = true;
            jklToggle[0] = !jklToggle[0];
        }
        if (current.kKey.wasPressedThisFrame)
        {
            jklPress[1] = true;
            jklToggle[1] = true;
        }
        if (current.kKey.wasReleasedThisFrame)
        {
            jklPress[1] = true;
            jklToggle[1] = false;
        }
        if (current.lKey.wasPressedThisFrame)
        {
            jklPress[2] = true;
            jklToggle[2] = true;
            startTime = 0;
        }

        if (startTime >= 0f)
        {
            startTime += Time.deltaTime;
            if (startTime > 3f)
            {
                jklPress[2] = true;
                jklToggle[2] = false;
                startTime = -1;
            }
        }

        debugText.text = LRStick.Vol.ToString() + ", " + UDStick.Vol.ToString();

        //if (sw[0] == 0 && swOld[0] == 1)
        //{
        //    Jump = true;
        //}
        //else
        //{
        //    Jump = false;
        //}


        //Run = false;
        //Vector2 moveDirection = new(0, 0);
        //// x軸
        //if (vol[0] < 1500)
        //{
        //    moveDirection.x = -1;
        //    if (vol[0] < 800)
        //    {
        //        Run = true;
        //    }
        //} else if (vol[0] > 3000)
        //{
        //    moveDirection.x = 1;
        //    if (vol[0] > 3600)
        //    {
        //        Run = true;
        //    }
        //}
        //// y軸
        //if (vol[1] < 1500)
        //{
        //    moveDirection.y = -1;
        //    if (vol[1] < 1000)
        //    {
        //        Run = true;
        //    }
        //} else if (vol[1] > 3000)
        //{ 
        //    moveDirection.y = 1;
        //    if (vol[1] > 3600)
        //    {
        //        Run = true;
        //    }
        //}
        //moveDirection.Normalize();
        //Horizontal= moveDirection.x;
        //Vertical= moveDirection.y;


        //string str = string.Format("vol:{0} {1}, sw:{2} {3} {4}", vol[0], vol[1], sw[0], sw[1], sw[2]);
        //debugText.text = str;

        //for(int i = 0; i < 3; i++)
        //{
        //    swOld[i] = sw[i];
        //}
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

// アナログ値をデジタル値に変換するクラス
// HIGH側しきい値を超えたときに1、LOW側しきい値を下回ったときに-1、その他は0に変換する。
// GetKeyDown* で押したとき、GetKeyUp* で離したとき、GetKey* で連射対策前のデータが取得できる。
// コンストラクタでしきい値を設定、ループ処理の最後でupdateを呼ぶ。

public class VolToSw
{
    int vol;
    int previousValue;
    int previousSwStateL;
    int currentSwStateL;
    int previousSwStateH;
    int currentSwStateH;
    int thresholdLowValue;
    int thresholdHighValue;

    bool preparation;

    public VolToSw(int thresholdLow, int thresholdHigh)
    {
        thresholdLowValue = thresholdLow;
        thresholdHighValue = thresholdHigh;
        previousValue = -1;
        preparation = false;
    }

    public int Vol
    {
        set
        {
            this.vol = value;
            if (previousValue > -1)
            {
                preparation = true;
            }

            if (value > thresholdHighValue)
            {
                currentSwStateH = 0;
            }
            else
            {
                currentSwStateH = 1;
            }

            if (value < thresholdLowValue)
            {
                currentSwStateL = 0;
            }
            else
            {
                currentSwStateL = 1;
            }
        }

        get { return vol; }
    }

    public void Update()
    {
        previousValue = vol;
        previousSwStateH = currentSwStateH;
        previousSwStateL = currentSwStateL;
    }

    public bool GetKeyDownH()
    {
        if (preparation && previousSwStateH == 1 && currentSwStateH == 0)
        {
            return true;
        }
        else
            return false;
    }

    public bool GetKeyUpH()
    {
        if (preparation && previousSwStateH == 0 && currentSwStateH == 1)
        {
            return true;
        }
        else
            return false;
    }

    public bool GetKeyH()
    {
        if (currentSwStateH == 0)
            return true;
        else
            return false;
    }
    public bool GetKeyDownL()
    {
        if (preparation && currentSwStateL == 0 && previousSwStateL == 1)
            return true;
        else
            return false;
    }

    public bool GetKeyUpL()
    {
        if (preparation && currentSwStateL == 1 && previousSwStateL == 0)
            return true;
        else
            return false;
    }

    public bool GetKeyL()
    {
        if (currentSwStateL == 0)
            return true;
        else
            return false;
    }
}