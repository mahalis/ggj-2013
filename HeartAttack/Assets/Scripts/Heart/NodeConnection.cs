using UnityEngine;
using System.Collections;

public class NodeConnection : MonoBehaviour {
	public PortColor portColor;
	public bool isConnected;
	
	public void connectWithRope(RopeMovementController rc) {
		rc.stopDragging();
    	rc.rigidbody.MovePosition(this.transform.position);
    	rc.isConnected = true;
    	this.isConnected = false;
	}
	public void disconnectRope() {
		this.isConnected = false;
	}
}
