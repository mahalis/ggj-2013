using UnityEngine;
using System.Collections;

public class RopeController : MonoBehaviour {
	
	public GameObject segment;
	public int numSegments;
	public bool shouldCreateBent;
	
	// Use this for initialization
	void Start () {
		Vector3 position = transform.position;
		Rigidbody lastBody = GetComponent<Rigidbody>();
		Quaternion startRotation = lastBody.transform.rotation;
		Vector3 baseOffset = new Vector3(0, -0.6f, 0);
		Vector3 transformedOffset = startRotation * baseOffset;
		Quaternion bendRotation = Quaternion.AngleAxis(20, new Vector3(0, 0, (position.x > 0) ? 1 : -1));
		for(int i = 0; i < numSegments; i++) {
			
			if (shouldCreateBent && i < 4) {
				position += transformedOffset * 0.5f;
				transformedOffset = bendRotation * transformedOffset;
				position += transformedOffset * 0.5f;
				startRotation *= bendRotation;
			} else {
				position += transformedOffset;
			}
			GameObject newSegment = (GameObject)Instantiate(segment, position, startRotation);
			newSegment.transform.parent = this.transform.parent;
			ConfigurableJoint joint = ((ConfigurableJoint)(newSegment.GetComponent<ConfigurableJoint>()));
			joint.connectedBody = lastBody;
			lastBody = newSegment.GetComponent<Rigidbody>();
		}
		//lastBody.AddForce(new Vector3(500, 0, 0));

		lastBody.gameObject.AddComponent<RopeMovementController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
