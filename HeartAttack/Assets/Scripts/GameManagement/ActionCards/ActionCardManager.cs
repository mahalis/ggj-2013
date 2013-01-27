using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionCardManager : MonoBehaviour {
	public List<GameObject> availableActionCards;
	List<ActionCard> activeActionCards;

	float ACTION_CARD_SIZE = 3f;
	
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
		this.gameObject.transform.localPosition = new Vector3(-ACTION_CARD_SIZE/2f -0.5f,ACTION_CARD_SIZE/2f,0);
	}	

	void Update() {
		
	}

	public void addActiveActionCard(ActionCard newActionCard) {
		activeActionCards.Add(newActionCard);
	}
	
	protected void layoutCards () {
		for (int i = 0; i < activeActionCards.Count; i++) {
			activeActionCards[i].targetPosition = new Vector2(0, -ACTION_CARD_SIZE * (i + 0.5f));
		}
	}
	
	public void generateNewAction(int numColors) {
		// randomly pick from prefabs
		int randomNum = Random.Range(0,availableActionCards.Count);
		GameObject selectedActionCard = availableActionCards[randomNum];
		GameObject actionCardGO = Instantiate(selectedActionCard,Vector3.zero,Quaternion.identity) as GameObject;
		actionCardGO.transform.parent = this.transform;
		ActionCard newActionCard = actionCardGO.GetComponent<ActionCard>();
		
		newActionCard.portColors = buildPortColorListOfLength(Mathf.Min(numColors, GameConfig.NUMBER_OF_PORT_COLORS), activeActionCards.Count);

		newActionCard.transform.localPosition = Vector3.zero;
		activeActionCards.Insert(0, newActionCard);
		layoutCards();
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


	public void checkForActionCompletion(List<PortColor> connectedPortColors) {
		List<ActionCard> cardsToRemove = new List<ActionCard>();
		for (int i = 0; i < activeActionCards.Count; i++) {
			ActionCard activeActionCard = activeActionCards[i];
			int requiredConnections = activeActionCard.portColors.Count;
			if (connectedPortColors.Count == requiredConnections){
				int correctConnections = 0;
				foreach (PortColor pc in connectedPortColors) {
					if (activeActionCard.portColors.Contains(pc)){
						correctConnections ++;
					}
				}
				if (correctConnections == requiredConnections) {
					cardsToRemove.Add(activeActionCard);
					GameManager.getInstance().completedActionCardWithNumberOfColors(activeActionCard.portColors.Count);
				}
			}
		}
		
		foreach(ActionCard card in cardsToRemove) {
			activeActionCardHasBeenCompleted(card);
		}
		layoutCards();
	}

	public int  numberOfActiveCards () {
		return activeActionCards.Count;
	}

	void activeActionCardHasBeenCompleted(ActionCard card) {
		card.targetPosition = card.targetPosition + new Vector2(2*ACTION_CARD_SIZE, 0);
		activeActionCards.Remove(card);
	}

	public void reset() {
		foreach(ActionCard ac in activeActionCards){
			Destroy(ac.gameObject);
		}
		activeActionCards = new List<ActionCard>();
	}

}
