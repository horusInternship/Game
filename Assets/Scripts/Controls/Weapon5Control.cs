using UnityEngine;
using System.Collections;

//JoaoWeapons
public class Weapon5Control : MonoBehaviour {
	WeaponStatus status;
	Vector3 _direction;
	Quaternion _lookRotation;
	bool exploded = false;

	public void Init(WeaponStatus status){
		this.status = new WeaponStatus(status.target,
		                               status.damage, 
 										status.attack_range,
		                               status.attack_duration);
		this.transform.localScale = new Vector3(2*status.attack_range, 2*status.attack_range, 2*status.attack_range);
	}

	Vector3 startMarker;
	Vector3 endMarker;
	
	// Use this for initialization
	void Start () {
		startMarker=transform.position;
		endMarker=status.target.transform.position;
		/*_direction= (this.status.enemy.transform.position - this.transform.position).normalized;
		_lookRotation= Quaternion.LookRotation(_direction);
		this.transform.rotation = _lookRotation;
		this.transform.GetComponentInChildren<SpriteRenderer>().enabled=true;*/
	}
	float timetoreach=0;
	// Update is called once per frame
	void Update () {

		if(!exploded){
		if(timetoreach<=0.5f) {
			timetoreach+=Time.deltaTime;
			//this.transform.position =  Vector3.Lerp(startMarker, endMarker, timetoreach/0.5f);
				Debug.Log ("Give range "+this.status.attack_range+" damage "+this.status.damage);
				
				Collider2D [] enemiescolider=Physics2D.OverlapCircleAll(this.transform.position,this.status.attack_range, 1 << LayerMask.NameToLayer("Enemy"));
				Debug.Log ("enemiescollider "+enemiescolider.Length);
				for(int i=0;i<enemiescolider.Length;i++) {
					enemiescolider[i].gameObject.GetComponent<EnemyControl>().TakeDamage(this.status.damage, 5);
				}
				exploded=true;
				
				
				StartCoroutine(ActionAfterTimer.Set(1f, delegate{
					Destroy(this.transform.gameObject);	
				}));
		} else {
			 Destroy(this.transform.gameObject);	
		 }
		
		}
	}
	
}
//JoaoWeapons