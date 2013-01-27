using UnityEngine;
using System.Collections;

public class NodeConnection : MonoBehaviour {
	public PortColor portColor;
	public bool isConnected;
	
	protected ParticleSystem spurt;
	
	void Start() {
		spurt = this.GetComponentInChildren<ParticleSystem>();
	}
	
	public void connectWithRope(RopeMovementController rc) {
		rc.stopDragging();
    	rc.rigidbody.MovePosition(this.transform.position);
    	rc.rigidbody.MoveRotation(this.transform.rotation);
    	rc.isConnected = true;
    	this.isConnected = true;
	}
	public void disconnectRope() {
		spurt.Play();
		this.isConnected = false;
	}
}
