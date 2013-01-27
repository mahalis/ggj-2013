using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour,IEventListener {
	AudioSource upbeatSource;
	AudioSource downbeatSource;
	AudioSource themeSource;
	
	private static SoundManager instance;
	public static SoundManager getInstance() {
	    if (instance == null) {
			instance = GameObject.Find("MainCamera").GetComponent<SoundManager>();
	    }
	    return instance;
	}

	void Awake () {
		EventManager.instance.AddListener(instance as IEventListener, "NodeDisconnectedEvent");

		upbeatSource = this.gameObject.AddComponent<AudioSource>();
		downbeatSource = this.gameObject.AddComponent<AudioSource>();
		AudioClip clip = UnityEngine.Resources.Load("Audio/heartUpBeat") as AudioClip;
		upbeatSource.clip = clip;
		clip = UnityEngine.Resources.Load("Audio/heartDownBeat") as AudioClip;
		downbeatSource.clip = clip;

		playTheme();	
	}

	
	bool IEventListener.HandleEvent(IEvent evt) {
	    switch (evt.GetName()) {
	    	case "NodeDisconnectedEvent" :
	    		break;
	    }
	    return false;
	}


	public void playSoundEffect (string effectName) {
		//Debug.Log("playSoundEffect "+ effectName);
		AudioSource source;
		AudioClip clip;

		source = this.gameObject.AddComponent("AudioSource") as AudioSource;
		clip = UnityEngine.Resources.Load("Audio/"+effectName) as AudioClip;
		

		if (clip != null && source != null) {
			source.clip = clip;
			source.Play();
			Destroy(source,clip.length);
		} 
		else {
			Debug.LogWarning("Could not locate sound effect named : " + effectName);
		}
			
	}

	void playTheme () {
		
		AudioClip clip;
		themeSource = this.gameObject.AddComponent("AudioSource") as AudioSource;
		clip = UnityEngine.Resources.Load("Audio/theme") as AudioClip;
		themeSource.volume  = 0.5f;
		

		if (clip != null && themeSource != null) {
			themeSource.clip = clip;
			themeSource.loop = true;
			themeSource.Play();
		} 			
	}

	public void playGameOver () {
		themeSource.Pause();
		playSoundEffect("gameOver");
	}
	public void resumeTheme () {
		themeSource.Play();
	}

	public void playHeartUpBeat() {
		upbeatSource.Play();
	}

	public void playHeartDownBeat() {
		downbeatSource.Play();		
	}
}
