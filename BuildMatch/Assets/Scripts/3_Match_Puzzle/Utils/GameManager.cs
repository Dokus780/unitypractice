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
    //public GameObject victoryPanel; 
    //public GameObject losePanel;
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
    public bool isGameEnded;

    public TMP_Text stageText;
    public TMP_Text pointsText;
    public TMP_Text timeText;

    // TODO : 1. ���߿� ���� bag ������Ʈ�� ���� �ʿ�
    public Sprite[] bagSprites;

    public GameObject bag1;
    private TMP_Text bag1Text;
    private int bag1CurrentCount;
    private int bag1GoalCount;
    public PotionType bag1Type;
    public GameObject bag2;
    private TMP_Text bag2Text;
    private int bag2CurrentCount;
    private int bag2GoalCount;
    public PotionType bag2Type;
    public GameObject bag3;
    private TMP_Text bag3Text;
    private int bag3CurrentCount;
    private int bag3GoalCount;
    public PotionType bag3Type;
    public GameObject bag4;
    private TMP_Text bag4Text;
    private int bag4CurrentCount;
    private int bag4GoalCount;
    public PotionType bag4Type;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        stageText.text = "Stage " + stageNumber;
        SetUpBag();
        warningImage = warningUI.GetComponent<Image>();
        Debug.Log(warningImage.color);
    }

    private void SetUpBag()
    {
        //Sprite[] sprite_1 = Resources.LoadAll<Sprite>("Sprites/Puzzle Blocks Icon Pack/png/blockBlueDimond");
        //Debug.Log("sprite" + sprite_1[0]);
        bag1Text = bag1.GetComponentInChildren<TMP_Text>();
        bag2Text = bag2.GetComponentInChildren<TMP_Text>();
        bag3Text = bag3.GetComponentInChildren<TMP_Text>();
        bag4Text = bag4.GetComponentInChildren<TMP_Text>();
        bag1Type = PotionType.BlueBlock;
        bag1GoalCount = 25;
        bag1CurrentCount = bag1GoalCount;
        bag2Type = PotionType.GreenBlock;
        bag2GoalCount = 30;
        bag2CurrentCount = bag2GoalCount;
        bag3Type = PotionType.PinkBlock;
        bag3GoalCount = 35;
        bag3CurrentCount = bag3GoalCount;
        bag4Type = PotionType.RedBlock;
        bag4GoalCount = 40;
        bag4CurrentCount = bag4GoalCount;

        bag1Text.text = bag1CurrentCount.ToString() + " / " + bag1GoalCount.ToString();
        bag2Text.text = bag2CurrentCount.ToString() + " / " + bag2GoalCount.ToString();
        bag3Text.text = bag3CurrentCount.ToString() + " / " + bag3GoalCount.ToString();
        bag4Text.text = bag4CurrentCount.ToString() + " / " + bag4GoalCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePoint();
        UpdateBag();
    }

    private void UpdatePoint()
    {
        pointsText.text = string.Format("{0:D9}", points);
        // move, goal ���� ����
        //movesText.text = "Moves: " + moves.ToString();
        //goalText.text = "Points: " + goal.ToString();
        // string.Format({0��° �Ű�����:ǥ���ڸ���}, {1��° �Ű�����:ǥ���ڸ���});
        // 00:30���� ǥ�õ�

        if (isGameRunning)
        {
            CheckRemainTime();
        }

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
        // TODO 1. ���� 10�ʿ��� 1�ʸ� �ش� �Լ� ������
        else if (min == 0 && sec <= warningSec + 1 && sec >= warningSec)
        {
            WarningLeftTime();
        }
        else if (min == 0 && sec <= 0f)
        {
            LoseGame();
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
    public void ProcessTurn(int _pointsToGain, bool _subtractMoves, int _bag1SubtractCount, int _bag2SubtractCount, int _bag3SubtractCount, int _bag4SubtractCount)
    {
        if (!isGameRunning)
        {
            isGameRunning = true;
        }

        points += _pointsToGain;

        bag1CurrentCount -= _bag1SubtractCount;
        bag2CurrentCount -= _bag2SubtractCount;
        bag3CurrentCount -= _bag3SubtractCount;
        bag4CurrentCount -= _bag4SubtractCount;

        // TODO : 1. ���� �ð� ���� �ٱ��Ͽ� �ʿ��� �� ����� �� �¸�
        if (points >= goal)
        {
            // win game
            isGameEnded = true;

            // Display a victory screen.
            backgroundPanel.SetActive(true);
            clearPanel.SetActive(true);
            PotionBoard.Instance.potionParent.SetActive(false);
            isGameRunning = false;
            return;
        }

    }

    private void WarningLeftTime()
    {
        timeText.color = Color.red;
        // TODO : 1. ���� ȿ�� �߰�
        warningUI.SetActive(true);
        float duration = 1f; // This will be your time in seconds.
        float smoothness = 0.2f; // This will determine the smoothness of the lerp. Smaller values are smoother. Really it's the time between updates.
        Color originColor = warningImage.GetComponent<Image>().color; // This is the state of the color in the current interpolation.
        Color currentColor = originColor;
        Color fullColor = new Color(1, 1, 1, 1);
        bool lerpColorDirection = true; // true�� �� ������������ ��������
       
        //IEnumerator LerpColor()
        //{
        //    float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        //    float increment = smoothness / duration; //The amount of change to apply.
        //    if (lerpColorDirection)
        //    {
        //        while (progress < 1)
        //        {
        //            currentColor = Color.Lerp(originColor, fullColor, progress);
        //            progress += increment;
        //            warningUI.GetComponent<Image>().color = currentColor;
        //            Debug.Log("currentColor : " + currentColor);
        //            Debug.Log($"progress : {progress}");
        //            yield return new WaitForSeconds(smoothness);
        //        }
        //        lerpColorDirection = false;
        //    } else
        //    {
        //        while (progress > 0)
        //        {
        //            currentColor = Color.Lerp(originColor, fullColor, progress);
        //            progress -= increment;
        //            warningUI.GetComponent<Image>().color = currentColor;
        //            Debug.Log("currentColor : " + currentColor);
        //            Debug.Log($"progress : {progress}");

        //            yield return new WaitForSeconds(smoothness);
        //        }
        //        lerpColorDirection = true;
        //    }
        //}

        if(lerpColorDirection)
        {
            currentColor = Color.Lerp(originColor, fullColor, Time.deltaTime * smoothness);
            warningUI.GetComponent<Image>().color = currentColor;
            Debug.Log(currentColor.a);
            if (currentColor.a == 1)
            {
                Debug.Log("1 ����");
                lerpColorDirection = false;
            }
        }

        //StartCoroutine(LerpColor());
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
        isGameEnded = true;
        warningUI.SetActive(false);
        backgroundPanel.SetActive(true);
        failedPanel.SetActive(true);
        PotionBoard.Instance.potionParent.SetActive(false);
        isGameRunning = false;

        //SceneManager.LoadScene("Main Menu");
        // string���� �� ���� �ְ� �ε��� �༭ ��� ���� ����
        //SceneManager.LoadScene(0);
    }
}
