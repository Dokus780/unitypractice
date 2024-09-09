using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInstance : MonoBehaviour, INPC
{
    public PlayerInstance player { get; private set; }

    public CharacterManager characterManager { get; private set; }

    public FollowCamera _FollowCamera = null;

    [SerializeField] private GameObject _QuestBalloon { get; set; }

    [SerializeField] private GameObject _TempPlayer = null;

    private void Awake()
    {
        player = GetComponent<PlayerInstance>();
        _FollowCamera = GameObject.Find("FollowCamera").GetComponent<FollowCamera>();
        _QuestBalloon = GetComponentInChildren<SpriteRenderer>().gameObject;
    }

    private void Update()
    {

    }

    private void OnMouseDown()
    {
        // TODO : NPC ����Ʈ �ϼ�

        // 0. NPC�� �Ÿ� üũ �� ����� ������ ����
        // 1. ��Ʈ�ѷ� UI â ��Ȱ��ȭ
        // 2. ��ȭ UI â Ȱ��ȭ -> ����Ʈ ���¿� ���� �ٸ� ��ȭ ������??
        // 3. NPC �� ĳ���� Ȱ��ȭ & ����Ʈ��ǳ�� ��Ȱ��ȭ
        _TempPlayer.SetActive(true);
        _QuestBalloon.SetActive(false);
        // 4. �÷��̾� ĳ���� ��Ȱ��ȭ
        player.gameObject.SetActive(false);
        // 5. ī�޶� NPC���� ��ġ ���߱�
        // 6. ������ Ŭ�� �� ����ġ
        _FollowCamera.SwitchTransformCamera(_TempPlayer.transform);

        //_FollowCamera.SwitchTargetCamera(2);
    }
}
