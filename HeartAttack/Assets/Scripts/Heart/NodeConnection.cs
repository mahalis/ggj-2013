using UnityEngine;
using System.Collections;

public class NodeConnection : MonoBehaviour {

	public FixedJoint fixedJoint;

	void Start() {
		SphereCollider sc = this.gameObject.AddComponent<SphereCollider>();
		sc.isTrigger = true;
		fixedJoint = this.gameObject.AddComponent<FixedJoint>();
		rigidbody.constraints = RigidbodyConstraints.FreezeAll;
	}

	void OnTriggerEnter(Collider other) {
        RopeMovementController rmc = other.GetComponent<RopeMovementController>();
        if (rmc != null) {
        	rmc.stopDragging();
        	rmc.rigidbody.MovePosition(this.transform.position);
        	//fixedJoint.connectedBody = other.attachedRigidbody;
        	rmc.isConnected = true;
        }        
    }
}
