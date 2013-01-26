using UnityEngine;
using System.Collections;

public static class PortColorRGB {

	public static Color getRGBForPortColor (PortColor pc) {
		Color returnColor;
		switch(pc){
			case PortColor.RED : 
				returnColor = new Color(1f,0f,0f,1f);
			break;
			case PortColor.GREEN : 
				returnColor =  new Color(0f,1f,0f,1f);
			break;
			case PortColor.BLUE : 
				returnColor = new Color(0f,0f,1f,1f);
			break;
			case PortColor.YELLOW : 
				returnColor = new Color(1f,1f,0f,1f);
			break;
			case PortColor.CYAN :
				returnColor = new Color(0f,1f,1f,1f);
			break;
			case PortColor.MAGENTA :
				returnColor = new Color(1f,0f,1f,1f);
				break;
			default :
				returnColor = new Color(0f,0f,0f,1f); 
			break;
		}
		return returnColor;
	}
}