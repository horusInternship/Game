using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	public bool touch_control = false;
	
	private Vector2 startpos;
	private Vector3 intro_start_pos;
	private Vector3 intro_end_pos;
	private Vector3 old_mousepos;
	private float minx=6, maxx=8;
	private float miny=5, maxy=10;
	
	private float saveminx=6, savemaxx=8;
	private float saveminy=5, savemaxy=10;
	
	//JoaoZoomChangeOrientation
	private float savedmapsize_x;
	private float ar,oldar;
	//JoaoZoomChangeOrientation
	//JoaoZoom
	private float minmagnitude=2.4f, maxmagnitude=5f; //6.5 Squares 12 Squares
	private float minmagnitudeportrait=6f, maxmagnitudeportrait=12f;
	public float ZoomSpeed = 1f,MovementSpeed=1f; 
	private float currmagnitude=5f;

	private int PrevNumTouch=0;
	//JoaoZoom

	GameObject UI_Tutorial;

	//NewZoom
	float minOrtosize=0,maxOrtosize=0;
	//NewZoom
	
	private bool canmove=false;
	private bool intro_anim = false;
	private int intro_state = 0;
	private float intro_timer = 0;
	private void OnGUI(){
		//Vector3 vpp = Camera.main.WorldToViewportPoint(new Vector3(0, GlobalData.difficulty_ymap_size[GlobalData.current_difficulty], 0));
		//GUI.Label(new Rect(0,0, Screen.width, Screen.height), "vpp y "+ vpp.y);
	}
	public void Init(int mapsize_x, int mapsize_y, Vector3 start_pos){
		GlobalData.introlevelanim=true;
		UI_Tutorial=GameObject.Find("Canvas").transform.Find("InGame").transform.Find("Tutorial").gameObject;
		mapsize_x=GlobalData.dificulty_xmap_size;
		savedmapsize_x=mapsize_x;
		float ar = this.GetComponent<Camera>().aspect/1.5f;
		oldar=ar;
		minx = 5*ar;
		maxx = (mapsize_x)-minx;
		miny = 5*ar;



		this.transform.position=new Vector3(0,0,-10);

		Vector3 VPPmin=Camera.main.WorldToViewportPoint(new Vector3(-4f,0,0));
		Vector3 VPPmax=Camera.main.WorldToViewportPoint(new Vector3(-16f,0,0));
		if(mapsize_y==12)
		{
			VPPmax=Camera.main.WorldToViewportPoint(new Vector3(-6f,0,0));
		}
		else if(mapsize_y==16)
		{
			VPPmax=Camera.main.WorldToViewportPoint(new Vector3(-8f,0,0));
		}
		else if(mapsize_y==32)
		{
			VPPmax=Camera.main.WorldToViewportPoint(new Vector3(-8f,0,0));
		}
		
		while(VPPmax.x<=0)
		{
			Camera.main.orthographicSize+=0.01f;
			if(VPPmin.x<=0)
			{
				minOrtosize=Camera.main.orthographicSize;
			}
			VPPmin=Camera.main.WorldToViewportPoint(new Vector3(-4f,0,0));
			if(mapsize_y==12)
			{
				VPPmax=Camera.main.WorldToViewportPoint(new Vector3(-6f,0,0));
			}
			else if(mapsize_y==16)
			{
				VPPmax=Camera.main.WorldToViewportPoint(new Vector3(-8f,0,0));
			}
			else if(mapsize_y==32)
			{
				VPPmax=Camera.main.WorldToViewportPoint(new Vector3(-8f,0,0));
			}

		}
		maxOrtosize=Camera.main.orthographicSize;
		Debug.Log (minOrtosize+" min max orto "+maxOrtosize);





		while(VPPmin.x>0)
		{
			Camera.main.orthographicSize-=0.01f;
			VPPmin=Camera.main.WorldToViewportPoint(new Vector3(-4f,0,0));	
		}
		minOrtosize=Camera.main.orthographicSize;

		Debug.Log (minOrtosize+" min max orto "+maxOrtosize);

		Camera.main.orthographicSize=3;

		float dpivalue=GetDpiCamera();
		
		maxy = (mapsize_y==12? 8 : 
				mapsize_y==16? 12 : 
		        mapsize_y == 32? 28 :
		        mapsize_y == 64? 60 : 100);

		/*maxmagnitude=(mapsize_y==12? 3.9f*dpivalue*1.5f/this.camera.aspect: 
		              mapsize_y==16? 5.2f*dpivalue*1.5f/this.camera.aspect: 
		              mapsize_y == 32? 6.7f*dpivalue*1.5f/this.camera.aspect: 
		              mapsize_y == 64? 11f*dpivalue*1.5f/this.camera.aspect:  10f);

		maxmagnitudeportrait=(mapsize_y==12? 8.8f*dpivalue*1.5f/this.camera.aspect : 
		                      mapsize_y==16? 12f*dpivalue*1.5f/this.camera.aspect : 
		                      mapsize_y == 32? 16f*dpivalue*1.5f/this.camera.aspect :
		                      mapsize_y == 64? 20f*dpivalue*1.5f/this.camera.aspect : 22f*dpivalue*1.5f/this.camera.aspect);*/


		//6

		Debug.Log (maxmagnitude+" "+maxmagnitudeportrait+" "+Screen.dpi);

		//intro_start_pos = new Vector3(maxx/2f, 3.22f, -10);
		intro_start_pos=new Vector3(start_pos.x-2f, 3.22f, -10);
		//intro_start_pos = new Vector3(start_pos.x, 3.22f, -10);

		float endposy=(mapsize_y==12? 10f : 
		                mapsize_y==16? 14f : 
		                mapsize_y == 32? 30f :
		                mapsize_y == 64? 58.5f : 100);
		intro_end_pos = new Vector3(mapsize_x/2, endposy, -10);

		if(GlobalData.current_level==1 && GlobalData.current_difficulty==0)
			intro_end_pos = new Vector3(intro_end_pos.x, intro_end_pos.y+1f, -10);

		transform.position = intro_start_pos;
		intro_timer = 0;
		intro_anim = true;
		
		saveminx=minx;
		saveminy=miny;
		savemaxx=maxx;
		savemaxy=maxy;
		
		canmove = false;
	}


	private float GetDpiCamera(){
				if(Screen.dpi >=0 && Screen.dpi < 160){
					return  1f; 
		}else if(Screen.dpi >=160 && Screen.dpi < 250){
			return 0.9f;
		}else if(Screen.dpi >=250 && Screen.dpi < 330){
			return 0.8f;
		}else if(Screen.dpi >=330 && Screen.dpi < 490){
			return 0.7f;
		}else if(Screen.dpi >=490 && Screen.dpi < 650){
			return 0.6f;
		}else if(Screen.dpi >=650){
			return 0.6f;
		}else return 1;
	}
	
	//JoaoZoom
	private void Start(){
		MovementSpeed=(Camera.main.orthographicSize)/(maxmagnitude-minmagnitude)+0.01f;
	}


	bool started_followingDarkServer=false,endedfollowingdarkserver=false,addedadjustarrow=false;
	float savedOrto=0f,savedtimescale=0f,timetofollow=2.5f,currtimefollowing=0f;

	private void Update(){
		
		
		if(canmove){
			Movement();
			/*if (Input.touchCount == 2)
			{
				PrevNumTouch=2;
				CheckZoom();
			}
			else
			{
				if(PrevNumTouch!=2)
					Movement();
			}*/
		}else{
			IntroCamMove();
			//canmove=true;
		}

		if(PrevNumTouch==2 && Input.touchCount==0)
		{
			PrevNumTouch=0;

		}

		if(UI_Tutorial!=null)
		{
			if(UI_Tutorial.activeSelf && GlobalData.TutCanBuildFree && GlobalData.current_tutorial==7 && !endedfollowingdarkserver)
			{


				if(GameObject.Find("TD_Level(Clone)").gameObject.transform.Find("Enemies").Find("0")!=null){
					if(!started_followingDarkServer)
					{
						savedOrto=Camera.main.orthographicSize;
						Camera.main.orthographicSize=2;
						savedtimescale=Time.timeScale;
						Time.timeScale=0.33f;
						started_followingDarkServer=true;
					}
					else
					{
						if(currtimefollowing<timetofollow)
						{
							float valuex=UI_Tutorial.transform.Find("Arrows").GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition.x;
							float valuey=UI_Tutorial.transform.Find("Arrows").GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition.y;
							if(!addedadjustarrow)
							{
								valuex+=150*Screen.width/561;
								valuey-=100*Screen.width/561;
								addedadjustarrow=true;
							}
							UI_Tutorial.transform.Find("Arrows").GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition=new Vector2(valuex,valuey);
							//Todo Center camera in dark server, red lights and 3 secondslowdown
							Vector3 DarkServerPos=GameObject.Find("TD_Level(Clone)").gameObject.transform.Find("Enemies").gameObject.transform.Find("0").gameObject.transform.Find("Enemy").transform.position;
							Camera.main.transform.position= new Vector3 (DarkServerPos.x,DarkServerPos.y,-10);
							currtimefollowing+=Time.deltaTime;
						}
						else
						{
							Camera.main.orthographicSize=savedOrto;
							Time.timeScale=savedtimescale;
							endedfollowingdarkserver=true;
						}
					}

				}
			}
		}
		
	}
	
	private void IntroCamMove(){
		if(intro_anim){
			if(intro_state == 0){
				if(intro_timer < 2f){
					intro_timer+=Time.deltaTime; 
				}else{
					intro_state = 1;
					intro_timer = 0;
				}
			}else if(intro_state == 1){
				if(intro_timer < 5){
					intro_timer+=Time.deltaTime;

						transform.position = Vector3.Lerp(intro_start_pos, intro_end_pos, intro_timer/5f);

					this.GetComponent<Camera>().orthographicSize = Mathf.Lerp(3, maxmagnitude, (-2/5f)+(intro_timer/5f));
				}else{
					intro_state = 2;
					intro_timer = 0;
				}
			}else if(intro_state == 2){
				if(intro_timer < 1){
					intro_timer+=Time.deltaTime; 
				}else{
					intro_state = 0;
					intro_timer = 0;
					intro_anim = false;
					GlobalData.introlevelanim=false;
					canmove=true;
					currmagnitude=Camera.main.orthographicSize;
				}
			}
			
		}
	}
	
	
	private void Movement(){
		//if (Input.touchCount == 1 && (!UI_Tutorial.activeSelf || GlobalData.TutCanBuildFree) && PlayerData.level_state==0)
		//if((!UI_Tutorial.activeSelf || GlobalData.TutCanBuildFree) && PlayerData.level_state==0)
		if ((!UI_Tutorial.activeSelf || GlobalData.TutCanBuildFree) && PlayerData.level_state==0)
		{
			if(canmove){
				if(Input.GetMouseButtonDown(0)){
					startpos = Input.mousePosition;
					old_mousepos = Input.mousePosition;
				}else if(Input.GetMouseButton(0)){
					//Check if mouse position is different
					
					Vector2 diff_pos = new Vector2((Input.mousePosition.x - startpos.x)/Screen.width, (Input.mousePosition.y - startpos.y)/Screen.height);
					
					//JoaoZoomChangeOrientation
					ar= this.GetComponent<Camera>().aspect/1.5f; 
					if(oldar!=ar)
					{
						saveminx=saveminx*ar/oldar;
						savemaxx=(savedmapsize_x)-saveminx;
						oldar=ar;
					}
					//JoaoZoomChangeOrientation
					
					minx=saveminx-5.5f+Camera.main.orthographicSize;
					maxx=savemaxx+6f-Camera.main.orthographicSize;
					
					miny=saveminy-5.5f+Camera.main.orthographicSize;
					maxy=savemaxy+6f-Camera.main.orthographicSize;
					
					
					float newx = transform.position.x - diff_pos.x*8.2f;
					float newy = transform.position.y - diff_pos.y*8.2f;
					
					
					
					if(transform.position.x<minx)
					{
						if(newx<transform.position.x)
							newx=transform.position.x;
					}
					else 
					{
						if(newx<minx)
							newx=minx;
					}
					
					if(transform.position.x>maxx)
					{
						if(newx>transform.position.x)
							newx=transform.position.x;
					}
					else 
					{
						if(newx>maxx)
							newx=maxx;
					}
					if(transform.position.y<miny)
					{
						if(newy<transform.position.y)
							newy=transform.position.y;
					}
					else 
					{
						if(newy<miny)
							newy=miny;
					}
					if(transform.position.y>maxy)
					{
						if(newy>transform.position.y)
							newy=transform.position.y;
					}
					else 
					{
						if(newy>maxy)
							newy=maxy;
					}
					
					
					
					transform.position = new Vector3(newx,newy,-10f);
					startpos = Input.mousePosition;
					
				}
			}
		}
	}
	
	
	
	
	
	private void CheckZoom(){
		if ((!UI_Tutorial.activeSelf || GlobalData.TutCanBuildFree)&& PlayerData.level_state==0)
		{
			//if (Input.touchCount == 2)
			//{
				Touch touchZero = Input.GetTouch(0);
				Touch touchOne = Input.GetTouch(1);
				
				Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
				Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
				
				float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
				float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
				
				float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
				
				//ZoomSpeed=(Camera.main.orthographicSize-minmagnitude)/((maxmagnitude-minmagnitude)*100f)+0.02f; //is to fast increase 15 value
				ZoomSpeed=0.01f;
				
				
				currmagnitude+=deltaMagnitudeDiff* ZoomSpeed;
				/*if(currmagnitude<minmagnitude)
					currmagnitude=minmagnitude;
				if(currmagnitude>maxmagnitude)
					currmagnitude=maxmagnitude;*/


				float vminmagnitude,vmaxmaginitude;
				if(GlobalData.orientationlandscape)
				{
					vminmagnitude=minOrtosize;
					vmaxmaginitude=maxOrtosize;
				}
				else
				{
					vminmagnitude=minOrtosize;              
					vmaxmaginitude=maxOrtosize;
				}
				if(Camera.main.orthographicSize<vminmagnitude)
				{
					if(currmagnitude<Camera.main.orthographicSize)
						currmagnitude=Camera.main.orthographicSize;
				}
				else 
				{
					if(currmagnitude<vminmagnitude)
						currmagnitude=vminmagnitude;
				}

				if(Camera.main.orthographicSize>vmaxmaginitude)
				{
					if(currmagnitude>Camera.main.orthographicSize)
						currmagnitude=Camera.main.orthographicSize;
				}
				else 
				{
					if(currmagnitude>vmaxmaginitude)
						currmagnitude=vmaxmaginitude;
				}
				
				//Camera.main.orthographicSize = Mathf.Clamp(currmagnitude, minmagnitude, maxmagnitude);
				Camera.main.orthographicSize = currmagnitude;
			//}
		}
	}
	//JoaoZoom
}
