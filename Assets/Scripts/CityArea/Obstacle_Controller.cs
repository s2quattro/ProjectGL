using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Controller : MonoBehaviour {

    public GameObject obstacle;

    int rand;
    float delta = 0f;
    Vector3 v;
    Quaternion q;

    // Use this for initialization
    void Start () {
		


	}
	
	// Update is called once per frame
	void Update () {

        if(delta <= 0f)
        {
            delta = 0.5f;
            rand = Random.Range(0, 4);
            v.z = Random.Range(0, 360f);
            v.y = 0f;
            v.x = 0f;

            q.z = Random.Range(0, 360f);
            q.x = 0f;
            q.y = 0f;
            
            
        }
        else
        {
            delta -= Time.deltaTime;

            obstacle.transform.rotation = Quaternion.Lerp(obstacle.transform.rotation, q, Time.deltaTime * 2f);

            switch (rand)
            {
                case 0:
                    {
                        obstacle.transform.Translate(Vector2.up * Time.deltaTime * 1.5f);
                        break;
                    }
                case 1:
                    {
                        obstacle.transform.Translate(Vector2.right * Time.deltaTime * 1.5f);
                        break;
                    }
                case 2:
                    {
                        obstacle.transform.Translate(Vector2.down * Time.deltaTime * 1.5f);
                        break;
                    }
                case 3:
                    {
                        obstacle.transform.Translate(Vector2.left * Time.deltaTime * 1.5f);
                        break;
                    }

            }

        }
	}
}
