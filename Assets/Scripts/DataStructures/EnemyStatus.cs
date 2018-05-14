using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnemyStatus {

	public int energy_bonus = 5;
	public int main_attack = 20;
	public int attack_str = 10;
	public float cooldown_time = 1;
	public int health = 100;
	public float movement_speed = 1;

	public int type = 0;


	public EnemyStatus(int energy_bonus, int main_attack, int attack_str, float cooldown_time, int health, float movement_speed, int type){
		this.energy_bonus = energy_bonus;
		this.main_attack = main_attack;
		this.attack_str = attack_str;
		this.cooldown_time = cooldown_time;
		this.health = health;
		this.movement_speed = movement_speed;
		this.type = type;
	}
}
