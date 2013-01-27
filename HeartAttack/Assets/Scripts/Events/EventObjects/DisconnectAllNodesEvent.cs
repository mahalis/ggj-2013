using UnityEngine;
using System.Collections;

public class DisconnectAllNodesEvent : IEvent {
	public DisconnectAllNodesEvent() {
	}    
    
    string IEvent.GetName() {
        return this.GetType().ToString();
    }
    
    object IEvent.GetData() {
        return 0;
    }
}
