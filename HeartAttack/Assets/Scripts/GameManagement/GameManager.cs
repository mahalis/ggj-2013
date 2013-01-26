using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public ViewManager viewManager;

	// Use this for initialization
	void Start () {
		//ActionCardManager.getInstance().generateNewAction(1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void testingButtonPressed () {
		ActionCardManager.getInstance().generateNewAction(1);
	}

	public void blue () {
		ActionCardManager.getInstance().checkForActionCompletion(new List<PortColor>(){PortColor.BLUE});
	}
	public void green () {
		ActionCardManager.getInstance().checkForActionCompletion(new List<PortColor>(){PortColor.GREEN});
	}
	public void red () {
		ActionCardManager.getInstance().checkForActionCompletion(new List<PortColor>(){PortColor.RED});
	}
	public void yellow () {
		ActionCardManager.getInstance().checkForActionCompletion(new List<PortColor>(){PortColor.YELLOW});
	}


}
