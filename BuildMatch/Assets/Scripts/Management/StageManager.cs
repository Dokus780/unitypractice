using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : 1. PuzzleManager�� �ִ� Stage ���� �Űܿ���
//        2. �������� ���� -> �ӽ� DB���� ������ �����ͼ� �ش� ���������� ����

public class StageManager : MonoBehaviour
{
    public static StageManager Instance; // static reference;

    // ���� ��������
    public int stageNumber;

    public void StageClearReward()
    {
        // ���� ������ x, y position ��
        // (-238.8, 122), (-79.6, 122), (79.6, 122), (238.8, 122)
        // (-238.8, -59), (-79.6, -59), (79.6, -59), (238.8, -59)
    }
}
