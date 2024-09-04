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

    private void Awake()
    {
        _CharacterManager = VillageGameManager.GetManagerClass<CharacterManager>();
        _CharacterManager.player = this;

        _SoundManager = VillageGameManager.GetManagerClass<SoundManager>();

        playerMovement = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        // ���콺�� Ŭ���ؼ� �ν� �� ��ȭ
        // NPCInstance OnMouseDown���� NPC Ŭ�� �̺�Ʈ ó��
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;

        //if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 10.0f))
        //{
        //    Debug.Log(hit.transform.gameObject);
        //}
    }
}
