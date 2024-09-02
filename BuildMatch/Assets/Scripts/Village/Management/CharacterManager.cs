using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour, IManager
{
    public VillageGameManager gameManager
    {
        get
        {
            return VillageGameManager.gameManager;
        }
    }

    // ���̽�ƽ ���⺤��
    public Vector3 inputVector { get; set; }

    // ĳ���� �ν��Ͻ� ������Ƽ
    public PlayerInstance player { get; set; }
}
