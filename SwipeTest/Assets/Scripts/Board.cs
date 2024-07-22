using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait,
    move
}

public class Board : MonoBehaviour
{
    // currentState�� move �����϶��� ���콺 Ŭ�� ������
    public GameState currentState = GameState.move;

    public int width;
    public int height;
    // ���� ������ Dot�� ����߸��� ���� offSet��ŭ ���� �÷��� ���� 
    public int offSet;
    public GameObject tilePrefab;
    public GameObject[] dots;
    public GameObject destroyEffect;
    private BackgroundTile[,] allTiles;
    public GameObject[,] allDots;
    private FindMatches findMatches;


    // Start is called before the first frame update
    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();
        allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        SetUp();
    }

    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j + offSet);
                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity);
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "( " + i + ". " + j + " )";

                // �ٽ� �����ϴ� Ƚ�� 100���� �Ϸ��� ��
                int maxIterations = 0;

                int dotToUse = Random.Range(0, dots.Length);

                while(MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100)
                {
                    dotToUse = Random.Range(0, dots.Length);
                }
                maxIterations = 0;

                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.GetComponent<Dot>().row = j;
                dot.GetComponent<Dot>().column = i;

                dot.transform.parent = this.transform;
                dot.name = "( " + i + ". " + j + " )";
                allDots[i, j] = dot;
            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject piece)
    {
        // column�� 1���� ũ�� ���ʿ� �÷��� �ΰ� �ִٴ� �ű� ������ �������� �ѹ� ���� üũ�ϰ� �� �ѹ� �������� ���� üũ
        // row�� �������� 1���� ũ�� �Ʒ��� �ο찡 �ΰ� �ִٴ� ���̱� ������ �Ʒ��� �ѹ� ���� üũ�ϰ� �ѹ� �� �Ʒ��� üũ
        // ���� �ÿ� �̸� ��Ī�� �Ͼ�� �ʵ��� �ϱ� ����
        if (column > 1 && row > 1)
        {
            if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
            {
                return true;
            }

            if (allDots[column, row -1].tag == piece.tag && allDots[column, row-2].tag == piece.tag)
            {
                return true;
            }
        } 
        // �� ���Ǹ� ������ 0, 1 �÷��� 0, 1 �ο쿡�� ��Ī �߻���. �ش� �÷� �ο쿡 ���� ����
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }

            if (column > 1)
            {
                if (allDots[column - 1, row].tag == piece.tag && allDots[column -2, row].tag == piece.tag)
                {
                    return true;
                }
            }

        }

        return false;
    }

    // isMatched true�� üũ�� Dot Destroy
    private void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<Dot>().isMatched)
        {
            findMatches.currentMatches.Remove(allDots[column, row]);
            GameObject particle = Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity);
            // Instantiate�� �ϸ� unity���� �޸� �������� �ʾƼ� GameObject�� �޾Ƽ� Destroy �ʿ�
            // Destroy 2��° �Ű� ������ �ʸ� �ָ� �ش� �ð� ������ Destroy��
            Destroy(particle, .5f);
            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        for (int i=0; i<width; i++)
        {
            for (int j=0; j<height; j++)
            {
                if (allDots[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(DecreaseRowCo());
    }

    // coroutine���� �� �ڸ� row�� ����
    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for (int i=0; i<width; i++)
        {
            for (int j=0; j<height; j++)
            {
                if (allDots[i, j] == null)
                {
                    nullCount++;
                } else if(nullCount > 0)
                {
                    allDots[i, j].GetComponent<Dot>().row -= nullCount;
                    allDots[i, j] = null;
                }
                
            }
            nullCount = 0;
        }

        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    // ��Ī �� ���� ä���
    // 1. Dot ���� ����

    private void RefillBoard()
    {
        for (int i=0; i<width; i++)
        {
            for (int j=0; j<height; j++)
            {
                if (allDots[i,j] == null)
                {
                    // �����Ǵ� �� y�� offSet��ŭ ����
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int dotToUse = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    allDots[i, j] = piece;
                    piece.GetComponent<Dot>().row = j;
                    piece.GetComponent<Dot>().column = i;
                }
            }
        }
    }

    // 2. ���� �� ��Ī üũ
    private bool MatchesOnBoard()
    {
        for (int i=0; i<width; i++)
        {
            for (int j=0; j<height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        while(MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }

        // ���� ä�� ���� pause �ְ� move ���·� ����
        // // ��� ���� �ٽ� ���ڸ��� ���ư� ��(0.5�� ��ٸ� ��) move ���·� ����
        yield return new WaitForSeconds(.5f);
        currentState = GameState.move;
    }
}
