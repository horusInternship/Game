using UnityEngine;
using System.Collections;

public class TowerStatus {
	
	public int attack_str = 10;
	public float cooldown_time = 2;
	public int attack_range = 1;
	public int health = 100;
	//TODO Weaknesses
	public int type = 3;
	public int upgrade_level = 0;
	
	//JoaoWeapons
	public float weapon_attack_range;
	public float weapon_atack_duration;
	
	
	public TowerStatus(int attack_str, float cooldown_time, int attack_range, int health, int type, int upgrade_level,float weapon_attack_range,float weapon_atack_duration){
		this.attack_str = attack_str;
		this.cooldown_time = cooldown_time;
		this.attack_range = attack_range;
		this.health = health;
		this.type = type;
		this.upgrade_level =upgrade_level;
		this.weapon_attack_range =weapon_attack_range;
		this.weapon_atack_duration=weapon_atack_duration;
	}
}
