using UnityEngine;
using System.Collections;

public class SardineUserController : MonoBehaviour
{
	SardineCharacter sardineCharacter;

	float t = 0;
	float t2 = 1;
	float a = 0;

	void Start () 
	{
		sardineCharacter = GetComponent<SardineCharacter>();
	}
	

	void Update ()
	{

		t2 -= Time.deltaTime;

		if (t2 <= 0)
        {
			t -= Time.deltaTime;

			sardineCharacter.MoveForward();

			float nA = a;
			nA += Mathf.Sin(Time.time) * 20;

			if (transform.rotation.eulerAngles.y > nA)
			{
				sardineCharacter.TurnLeft();
			}
			else
			{
				sardineCharacter.TurnRight();
			}


			if (t <= 0)
			{
				t = Random.Range(2, 10);
				t2 = Random.Range(2, 10);
				a = Random.Range(-180, 180);
			}
		}
	}
}
