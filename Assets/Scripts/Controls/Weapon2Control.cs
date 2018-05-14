using UnityEngine;
using System.Collections;

public class Weapon2Control : MonoBehaviour {

	WeaponStatus status;
	float speed =0.5f;
	Vector3 Midpoint,startMarker;
	Quaternion _lookRotation;
	public void Init(WeaponStatus status){
		this.status = new WeaponStatus(status.target,
		                               status.damage, 
		                               status.attack_range,
		                               status.attack_duration);
	}
	
	float startTime, journeyLength;
	// Use this for initialization
	void Start () {
		startMarker=this.transform.position;
		startTime = Time.time;
		journeyLength = Vector3.Distance(this.transform.position, this.status.target.transform.position);
		
		/*_direction= (this.status.enemy.transform.position - this.transform.position).normalized;
		_lookRotation= Quaternion.LookRotation(_direction);
		this.transform.rotation = _lookRotation;
		this.transform.GetComponentInChildren<SpriteRenderer>().enabled=true;*/
	}

	float timer=0;
	// Update is called once per frame
	void Update () {
		if(status.target!=null){
			if(timer<=0.2f) {
				timer+=Time.deltaTime;
				Midpoint=(startMarker+status.target.transform.position)/2;
				this.transform.position=Midpoint;
				journeyLength = Vector3.Distance(this.transform.position, this.status.target.transform.position);
				 this.transform.localScale = new Vector3(journeyLength*2, this.transform.localScale.y, this.transform.localScale.z);
				Rotate ();
			} else {
 				this.status.target.GetComponent<EnemyControl>().TakeDamage(this.status.damage, 2);
				this.transform.parent = null;
				Destroy(this.gameObject);
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
