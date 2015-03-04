using UnityEngine;
using System.Collections;

public class PuzzleLillebror : MonoBehaviour {

	public DoorOpenController Door;
	public AudioClip[] needLillebrorSound;
	public AudioClip[] collectLillebrorSounds;
	public AudioClip sewingLillebrorSound;
	public AudioClip haveLillebrorSound;

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider other) 
	{
		if(other.tag == "Player")
		{
			AudioSource soundSource = GameObject.Find("HeadAudioSource").transform.GetComponent<AudioSource>();
			if(other.transform.GetComponent<Inventory>().lillebrorComplete)
			{
				soundSource.clip = haveLillebrorSound;
				soundSource.Play();
				Door.isLocked = false;
			}
			else
			{
				if(other.transform.GetComponent<CollectLillebrorUI>() == null)
				{
					if(other.transform.gameObject.GetComponent<CollectLillebrorUI>() == null)
					{
						CollectLillebrorUI objectiveController = other.transform.gameObject.AddComponent<CollectLillebrorUI>();
						objectiveController.lillebrorMessage = "Find Lillebror";
						objectiveController.multipleTask = false;
					}
				}

				if(needLillebrorSound.Length > 0)
				{
					soundSource.clip = needLillebrorSound[Random.Range(0, needLillebrorSound.Length)];
					soundSource.Play();
				}

				Debug.Log("You do not have lillebror collected yet");
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
