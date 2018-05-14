using UnityEngine;
using System.Collections;

public class EnemyLayerControl : MonoBehaviour {

 
		int child_sprites = 0;
		void Start(){
			child_sprites = transform.childCount;
		}
		
		void LateUpdate () {
			SetLayer();
		}
		
		void SetLayer(){
			if(child_sprites>0){
				int layerorder = GlobalData.difficulty_ymap_size[GlobalData.current_difficulty]*10 -  Mathf.CeilToInt(transform.position.y*10);
				if(layerorder!=transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder){
					if(child_sprites>0 && child_sprites<=2){
						transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = layerorder;
						if(child_sprites==2){
							transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = -100;
						}
					}
				}
			}
		}
}

