using UnityEngine;
using System.Collections;

public class TowerControl : MonoBehaviour {
	public TowerStatus status;
	private GameObject TowerBar;
	bool can_do = false;





	public void TakeDamage(int damage, int fromid){
		int weakness_multiplier = GlobalData.weakness_towers[this.status.type].Contains(fromid)? 2 : 1;

		status.health -= damage;
		if(status.health <= 0){
			SoundControl.PlaySFX(GlobalData.SFX_Paths[11], false, true, true);
			Destroy (this.gameObject);
		}
		//JoaoBarFollow
		TowerBar.GetComponent<ObjectEnergyBar>().curr_health=status.health;
		TowerBar.GetComponent<ObjectEnergyBar>().update_energy=true;
		//JoaoBarFollow
	}



 
	GameObject Weapon;
	//JoaoWeapons
	public void Init(TowerStatus status){
		this.status = new TowerStatus(status.attack_str, 
		                              status.cooldown_time,
		                              status.attack_range, 
		                              status.health, 
		                              status.type,
		                              status.upgrade_level,
		                              status.weapon_attack_range,
		                              status.weapon_atack_duration);
		this.name = status.type.ToString();
		this.transform.GetChild(0).GetComponent<Animator>().SetInteger("status", status.upgrade_level);
		//this.GetComponent<SpriteRenderer>().sprite= Resources.Load<Sprite>(GlobalData.TOWERSPRITEPATHS[status.type][status.upgrade_level]);
		can_do = true;
		if(TowerBar==null){
		//JoaoBarFollow
		GameObject Tower_Bars=GameObject.Find("InGame").transform.Find("TD").Find("tower_bars").gameObject;
		TowerBar = (GameObject)Instantiate(Resources.Load("Prefabs/ObjectBar"));
		TowerBar.transform.parent=Tower_Bars.transform;
		TowerBar.GetComponent<ObjectEnergyBar>().Init(this.gameObject);
		//JoaoBarFollow
		}
	}
	//JoaoWeapons
	void Start () {
		//this.status.cooldown_time=5f;
		//           GlobalData.TOWERSUPGRADEVALUES[this.status.type][this.status.upgrade_level].health);
	}
	
	void Update () {
		PauseCheck();
		if(can_do && lvl_st == 0){
			Behaviour();
			CoolDown();
		}
	}

	int lvl_st=0;
	private void PauseCheck(){
		if(lvl_st!=PlayerData.level_state){
			if(lvl_st == 0 && PlayerData.level_state==2){
				can_do=false;
			}else if(lvl_st==2 && PlayerData.level_state==0){
				can_do=true;
			}
			lvl_st=PlayerData.level_state;
		}
	}
	
	public void UpgradeTower(){
		int t=this.status.type;


		int ul=this.status.upgrade_level+1;
		if(ul-1 < GlobalData.TOWER_UPGRADE_COSTS[t].Count){
			PlayerData.energy_queue.Add(-GlobalData.TOWER_UPGRADE_COSTS[t][ul-1]);
			Init (GlobalData.TOWERSUPGRADEVALUES[t][ul]);
			//JoaoBarFollow
			TowerBar.GetComponent<ObjectEnergyBar>().max_health=GlobalData.TOWERSUPGRADEVALUES[t][ul].health;
			TowerBar.GetComponent<ObjectEnergyBar>().update_energy=true;
			//JoaoBarFollow
		}
		else{
			Debug.Log ("Cant Upgrade");
		}
		//

	
	}
	
	public bool CanUpgradeLevel(){
		if(this.status.upgrade_level<=2)
			return true;
		else
			return false;
	}
	
	public bool CanRepair(){
		
		if(this.status.health<GlobalData.TOWERSUPGRADEVALUES[this.status.type][this.status.upgrade_level].health)
			return true;
		else
			return false;
	}
	
