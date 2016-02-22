using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class Manager : MonoBehaviour {
    [SerializeField]
    private Tile tilePrefab;
    [SerializeField]
    private string[] words;
    private List<char> letters = new List<char>();
    [SerializeField]
    private Tile[] tiles;
    [SerializeField]
    private Text textWordsToSpell;

    private float radius = 8;


    void Start()
    {
        //Display the words to spell on the UI
        textWordsToSpell.text = "";
        foreach (string s in words)
        {
            textWordsToSpell.text += s + "\n";
        }

        //Get all the letters for the tiles
        foreach (string s in words)
        {
            StringReader sr = new StringReader(s);
            char[] b = new char[s.Length];
            sr.Read(b, 0, s.Length);

            for (int i = 0; i < b.Length; i++)
            {
                if (true)
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

        //Place tiles in a circle around the playing board
        float arcAngleIncrement = 2 * Mathf.PI / letters.Count;
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].SetHomePosition(new Vector3(radius * Mathf.Cos(i * arcAngleIncrement), 1, radius * Mathf.Sin(i * arcAngleIncrement)));
        }
    }


    int freed = 0; //free the tiles one at a time to move to home position
    float delay = 0.2f;
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
            }
        }
    }

    public string[] Words()
    {
        return words;
    }
}
