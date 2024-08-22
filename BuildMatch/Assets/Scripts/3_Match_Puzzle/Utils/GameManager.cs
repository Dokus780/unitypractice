using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ���� ���ھ�, ���� �¸�/�й� ����, UI ����

// TODO : 1. �¸� / �й� ���� ����
//           -> ���� �ð��� �� �Ǿ��� �� �й� ó�� �Ϸ�. �ٱ��ϰ� �� ä������ Ŭ����Ǵ� �����δ� �����ؾ� ��.
//        2. ���� ���� ���� �� ���� �ð� �� ��ǥ ����
public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // static reference;

    public GameObject warningUI; // 10�� ������ �� ��� UI
    private Image warningImage;
    public float warningSec; // ��� �ߴ� ���� ���� �ð� (���� 10��)

    public GameObject backgroundPanel; // grey background �¸�/�й� ȭ�� Ŭ���� �� ���� ���� �ȵǰ� 
    public GameObject clearPanel;
    public GameObject failedPanel;

    public int goal; // the amount of points you need to get to win.
    public int points; // �ִ� ���� 9������ ex) 999999999

    // ���� ��������
    public int stageNumber;

    // ���� �ð�
    public int min;
    public float sec;

    // ���� ���ĺ��� true�� ����Ǹ鼭 �ð� üũ
    // true�϶��� �ð��� ������
    [SerializeField]
    private bool isGameRunning = false;

    // ���� �ð��� ����Ǿ��� �� 
    public bool isStageEnded;

    public TMP_Text stageText;
    public TMP_Text pointsText;
    public TMP_Text timeText;

    // TODO : 1. ���߿� ���� bag ������Ʈ�� ���� �ʿ�
    public Sprite[] bagSprites;

    public GameObject bag1;
    private TMP_Text bag1Text;
    private int bag1CurrentCount;
    private int bag1GoalCount; // 1 stage 15
    public PotionType bag1Type;
    private bool bag1Check; // currentCount == GoalCount �Ǹ� check��

    public GameObject bag2;
    private TMP_Text bag2Text;
    private int bag2CurrentCount;
    private int bag2GoalCount;
    public PotionType bag2Type;
    private bool bag2Check;

    public GameObject bag3;
    private TMP_Text bag3Text;
    private int bag3CurrentCount;
    private int bag3GoalCount;
    public PotionType bag3Type;
    private bool bag3Check;

    public GameObject bag4;
    private TMP_Text bag4Text;
    private int bag4CurrentCount;
    private int bag4GoalCount;
    public PotionType bag4Type;
    private bool bag4Check;


    // ��� UI ���� �÷���
    private Color originWarningColor;
    private Color fullWarningColor;

    // ���â Scale ������
    private Vector3 firstResultScale;
    private Vector3 middleResultScale;
    private Vector3 lastResultScale;

    float checkTime = 0;

    private void Awake()
    {
        Instance = this;
        warningImage = warningUI.GetComponent<Image>();
        originWarningColor = warningImage.GetComponent<Image>().color;
        fullWarningColor = new Color(1, 1, 1, 1);
            
        firstResultScale = Vector3.zero;
        middleResultScale = new Vector3(1.2f, 1.2f, 1);
        lastResultScale = new Vector3(1, 1, 1);
    }

    private void Start()
    {
        stageText.text = "Stage " + stageNumber;
        SetUpBag();
    }

    private void SetUpBag()
    {
        //Sprite[] sprite_1 = Resources.LoadAll<Sprite>("Sprites/Puzzle Blocks Icon Pack/png/blockBlueDimond");
        //Debug.Log("sprite" + sprite_1[0]);
        bag1Text = bag1.GetComponentInChildren<TMP_Text>();
        bag1Type = PotionType.BlueBlock;
        bag1GoalCount = 15;
        bag1CurrentCount = 0;
        bag1Text.text = bag1CurrentCount.ToString() + " / " + bag1GoalCount.ToString();
        bag1Check = false;

        bag2Text = bag2.GetComponentInChildren<TMP_Text>();
        bag2Type = PotionType.GreenBlock;
        bag2GoalCount = 30;
        bag2CurrentCount = 0;
        bag2Text.text = bag2CurrentCount.ToString() + " / " + bag2GoalCount.ToString();
        bag2Check = false;

        bag3Text = bag3.GetComponentInChildren<TMP_Text>();
        bag3Type = PotionType.PinkBlock;
        bag3GoalCount = 35;
        bag3CurrentCount = 0;
        bag3Text.text = bag3CurrentCount.ToString() + " / " + bag3GoalCount.ToString();
        bag3Check = false;

        bag4Text = bag4.GetComponentInChildren<TMP_Text>();
        bag4Type = PotionType.RedBlock;
        bag4GoalCount = 40;
        bag4CurrentCount = 0;
        bag4Text.text = bag4CurrentCount.ToString() + " / " + bag4GoalCount.ToString();
        bag4Check = false;

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePoint();
        UpdateTime();
        UpdateBag();
        TestTime();
    }

    private void UpdatePoint()
    {
        pointsText.text = string.Format("{0:D9}", points);
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

        timeText.text = string.Format("{0:D2} : {1:D2}", min, (int)sec);
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
            WarningLeftTime();
        }
        else if (min == 0 && sec <= 0f)
        {
            StageFailed();
            return;
        }
    }

    private void UpdateBag()
    {
        if (bag1CurrentCount <= 0)
        {
            bag1CurrentCount = 0;
            bag1Text.color = Color.red;
        }

        if (bag2CurrentCount <= 0)
        {
            bag2CurrentCount = 0;
            bag2Text.color = Color.red;
        }

        if (bag3CurrentCount <= 0)
        {
            bag3CurrentCount = 0;
            bag3Text.color = Color.red;
        }

        if (bag4CurrentCount <= 0)
        {
            bag4CurrentCount = 0;
            bag4Text.color = Color.red;
        }

        bag1Text.text = bag1CurrentCount.ToString() + " / " + bag1GoalCount.ToString();
        bag2Text.text = bag2CurrentCount.ToString() + " / " + bag2GoalCount.ToString();
        bag3Text.text = bag3CurrentCount.ToString() + " / " + bag3GoalCount.ToString();
        bag4Text.text = bag4CurrentCount.ToString() + " / " + bag4GoalCount.ToString();
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

        bag2CurrentCount += _bag2AddCount;
        bag3CurrentCount += _bag3AddCount;
        bag4CurrentCount += _bag4AddCount;

        // TODO : 1. ���� �ð� ���� �ٱ��Ͽ� �ʿ��� �� ����� �� �¸�
        //if (points >= goal)
        //{
        //    // win game
        //    isGameEnded = true;

        //    // Display a victory screen.
        //    backgroundPanel.SetActive(true);
        //    clearPanel.SetActive(true);
        //    PotionBoard.Instance.potionParent.SetActive(false);
        //    isGameRunning = false;
        //    StartCoroutine(LerpClearPanelScale());
        //    return;
        //}

        if (bag1Check)
        {
            StageClear();
        }

    }

    private void WarningLeftTime()
    {
        timeText.color = Color.red;
        warningUI.SetActive(true);
        StartCoroutine(LerpWarningColor());
    }

    // 10�� �Ǹ� ��� UI ���������ϵ���(�÷��� a��(������) ����)
    private IEnumerator LerpWarningColor()
    {
        while (warningUI.GetComponent<Image>().color != fullWarningColor)
        {
            warningUI.GetComponent<Image>().color = Color.Lerp(originWarningColor, fullWarningColor, Mathf.PingPong(Time.time, 1));
            yield return null;
        }
    }

    // ���â ũ�� ����
    private IEnumerator LerpClearPanelScale()
    {
        bool firstCheck = false;
        bool lastCheck = false;
        float checkTime = 0;
        float lerpSpeed = 0.2f;

        while (!firstCheck || !lastCheck)
        {
            clearPanel.transform.localScale = Vector3.Lerp(firstResultScale, middleResultScale, Mathf.PingPong(checkTime += lerpSpeed, 1));
            Debug.Log(checkTime);
            if (clearPanel.transform.localScale.x >= middleResultScale.x - 0.1f)
            {
                firstCheck = true;
            }
            if (clearPanel.transform.localScale.x <= lastResultScale.x && firstCheck)
            {
                lastCheck = true;
            }
            yield return null;
        }
    }

    private IEnumerator LerpFailedPanelScale()
    {
        bool firstCheck = false;
        bool lastCheck = false;

        failedPanel.transform.localScale = Vector3.zero;

        while (!firstCheck || !lastCheck)
        {
            failedPanel.transform.localScale = Vector3.Lerp(firstResultScale, middleResultScale, Mathf.PingPong(Time.time, 1));
            Debug.Log(Mathf.PingPong(Time.time, 1));
            if (failedPanel.transform.localScale.x >= middleResultScale.x - 0.1f)
            {
                firstCheck = true;
            }
            if (failedPanel.transform.localScale.x <= lastResultScale.x && firstCheck)
            {
                lastCheck = true;
            }
            yield return null;
        }
    }

    private void StageClear()
    {
        isStageEnded = true;
        warningUI.SetActive(false);
        // Display a victory screen.
        backgroundPanel.SetActive(true);
        clearPanel.SetActive(true);
        PotionBoard.Instance.potionParent.SetActive(false);
        isGameRunning = false;
        StartCoroutine(LerpClearPanelScale());
    }

    private void StageFailed()
    {
        isStageEnded = true;
        warningUI.SetActive(false);
        backgroundPanel.SetActive(true);
        failedPanel.SetActive(true);
        PotionBoard.Instance.potionParent.SetActive(false);
        isGameRunning = false;
        StartCoroutine(LerpFailedPanelScale());
    }

    // ���� �ð� ���� �ٱ��Ͽ� �ʿ��� �� ����� �� �¸�
    // ���� ���â ��� ��ư �̰� ����� : HomeButton, RestartButton, NextButton, MapButton
    // TODO : 1. ��ư���� ���� �޼��� ������ ��
    private void WinGame()
    {
        //SceneManager.LoadScene("Main Menu");
        // string���� �� ���� �ְ� �ε��� �༭ ��� ���� ����
        SceneManager.LoadScene(0);
    }

    // ���� �ð��� �� ������ �й�
    private void LoseGame()
    {

        //SceneManager.LoadScene("Main Menu");
        // string���� �� ���� �ְ� �ε��� �༭ ��� ���� ����
        //SceneManager.LoadScene(0);
    }

    private void TestTime()
    {
        //float lerpSpeed = 0.2f;
        //Debug.Log(Vector3.Lerp(firstResultScale, middleResultScale, Mathf.PingPong(checkTime += lerpSpeed, 1)));
    }
}
