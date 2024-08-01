using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ڳ� SDK namespace
using BackEnd;

public class BackendManager : MonoBehaviour
{
    private void Start()
    {
        var bro = Backend.Initialize(); // �ڳ� �ʱ�ȭ

        //�ڳ� �ʱ�ȭ�� ���� ���䰪
        if (bro.IsSuccess())
        {
            Debug.Log($"�ʱ�ȭ ����: {bro}"); // statusCode 204 Success
        }
        else
        {
            Debug.LogError($"�ʱ�ȭ ����: {bro}"); // statusCode  4XX Error
        }
        
    }
}
