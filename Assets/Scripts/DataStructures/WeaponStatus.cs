using UnityEngine;
using System.Collections;

public class WeaponStatus : MonoBehaviour {

	public int damage = 10;
	public GameObject target;
	public float attack_range=1;
	public float attack_duration=1;
	
	public WeaponStatus(GameObject target, int damage){
		this.target = target; 
		this.damage = damage;
	}



	public WeaponStatus(GameObject target, int damage, float attack_range, float attack_duration){
		this.target = target;
		this.damage = damage;
		this.attack_range = attack_range;
		this.attack_duration = attack_duration;
	}



}
