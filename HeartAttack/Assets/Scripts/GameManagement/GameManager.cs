using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour,IEventListener {
	public ViewManager viewManager;
	public List<NodeConnection> nodeConnections;
	public ParticleSystem bloodSpurt;
	public HeartBeatController heartBeatController;

	float currentHeartRate; // beats per minute
	float nextCardTime;
	
	const float RANDOM_TIME_ADJUSTMENT = 0.5f;
	const float BASE_HEART_RATE = 60;
	const float HEART_RATE_GROWTH = 0.4f; // beats per minute per second
	const float HEART_RATE_TIME_LOSS = 0.03f; // number of seconds per bpm
	const float BASE_CARD_INTERVAL = 6.0f;
	
	const float EASY_CHANCE = 0.25f; // remove 1 color from next card
	const float SUPER_EASY_CHANCE = 0.1f; // remove 2 colors from next card
	// note that total likelihood of getting at least one color removed is the sum of the above, in this case 15%

	const int MAX_ACTION_CARDS =5;

	float currentScore = 0f;
	bool gameIsActive = false;
	
	private static GameManager instance;
	public static GameManager getInstance() {
	    if (instance == null) {
			instance = GameObject.Find("GameManager").GetComponent<GameManager>();
			EventManager.instance.AddListener(instance as IEventListener, "NodeConnectionsChangedEvent");	
	    }
	    return instance;
	}

	// Use this for initialization
	void Start () {
		currentHeartRate = BASE_HEART_RATE;
		viewManager.setBpm(currentHeartRate);
		heartBeatController.setBPM(currentHeartRate);
	}
	
	// Update is called once per frame
	void Update () {
		if(gameIsActive){
			float now = Time.time;
			currentHeartRate += Time.deltaTime * HEART_RATE_GROWTH;
			viewManager.setBpm(currentHeartRate);
			heartBeatController.setBPM(currentHeartRate);
			if(now > nextCardTime) {
				if (ActionCardManager.getInstance().numberOfActiveCards() < MAX_ACTION_CARDS){
					newCard ();
					nextCardTime = now + (BASE_CARD_INTERVAL - (currentHeartRate - BASE_HEART_RATE) * HEART_RATE_TIME_LOSS) + Random.Range(-RANDOM_TIME_ADJUSTMENT, RANDOM_TIME_ADJUSTMENT);
				} else {
					// Game Over
					/*
					SoundManager.getInstance().playGameOver();
					viewManager.setGameOverViewVisible(true);
					viewManager.setScore((int)currentScore);
					gameIsActive = false;
					*/
				}				
			}
		}
	}
	
	void newCard() {
		int numColors = Mathf.Min(Mathf.RoundToInt(5 * Mathf.Pow(Mathf.Min(currentHeartRate - BASE_HEART_RATE, 60) / 60.0f, 0.3f)), 4);
		if (Random.Range(0.0f, 1.0f) < EASY_CHANCE) {
			//Debug.Log ("Easy, subtracting 1");
			numColors -= 1;
		} else if (Random.Range(0.0f, 1.0f) < SUPER_EASY_CHANCE) {
			//Debug.Log("Super easy, subtracting 2");
			numColors -= 2;
		}
		if (numColors < 1) numColors = 1;
		
		ActionCardManager.getInstance().generateNewAction(numColors);
	}

	public void testingButtonPressed () {
		//ActionCardManager.getInstance().generateNewAction(1);
		EventManager.instance.TriggerEvent(new DisconnectAllNodesEvent());
	}

	bool IEventListener.HandleEvent(IEvent evt) {
	    switch (evt.GetName()) {
	    	case "NodeConnectionsChangedEvent" :
	    		nodeConnectionsChanged();
	    		break;
	    }
	    return false;
	}

	public void nodeConnectionsChanged () {
		List<PortColor> connectedColors = new List<PortColor>();
		foreach(NodeConnection nc in nodeConnections) {
			if (nc.isConnected){
				connectedColors.Add(nc.portColor);
			}
		}
		ActionCardManager.getInstance().checkForActionCompletion(connectedColors);
	}

	public void startGame () {
		currentScore = 0;
		gameIsActive = true;
		viewManager.hideStartGameView();
	}

	public void replayGame () {
		SoundManager.getInstance().resumeTheme();
		currentScore = 0;
		ActionCardManager.getInstance().reset();
		currentHeartRate = BASE_HEART_RATE;
		gameIsActive = true;
		viewManager.setGameOverViewVisible(false);
		foreach(NodeConnection nc in nodeConnections) {
			if (nc.isConnected) {
				nc.disconnectRope();
			}
		}
	}

	public void completedActionCardWithNumberOfColors(int numColors) {
		int prime = 13;
		currentScore += (prime * numColors) * currentHeartRate;
	}
}
