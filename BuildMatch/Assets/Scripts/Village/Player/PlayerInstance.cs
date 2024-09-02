using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ٸ� �ڵ����� �־���
[RequireComponent(typeof(CharacterController))]
public class PlayerInstance : MonoBehaviour
{
    // CharacterManager ���� ����
    private CharacterManager _CharacterManager = null;

    // PlayerMovement�� ���� ������Ƽ
    public PlayerMovement playerMovement { get; private set; }

    // CharacterController ������Ƽ
    public CharacterController controller { get; private set; }

    private void Awake()
    {
        _CharacterManager = VillageGameManager.GetManagerClass<CharacterManager>();
        _CharacterManager.player = this;

        playerMovement = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();
    }
}
