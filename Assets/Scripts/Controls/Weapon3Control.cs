using UnityEngine;
using System.Collections;

//JoaoWeapons2109
public class Weapon3Control : MonoBehaviour {
	WeaponStatus status;
	Vector3 _direction;
	Quaternion _lookRotation;
	public void Init(WeaponStatus status){
		this.status = new WeaponStatus(status.target,
		                               status.damage,
		                               status.attack_range,
		                               status.attack_duration);
	}
	
	float anglebetweenShots;
	int oldplevel=0;
	int plevel=0;
	Transform startMarker;
	Vector3 StartMarkPos;
	Vector3 endMarker;
	
	Vector3 vectordifference;
	float angle_betweenstartandend;
	
	// Use this for initialization
	void Start() {
		endMarker=status.target.transform.position;
		StartMarkPos=this.transform.position;
		vectordifference = endMarker - StartMarkPos;
		angle_betweenstartandend = Mathf.Atan2(vectordifference.y, vectordifference.x);
	}
	
	
	float timetoreach=0;
	float endtimetoreach=1.5f;
	// Update is called once per frame
	
	
	void Update () {
		if(status.target!=null){
		
		
		if(timetoreach<=1.5f)
		{
			float timepercent=timetoreach/endtimetoreach;
			if(timepercent<=0.33f)
				plevel=1;
			else if(timepercent>0.33f && timepercent<=0.66f)
				plevel=2;
			else if(timepercent>0.6f)
				plevel=3;
			
			if(oldplevel!=plevel)
			{
				CreateShockWave(plevel);
				oldplevel=plevel;
			}
			timetoreach+=Time.deltaTime;
			
			
			
		}
		else
		{
			Debug.Log ("Give range "+this.status.attack_range+" damage "+this.status.damage);
			
			Collider2D [] enemiescolider=Physics2D.OverlapCircleAll(this.transform.position,this.status.attack_range, 1 << LayerMask.NameToLayer("Enemy"));
			Debug.Log ("enemiescollider "+enemiescolider.Length);
			for(int i=0;i<enemiescolider.Length;i++)
			{
				enemiescolider[i].gameObject.GetComponent<EnemyControl>().TakeDamage(this.status.damage, 3);
			}
			//Physics2D.OverlapCircleAll(this.position,this.status.attack_range);
			//(this.position,this.status.attack_range);
			
			//this.status.enemy.GetComponent<joaotestenemy>().TakeDamage(this.status.damage);
			this.transform.parent = null;
			Destroy(this.gameObject);
		}
		}
		
	}
	
	
	
	void CreateShockWave(int PowLvl){
		switch (PowLvl){
		case 1:
			Shoot(7); // one shot straight ahead
			break;
		case 2:
			Shoot(5);
			break;
		case 3:
			Shoot(3);
			break;
		}
	}
	
	
	
	/*void Shoot(float angle){
		GameObject particle = (GameObject)Instantiate(Resources.Load("Towers/TowerWeapons/Particle"));


		Vector3 newPos = StartMarkPos + Quaternion.AngleAxis(angle, Vector3.forward)*this.transform.position;
		particle.transform.position=newPos;
		particle.GetComponentInChildren<particlescript>().particleangle(this.status.damage,this.status.atack_range);
	}*/
	void Shoot(int numberofobjects){
		for(int i=1;i<numberofobjects;i++)
		{
			float angle = i * Mathf.PI /numberofobjects -angle_betweenstartandend;
			
			
			GameObject particle = (GameObject)Instantiate(Resources.Load("Prefabs/Weapons/Particle"));
			particle.transform.position =new Vector3(StartMarkPos.x+ 1f * Mathf.Sin(angle),StartMarkPos.y+ 1f * Mathf.Cos(angle),StartMarkPos.z);
			Vector2 differencepos=StartMarkPos-particle.transform.position;
			float timetodestroy=endtimetoreach-timetoreach;
			particle.GetComponentInChildren<Weapon3_Particle>().Init(this.status.damage,this.status.attack_range,angle,differencepos,timetodestroy);
			particle.transform.parent=this.transform;
		}
		
	}
}
//JoaoWeapons2109