	public void RepairTower(){
		if(PlayerData.current_energy>GlobalData.TOWER_Repair_COSTS[this.status.type][this.status.upgrade_level]){
			PlayerData.energy_queue.Add(-GlobalData.TOWER_Repair_COSTS[this.status.type][this.status.upgrade_level]);
			this.status.health=GlobalData.TOWERSUPGRADEVALUES[this.status.type][this.status.upgrade_level].health;


			//JoaoBarFollow
			TowerBar.GetComponent<ObjectEnergyBar>().curr_health=status.health;
			TowerBar.GetComponent<ObjectEnergyBar>().update_energy=true;
			//JoaoBarFollow
		}
	}
	
	//JoaoWeapons2109
	void Targetting(){
		
	//	Debug.Log ("Targetting");
		//TOWER RANGE
		Collider2D Enemy = Physics2D.OverlapCircle(transform.position, 5, 1 << LayerMask.NameToLayer("Enemy"));
	

		if(Enemy!=null){
			if(CanTarget(Enemy.gameObject)){
			can_do_action = false;
			
			if(this.status.type==1)
			{
				Weapon = (GameObject)Instantiate(Resources.Load("Prefabs/Weapons/1"));
				Weapon.transform.parent = this.transform;
				Weapon.transform.position=this.transform.position;
				Weapon.transform.GetComponentInChildren<Weapon1Control>().Init(new WeaponStatus(Enemy.gameObject, this.status.attack_str), this.transform.position);
			} else if(this.status.type==2)
			{
				Weapon = (GameObject)Instantiate(Resources.Load("Prefabs/Weapons/2"));
				Weapon.transform.parent = this.transform;
				
				Weapon.transform.position=this.transform.position;
				Weapon.transform.GetComponentInChildren<Weapon2Control>().Init(new WeaponStatus(Enemy.gameObject,this.status.attack_str, this.status.weapon_attack_range,this.status.weapon_atack_duration));
			} else if(this.status.type==3)
			{
				Weapon = (GameObject)Instantiate(Resources.Load("Prefabs/Weapons/3"));
				Weapon.transform.parent = this.transform;
				
				Weapon.transform.position=this.transform.position;
				Weapon.transform.GetComponentInChildren<Weapon3Control>().Init(new WeaponStatus(Enemy.gameObject, this.status.attack_str, this.status.weapon_attack_range,this.status.weapon_atack_duration));
			}else if(this.status.type==4)
			{
				Weapon = (GameObject)Instantiate(Resources.Load("Prefabs/Weapons/4"));
				Weapon.transform.localScale = new Vector3(1+status.weapon_attack_range*2, 
				                                         1+status.weapon_attack_range*2, 
				                                          1+status.weapon_attack_range*2);
				Weapon.transform.parent = this.transform;
					
				Weapon.transform.position=this.transform.position;
					Weapon.transform.GetComponentInChildren<Weapon4Control>().Init(new WeaponStatus(Enemy.gameObject, this.status.attack_str,this.status.weapon_attack_range,this.status.weapon_atack_duration),this.status.upgrade_level);
			}else if(this.status.type==5)
			{
				Weapon = (GameObject)Instantiate(Resources.Load("Prefabs/Weapons/5"));
				Weapon.transform.parent = this.transform;
				
				Weapon.transform.position=this.transform.position;
				Weapon.transform.GetComponentInChildren<Weapon5Control>().Init(new WeaponStatus(Enemy.gameObject,this.status.attack_str, this.status.weapon_attack_range,this.status.weapon_atack_duration));
			}
				SoundControl.PlaySFX(GlobalData.SFX_Paths[status.type == 1 ? 6 : 
				                                            status.type == 2? 7 : 
				                                            status.type == 3? 8 : 9], false, true, true);
		}
		}
	}

	bool CanTarget(GameObject enemy){
		return !enemy.GetComponent<EnemyControl>().ishiding;
	}
	//JoaoWeapons2109
	
	float cooldown_timer = 0;
	bool can_do_action = false; //permission for attacking
	void CoolDown(){
		if(!can_do_action){
			if(cooldown_timer <  this.status.cooldown_time){
				cooldown_timer+=Time.deltaTime;
			}else{
				cooldown_timer = 0;
				can_do_action = true;
			}
		}
	}
	
	void Behaviour(){
		if(can_do_action){
			Targetting();
		}
	}
}
