using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConnectedDevicesExample : MonoBehaviour
{
    // ������
    private void Awake()
    {
        // �f�o�C�X�̕ύX���Ď�
        InputSystem.onDeviceChange += OnDeviceChange;

        // �ŏ��ɐڑ�����Ă���f�o�C�X�ꗗ�����O�o��
        PrintAllDevices();
    }

    // �I������
    private void OnDestroy()
    {
        // �f�o�C�X�ύX�̊Ď�����
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    // �S�Ẵf�o�C�X�����O�o��
    private void PrintAllDevices()
    {
        // �f�o�C�X�ꗗ���擾
        var devices = InputSystem.devices;

        // ���݂̃f�o�C�X�ꗗ�����O�o��
        var sb = new StringBuilder();
        sb.AppendLine("���ݐڑ�����Ă���f�o�C�X�ꗗ");

        for (var i = 0; i < devices.Count; i++)
        {
            sb.AppendLine($" - {devices[i]}");
        }

        print(sb);
    }

    // �f�o�C�X�̕ύX�����m�������̏���
    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                print($"�f�o�C�X {device} ���ڑ�����܂����B");
                break;

            case InputDeviceChange.Disconnected:
                print($"�f�o�C�X {device} ���ؒf����܂����B");
                break;

            case InputDeviceChange.Reconnected:
                print($"�f�o�C�X {device} ���Đڑ�����܂����B");
                break;

            default:
                // �ڑ���ؒf�ȊO�̕ύX�͖���
                return;
        }

        // �ڑ���ؒf���������ꍇ�́A�S�Ẵf�o�C�X���Ăу��O�o��
        PrintAllDevices();
    }
}