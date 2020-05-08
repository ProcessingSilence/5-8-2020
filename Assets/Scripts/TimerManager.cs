using UnityEngine;
public class TimerManager : MonoBehaviour
{
	// Since the timer is a DontDestroyOnLoad object, this manager will make sure that it only spawns once so that
	// multiple timers do not stack over one another.
	public GameObject TimerObj;
	public GameObject MouseSpeedObj;
	private GameObject findTimer;
	void Awake()
	{
		findTimer =  GameObject.FindGameObjectWithTag("Timer");
		if (findTimer == null)
		{
			Debug.Log("Timer not detected, timer created.");
			Instantiate(TimerObj);
		}
		else
		{
			Debug.Log("Timer detected.");
		}
		
		findTimer =  GameObject.FindGameObjectWithTag("MouseSpeedThing");
		if (findTimer == null)
		{
			Debug.Log("Mouse speed setting setter not detected, mouse speed thing created.");
			Instantiate(MouseSpeedObj);
		}
		else
		{
			Debug.Log("Mouse speed setting setter detected.");
		}
	}
}