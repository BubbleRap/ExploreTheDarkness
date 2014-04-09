using UnityEngine;
using System.Collections;

public class Object : MonoBehaviour {

	public bool firstperson;
	public AudioClip soundClip; 
	private AudioSource audioSource;
	public bool internalPlay;
	public bool hitObject = false;
	public Shader shader1 = Shader.Find("Diffuse");
    public Shader shader2 = Shader.Find("Rim Diffuse");

	// Use this for initialization
	void Start () {
		if(internalPlay)
		{
			audioSource = transform.gameObject.GetComponent<AudioSource>();
		}
		else
		{
			audioSource = Camera.main.gameObject.GetComponent<AudioSource>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(hitObject)
		{
			if(transform.renderer.material.shader == shader1)
			{
				transform.renderer.material.shader = shader2;
			}
		}
		else
		{
			if(transform.renderer.material.shader != shader1)
			{
				transform.renderer.material.shader = shader1;
			}
		}
		hitObject = false;
	}

	public void PlayAudio () {
		audioSource.clip = soundClip;
		audioSource.Play();
	}
}
