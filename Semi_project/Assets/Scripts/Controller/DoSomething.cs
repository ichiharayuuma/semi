// Unityでシリアル通信、Arduinoと連携する雛形

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class DoSomething : MonoBehaviour
{
    // 制御対象のオブジェクト用に宣言しておいて、Start関数内で名前で検索
    GameObject targetObject;
    Player targetScript; // UnityプロジェクトにPlayerオブジェクトがいる前提
    GameObject targetModel;
    StarterAssetsInputs targetModelScript;

    private float x = 0f;
    private float y = 0f;

    // シリアル通信のクラス、クラス名は正しく書くこと
    public SerialHandler serialHandler;

    void Start()
    {
        // 制御対象のオブジェクトを取得
        targetObject = GameObject.Find("Player"); // UnityのヒエラルキーにPlayerオブジェクトがいること。
        // 制御対象に関連付けられたスクリプトを取得。
        // 大文字、小文字を区別するので、player.csを作ったのなら「p」layer。
        targetScript = targetObject.GetComponent<Player>(); // こちらはスクリプトの名前

        // 信号受信時に呼ばれる関数としてOnDataReceived関数を登録
        serialHandler.OnDataReceived += OnDataReceived;

        // targetModel = GameObject.Find("unitychan_dynamic");
        targetModel = GameObject.Find("harazemichan");
        targetModelScript = targetModel.GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        //文字列を送信するなら例えばココ
        //serialHandler.Write("hogehoge");
        if (targetScript.jklPress[0])
        {
            targetScript.jklPress[0] = false;
            if (targetScript.jklToggle[0])
            {
                // LED ON
                serialHandler.Write("a");
            }
            else
            {
                // LED OFF
                serialHandler.Write("b");
            }
        }

        if (targetScript.jklPress[1])
        {
            targetScript.jklPress[1] = false;
            if (targetScript.jklToggle[1])
            {
                // LED ON
                serialHandler.Write("c");
            }
            else
            {
                // LED OFF
                serialHandler.Write("d");
            }
        }

        if (targetScript.jklPress[2])
        {
            targetScript.jklPress[2] = false;
            if (targetScript.jklToggle[2])
            {
                // LED ON
                serialHandler.Write("e");
            }
            else
            {
                // LED OFF
                serialHandler.Write("f");
            }
        }
    }

    //受信した信号(message)に対する処理
    void OnDataReceived(string message)
    {
        // ここでデコード処理等を記述
        if (message == null) return;
        if(message.Length >= 13 && message[0] == 'S' && message[12] == 'E')
        {
            //targetScript.debugText.text = message;
            string receivedData;
            int t;

            // Volume : x
            receivedData = message.Substring(1, 4);
            int.TryParse(receivedData, out t);
            targetScript.vol[0] = t;

            targetScript.LRStick.Vol = t;

            // Volume : y
            receivedData = message.Substring(5, 4);
            int.TryParse(receivedData, out t);
            targetScript.vol[1] = t;

            targetScript.UDStick.Vol = t;

            // Switch 1
            receivedData = message.Substring(9, 1);
            int.TryParse(receivedData, out t);
            targetScript.sw[0] = t;

            // Switch 2
            receivedData = message.Substring(10, 1);
            int.TryParse(receivedData, out t);
            targetScript.sw[1] = t;

            // Switch 3
            receivedData = message.Substring(11, 1);
            int.TryParse(receivedData, out t);
            targetScript.sw[2] = t;

            bool isMoveChange = false;
            if (targetScript.LRStick.GetKeyDownH() == true)
            {
                x = 1f;
                isMoveChange = true;
            }
            else if (targetScript.LRStick.GetKeyUpH() == true)
            {
                x = 0f;
                isMoveChange = true;
            }
            if (targetScript.LRStick.GetKeyDownL() == true)
            {
                x = -1f;
                isMoveChange = true;
            }
            else if (targetScript.LRStick.GetKeyUpL() == true)
            {
                x = 0f;
                isMoveChange = true;
            }

            if (targetScript.UDStick.GetKeyDownH() == true)
            {
                y = 1f;
                isMoveChange = true;
            }
            else if (targetScript.UDStick.GetKeyUpH() == true)
            {
                y = 0f;
                isMoveChange = true;
            }
            if (targetScript.UDStick.GetKeyDownL() == true)
            {
                y = -1f;
                isMoveChange = true;
            }
            else if (targetScript.UDStick.GetKeyUpL() == true)
            {
                y = 0f;
                isMoveChange = true;
            }

            if (isMoveChange)
            {
                targetModelScript.MoveInput(new Vector2(x, y));
            }

            targetScript.LRStick.Update();
            targetScript.UDStick.Update();

            if (targetScript.sw[2] == 0 && targetScript.swOld[2] == 1)
                targetModelScript.JumpInput(true);
        }
    }
}
