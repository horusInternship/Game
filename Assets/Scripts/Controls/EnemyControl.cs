using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour {
	public EnemyStatus status;
	GameObject EnemyBar, EnemyArrow;
	bool move;
	int ey, ex, edir;
	int nexty, nextx;
	Vector3 nextWorldPos;
	int nsteps=0;//safety measure;

	float initialmovementspeed;
	GameObject UI_Tutorial;
	Animator anim;
	public void Init(EnemyStatus status, int startx, int starty, int edir){
		this.status = new EnemyStatus(status.energy_bonus, 
		                              status.main_attack, 
		                              status.attack_str, 
		                              status.cooldown_time, 
		                              status.health, 
		                              status.movement_speed, 
		                              status.type);
		this.ey = starty + GlobalData.level_y_start;
		this.ex = startx;

		this.transform.position = new Vector3(startx, YWorldPos(ey));
	//	Debug.Log ("EPOSX ON INIT"+ startx);
		this.edir = edir;

		this.nextx = edir==3? startx++ : startx;
		this.nexty = edir==1? ey++ : ey;

		this.nextWorldPos = new Vector3(this.nextx, YWorldPos(this.nexty));
		this.move = true;

		this.name = status.type.ToString();

		if(this.transform.childCount>0){
			anim = this.transform.GetChild(0).GetComponent<Animator>();
		}

		//JoaoBarFollow
		GameObject Enemy_Bars=GameObject.Find("InGame").transform.Find("TD").Find("enemy_bars").gameObject;
		EnemyBar = (GameObject)Instantiate(Resources.Load("Prefabs/ObjectBar"));
		EnemyBar.transform.parent=Enemy_Bars.transform;
		EnemyBar.GetComponent<ObjectEnergyBar>().Init(this.gameObject);
		//JoaoBarFollow

		initialmovementspeed=this.status.movement_speed;
	}


	private void SetAnimDir(int dir){
		if(anim!=null){
			if(dir!=anim.GetInteger("dir")){
				anim.SetInteger("dir", dir);
				transform.rotation=Quaternion.Euler(0,dir==2?180:0,0);
			}
		}
	}
	private void SetAnimStatus(int status){
		if(anim!=null){
			if(status!=anim.GetInteger("status")){
				anim.SetInteger("status", status);
			}
		}
	}

	void Start () {
		UI_Tutorial=GameObject.Find("Canvas").transform.Find("InGame").transform.Find("Tutorial").gameObject;

		Debug.Log (this.status.type);
		if(this.status.type==0)
		{
			GameObject.Find("Canvas").transform.Find("InGame").transform.Find("DarkServerAlert").gameObject.SetActive(true);
			StartCoroutine(ActionAfterTimer.Set(3, delegate {
				GameObject.Find("Canvas").transform.Find("InGame").transform.Find("DarkServerAlert").gameObject.SetActive(false);
			}));
		}

		//JoaoEnemyArrow
		EnemyArrow=this.transform.Find("Arrow").gameObject;
		//JoaoEnemyArrow
	}

	bool tutatack=false;

	void FixedUpdate () {
		//JoaoEnemyArrow
		CheckIfVisible();
		//JoaoEnemyArrow

		PauseCheck();
		if(lvl_st==0){
			Move ();
			Behaviour();
			CoolDown();
		
		}

		if(!tutatack && UI_Tutorial.activeSelf && GlobalData.enemyatacktower_tut)
		{
			Targetting();
			tutatack=true;
			GameObject.FindGameObjectWithTag("TD_Level").GetComponent<TDLevelControl>().ActedOnTutorial();
		}
	}



	//JoaoEnemyArrow

	float xarrowposvalue,yarrowposvalue;
	void CheckIfVisible(){
		Vector3 EnemyPos=Camera.main.WorldToViewportPoint(this.transform.position);
		
		if(EnemyPos.x<1f && EnemyPos.x>0f && EnemyPos.y<1f && EnemyPos.y>0f)
		{
			if(EnemyArrow.GetComponent<SpriteRenderer>().enabled)
				EnemyArrow.GetComponent<SpriteRenderer>().enabled=false;
		//	Debug.Log (this.gameObject.name+" inview ");
		}
		else
		{
			if(!EnemyArrow.GetComponent<SpriteRenderer>().enabled)
				EnemyArrow.GetComponent<SpriteRenderer>().enabled=true;
			
			if(EnemyPos.x>0.95f)
				xarrowposvalue=0.95f;
			else if(EnemyPos.x<0.05f)
				xarrowposvalue=0.05f;
			else
				xarrowposvalue=EnemyPos.x;
			
			if(EnemyPos.y>0.95f)
				yarrowposvalue=0.95f;
			else if(EnemyPos.y<0.05f)
				yarrowposvalue=0.05f;
			else
				yarrowposvalue=EnemyPos.y;
			
			Vector3 ArrowPos=new Vector3(xarrowposvalue,yarrowposvalue,10);
			EnemyArrow.transform.position=Camera.main.ViewportToWorldPoint(ArrowPos);
			
			
			float angle = Mathf.Rad2Deg*Mathf.Atan2(EnemyArrow.transform.position.x-this.transform.position.x, EnemyArrow.transform.position.y-this.transform.position.y);
			
			
			
			if(EnemyPos.x>1 || EnemyPos.x<0)
			{
				if(EnemyPos.y>0 && EnemyPos.y<1)
					EnemyArrow.transform.rotation = Quaternion.Euler(0,0, 180+angle);
				else 
				{
					float changefixedangle=0;
					if(EnemyPos.y>1)
						changefixedangle=1;
					else
						changefixedangle=-1;
					if(EnemyPos.x<0)
						EnemyArrow.transform.rotation = Quaternion.Euler(0,0, changefixedangle*90+angle);
					else
						EnemyArrow.transform.rotation = Quaternion.Euler(0,0, changefixedangle*-90+angle);
				}
			}
			else	
				EnemyArrow.transform.rotation = Quaternion.Euler(0,0, angle);
			
		}
	}
	//JoaoEnemyArrow




	int lvl_st=0;
	private void PauseCheck(){
		if(lvl_st!=PlayerData.level_state){
			if(lvl_st == 0 && PlayerData.level_state==2){
				Stop();
			}else if(lvl_st==2 && PlayerData.level_state==0){
				UnStop();
			}
			lvl_st=PlayerData.level_state;
		}
	}

	//void EnemyPaused(){
		//rigidbody2D.velocity = Vector2.zero;
		//GetComponent<Animator>().speed = 0;
	//}

	void Move(){
		if(move){
			if(HasReachedNextPos()){
				SetNextDirPos();
				SoundControl.PlaySFX(GlobalData.SFX_Paths[status.type == 0 ? 3 : 5 ], false, true, true);
				 
			}

			if(this.status.movement_speed!=initialmovementspeed)
				UnStop_AfterTime();
		}
			
	}

	Vector2 v_before_stop = Vector2.zero;
	public void Stop(){
		v_before_stop = this.GetComponent<Rigidbody2D>().velocity;
		this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		Debug.Log ("Stopping "+status.type);
		SoundControl.StopSound(GlobalData.SFX_Paths[status.type == 0 ? 3 : 5 ]);
		//TODO stop behaviour cycle too
	}

	private float unstop_timer = 0;
	private float unstop_time = 0;
	private bool unstop_after_time=false;
	public void Stop(int unstop_time,float reducedenemymovement){
		v_before_stop = this.GetComponent<Rigidbody2D>().velocity;
		this.unstop_timer = 0;
		this.unstop_time = unstop_time;
		this.unstop_after_time=true;
		this.status.movement_speed=initialmovementspeed*reducedenemymovement;
		SoundControl.StopSound(GlobalData.SFX_Paths[status.type == 0 ? 3 : 5 ]);
		//Stop();
	}

	private void UnStop_AfterTime(){
		if(unstop_after_time){
			if(unstop_timer<unstop_time){
				unstop_timer+=Time.fixedDeltaTime;
			}else{
				unstop_after_time = false;
				unstop_timer = 0;
				UnStop();
			}

		}
	}

	public void UnStop(){
		this.GetComponent<Rigidbody2D>().velocity=v_before_stop;
		this.status.movement_speed=initialmovementspeed;
		SoundControl.PlaySFX(GlobalData.SFX_Paths[status.type == 0 ? 3 : 5 ], false, true, true);
		//TODO stop behaviour cycle too
	}

	void Targetting(){
	
		Collider2D tower = Physics2D.OverlapCircle(transform.position, 2, 1 << LayerMask.NameToLayer("Tower"));
		if(tower!=null){
			can_do_action = false;
			 Debug.Log ("  KILLER RAY TO THE TOWER! KEKEK");
			GameObject weapon = (GameObject)Instantiate(Resources.Load("Prefabs/Weapons/1"));
			weapon.transform.parent = this.transform;
			weapon.transform.position=this.transform.position;
			weapon.GetComponent<Weapon1Control>().Init(new WeaponStatus(tower.gameObject, status.attack_str), this.transform.position);
			SetAnimStatus(1);
			StartCoroutine(ActionAfterTimer.Set(0.25f, delegate {
				SetAnimStatus(0);
			}));
		//	weapon.transform.GetComponentInChildren<Weapon1Control>().Init();
		}
	}
	public bool ishiding = false;
	void Hiding(){
		can_do_action = false;
		ishiding = !ishiding;
		GetComponent<Collider2D>().enabled = !GetComponent<Collider2D>().enabled ;
		if(anim.GetInteger("status")==0){
			SetAnimStatus(1);
			//GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
		}else if(anim.GetInteger("status")==1 && anim.GetCurrentAnimatorStateInfo(0).IsName("New Layer.4_underground")){
			SetAnimStatus(4);
			StartCoroutine(ActionAfterTimer.Set(0.25f, delegate{
				SetAnimStatus(0);
			}));
		}
	}
	void Behaviour(){
		if(can_do_action){
			if(status.type == 0 || status.type == 5 || status.type == 6){
				Targetting();	
			}else if(status.type == 4){
				Hiding();
			}
		}

		ReachedMainTower();
	}

	void ReachedMainTower(){
		Collider2D mt = Physics2D.OverlapCircle(transform.position, 0.25f, 1 << LayerMask.NameToLayer("MainTower"));
		if(mt!=null){
		//	Debug.Log (status.main_attack);
			//PlayerData.current_energy -= status.main_attack;
			PlayerData.energy_queue.Add(-status.main_attack);

			if(PlayerData.main_tower!=null){
				PlayerData.main_tower.GetComponent<Animator>().SetTrigger("damage");
			}
			this.transform.parent = null;
			Destroy (this.gameObject);
		}
	}

	void SetNextDirPos(){
		ex = nextx;
		ey = nexty;
		if(!GlobalData.map[nextx, nexty].Equals("g")){
		//while(!GlobalData.map[ex,ey].Equals("x") && !GlobalData.map[ex,ey].Equals("g") && nsteps<1000){
			if(GlobalData.TILEDIR.ContainsKey(GlobalData.map[nextx,nexty])){
				bool u = GlobalData.TILEDIR[GlobalData.map[nextx,nexty]][0];
				bool d = GlobalData.TILEDIR[GlobalData.map[nextx,nexty]][1];
				bool l = GlobalData.TILEDIR[GlobalData.map[nextx,nexty]][2];
				bool r = GlobalData.TILEDIR[GlobalData.map[nextx,nexty]][3];
				
				if(edir==0){ //going up
					if(r){
						edir = 3;
						nextx++;
					}else if(u){
						nexty--;
					}else if(l){
						edir = 2;
						nextx--;
					}
				}else if(edir == 1){ //going down
					if(d){
						nexty++;
					}else if(r){
						edir = 3;
						nextx++;
					}else if(l){
						edir = 2;
						nextx--;
					}
				}else if(edir == 2){ //going left
					if(d){
						edir = 1;
						nexty++;
					}else if(l){
						nextx--;
					}else if(u){
						edir = 0;
						nexty--;
					}
				}else if(edir == 3){ //going right
					if(r){
						nextx++;
					}else if(d){
						edir = 1;
						nexty++;
					}else if(u){
						edir = 0;
						nexty--;
					}
				}
			}
		//	Debug.Log (GlobalData.map[nextx,nexty] + " at pos(" +nextx+ ", "+ nexty+") edir: "+edir );
			nsteps++;

			nextWorldPos = new Vector3(nextx, YWorldPos(nexty));
			SetAnimDir(edir);
			SetMovementSpeed();
		//}
		}
	}


	bool HasReachedNextPos(){
		return Vector3.Distance(this.transform.position, nextWorldPos) < Mathf.Clamp(0.05f*status.movement_speed, 0.05f, 999);
	}
	void SetMovementSpeed(){
		this.GetComponent<Rigidbody2D>().velocity = new Vector2(nextWorldPos.x-transform.position.x, 
		                                        nextWorldPos.y - transform.position.y).normalized * status.movement_speed;
	}
	float YWorldPos(int y){
	//	return (GlobalData.difficulty_ymap_size[GlobalData.current_difficulty]- y );
	//	Debug.Log (y + " | ylevelstart "+ GlobalData.level_y_start+ " | ylevelend " + GlobalData.level_y_end);
		return (GlobalData.level_y_end - y );
	}
 
	float cooldown_timer = 0;
	bool can_do_action = false; //permission for attacking
	void CoolDown(){
		if(!can_do_action){
			if(cooldown_timer < status.cooldown_time){
				cooldown_timer+=Time.fixedDeltaTime;
			}else{
				cooldown_timer = 0;
				can_do_action = true;
			}
		}
	}

	public bool isdead = false;
	public void TakeDamage(int damage, int fromid){
		if(!ishiding){
		int weakness_multiplies = GlobalData.weakness_enemies[status.type].Contains(fromid)? 2 : 1;

		status.health -= damage;
		SetAnimStatus(2);

		if(status.health<=0){
			SoundControl.PlaySFX(GlobalData.SFX_Paths[status.type == 0 ? 2 : 4 ], false, true, true);
			if(status.type == 0){
				isdead = true;
				Stop();
				//TODO Start Defeated Animation
				SetAnimStatus(3);
				PlayerData.energy_queue.Add(this.status.energy_bonus);
				PlayerData.current_score += this.status.energy_bonus;
				
				
			}else{
				//TODO START Destroyed Animation
				Stop ();
				SetAnimStatus(3);


				//Give energy
					GameObject EnergyParticle = (GameObject)Instantiate(Resources.Load("Prefabs/EnergyParticle"));
					EnergyParticle.transform.position=this.transform.position;
					//EnergyParticle.GetComponent<energyparticle>().Init(this.status.energy_bonus);

				StartCoroutine(ActionAfterTimer.Set(0.5f, delegate {
						PlayerData.energy_queue.Add(this.status.energy_bonus);
						PlayerData.current_score += this.status.energy_bonus;
					Destroy (this.gameObject);
				}));
			
			}

		}else{
			StartCoroutine(ActionAfterTimer.Set(0.5f, delegate { //after being hit go back to walking
				SetAnimStatus(0);
			}));
		}

		}
		//JoaoBarFollow
		EnemyBar.GetComponent<ObjectEnergyBar>().curr_health=status.health;
		EnemyBar.GetComponent<ObjectEnergyBar>().update_energy=true;
		//JoaoBarFollow
	}
}
