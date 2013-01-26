using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionCardManager : MonoBehaviour {
	public List<GameObject> availableActionCards;
	List<ActionCard> activeActionCards;

	float targetTransitionY = 0f;
	float startTransitionY = 0;
	float transitionTime = 0f;
	bool isTransitioning = false;

	float targetTransitionX = 0f;
	float startTransitionX = 0;
	float transitionCompletedTime = 0f;
	bool isTransitioningCompleted = false;
	
	private static ActionCardManager instance;
	public static ActionCardManager getInstance() {
	    if (instance == null) {
			instance = GameObject.Find("MainCamera/MainView/UpperRightAnchor/ActionCardManager").GetComponent<ActionCardManager>();
	    }
	    return instance;
	}


	void Awake () {
		if (availableActionCards.Count == 0){
			Debug.LogError("NO ACTION PREFABS DEFINED!!!");
		}

		activeActionCards = new List<ActionCard>();
		this.gameObject.transform.localPosition = new Vector3(-GameConfig.ACTION_CARD_HEIGHT/2f,GameConfig.ACTION_CARD_HEIGHT/2f,0);
	}	

	void Update() {
		if (isTransitioning) {
			transitionActiveCards();
		}

		if (isTransitioningCompleted) {
			transitionCompletedActionCardOut();
		}
	}

	public void addActiveActionCard(ActionCard newActionCard) {
		activeActionCards.Add(newActionCard);
	}

	public void generateNewAction(int numColors) {
		// randomly pick from prefabs
		int randomNum = Random.Range(0,availableActionCards.Count);
		GameObject selectedActionCard = availableActionCards[randomNum];
		Debug.Log(selectedActionCard);
		GameObject actionCardGO = Instantiate(selectedActionCard,Vector3.zero,Quaternion.identity) as GameObject;
		actionCardGO.transform.parent = this.transform;
		ActionCard newActionCard = actionCardGO.GetComponent<ActionCard>();
		
		// TODO : link numColors to colors generated
		newActionCard.portColors = buildPortColorListOfLength(numColors, activeActionCards.Count+1);

		newActionCard.transform.localPosition = new Vector3(0,GameConfig.ACTION_CARD_HEIGHT * activeActionCards.Count,0);

		activeActionCards.Add(newActionCard);
		pushActiveCardsOnScreen();
	}

	List<PortColor> buildPortColorListOfLength(int numColors,int targetIndex) {
		List<PortColor> portColors = new List<PortColor>();
		bool success = false;
		for (int i = 0; i < numColors; i ++) {
			success = false;
			do {
				int randomNum = Random.Range(1,GameConfig.NUMBER_OF_PORT_COLORS+1);
				Debug.Log(randomNum);
				PortColor pc = (PortColor)randomNum;
				if(!portColors.Contains(pc)){
					portColors.Add(pc);
					success = true;
				}			
			} while(!success);
		}
		return portColors;
	}

	void pushActiveCardsOnScreen() {
		startTransitionY = this.gameObject.transform.localPosition.y;
		targetTransitionY = ((activeActionCards.Count * GameConfig.ACTION_CARD_HEIGHT) - (GameConfig.ACTION_CARD_HEIGHT/2f)) * -1; // subtract half a card for proper alignment
		isTransitioning = true;
		transitionTime = 0f;
	}

	void transitionActiveCards () {
		if (transitionTime > 1f){
			isTransitioning = false;
		}
		this.gameObject.transform.localPosition = new Vector3(
			this.gameObject.transform.localPosition.x,
			Mathf.Lerp(startTransitionY,targetTransitionY,transitionTime),
			this.gameObject.transform.localPosition.z
		);
		transitionTime += Time.deltaTime * 2f;
	}


	public void checkForActionCompletion(List<PortColor> connectedPortColors) {
		ActionCard activeActionCard = activeActionCards[0];
		int requiredConnections = activeActionCard.portColors.Count;
		int correctConnections = 0;
		foreach (PortColor pc in connectedPortColors) {
			if (activeActionCard.portColors.Contains(pc)){
				correctConnections ++;
			}
		}
		if (correctConnections == requiredConnections) {
			activeActionCardHasBeenCompleted();
		}
	}

	void activeActionCardHasBeenCompleted() {
		ActionCard completedCard = activeActionCards[0];
		startTransitionX = completedCard.transform.localPosition.x;
		targetTransitionX = startTransitionX + GameConfig.ACTION_CARD_HEIGHT;

		transitionCompletedTime = 0f;
		isTransitioningCompleted = true;
	}

	void transitionCompletedActionCardOut () {
		if (transitionCompletedTime > 1f){
			isTransitioningCompleted = false;
			Destroy(activeActionCards[0].gameObject);
			activeActionCards.RemoveAt(0);
			StartCoroutine(reorderActiveCards());
		} else {
			ActionCard completedCard = activeActionCards[0];
			completedCard.transform.localPosition = new Vector3(
				Mathf.Lerp(startTransitionX,targetTransitionX,transitionCompletedTime),
				completedCard.transform.localPosition.y,			
				completedCard.transform.localPosition.z
			);
			transitionCompletedTime += Time.deltaTime * 3f;
		}
	}

	IEnumerator reorderActiveCards () {
		while(isTransitioning) {
			yield return null;
		}

		this.gameObject.transform.localPosition = new Vector3(
			this.gameObject.transform.localPosition.x,
			(((activeActionCards.Count * GameConfig.ACTION_CARD_HEIGHT) - (GameConfig.ACTION_CARD_HEIGHT/2f)) * -1),
			this.gameObject.transform.localPosition.z
		);

		for (int i = 0; i < activeActionCards.Count; i ++){
			activeActionCards[i].transform.localPosition = new Vector3(0,GameConfig.ACTION_CARD_HEIGHT * i,0);
		}
	}

}
