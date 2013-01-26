using UnityEngine;
using System.Collections;

public class PresentNewActionCardEvent : IEvent {
	public PresentNewActionCardEvent() {
	}    
    
    string IEvent.GetName() {
        return this.GetType().ToString();
    }
    
    object IEvent.GetData() {
        return 0;
    }
}
