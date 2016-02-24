using UnityEngine;
using System.Collections;

public class RisingLetter : MonoBehaviour {
    [SerializeField]
    TextMesh textMesh;
    public static float yValue = 0;
    public static float count = 0f;
	// Use this for initialization
	void Start () {
        count += 1;
        yValue = transform.position.y;
	}

    public void SetText(string s)
    {
        textMesh.text = s;
    }


	// Update is called once per frame
	void Update () {
        yValue +=  Time.deltaTime * 10f/count;
        transform.position = new Vector3(transform.position.x, yValue, transform.position.z);
        if (yValue > 20)
        {
            Destroy(this.gameObject);
            count--;
        }
	}
}
