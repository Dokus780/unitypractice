using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    private PlayerInstance _PlayerInstance = null;

    // TODO : 1. ���� ���� ¥�� ó���ϱ�
    private SoundManager _SoundManager = null;

    private StageManager _StageManager = null;

    private LoadManager _LoadManager = null;

    private void Awake()
    {
        _PlayerInstance = GameManager.GetManagerClass<CharacterManager>().player;
        _SoundManager = GameManager.GetManagerClass<SoundManager>();
        _StageManager = GameManager.GetManagerClass<StageManager>();
        _LoadManager = GameManager.GetManagerClass<LoadManager>();
    }

    #region VillageScene
    // �ʹ�ư ������ �������� ���� ������ �̵�
    public void OnMapButtonClicked()
    {
        _SoundManager.PlayUIClickSound(Vector3.zero);
        _LoadManager.LoadScene(LoadManager.SceneName.StageScene);
    }

    // Ȩ��ư ������ ������ ������ �̵�
    public void OnHomeButtonClicked()
    {
        _SoundManager.PlayUIClickSound(Vector3.zero);
        _LoadManager.LoadScene(LoadManager.SceneName.VillageScene);
    }

    // �ӽ÷� NPC Ŭ�� �� �ٽ� ����ġ�� �ǵ��ư���
    public void ReturnOriginCamera()
    {
        _PlayerInstance.isQuestEnd = true;
    }
    #endregion

    #region StageScene
    public void OnStage1ButtonClicked()
    {
        _SoundManager.PlayBackgroundSound(Vector3.zero);
        _StageManager.stageNumber = 1;
        _LoadManager.LoadScene(LoadManager.SceneName.PuzzleScene);
    }

    public void OnStage2ButtonClicked()
    {
        // TODO : 1. �������� �Ŵ������� ����
        _SoundManager.PlayBackgroundSound(Vector3.zero);

    }
    #endregion
}
