using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// ボタンを1つだけ持つカスタムデバイスの入力状態
// 自作コントローラー用
[StructLayout(LayoutKind.Explicit)]
public struct MyControllerState : IInputStateTypeInfo
{
    // フォーマット識別子
    public FourCC format => new FourCC('M', 'C', 'T', 'R');

    // ボタン
    [FieldOffset(0)]
    [InputControl(name = "button", layout = "Button", bit = 0, displayName = "One Button")]
    public byte button;
}

// ボタンを1つだけ持つカスタムデバイス
[InputControlLayout(displayName = "My Controller", stateType = typeof(MyControllerState))]
#if UNITY_EDITOR
// Unityエディタで初期化処理を呼び出すのに必要
[UnityEditor.InitializeOnLoad]
#endif
public class MyController : InputDevice, IInputUpdateCallbackReceiver
{
    // ボタン
    public ButtonControl button { get; private set; }

    // 初期化
    static MyController()
    {
        // デバイスのレイアウトを登録
        InputSystem.RegisterLayout<MyController>();
    }

    // セットアップ完了時に呼び出される
    protected override void FinishSetup()
    {
        base.FinishSetup();

        // ボタンを取得
        button = GetChildControl<ButtonControl>("button");
    }

    public void OnUpdate()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        // マウスの左ボタンが押されている場合はボタンを押下状態にする
        var state = new MyControllerState
        {
            button = mouse.leftButton.isPressed ? (byte)1 : (byte)0
        };

        // 入力状態をキューに追加
        InputSystem.QueueStateEvent(this, state);
    }
}