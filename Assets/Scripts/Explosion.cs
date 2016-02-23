using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{

    private float lifeTime = 0;
    public float lifeSpan = 5;

    // Update is called once per frame
    void FixedUpdate()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime > lifeSpan)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}