using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionCard : MonoBehaviour {
	float BAR_WIDTH = 1.7f;


	public List<PortColor> portColors;
	public tk2dSprite cardSprite;
	public GameObject portColorPrefab;

	void Start () {
		buildPortColorBar();
	}


	void buildPortColorBar ()  {
		if (portColorPrefab) {
			float xOffset = ((portColors.Count-1) * BAR_WIDTH)/2f;	
			for (int i = 0; i < portColors.Count; i ++) {
				PortColor pc = portColors[i];
				GameObject colorBar = Instantiate(portColorPrefab,Vector3.zero,Quaternion.identity) as GameObject;
				colorBar.transform.parent = this.transform;
				tk2dSprite barSprite = colorBar.GetComponent<tk2dSprite>();
				barSprite.color = PortColorRGB.getRGBForPortColor(pc);
				colorBar.transform.localPosition = new Vector3((BAR_WIDTH*i)-xOffset, -1.25f, 0);
				colorBar.transform.localScale = Vector3.one;
			}
		} else {
			Debug.Log("Port color prefab missing!!");
		}
	}

}
