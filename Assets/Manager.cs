using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Manager : MonoBehaviour {
    public Tile tilePrefab;
    public string[] words;
    private List<char> letters = new List<char>();
    public Tile[] tiles;

    float radius = 8;

    // Use this for initialization
    void Start()
    {
        //Get all the letters for the tiles from the array of words
        foreach (string s in words)
        {
            StringReader sr = new StringReader(s);
            char[] b = new char[s.Length];
            sr.Read(b, 0, s.Length);

            for (int i = 0; i < b.Length; i++)
            {
                if (!letters.Contains(b[i]))
                {
                    letters.Add(b[i]);
                }
            }
        }

        //Create one tile for each letter
        tiles = new Tile[letters.Count];
        int j = 0;
        foreach (char c in letters)
        {
            tiles[j] = Instantiate(tilePrefab, new Vector3(0, 0, 0), Quaternion.identity) as Tile;
            tiles[j].SetLetter(c);

            j++;
        }

        float arcAngleIncrement = 2 * Mathf.PI / letters.Count;
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].SetHomePosition(new Vector3(radius * Mathf.Cos(i * arcAngleIncrement), 1, radius * Mathf.Sin(i * arcAngleIncrement)));
        }

        Debug.Log("start");
    }


    int freed = 0;
    float delay = 0.1f;
    float timeElapesed = 0;
    void Update()
    {
        if (freed < tiles.Length)
        {
            timeElapesed += Time.deltaTime;
            if (timeElapesed >= delay)
            {
                timeElapesed = 0;
                tiles[freed++].SetFree();
                Debug.Log("freed" + freed.ToString());
            }
        }
    }
}
