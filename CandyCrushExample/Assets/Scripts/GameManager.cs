using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// ���� ���ھ�, ���� �¸�/�й� ����, UI ����

// TODO : 1. UI ����
//        2. ���� ���� �ð� ����
//        3. �¸� / �й� ���� ����
//        4. ���� ����
public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // static reference;

    public GameObject backgroundPanel; // grey background 
    public GameObject victoryPanel; // �¸�/�й� ȭ�� Ŭ���� �� ���� ���� �ȵǰ� 
    public GameObject losePanel;

    public int goal; // the amount of points you need to get to win.
    public int moves; // the number of turns you can take
    public int points; // the crrent points you have earned.

    // ���� �ð�


    // ���� �ð��� ����Ǿ��� �� 
    public bool isGameEnded;

    public TMP_Text stageText;
    public TMP_Text pointsText;
    public TMP_Text timeText;

    // move, goal ���� ����
    public TMP_Text movesText;
    public TMP_Text goalText;
    

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize(int _moves, int _goal)
    {
        // move, goal ���� ����
        moves = _moves;
        goal = _goal;
    }

    // Update is called once per frame
    void Update()
    {
        pointsText.text = "Points: " + points.ToString();
        // move, goal ���� ����
        movesText.text = "Moves: " + moves.ToString();
        goalText.text = "Points: " + goal.ToString();
    }

    public void ProcessTurn(int _pointsToGain, bool _subtractMoves)
    {
        points += _pointsToGain;

        // TODO : Ƚ���� ����
        if (_subtractMoves)
        {
            moves--;
        }

        // TODO : ���� �ð� ���� �ٱ��Ͽ� �ʿ��� �� ����� �� �¸�
        if (points >= goal)
        {
            // win game
            isGameEnded = true;

            // Display a victory screen.
            backgroundPanel.SetActive(true);
            victoryPanel.SetActive(true);
            PotionBoard.Instance.potionParent.SetActive(false);
            return;
        }
        // TODO : ���� �ð��� �� ������ �й�
        if (moves == 0)
        {
            // lose game
            isGameEnded = true;
            backgroundPanel.SetActive(true);
            losePanel.SetActive(true);
            PotionBoard.Instance.potionParent.SetActive(false);

            return;
        }
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
