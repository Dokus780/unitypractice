using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageUIManager : MonoBehaviour
{
    public GameObject _ControllerUI = null;
    public GameObject _QuestUI = null;

    private void Awake()
    {
        _ControllerUI = GameObject.Find("Controller");
        // TODO : 1. UI ������, ���� ���� ���� ���� ¥��
        // ���� ��Ȱ��ȭ �� ����Ƽ���� ���� �־ �����
        //_QuestUI = GameObject.Find("Quest");
        //_QuestUI.SetActive(false);
    }
}
