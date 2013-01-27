using UnityEngine;
using System.Collections;

public class NodeConnectionsChangedEvent : IEvent {
	public NodeConnectionsChangedEvent() {
	}    
    
    string IEvent.GetName() {
        return this.GetType().ToString();
    }
    
    object IEvent.GetData() {
        return 0;
    }
}
