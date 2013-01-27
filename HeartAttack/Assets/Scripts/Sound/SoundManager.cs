using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour,IEventListener {

	
	private static SoundManager instance;
	public static SoundManager getInstance() {
	    if (instance == null) {
			instance = GameObject.Find("MainCamera").GetComponent<SoundManager>();
	    }
	    return instance;
	}

	void Awake () {
		EventManager.instance.AddListener(instance as IEventListener, "NodeDisconnectedEvent");	
	}

	
	bool IEventListener.HandleEvent(IEvent evt) {
	    switch (evt.GetName()) {
	    	case "NodeDisconnectedEvent" :
	    		break;
	    }
	    return false;
	}


	public void playSoundEffect (string effectName) {
		Debug.Log("playSoundEffect "+ effectName);
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
}
