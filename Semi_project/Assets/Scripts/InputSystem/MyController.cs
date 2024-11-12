using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// �{�^����1�������J�X�^���f�o�C�X�̓��͏��
// ����R���g���[���[�p
[StructLayout(LayoutKind.Explicit)]
public struct MyControllerState : IInputStateTypeInfo
{
    // �t�H�[�}�b�g���ʎq
    public FourCC format => new FourCC('M', 'C', 'T', 'R');

    // �{�^��
    [FieldOffset(0)]
    [InputControl(name = "button", layout = "Button", bit = 0, displayName = "One Button")]
    public byte button;
}

// �{�^����1�������J�X�^���f�o�C�X
[InputControlLayout(displayName = "My Controller", stateType = typeof(MyControllerState))]
#if UNITY_EDITOR
// Unity�G�f�B�^�ŏ������������Ăяo���̂ɕK�v
[UnityEditor.InitializeOnLoad]
#endif
public class MyController : InputDevice, IInputUpdateCallbackReceiver
{
    // �{�^��
    public ButtonControl button { get; private set; }

    // ������
    static MyController()
    {
        // �f�o�C�X�̃��C�A�E�g��o�^
        InputSystem.RegisterLayout<MyController>();
    }

    // �Z�b�g�A�b�v�������ɌĂяo�����
    protected override void FinishSetup()
    {
        base.FinishSetup();

        // �{�^�����擾
        button = GetChildControl<ButtonControl>("button");
    }

    public void OnUpdate()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        // �}�E�X�̍��{�^����������Ă���ꍇ�̓{�^����������Ԃɂ���
        var state = new MyControllerState
        {
            button = mouse.leftButton.isPressed ? (byte)1 : (byte)0
        };

        // ���͏�Ԃ��L���[�ɒǉ�
        InputSystem.QueueStateEvent(this, state);
    }
}