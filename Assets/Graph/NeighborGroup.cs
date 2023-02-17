using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborGroup : MonoBehaviour
{
    public HashSet<Vector2Int> firstLevel = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> secondLevel = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> thirdLevel = new HashSet<Vector2Int>();

    void Start()
    {
        //CreateLevels();
        //PrintLevel(thirdLevel);              
    }   

    public void CreateLevels()
    {
        CreateFirstLevel();
        CreateSecondLevel();
        CreateThirdLevel();
    }


    void CreateFirstLevel()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }
                else
                {
                    Vector2Int neighbor = new Vector2Int(x, y);
                    firstLevel.Add(neighbor);
                }
            }
        }
    }


    void CreateSecondLevel()
    {
        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2;  y <= 2; y++)
            {
                Vector2Int current = new Vector2Int(x, y);
                if (firstLevel.Contains(current)) { continue; }
                if (x == 0 && y == 0) { continue; }

                Vector2Int neighbor = new Vector2Int(x, y);
                secondLevel.Add(neighbor);
            }
        }
    }

    void CreateThirdLevel()
    {
        for (int x = -3; x <= 3; x++)
        {
            for (int y = -3; y <= 3; y++)
            {
                Vector2Int current = new Vector2Int(x, y);
                if (firstLevel.Contains(current) || secondLevel.Contains(current)) { continue; }
                if (x == 0 && y == 0) { continue; }

                Vector2Int neighbor = new Vector2Int(x, y);
                thirdLevel.Add(neighbor);
            }
        }
    }



    //DEBUG
    void PrintLevel(HashSet<Vector2Int> level)
    {
        foreach (var item in level)
        {
            Debug.Log(item);
        }
    }



}
