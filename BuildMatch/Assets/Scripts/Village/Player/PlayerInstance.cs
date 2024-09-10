using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ٸ� �ڵ����� �־���
[RequireComponent(typeof(CharacterController))]
public class PlayerInstance : MonoBehaviour
{
    // CharacterManager ���� ����
    private CharacterManager _CharacterManager = null;

    // SoundManager ���� ����
    private SoundManager _SoundManager = null;

    // PlayerMovement�� ���� ������Ƽ
    public PlayerMovement playerMovement { get; private set; }

    // CharacterController ������Ƽ
    public CharacterController controller { get; private set; }

    // ����Ʈ ��ȭ ���� ����
    public bool isQuestEnd { get; set; } = false;

    private void Awake()
    {
        _CharacterManager = GameManager.GetManagerClass<CharacterManager>();
        _CharacterManager.player = this;

        _SoundManager = GameManager.GetManagerClass<SoundManager>();

        playerMovement = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {

    }
}
