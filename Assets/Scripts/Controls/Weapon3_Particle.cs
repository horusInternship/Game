using UnityEngine;
using System.Collections;

//JoaoWeapons2109
public class Weapon3_Particle : MonoBehaviour {
	
	int saveddamage;
	
	Quaternion moveAng = Quaternion.Euler(315,0,0);
	float currtimetodestroy=0f, timetodestroy=5f,savedatackrange=0,savedangle=0,savedtimetodestroy=0;
	Vector2 savedmovingvector;
	// Use this for initialization
	void Start () {
		
	}
	
	public void Init(int Damage,float atack_range,float angle,Vector2 movingvector,float timetodestroy){
		saveddamage=Damage;
		savedatackrange=atack_range;
		savedangle=angle;
		savedmovingvector=movingvector;
		savedtimetodestroy=timetodestroy;
		//Vector3 newPos = Quaternion.AngleAxis(angle, Vector3.forward)* Vector3.up;
	}
	
	void Update () {
		if(currtimetodestroy<savedtimetodestroy)
		{
			
			
			GetComponent<Rigidbody2D>().velocity = -savedmovingvector*1.2f;
			//rigidbody2D.velocity=savedmovingvector*2f;
			//rigidbody2D.velocity=transform.forward*2f;
			currtimetodestroy+=Time.deltaTime;
			Collider2D enemycolider=Physics2D.OverlapCircle(this.transform.position,savedatackrange, 1 << LayerMask.NameToLayer("Enemy"));
			if(enemycolider!=null)
			{
				Debug.Log ("GiveDamage");
				enemycolider.gameObject.GetComponent<EnemyControl>().TakeDamage(saveddamage,3);
				Destroy(this.transform.gameObject);
			}
		}
		else
		{
			Destroy(this.transform.gameObject);
		}
		
	}
	
	
}
//JoaoWeapons2109
