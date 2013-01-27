using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour,IEventListener {

	public Texture defaultBackground;
	public Texture zapBackground1;
	public Texture zapBackground2;
	
	protected Material mat;
	
	protected float startedZappingTime = -1;
	
	const float ZAP_DURATION = 1.5f;
	
	protected Vector3 defaultScale;
	
	// Use this for initialization
	void Start () {
		mat = this.GetComponent<MeshRenderer>().material;
		defaultScale = this.transform.localScale;
		EventManager.instance.AddListener(this as IEventListener, "DisconnectAllNodesEvent");
	}
	
	public void startZapping () {
		startedZappingTime = Time.time;
	}
	
	bool IEventListener.HandleEvent(IEvent evt) {
	    switch (evt.GetName()) {
	    	case "DisconnectAllNodesEvent" :
	    		startZapping();
	    		break;
	    }
	    return false;
	}
	
	// Update is called once per frame
	void Update () {
		if (startedZappingTime > 0) {
			if (Time.time - startedZappingTime < ZAP_DURATION) {
				int whichTexture = Random.Range (0,3);
				mat.SetTexture("_MainTex",whichTexture == 2 ? zapBackground2 : (whichTexture == 1 ? zapBackground1 : defaultBackground));
				float scaleFactor = Random.Range(1.0f, 1.1f);
				this.transform.localScale = defaultScale * scaleFactor;
			} else {
				startedZappingTime = -1;
				mat.SetTexture("_MainTex", defaultBackground);
				this.transform.localScale = defaultScale;
			}
		}
	}
}
