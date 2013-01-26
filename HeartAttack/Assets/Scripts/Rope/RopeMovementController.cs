using UnityEngine;
using System.Collections;

public class RopeMovementController : MonoBehaviour {

	bool isFollowingMouse = false;

	int TIP_LAYER = 8;
	int layerMask = 1 << 8;

	// Use this for initialization
	void Start () {
		this.gameObject.AddComponent<BoxCollider>();
		this.gameObject.layer = TIP_LAYER;
	}
	
	// Update is called once per frame
	void Update () {
		checkForInteraction();
		
	}
	void FixedUpdate () {
		if (isFollowingMouse) {
			followMouse();
		}
	}


	void checkForInteraction() {
		if ( Input.GetMouseButtonDown(0) ){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		    if (Physics.Raycast(ray.origin,ray.direction,out hit,Mathf.Infinity,layerMask)){
		        if (hit.transform == this.transform){
			        isFollowingMouse = true;
			        Debug.Log("GRABBED ROPE");
			        rigidbody.AddForce(new Vector3(500, 0, 0));
			    }
		    }
		} else if ( Input.GetMouseButtonUp(0) ) {
			isFollowingMouse = false;
		}
	}

	void followMouse () {
		Vector3 mouseWorldPt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Debug.Log("mouseWorldPt " + mouseWorldPt);
		//Vector3 newPos = this.transform.localPosition;
		//Debug.Log("current : " + this.transform.localPosition);
		//Debug.Log("newPos : " + newPos);
		//rigidbody.MovePosition(newPos);

		//Vector3 localPt = transform.parent.InverseTransformPoint(mouseWorldPt);
		//Debug.Log("localPt : " + localPt);
	}
}
