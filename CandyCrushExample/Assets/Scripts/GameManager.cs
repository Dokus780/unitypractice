using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// ���� ���ھ�, ���� �¸�/�й� ����, UI ����

// TODO : 1. UI ���� : ���� ���� -> 0,0 ����. ī�޶� y�� �÷��� �ذ���
//           -> 240716 �Ϸ�, ���� �θ� �������� ��ġ �����ϸ� �� ���� �� ����
//        2. ���� ���� �ð� ���� 00:00 ��, ��
//           -> 240717 �Ϸ�. ���� �� ��Ī�� �������� ������ �ð��� �������� ����, ���� �ð��� 0�� �Ǿ��� �� �й�
//        3. �¸� / �й� ���� ����
//           -> ���� �ð��� �� �Ǿ��� �� �й� ó�� �Ϸ�. �ٱ��ϰ� �� ä������ Ŭ����Ǵ� �����δ� �����ؾ� ��.
//        4. ���� ���� ���� �� ���� �ð� �� ��ǥ ����
public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // static reference;

    public GameObject backgroundPanel; // grey background �¸�/�й� ȭ�� Ŭ���� �� ���� ���� �ȵǰ� 
    public GameObject victoryPanel; 
    public GameObject losePanel;

    public int goal; // the amount of points you need to get to win.
    //public int moves; // the number of turns you can take
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

    // move, goal ���� ����
    //public TMP_Text movesText;
    //public TMP_Text goalText;
    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        stageText.text = "Stage " + stageNumber;
    }

    //public void Initialize(int _moves, int _goal)
    //{
    //    // move, goal ���� ����
    //    //moves = _moves;
    //    //goal = _goal;
    //}

    // Update is called once per frame
    void Update()
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
        else if (min == 0 && sec <= 0f)
        {
            // lose game
            isGameEnded = true;
            backgroundPanel.SetActive(true);
            losePanel.SetActive(true);
            PotionBoard.Instance.potionParent.SetActive(false);
            isGameRunning = false;
            return;
        }
    } 

    public void ProcessTurn(int _pointsToGain, bool _subtractMoves)
    {
        if (!isGameRunning)
        {
            isGameRunning = true;
        }

        points += _pointsToGain;

        // TODO : Ƚ���� ����
        //if (_subtractMoves)
        //{
        //    moves--;
        //}

        // TODO : ���� �ð� ���� �ٱ��Ͽ� �ʿ��� �� ����� �� �¸�
        if (points >= goal)
        {
            // win game
            isGameEnded = true;

            // Display a victory screen.
            backgroundPanel.SetActive(true);
            victoryPanel.SetActive(true);
            PotionBoard.Instance.potionParent.SetActive(false);
            isGameRunning = false;
            return;
        }
        // TODO : ���� �ð��� �� ������ �й�
        //        -> ���⼭ �ϸ� �ȵǼ� Update���� �й� ���� ó���ϵ��� ������
        //if (moves == 0)
        //if (min == 0 && sec == 0)
        //{
        //    Debug.Log("Lose");
        //    // lose game
        //    isGameEnded = true;
        //    backgroundPanel.SetActive(true);
        //    losePanel.SetActive(true);
        //    PotionBoard.Instance.potionParent.SetActive(false);
        //    isGameRunning = false;
        //    return;
        //}
    }

    // ���� �ð� ���� �ٱ��Ͽ� �ʿ��� �� ����� �� �¸�
    public void WinGame()
    {
        //SceneManager.LoadScene("Main Menu");
        // string���� �� ���� �ְ� �ε��� �༭ ��� ���� ����
        SceneManager.LoadScene(0);
    }

    // ���� �ð��� �� ������ �й�
    public void LoseGame()
    {
        //SceneManager.LoadScene("Main Menu");
        // string���� �� ���� �ְ� �ε��� �༭ ��� ���� ����
        SceneManager.LoadScene(0);
    }
}
