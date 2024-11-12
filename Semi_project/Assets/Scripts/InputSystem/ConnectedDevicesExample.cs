using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConnectedDevicesExample : MonoBehaviour
{
    // 初期化
    private void Awake()
    {
        // デバイスの変更を監視
        InputSystem.onDeviceChange += OnDeviceChange;

        // 最初に接続されているデバイス一覧をログ出力
        PrintAllDevices();
    }

    // 終了処理
    private void OnDestroy()
    {
        // デバイス変更の監視解除
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    // 全てのデバイスをログ出力
    private void PrintAllDevices()
    {
        // デバイス一覧を取得
        var devices = InputSystem.devices;

        // 現在のデバイス一覧をログ出力
        var sb = new StringBuilder();
        sb.AppendLine("現在接続されているデバイス一覧");

        for (var i = 0; i < devices.Count; i++)
        {
            sb.AppendLine($" - {devices[i]}");
        }

        print(sb);
    }

    // デバイスの変更を検知した時の処理
    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                print($"デバイス {device} が接続されました。");
                break;

            case InputDeviceChange.Disconnected:
                print($"デバイス {device} が切断されました。");
                break;

            case InputDeviceChange.Reconnected:
                print($"デバイス {device} が再接続されました。");
                break;

            default:
                // 接続や切断以外の変更は無視
                return;
        }

        // 接続や切断があった場合は、全てのデバイスを再びログ出力
        PrintAllDevices();
    }
}