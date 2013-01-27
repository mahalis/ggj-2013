using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RopeMovementController : MonoBehaviour {

	bool isFollowingMouse = false;
	public bool isConnected = false;
	NodeConnection activeNodeConnection;

	Quaternion preConnectionRotation;

	Vector2 grabPosition;

	int TIP_LAYER = 8;
	int layerMask = 1 << 8;

	// Use this for initialization
	void Start () {
		BoxCollider bc = this.gameObject.AddComponent<BoxCollider>();
		bc.size = new Vector3(5,5,5);
		bc.isTrigger = true;

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
		        	grabPosition = Input.mousePosition;
			        isFollowingMouse = true;
			        rigidbody.isKinematic = true;
			        isConnected = false;
			        if (activeNodeConnection != null){
			        	activeNodeConnection.disconnectRope();
			        	this.transform.rotation = preConnectionRotation;
			        	EventManager.instance.TriggerEvent(new NodeConnectionsChangedEvent());
						Instantiate(GameManager.getInstance().bloodSpurt,activeNodeConnection.transform.position, activeNodeConnection.transform.rotation);
			        }
			        activeNodeConnection = null;
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

		checkForAttachment();
	}

	void checkForAttachment() {
		if (Vector2.Distance(grabPosition, Input.mousePosition) > 75f){
			List<NodeConnection> connections = GameManager.getInstance().nodeConnections;
			foreach (NodeConnection nc in connections) {
				float dist = Vector2.Distance(this.transform.position, nc.transform.position);
				if (dist < 1f && !nc.isConnected) {
					preConnectionRotation = this.transform.rotation;
					isConnected = true;
					nc.connectWithRope(this);
					activeNodeConnection = nc;
					EventManager.instance.TriggerEvent(new NodeConnectionsChangedEvent());
					break;
				}
			}
		}		
	}

	public void stopDragging() {
		isFollowingMouse = false;
	}
}
