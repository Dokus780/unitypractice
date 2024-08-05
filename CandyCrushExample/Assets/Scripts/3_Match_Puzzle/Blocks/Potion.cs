using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// Potion -> Block
// �̸� ���� �� �޸� ���� �߻�

// TODO : 1. Potion -> Block �����丵
public class Potion : MonoBehaviour
{
    public PotionType potionType;

    // ��ǥ�� �ǹ��ϴ°� �ƴ϶� ��°��
    public int xIndex;
    public int yIndex;

    // TODO : 1. PotionBoard�� �ش� ���� �����;� ��(����� �ӽ÷� unity GUI�� 3 ����(7x7�̴ϱ� 3��)
    //        2. �������� �����ϸ鼭 PotionBoard�� ���⼭�� spacingX, Y �� Manager�� ����
    public int spacingX = 3;
    public int spacingY = 3;

    // ���� �����ϴ��� -> ����ϵ��� �ٲٴ���
    // isMatched üũ�Ǹ� ������ �������� ����
    public bool isMatched;

    // TODO : 1. �ش� ���� ���ҵǸ� ��Ī�� �Ǵ��� ���� (true�� �߱�, ���Ʒ��� ��¦ ������ ����)
    //public bool isMatchable;

    private Vector2 currentPos; // firstTouchPosition
    private Vector2 targetPos; // finalTouchPosition
    public bool currentSwipeable;
    public float swipeAngle = 0;
    public float swipeResist = 0.2f;

    public bool isMoving;

    // Ŭ������ �� �̹��� �����ϱ� ���� Sprite ����
    [SerializeField]
    private Sprite[] sprites = new Sprite[2];

    // Ư���� üũ
    //public bool isBomb;
    //public bool isDrillVertical;
    //public bool isDrillVertical;
    //public bool isDrillVertical;  
    //public bool isDrillVertical;

    public Potion(int _x, int _y)
    {
        xIndex = _x;
        yIndex = _y;
        currentSwipeable = false;
        isMoving = false;
    }

    public void SetIndicies(int _x, int _y)
    {
        xIndex = _x;
        yIndex = _y;
    }

    public void OnMouseDown()
    {
        if (!isMoving)
        {
            currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public void OnMouseDrag()
    {
        if (!isMoving)
        {
            if (sprites.Length == 2)
            {
                GetComponent<SpriteRenderer>().sprite = sprites[1];
            }
        }
    }

    public void OnMouseUp()
    {
        if (!isMoving)
        {
            if (sprites.Length == 2)
            {
                GetComponent<SpriteRenderer>().sprite = sprites[0];
            }
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentSwipeable = CalculateAngle();
        }
    }

    bool CalculateAngle()
    {
        // swipeResist �̻��� �Է��� ���� ���� ����
        // swipeResist : 1f = ���� ��ü �Ÿ� 0.5f = ���� ���� �Ÿ�
        // swipeResist : 0.2f�� ��������.
        if (Mathf.Abs(targetPos.y - currentPos.y) > swipeResist || Mathf.Abs(targetPos.x - currentPos.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(targetPos.y - currentPos.y, targetPos.x - currentPos.x) * 180 / Mathf.PI;
        }

        return Mathf.Abs(targetPos.y - currentPos.y) > swipeResist || Mathf.Abs(targetPos.x - currentPos.x) > swipeResist;
    }

    // �������� Ŭ���� �� ���� �߻��ߴµ� ���� ���� ��ã��?
    // TODO : 1. ���� & ���� �ڷ�ƾ ���� ���� Ŭ����Ǹ� ���⼭ ������
    //        2. ���� �߰��� �����ϸ� �ǹ���
    public void MoveToTarget(Vector2 _targetPos)
    {
        //Debug.Log("move : [" + transform.position.x + ", " + transform.position.y + "] -> [" + _targetPos.x + ", " + _targetPos.y + "]");
        StartCoroutine(MoveCoroutine(_targetPos));
    }

    // TODO : 1. �� �ٲ�� �ð� �ڿ������� ����
    //        2. ���� �� �����Ǿ� �� �ڸ��� �������� �͵� ���⼭ ó���ϴµ� ���� �޼��� ������ �ҵ�
    private IEnumerator MoveCoroutine(Vector2 _targetPos)
    {
        isMoving = true;

        Vector2 startPosition = transform.position;

        float distance = Vector2.Distance(startPosition, _targetPos);
        // ���� �ִϸ��̼� ���� �ð���
        // �ӷ� = �Ÿ� / 0.1f * distance �Ͽ� �ӷ� �����ϰ�
        // ������ �ð� 0.2f���µ� ������ 0.06f * distance�� ����
        float duration = 0.06f * distance;

        float elaspedTime = 0f;

        // �����̴� �ӵ� �����ϰ� ���⼭
        while (elaspedTime < duration)
        {
            float t = elaspedTime / duration;

            //Debug.Log("potion : " + this);
            //Debug.Log("time : " + t);

            transform.position = Vector2.Lerp(startPosition, _targetPos, t);

            elaspedTime += Time.deltaTime;

            yield return null;
            //yield return new WaitForSeconds(0.01f);
        }

        transform.position = _targetPos;
        isMoving = false;
    }
}

// TODO : 1. Ư�� ��
public enum PotionType
{
    // �⺻ ��
    BlueBlock,  
    GreenBlock,
    OrangeBlock,
    PinkBlock,
    PurpleBlock,
    RedBlock,
    YellowBlock,
    // Ư�� ��
    Bomb, // ��ź
    DrillVertical, // �帱 ����
    DrillHorizontal, // �帱 ����
    //Pick, // ���
    PickLeft, // ��� ���밢(���� �����)
    PickRight, // ��� �밢(������ �����)
    Prism, // ������
    
}