using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TextLoader : MonoBehaviour {
	public int index=0;
	private int initial_fontsize=0;
	private bool updated=false,updated2=false;
 	

	void Update(){
		if(index!=-1)
			LoadText();
		AdjustTextScale();
	}
	
	void LoadText(){
		if(!updated){
			if(GlobalData.Languages.Count>index ){
				Debug.Log(GlobalData.Languages[index]+" "+Screen.dpi);

				this.GetComponent<Text>().text = GlobalData.Languages[index];
				//this.GetComponent<Text>().fontSize=(int) (this.GetComponent<Text>().fontSize*GetDpi());
				//this.GetComponent<Text>().fontSize=(int) (this.GetComponent<Text>().fontSize*Screen.width/resolutionx());


				//this.GetComponent<Text>().fontSize=(int) (this.GetComponent<Text>().fontSize*Screen.width/resolution());
				//int scale_value=(int) (this.GetComponent<RectTransform>().localScale.x*Screen.width/resolution());
				//this.GetComponent<RectTransform>().localScale =new Vector3(scale_value,scale_value,scale_value);

				Debug.Log("here loadtext "+this.GetComponent<Text>().fontSize*GetDpi());
				updated = true;			
			}
		}
	}
	
	//JoaoAdjustTextScale
	void AdjustTextScale(){
		if(!updated2){
			this.GetComponent<Text>().fontSize=(int) (this.GetComponent<Text>().fontSize*GetDpi());
			this.GetComponent<Text>().fontSize=(int) (this.GetComponent<Text>().fontSize*Screen.width/resolutionx());

			updated2 = true;	
		}
	}

	/*private float GetDpi(){
		if(Screen.dpi >=0 && Screen.dpi < 160){
			return  0.75f; 
		}else if(Screen.dpi >=160 && Screen.dpi < 250){
			return 1f;
		}else if(Screen.dpi >=250 && Screen.dpi < 330){
			return 1.25f;
		}else if(Screen.dpi >=330 && Screen.dpi < 490){
			return 2f;
		}else if(Screen.dpi >=490 && Screen.dpi < 650){
			return 3f;
		}else if(Screen.dpi >=650){
			return 3f;
		}else return 1;
	}*/


	private float GetDpi(){
		Debug.Log (Screen.dpi);
		if(Screen.dpi >=0 && Screen.dpi < 160){   //0 - 0.65      // normal resolution 480
			return  0.60f; 
		}else if(Screen.dpi >=160 && Screen.dpi < 200){  //1  -1  // ipad mini
			return 0.8f;
		}else if(Screen.dpi >=200 && Screen.dpi < 250){  //1  -1
			return 1f;
		}else if(Screen.dpi >=250 && Screen.dpi < 300){ //2  -1.25  // iPad Retina 
			return 1.25f;
		}else if(Screen.dpi >=300 && Screen.dpi < 335){  //2  -1.25  //iphone 4,5,6
			return 1.35f;
		}else if(Screen.dpi >=335 && Screen.dpi < 400){  //3 2 
			return 1.55f;
		}else if(Screen.dpi >=400 && Screen.dpi < 450){  // 3 2 // iPhones 6 plus, Galaxy S4, Galaxy S5
			return 1.85f;
		}else if(Screen.dpi >=450 && Screen.dpi < 490){  // 3 2 // HTC One
			return 2.25f;
		}else if(Screen.dpi >=490 && Screen.dpi < 550){  // 4 3
			return 2.5f;  
		}else if(Screen.dpi >=550 && Screen.dpi < 600){  // 4 3 // Galaxy S6
			return 2.85f; //before 3   // Testing 2.5 3 3.5
		}else if(Screen.dpi >=650){
			return 4f;
		}else return 1;
	}

	private float resolutionx(){
		if(Screen.dpi >=0 && Screen.dpi < 160){   //0 - 0.65      // normal resolution 480
			return  480f; 
		}else if(Screen.dpi >=160 && Screen.dpi < 200){  //1  -1  // ipad mini
			return 600f;
		}else if(Screen.dpi >=200 && Screen.dpi < 250){  //1  -1
			return 800f;
		}else if(Screen.dpi >=250 && Screen.dpi < 300){ //2  -1.25  // iPad Retina 
			return 960f;
		}else if(Screen.dpi >=300 && Screen.dpi < 335){  //2  -1.25  //iphone 4,5,6
			return 1280f;
		}else if(Screen.dpi >=335 && Screen.dpi < 400){  //3 2 
			return 1280f;
		}else if(Screen.dpi >=400 && Screen.dpi < 450){  // 3 2 // iPhones 6 plus, Galaxy S4, Galaxy S5
			return 1920f;
		}else if(Screen.dpi >=450 && Screen.dpi < 490){  // 3 2 // HTC One
			return 1920f;
		}else if(Screen.dpi >=490 && Screen.dpi < 550){  // 4 3
			return 2560f;  
		}else if(Screen.dpi >=550 && Screen.dpi < 600){  // 4 3 // Galaxy S6
			return 2560f; //before 3   // Testing 2.5 3 3.5
		}else if(Screen.dpi >=650){
			return 2560f;
		}else return 2560f;
	}
	
	private float resolutiony(){
		if(Screen.dpi >=0 && Screen.dpi < 160){   //0 - 0.65      // normal resolution 480
			return  320f; 
		}else if(Screen.dpi >=160 && Screen.dpi < 200){  //1  -1  // ipad mini
			return 600f;
		}else if(Screen.dpi >=200 && Screen.dpi < 250){  //1  -1
			return 800f;
		}else if(Screen.dpi >=250 && Screen.dpi < 300){ //2  -1.25  // iPad Retina 
			return 960f;
		}else if(Screen.dpi >=300 && Screen.dpi < 335){  //2  -1.25  //iphone 4,5,6
			return 1280f;
		}else if(Screen.dpi >=335 && Screen.dpi < 400){  //3 2 
			return 1280f;
		}else if(Screen.dpi >=400 && Screen.dpi < 450){  // 3 2 // iPhones 6 plus, Galaxy S4, Galaxy S5
			return 1920f;
		}else if(Screen.dpi >=450 && Screen.dpi < 490){  // 3 2 // HTC One
			return 1920f;
		}else if(Screen.dpi >=490 && Screen.dpi < 550){  // 4 3
			return 2560f;  
		}else if(Screen.dpi >=550 && Screen.dpi < 600){  // 4 3 // Galaxy S6
			return 2560f; //before 3   // Testing 2.5 3 3.5
		}else if(Screen.dpi >=650){
			return 2560f;
		}else return 2560f;
	}
}
