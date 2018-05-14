using UnityEngine;
using System.Collections;

//JoaoWeapons
public class Weapon4Control : MonoBehaviour {
	WeaponStatus status;
	bool has_attacked;
	int reducedenemyspeedlevel;
	public void Init(WeaponStatus status,int upgd_level){
		this.status = new WeaponStatus(status.target, 
		                               status.damage,		                              
		                               status.attack_range,
		                               status.attack_duration);
		float r = 1.625f + 1.625f*this.status.attack_range;
		this.transform.localScale = new Vector3(r, r, r);
		reducedenemyspeedlevel=upgd_level;
	}
	float activetime=0;
	// Use this for initialization
	void Start () {

	}
	// Update is called once per frame
	void Update () {

		if(activetime<=this.status.attack_duration){
			if(!has_attacked){
				Debug.Log("stopper attack range " + this.status.attack_range);
				Collider2D [] enemiescolider=Physics2D.OverlapCircleAll(this.transform.position,this.status.attack_range, 1 << LayerMask.NameToLayer("Enemy"));
				for(int i=0;i<enemiescolider.Length;i++){
					Debug.Log ("stopping enemy "+reducedenemyspeedlevel);
					enemiescolider[i].gameObject.GetComponent<EnemyControl>().Stop(status.damage,GlobalData.Tower4percentslowdown[reducedenemyspeedlevel]);
				}
				if(enemiescolider.Length>0){
					StartCoroutine(ActionAfterTimer.Set(0.5f, delegate {
						Destroy (this.gameObject);
						has_attacked= true;
					}));
				}
			}
			activetime+=Time.deltaTime;
		}else{
			Destroy(this.transform.gameObject);
		}
		
	}
	
	
}
//JoaoWeapons
