using UnityEngine;
using System.Collections;

public class RopeMovementController : MonoBehaviour {

	bool isFollowingMouse = false;
	public bool isConnected = false;

	int TIP_LAYER = 8;
	int layerMask = 1 << 8;

	// Use this for initialization
	void Start () {
		BoxCollider bc = this.gameObject.AddComponent<BoxCollider>();
		bc.size = new Vector3(5,5,5);

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
			        rigidbody.isKinematic = true;
			        isConnected = false;
			    }
		    }
		} else if ( Input.GetMouseButtonUp(0) ) {
			isFollowingMouse = false;
			if (!isConnected){
				rigidbody.isKinematic = false;	
			}
			
		}
	}

	void followMouse () {
		Vector3 pos = Input.mousePosition;
		pos.z = this.transform.position.z;
		Vector3 mouseWorldPt = Camera.main.ScreenToWorldPoint(pos);
		rigidbody.MovePosition(mouseWorldPt);
	}

	public void stopDragging() {
		isFollowingMouse = false;
	}
}
