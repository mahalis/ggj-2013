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

	float ACTION_CARD_SIZE = 2f;
	
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
		this.gameObject.transform.localPosition = new Vector3(-ACTION_CARD_SIZE/2f,ACTION_CARD_SIZE/2f,0);
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
		GameObject actionCardGO = Instantiate(selectedActionCard,Vector3.zero,Quaternion.identity) as GameObject;
		actionCardGO.transform.parent = this.transform;
		ActionCard newActionCard = actionCardGO.GetComponent<ActionCard>();
		
		newActionCard.portColors = buildPortColorListOfLength(numColors, activeActionCards.Count);

		newActionCard.transform.localPosition = new Vector3(0,ACTION_CARD_SIZE * activeActionCards.Count,0);

		activeActionCards.Add(newActionCard);
		pushActiveCardsOnScreen();
	}

	List<PortColor> buildPortColorListOfLength(int numColors,int targetIndex) {
		bool passedPreviousCompare = false;

		List<PortColor> portColors;
		do {
			portColors = new List<PortColor>();
			for (int i = 0; i < numColors; i ++) {
				bool success = false;
				do {
					int randomNum = Random.Range(1,GameConfig.NUMBER_OF_PORT_COLORS);
					PortColor pc = (PortColor)randomNum;
					if(!portColors.Contains(pc)){
						portColors.Add(pc);
						success = true;
					}			
				} while(!success);
			}
			if (targetIndex == 0) { // nothing to compaer to
				passedPreviousCompare = true;
			} else { // guarantee no direct color combo repeats
				ActionCard ac = activeActionCards[targetIndex-1];
				// first check for num diff
				if (ac.portColors.Count != numColors) {
					passedPreviousCompare = true;
				} else {
					bool isDiff = false;
					foreach (PortColor pc in portColors){
						if (!ac.portColors.Contains(pc)){
							isDiff = true;
							break;
						}
					}
					if (isDiff){
						passedPreviousCompare = true;
					}
				}				
			}
		}while(!passedPreviousCompare);

		return portColors;
	}

	void pushActiveCardsOnScreen() {
		startTransitionY = this.gameObject.transform.localPosition.y;
		targetTransitionY = ((activeActionCards.Count * ACTION_CARD_SIZE) - (ACTION_CARD_SIZE/2f)) * -1; // subtract half a card for proper alignment
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
		if (activeActionCards.Count >0) {
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
	}

	void activeActionCardHasBeenCompleted() {
		ActionCard completedCard = activeActionCards[0];
		startTransitionX = completedCard.transform.localPosition.x;
		targetTransitionX = startTransitionX + ACTION_CARD_SIZE;

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
			transitionCompletedTime += Time.deltaTime * 5f;
		}
	}

	IEnumerator reorderActiveCards () {
		while(isTransitioning) {
			yield return null;
		}

		this.gameObject.transform.localPosition = new Vector3(
			this.gameObject.transform.localPosition.x,
			(((activeActionCards.Count * ACTION_CARD_SIZE) - (ACTION_CARD_SIZE/2f)) * -1),
			this.gameObject.transform.localPosition.z
		);

		for (int i = 0; i < activeActionCards.Count; i ++){
			activeActionCards[i].transform.localPosition = new Vector3(0,ACTION_CARD_SIZE * i,0);
		}
	}

}
