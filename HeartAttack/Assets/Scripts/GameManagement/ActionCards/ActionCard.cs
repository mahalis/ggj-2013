using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionCard : MonoBehaviour {
	float BAR_WIDTH = 20f;
	float BAR_HEIGHT= 20f;
	float CARD_HEIGHT= 256f;


	public List<PortColor> portColors;
	public tk2dSprite cardSprite;
	public GameObject portColorPrefab;

	void Start () {
		buildPortColorBar();
	}


	void buildPortColorBar ()  {
		if (portColorPrefab) {
			GameObject portColorsParent = new GameObject();
			portColorsParent.name = "PortColors";
			portColorsParent.transform.parent = this.transform;
			portColorsParent.transform.localPosition = Vector3.zero;

			float xOffset = (portColors.Count * BAR_WIDTH)/2f;	
			for (int i = 0; i < portColors.Count; i ++) {
				PortColor pc = portColors[i];
				GameObject colorBar = Instantiate(portColorPrefab,Vector3.zero,Quaternion.identity) as GameObject;
				colorBar.transform.parent = portColorsParent.transform;
				tk2dSprite barSprite = colorBar.GetComponent<tk2dSprite>();
				// TODO : Get rgb forom config
				barSprite.color = PortColorRGB.getRGBForPortColor(pc);
				colorBar.transform.localPosition = new Vector3((BAR_WIDTH*2 * i)-xOffset, -CARD_HEIGHT/2f + BAR_HEIGHT, -1f);
				colorBar.transform.localScale = new Vector3(BAR_WIDTH, BAR_HEIGHT, 1);
			}
		} else {
			Debug.Log("Port color prefab missing!!");
		}
	}

}
