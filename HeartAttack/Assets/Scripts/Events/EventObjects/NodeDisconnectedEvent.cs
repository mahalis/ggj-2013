using UnityEngine;
using System.Collections;

public class NodeDisconnectedEvent : IEvent {
	public NodeDisconnectedEvent() {
	}    
    
    string IEvent.GetName() {
        return this.GetType().ToString();
    }
    
    object IEvent.GetData() {
        return 0;
    }
}
