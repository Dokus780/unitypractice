using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ���� ���ھ�, ���� �¸�/�й� ����, UI ����

// TODO : 1. �������� ���� �������� �� ���� �ð� �� ��ǥ ����
//        
public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance; // static reference;

    private PuzzleUIManager _PuzzleUIManager = null;

    private PotionBoard board;

    public float warningSec; // ��� �ߴ� ���� ���� �ð� (���� 10��)

    //public int goal;
    public int points; // �ִ� ���� 9������ ex) 999999999

    // ���� �ð�
    public int min;
    public float sec;

    // ���� ���ĺ��� true�� ����Ǹ鼭 �ð� üũ
    // true�϶��� �ð��� ������
    public bool isGameRunning = false;

    // ���� �ð��� ����Ǿ��� �� 
    public bool isStageEnded;

    // TODO : 1. ���߿� ���� bag ������Ʈ�� ���� �ʿ�
    public Sprite[] bagSprites;

    public GameObject bag1;
    private TMP_Text bag1Text;
    private Image bag1ClearImage;
    private int bag1CurrentCount;
    [SerializeField]
    private int bag1GoalCount; // 1 stage 15
    public PotionType bag1Type;
    private bool bag1Check; // currentCount == GoalCount �Ǹ� check��

    private Image[] bagImageList;

    private void Awake()
    {
        Instance = this;

        board = FindObjectOfType<PotionBoard>();
        _PuzzleUIManager = GetComponentInChildren<PuzzleUIManager>();
    }

    private void Start()
    {
        SetUpBag();
    }

    private void SetUpBag()
    {
        bag1Text = bag1.GetComponentInChildren<TMP_Text>();
        bag1Type = PotionType.RedBlock;
        bag1GoalCount = 15;
        bag1CurrentCount = 0;
        bag1Text.text = bag1CurrentCount.ToString() + " / " + bag1GoalCount.ToString();
        bagImageList = bag1.GetComponentsInChildren<Image>();
        bag1ClearImage = bagImageList[2];
        bag1ClearImage.gameObject.SetActive(false);
        bag1Check = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePoint();
        UpdateTime();
        UpdateBag();
    }

    private void UpdatePoint()
    {
        _PuzzleUIManager.pointsText.text = string.Format("{0:D9}", points);
        // move, goal ���� ����
    }

    private void UpdateTime()
    {
        if (isGameRunning)
        {
            CheckRemainTime();
        }

        // string.Format({0��° �Ű�����:ǥ���ڸ���}, {1��° �Ű�����:ǥ���ڸ���});
        // 00:30���� ǥ�õ�

        _PuzzleUIManager.timeText.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);
    }

    private void CheckRemainTime()
    {
        sec -= Time.deltaTime;

        if (min != 0 && sec <= 0f)
        {
            min -= 1;
            sec = 59f;
        }
        else if (min == 0 && sec <= warningSec + 1 && sec >= warningSec)
        {
            _PuzzleUIManager.WarningLeftTime();
        }
        else if (min == 0 && sec <= 0f)
        {
            sec = 0;
            StageFailed();
            return;
        }
    }

    private void UpdateBag()
    {
        bag1Text.text = bag1CurrentCount.ToString() + "/" + bag1GoalCount.ToString();

        if (bag1Check)
        {
            bag1Text.gameObject.SetActive(false);
            bag1ClearImage.gameObject.SetActive(true);
        }
    }

    // TODO : 1. �Ű����� _subtractMoves ���� -> �������� Ƚ�� -�� ���������� �� �߾��� 
    public void ProcessTurn(int _pointsToGain, bool _subtractMoves, int _bag1AddCount, int _bag2AddCount, int _bag3AddCount, int _bag4AddCount)
    {
        if (!isGameRunning)
        {
            isGameRunning = true;
        }

        points += _pointsToGain;


        bag1CurrentCount += _bag1AddCount;
        if (bag1CurrentCount >= bag1GoalCount)
        {
            bag1CurrentCount = bag1GoalCount;
            bag1Check = true;
        }

        // TODO : 1. �������� �� ��� ���� ���� üũ �Լ� �����(bag1�� üũ���� ����)
        if (bag1Check)
        {
            StageClear();
        }

    }

    private void StageClear()
    {
        StartCoroutine(_PuzzleUIManager.LerpClearPanelScale());
    }

    // Ŭ���� ���� ����
    // TODO: 1. ���������� ���� ���� ����
    //       -> �޴� �ڿ�, ���, ������ ��    

        
    private void StageFailed()
    {
        StartCoroutine(_PuzzleUIManager.LerpFailedPanelScale());
    }
}
