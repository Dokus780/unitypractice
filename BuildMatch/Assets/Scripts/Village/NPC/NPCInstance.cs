using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInstance : MonoBehaviour, INPC
{
    public PlayerInstance _PlayerInstance = null;

    public FollowCamera _FollowCamera = null;

    public VillageUIManager _VillageUIManager = null;

    [SerializeField] private GameObject _QuestBalloon { get; set; }

    [SerializeField] private GameObject _TempPlayer = null;



    private void Awake()
    {
        _PlayerInstance = GameManager.GetManagerClass<CharacterManager>().player;
        _FollowCamera = GameObject.Find("FollowCamera").GetComponent<FollowCamera>();
        _QuestBalloon = GetComponentInChildren<SpriteRenderer>().gameObject;
        _VillageUIManager = GameObject.Find("VillageUIManager").GetComponent<VillageUIManager>();
    }

    private void Update()
    {

    }

    private void OnMouseDown()
    {
        // TODO : NPC ����Ʈ �ϼ�(�Ÿ� üũ, ��ȭ UI)

        // 0. NPC�� �Ÿ� üũ �� ����� ������ ����
        // 1. ��Ʈ�ѷ� UI â ��Ȱ��ȭ
        _VillageUIManager._ControllerUI.SetActive(false);
        // 2. ��ȭ UI â Ȱ��ȭ -> ����Ʈ ���¿� ���� �ٸ� ��ȭ ������??
        _VillageUIManager._QuestUI.SetActive(true);
        // 3. NPC �� ĳ���� Ȱ��ȭ & ����Ʈ��ǳ�� ��Ȱ��ȭ
        _TempPlayer.SetActive(true);
        _QuestBalloon.SetActive(false);
        // 4. �÷��̾� ĳ���� ��Ȱ��ȭ
        _PlayerInstance.gameObject.SetActive(false);
        // 5. ī�޶� NPC���� ��ġ ���߱�(ī�޶� Ȯ�� �ʿ�)
        _FollowCamera.SwitchTransformNPCCamera(_TempPlayer.transform);
        // 6. ������ Ŭ�� �� ����ġ
        StartCoroutine(ReturnCamera());

    }

    private IEnumerator ReturnCamera()
    {
        yield return new WaitUntil(() => _PlayerInstance.isQuestEnd);

        _FollowCamera.SwitchTransformPlayerCamera(_PlayerInstance.transform);

        _PlayerInstance.gameObject.SetActive(true);
        _QuestBalloon.SetActive(true);

        _TempPlayer.SetActive(false);

        _VillageUIManager._QuestUI.SetActive(false);

        _VillageUIManager._ControllerUI.SetActive(true);

        _PlayerInstance.isQuestEnd = false;
    }
}
