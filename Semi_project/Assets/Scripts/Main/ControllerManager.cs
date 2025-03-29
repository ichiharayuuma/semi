using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public SerialHandler serialHandler;

    [SerializeField] private PlayerController player;

    public int peltier = 9; // 0 -> cool, 1 -> hot , 9 -> nothing

    void Start()
    {
        serialHandler.OnDataReceived += OnDataRecieved;
    }

    void Update()
    {
        var str = peltier.ToString();
        serialHandler.Write(str);
        Debug.Log(str);
    }

    void OnDataRecieved(string message)
    {
        if (message == null) return;
        if (message.Length >= 16 && message[0] == 'S' && message[16] == 'E') // S, AccY, GyroY, GyroZ, R, E
        {
            // Debug.Log(message);
            string recData;
            int t;

            // acc y
            recData = message.Substring(1, 4);
            int.TryParse(recData, out t);
            player.acc[1] = t;

            // gyro y
            recData = message.Substring(5, 4);
            int.TryParse(recData, out t);
            player.gyro[1] = t;

            // gyro z
            recData = message.Substring(9, 4);
            int.TryParse(recData, out t);
            player.gyro[2] = t;

            // resistor
            recData = message.Substring(13, 3);
            int.TryParse(recData, out t);
            player.resi = t;
        }
    }
}
