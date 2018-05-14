using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FontSizeController : MonoBehaviour {
	private Text t;
	
	public int font_type = 1;
	public bool adjust_to_scale = false;
	public float parent_scale = 0.65f;
	private void Start(){
		//t = this.GetComponent<Text>();
		this.GetComponent<Text>().fontSize=(int) (this.GetComponent<Text>().fontSize*GetDpi());

		this.GetComponent<Text>().fontSize=(int) (this.GetComponent<Text>().fontSize*Screen.width/resolutionx());

		//float changeanchooredminx=0.5f-this.GetComponent<RectTransform>().offsetMin.x;
		//float changeanchooredmaxx=1f-this.GetComponent<RectTransform>().offsetMax.x;

		//this.GetComponent<RectTransform>().offsetMin = new Vector2(this.GetComponent<RectTransform>().offsetMin.x, offsetymin);
		//this.GetComponent<RectTransform>().offsetMax = new Vector2(this.GetComponent<RectTransform>().offsetMax.x, offsetymax);


		/*float scale_valuex=Screen.width/resolutionx();
		float scale_valuey=Screen.height/resolutiony();
		this.GetComponent<RectTransform>().localScale =new Vector3(scale_valuex,scale_valuey,1);
		this.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,0);*/
	} 

	/*
	private float GetDpi(){
		  if(Screen.dpi >=0 && Screen.dpi < 160){
			return 1f;//0.75f;
		}else if(Screen.dpi >=160 && Screen.dpi < 250){
			return 1;
		}else if(Screen.dpi >=250 && Screen.dpi < 330){
			return 2f;
		}else if(Screen.dpi >=330 && Screen.dpi < 490){
			return 2.5f;
		}else if(Screen.dpi >=490 && Screen.dpi < 650){
			return 2.5f;
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
