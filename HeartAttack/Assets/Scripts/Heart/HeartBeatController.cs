using UnityEngine;
using System.Collections;

public class HeartBeatController : MonoBehaviour {

	float heartScale = 15f;
	
	float pulseWidthMultiplier = 25f;
	float beatPeriod = 1f;

	float smallerContractionScale = 0.98f;
	float biggerContractionScale = 0.92f;

	float smallerContractionTarget = 0f;
	float largerContractionTarget = 0f;

	float beatAccum = 0f;

	float BPM = 60f;

	enum HeartState {
		contractSamll,
		contractLarge,
		expandFromSmall,
		expandFromLarge,
		pulseWait,
		periodWait
	}

	HeartState heartState = HeartState.periodWait;
	IEnumerator stateMachine;
	
	void Start() {
		stateMachine = changeBeatState();
		smallerContractionTarget = heartScale * smallerContractionScale;
		largerContractionTarget = heartScale * biggerContractionScale;

		this.transform.localScale = new Vector3(heartScale,heartScale,heartScale);
		changeBeatState();
	}

	void Update() {

		switch(heartState) {
			case HeartState.contractSamll :
			case HeartState.contractLarge :
				contract();
			break;
			case HeartState.expandFromSmall :
			case HeartState.expandFromLarge :
				expand();
			break;
			case HeartState.pulseWait :
			case HeartState.periodWait :
				wait();
			break;
		}
	}

	void contract() {
		if (beatAccum > 1f){
			moveEmOnUp();
		} else {
			float newScale;
			if (heartState == HeartState.contractSamll){
				newScale = Mathf.Lerp(heartScale,smallerContractionTarget,beatAccum);
			} else {
				newScale = Mathf.Lerp(heartScale,largerContractionTarget,beatAccum);
			}
			
			this.gameObject.transform.localScale = new Vector3(
				newScale,
				newScale,
				newScale
			);
			beatAccum += Time.deltaTime + pulseWidthMultiplier;
		}		
	}

	void expand() {
		if (beatAccum > 1f){
			moveEmOnUp();
		} else {
			float newScale;
			if (heartState == HeartState.expandFromSmall){
				newScale = Mathf.Lerp(smallerContractionTarget,heartScale,beatAccum);
			} else {
				newScale = Mathf.Lerp(largerContractionTarget,heartScale,beatAccum);
			}
			
			this.gameObject.transform.localScale = new Vector3(
				newScale,
				newScale,
				newScale
			);
			beatAccum += Time.deltaTime * pulseWidthMultiplier;
		}		
	}

	void wait() {
		if (beatAccum > 1f){
			this.moveEmOnUp();
		} else {
			if (heartState == HeartState.pulseWait){
				beatAccum += Time.deltaTime * pulseWidthMultiplier;
			} else {
				beatAccum += Time.deltaTime * beatPeriod;
			}		
		}
	}

	public void setBPM(float newBPM) {
		BPM = newBPM;
		float beatsPerSec = BPM/60f;
		float pulseLength = (1f/pulseWidthMultiplier) * 6f;
		float newPeriodWait = Mathf.Clamp((1f/beatsPerSec)-pulseLength,0,10);
		this.beatPeriod = 1f/newPeriodWait;
		//Debug.Log("PERIOD WAIT : " + this.beatPeriod);
	}

	void moveEmOnUp () {
		stateMachine.MoveNext();
	}

	IEnumerator changeBeatState() {
		while(true) {
			beatAccum = 0f;
			heartState = HeartState.contractSamll;
			SoundManager.getInstance().playHeartUpBeat();
			yield return null;
			beatAccum = 0f;
			heartState = HeartState.pulseWait;
			yield return null;
			beatAccum = 0f;
			heartState = HeartState.expandFromSmall;
			yield return null;
			beatAccum = 0f;
			heartState = HeartState.pulseWait;
			yield return null;
			beatAccum = 0f;
			SoundManager.getInstance().playHeartDownBeat();
			heartState = HeartState.contractLarge;
			yield return null;
			beatAccum = 0f;
			heartState = HeartState.pulseWait;
			yield return null;
			beatAccum = 0f;
			heartState = HeartState.expandFromLarge;
			yield return null;
			beatAccum = 0f;
			heartState = HeartState.periodWait;
			yield return null;
		}
	}

	
}
