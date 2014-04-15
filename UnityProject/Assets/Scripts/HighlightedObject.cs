using UnityEngine;
using System.Collections;

public class HighlightedObject : MonoBehaviour {

	public bool firstperson;
	public AudioClip soundClip; 
	private AudioSource audioSource;
	public bool internalPlay;
	public bool hitObject = false;
	private Shader shader1;
 	private  Shader shader2;
 	private GameObject buttonPrompt;

	// Use this for initialization
	void Start () {
		shader1 = Shader.Find("Diffuse");
		shader2 = Shader.Find("Rim Diffuse");

		if(internalPlay)
		{
			audioSource = transform.gameObject.GetComponent<AudioSource>();
		}
		else
		{
			audioSource = Camera.main.gameObject.GetComponent<AudioSource>();
		}

		buttonPrompt = Instantiate(Resources.Load<GameObject>("buttonPrompt"), new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f, transform.position.z + 0.5f), transform.rotation) as GameObject;
		buttonPrompt.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(hitObject)
		{
			if(transform.renderer.material.shader == shader1)
			{
				transform.renderer.material.shader = shader2;
				buttonPrompt.SetActive(true);
				buttonPrompt.transform.LookAt(Camera.main.gameObject.transform);
			}
		}
		else
		{
			if(transform.renderer.material.shader != shader1)
			{
				transform.renderer.material.shader = shader1;
				buttonPrompt.SetActive(false);
			}
		}
		hitObject = false;
	}

	public void PlayAudio () {
		audioSource.clip = soundClip;
		audioSource.Play();
	}

	public bool StoppedPlaying () {
		if(!audioSource.isPlaying)
		{
			return true;
		}
		return false;
	}
}
