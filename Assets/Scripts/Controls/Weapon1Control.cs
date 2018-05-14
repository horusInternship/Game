using UnityEngine;
using System.Collections;

public class Weapon1Control : MonoBehaviour {
	WeaponStatus status;
	float speed =0.5f;
	Vector3 _direction;
	Quaternion _lookRotation;


	bool weapon_from_enemy = false;

	bool cango=false;

	public void Init(WeaponStatus status, Vector3 startpos){
		this.status = new WeaponStatus(status.target, 
		                               status.damage,
		                               status.attack_range,
		                               status.attack_duration);
		if(status.target.GetComponent<EnemyControl>()!=null){
			Debug.Log ("enemy is target");
			weapon_from_enemy = false;
		}else if(status.target.GetComponent<TowerControl>()!=null){
			Debug.Log ("enemy is tower");
			weapon_from_enemy = true;
		}
		this.transform.position = startpos;
		this.startpos = startpos;
		this.cango = true;
		 
	}
	
	float journeyLength;
	Vector3 startpos;
 
	void Start () {
 
	}
	float weapon_timer=0;
 
	void Update () {
		if(status.target !=null){
		if(cango){
			if(weapon_timer<=1) {
				weapon_timer+=Time.deltaTime;
			 	this.transform.position =  Vector3.Lerp(startpos, this.status.target.transform.position, weapon_timer);
				Rotate();
				
			}else {
				cango = false;
				Debug.Log ("Give Damage "+this.status.damage);

				if(weapon_from_enemy){
					this.status.target.GetComponent<TowerControl>().TakeDamage(this.status.damage, transform.parent.GetComponent<EnemyControl>().status.type);
				}else{
					this.status.target.GetComponent<EnemyControl>().TakeDamage(this.status.damage, 1);
				}
				this.transform.parent=null;
				Destroy(this.gameObject);
			}
		}
		}else{
			this.transform.parent = null;
			Destroy (this.gameObject);
		}
	}



	void Rotate(){
		Vector3 a = this.transform.position;
		Vector3 b = this.status.target.transform.position;

		float angle = 90-Mathf.Rad2Deg*Mathf.Atan2(b.x - a.x, b.y - a.y);


		this.transform.rotation = Quaternion.Euler(0,0, angle);
	}
	
}

