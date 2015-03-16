using UnityEngine;
using System.Collections;

public class HUD_Flightpath : MonoBehaviour
{
	Rigidbody _rigidbody;

	// Use this for initialization
	void Start ()
	{
		_rigidbody = GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		var velPoint = _rigidbody.velocity.magnitude > 1f
			? Camera.main.WorldToScreenPoint(Camera.main.transform.position + (_rigidbody.velocity.normalized * 1000))
				: new Vector3(Screen.width / 2f, Screen.height / 2f, 1);
		
		if (velPoint.z > 0)
		{
			this.transform.position = velPoint;
		}
	}
}
