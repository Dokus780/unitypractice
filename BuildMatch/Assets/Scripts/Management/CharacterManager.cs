using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour, IManager
{
    public GameManager gameManager
    {
        get
        {
            return GameManager.gameManager;
        }
    }

    // ���̽�ƽ ���⺤��
    public Vector3 inputVector { get; set; }

    // ĳ���� �ν��Ͻ� ������Ƽ
    public PlayerInstance player { get; set; }
}
