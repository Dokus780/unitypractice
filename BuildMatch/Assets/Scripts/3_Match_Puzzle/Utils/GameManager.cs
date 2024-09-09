using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ���� ���ھ�, ���� �¸�/�й� ����, UI ����

// TODO : 1. �������� ���� �������� �� ���� �ð� �� ��ǥ ����
//        
public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // static reference;

    private PotionBoard board;

    public GameObject warningUI; // 10�� ������ �� ��� UI
    private Image warningImage;
    public float warningSec; // ��� �ߴ� ���� ���� �ð� (���� 10��)

    public GameObject backgroundPanel; // grey background �¸�/�й� ȭ�� Ŭ���� �� ���� ���� �ȵǰ� 
    public GameObject clearPanel;
    public GameObject failedPanel;

    public int goal; // the amount of points you need to get to win.
    public int points; // �ִ� ���� 9������ ex) 999999999

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
    private Image bag1ClearImage;
    private int bag1CurrentCount;
    [SerializeField]
    private int bag1GoalCount; // 1 stage 15
    public PotionType bag1Type;
    private bool bag1Check; // currentCount == GoalCount �Ǹ� check��

    private Image[] bagImageList;

    // ��� UI ���� �÷���
    private Color originWarningColor;
    private Color fullWarningColor;

    // ���â Scale ������
    private Vector3 firstResultScale;
    private Vector3 middleResultScale;
    private Vector3 lastResultScale;

    private void Awake()
    {
        Instance = this;

        board = FindObjectOfType<PotionBoard>();

        warningImage = warningUI.GetComponent<Image>();
        originWarningColor = warningImage.GetComponent<Image>().color;
        fullWarningColor = new Color(1, 1, 1, 1);
            
        firstResultScale = Vector3.zero;
        middleResultScale = new Vector3(1.2f, 1.2f, 1);
        lastResultScale = new Vector3(1, 1, 1);
    }

    private void Start()
    {
        //stageText.text = "Stage " + stageNumber;
        SetUpBag();
    }

    private void SetUpBag()
    {
        //Sprite[] sprite_1 = Resources.LoadAll<Sprite>("Sprites/Puzzle Blocks Icon Pack/png/blockBlueDimond");
        //Debug.Log("sprite" + sprite_1[0]);
        bag1Text = bag1.GetComponentInChildren<TMP_Text>();
        bag1Type = PotionType.RedBlock;
        bag1GoalCount = 15;
        bag1CurrentCount = 0;
        bag1Text.text = bag1CurrentCount.ToString() + " / " + bag1GoalCount.ToString();
        bagImageList = bag1.GetComponentsInChildren<Image>();
        //foreach(Image image in bagImageList)
        //{
        //    Debug.Log(image.name);
        //}
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

        timeText.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);
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
    // TODO : 1. �ִϸ��̼����θ� ó��
    private IEnumerator LerpClearPanelScale()
    {
        bool firstCheck = false;
        bool lastCheck = false;
        float lerpSpeed = 0.1f;

        float elaspedTime = 0f;

        float duration = 0.5f;

        // 0���� 1.2���� Ŀ��
        while (!firstCheck)
        {
            float t = elaspedTime / duration;
            clearPanel.transform.localScale = Vector3.Lerp(firstResultScale, middleResultScale, t);

            elaspedTime += Time.deltaTime;

            if (clearPanel.transform.localScale.x >= middleResultScale.x - 0.1f)
            {
                firstCheck = true;
            }
            
            yield return null;
        }

        elaspedTime = 0f;

        // 1.2���� 1���� �پ��
        while (!lastCheck)
        {
            float t = elaspedTime / duration;
            clearPanel.transform.localScale = Vector3.Lerp(middleResultScale, lastResultScale, t);

            elaspedTime += Time.deltaTime;

            if (clearPanel.transform.localScale.x <= lastResultScale.x && firstCheck)
            {
                lastCheck = true;
            }

            yield return null;
        }

    }

    private IEnumerator LerpFailedPanelScale()
    {
        yield return new WaitWhile(()=>board.isProcessMoving);

        // ���� StageFailed ����, ������ �̵��� ������ active ���� false�� ���� 
        isStageEnded = true;
        warningUI.SetActive(false);
        backgroundPanel.SetActive(true);
        failedPanel.SetActive(true);
        PotionBoard.Instance.potionParent.SetActive(false);
        isGameRunning = false;

        bool firstCheck = false;
        bool lastCheck = false;

        float elaspedTime = 0f;

        float duration = 0.5f;

        // 0���� 1.2���� Ŀ��
        while (!firstCheck)
        {
            float t = elaspedTime / duration;
            failedPanel.transform.localScale = Vector3.Lerp(firstResultScale, middleResultScale, t);

            elaspedTime += Time.deltaTime;

            if (failedPanel.transform.localScale.x >= middleResultScale.x - 0.1f)
            {
                firstCheck = true;
            }

            yield return null;
        }

        elaspedTime = 0f;

        // 1.2���� 1���� �پ��
        while (!lastCheck)
        {
            float t = elaspedTime / duration;
            failedPanel.transform.localScale = Vector3.Lerp(middleResultScale, lastResultScale, t);

            elaspedTime += Time.deltaTime;

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

    // Ŭ���� ���� ����
    // TODO: 1. ���������� ���� ���� ����
    //       -> �޴� �ڿ�, ���, ������ ��    

        
    private void StageFailed()
    {
        StartCoroutine(LerpFailedPanelScale());

        // �Ʒ� �ּ� ���� �ڷ�ƾ���� �ű� -> ������ ���� �� �Ʒ� active ���� false 

        //isStageEnded = true;
        //warningUI.SetActive(false);
        //backgroundPanel.SetActive(true);
        //failedPanel.SetActive(true);
        //PotionBoard.Instance.potionParent.SetActive(false);
        //isGameRunning = false;

    }

    // ���� �ð� ���� �ٱ��Ͽ� �ʿ��� �� ����� �� �¸�
    // ���� ���â ��� ��ư �̰� ����� : HomeButton, RestartButton, NextButton, MapButton
    // TODO : 1. ��ư���� ���� �޼��� ������ ��
    public void GoPuzzleScene()
    {
        SceneManager.LoadScene("PuzzleScene");
        // string���� �� ���� �ְ� �ε��� �༭ ��� ���� ����
        //SceneManager.LoadScene(0);
    }

    // ���� �ð��� �� ������ �й�
    public void GoVillageScene()
    {

        SceneManager.LoadScene("VillageScene");
        // string���� �� ���� �ְ� �ε��� �༭ ��� ���� ����
        //SceneManager.LoadScene(1);
    }
}
