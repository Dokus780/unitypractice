using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������� ������

// 3��ġ ��ǥ ������
//ID	    StageLevel	Priority_ID
//a10001    1	
//a20001	2	        a10001
//a30001	3	        a20001
//a40001	4	        a30001
//a50001	5	        a40001

// 3��ġ ��ǥ �䱸 ������
//ID	    Content	        ClearNum
//a10001	���� �� ����	15
//a20001	���� �� ����	15
//a20001	��Ȳ �� ����	15
//a20001	��� �� ����	15
//a30001	�ʷ� �� ����	20
//a30001	�Ķ� �� ����	20
//a30001	���� �� ����	20
//a40001	��Ȳ �� ����	10
//a40001	��� �� ����	10
//a40001	�ʷ� �� ����	10
//a40001	�Ķ� �� ����	10
//a40001	���� �� ����	10
//a50001	���� �� ����	15
//a50001	��Ȳ �� ����	15
//a50001	��� �� ����	15
//a50001	�ʷ� �� ����	15
//a50001	�Ķ� �� ����	15
//a50001	���� �� ����	15
//a50001	��ũ �� ����	15

// 3��ġ ��ǥ ���� ������
//    ID	RewardInfo	    RewardNum
//a10001	2�������� �ر�	
//a10001	���      	    100
//a20001	3�������� �ر�	
//a20001	���	            200
//a30001	4�������� �ر�	
//a30001	���	            300
//a40001	5�������� �ر�	
//a40001	���	            400
//a50001	���	            500

[Serializable]
public class StageData : ScriptableObject
{
    public string id;
    public int level;
    public string priority_id;
    public Bag[] objectBagList;
    public bool rewardCheckStage;
    public int rewardGold;
}