using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TDEnergyBar : MonoBehaviour {
	float timer=0;
	float time=0.5f;
	// Use this for initialization
	int energy;
	Image bar;
	Text txt;
	bool can_energize=false;
	public void Init() {
		PlayerData.energy_queue.Clear();
		energy = PlayerData.current_energy;
		bar =  this.transform.GetChild(1).GetComponent<Image>();
		bar.fillAmount = energy/100f;
		txt = this.transform.GetChild(2).GetComponent<Text>();
		txt.text = energy.ToString();
		can_energize = true;
		timer =0;
	}
	
	void Update () {
		EnergyBar();
	}


	void EnergyBar(){
		if(can_energize){
		if(PlayerData.energy_queue.Count>0){
			if(timer<time){
				timer+=Time.deltaTime;
				float current_energy = Mathf.Lerp(energy, energy+PlayerData.energy_queue[0], timer/time)/100f;
				bar.fillAmount = current_energy;
				txt.text = Mathf.CeilToInt(current_energy*100).ToString();

			}else{
				energy=Mathf.Clamp(energy+PlayerData.energy_queue[0], 0, 100);
				txt.text = Mathf.CeilToInt(energy).ToString();
				PlayerData.current_energy = energy;
				PlayerData.energy_queue.RemoveAt(0);
				timer=0;

			}
		}
		}
	}


}
