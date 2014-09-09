using UnityEngine;
using System.Collections;

public class DailyGift: MonoBehaviour {

	public System.DateTime ThisTime;
	public static int lianxutianshu;
	public GameObject Gambs;
	public GameObject GambCam;
	public GameObject GambUI;
	public static float OriTimeTime;
	public static DailyGift instance;
	bool Once=true;
	public GameObject _GamblingUI; 
	
	
	
	public void Awake()
	{
		if(instance==null)
		{
		  Once=true;
		  StartCoroutine(GetTime());
		  instance=this;
		}
			_GamblingUI=GameObject.Find("_GamblingUI");
	}
	
	public void OnLevelWasLoaded(int level)
	{
		if (level == 1)
		{
			_GamblingUI=GameObject.Find("_GamblingUI");
		}
		
	}
	
	public void SecondAwake()
	{
		Once=true;
		StartCoroutine(GetTime());
	}
	
	void MarkThisTime()
	{
		if(!PlayerPrefs.HasKey("LastTime"))
		{
			//MJW Firstload! InitData First!
			PlayerPrefs.SetString("LastTime",ThisTime.ToString());
			OriTimeTime = Time.time;
			Debug.Log(" MJW Firstload! InitData First!");
		}
		if(System.DateTime.Compare(ThisTime,System.DateTime.Parse(PlayerPrefs.GetString("LastTime")))>0)
		{
		  Debug.Log("Mark New");
		  PlayerPrefs.SetString("LastTime",ThisTime.ToString());
		  OriTimeTime = Time.time;
		}
		Debug.Log(System.DateTime.Parse(PlayerPrefs.GetString("LastTime")).Year.ToString()+":"+System.DateTime.Parse(PlayerPrefs.GetString("LastTime")).Date.ToString());
	}
	
//	public static bool isActTime1()
//	{
//		System.DateTime judgetime = System.DateTime.Parse(PlayerPrefs.GetString("LastTime"));
//		System.DateTime StartTime = System.DateTime.Parse("11/17/2013 00:00:01 AM");
//		System.DateTime OverTime  = System.DateTime.Parse("11/25/2013 00:00:01 AM");
////		Debug.Log(judgetime.Hour+":"+judgetime.Minute);
//		if(judgetime.Year==2013 && System.DateTime.Compare(judgetime,StartTime)>0 && System.DateTime.Compare(judgetime,OverTime)<0)
//		{
//			return true;
//		}
//		else
//		{
//			return false;
//		}
//	}
	
	public static bool isActTime1()
	{
		System.DateTime judgetime = System.DateTime.Parse(PlayerPrefs.GetString("LastTime"));
		System.DateTime StartTime = System.DateTime.Parse("11/25/2013 00:00:01 AM");
		System.DateTime OverTime  = System.DateTime.Parse("12/02/2013 00:00:01 AM");
//		Debug.Log(judgetime.Hour+":"+judgetime.Minute);
		if(judgetime.Year==2013 && System.DateTime.Compare(judgetime,StartTime)>0 && System.DateTime.Compare(judgetime,OverTime)<0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public static bool isActTime2()
	{
		System.DateTime judgetime = System.DateTime.Parse(PlayerPrefs.GetString("LastTime","1992-4-13 00:00:00"));
		System.DateTime StartTime = System.DateTime.Parse("12/09/2013 00:00:01 AM");
		System.DateTime OverTime  = System.DateTime.Parse("12/16/2013 00:00:01 AM");
//		Debug.Log(judgetime.Hour+":"+judgetime.Minute);
		if(judgetime.Year==2013 && System.DateTime.Compare(judgetime,StartTime)>0 && System.DateTime.Compare(judgetime,OverTime)<0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public static bool isActTime3()
	{
		System.DateTime judgetime = System.DateTime.Parse(PlayerPrefs.GetString("LastTime"));
		System.DateTime StartTime = System.DateTime.Parse("12/09/2013 00:00:01 AM");
		System.DateTime OverTime  = System.DateTime.Parse("12/16/2013 00:00:01 AM");
//		Debug.Log(judgetime.Hour+":"+judgetime.Minute);
		if(judgetime.Year==2013 && System.DateTime.Compare(judgetime,StartTime)>0 && System.DateTime.Compare(judgetime,OverTime)<0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	
	public static bool isSecondWeekEndTime()
	{
		System.DateTime judgetime = System.DateTime.Parse(PlayerPrefs.GetString("LastTime"));
		System.DateTime StartTime = System.DateTime.Parse("12/09/2013 00:00:01 AM");
		System.DateTime OverTime  = System.DateTime.Parse("12/16/2013 00:00:01 AM");
//		Debug.Log(judgetime.Hour+":"+judgetime.Minute);
		if(judgetime.Year==2013 && System.DateTime.Compare(judgetime,StartTime)>0 && System.DateTime.Compare(judgetime,OverTime)<0)
		{
			System.DateTime judgetimes = System.DateTime.Parse(PlayerPrefs.GetString("LastTime"));
			int nowtimeinint = judgetimes.Hour *3600 + judgetimes.Minute*60 + judgetimes.Second + (int)(Time.time - OriTimeTime);
			
			
		    if(72000  <=   nowtimeinint && nowtimeinint  <=  79200) 
		     {
			    return true;
		     }
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
	}
	
	
	public static bool isFirstWeekGiftTime()
	{
		System.DateTime judgetime = System.DateTime.Parse(PlayerPrefs.GetString("LastTime"));
		System.DateTime StartTime = System.DateTime.Parse("11/29/2013 00:00:01 AM");
		System.DateTime OverTime  = System.DateTime.Parse("12/02/2013 00:00:01 AM");
//		Debug.Log(judgetime.Hour+":"+judgetime.Minute);
		if(judgetime.Year==2013 && System.DateTime.Compare(judgetime,StartTime)>0 && System.DateTime.Compare(judgetime,OverTime)<0)
		{
			System.DateTime judgetimes = System.DateTime.Parse(PlayerPrefs.GetString("LastTime"));
			int nowtimeinint = judgetimes.Hour *3600 + judgetimes.Minute*60 + judgetimes.Second + (int)(Time.time - OriTimeTime);
			
			
		    if(72000  <=   nowtimeinint && nowtimeinint  <=  79200) 
		     {
			    return true;
		     }
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
	}
	
	
	void ForceMarkThisTime()
	{
		  PlayerPrefs.SetString("LastTime",ThisTime.ToString());
		  OriTimeTime = Time.time;
	}
	
	void CompareLastTime()
	{
		if(!PlayerPrefs.HasKey("LastTime") )
		{
			PlayerPrefs.SetString("LastTime","1992-4-13 00:00:00");
			
		}
		System.DateTime lasttime=System.DateTime.Parse(PlayerPrefs.GetString("LastTime"));
		
		Debug.Log("ThisTime"+ThisTime);
		Debug.Log("LastTime"+lasttime);
		if(System.DateTime.Compare(ThisTime.Date,lasttime.Date)>0)
		{
			PlayerPrefs.SetInt("GBCost",500);
			if(ThisTime.Year==lasttime.Year && ThisTime.Month==lasttime.Month && ThisTime.Day-lasttime.Day<=1)
			   OpenInNewDay(true);
			else
			   OpenInNewDay(false);
		}
		else if(System.DateTime.Compare(ThisTime.Date,lasttime.Date)==0)
		{
			OpenInToday();
		}
		else
		{
			WrongSystemTime();
		}
	}
	

        

	
	
	
	void WrongSystemTime()
	{
 		Debug.Log("Wrong SystemTime");
		GameObject.Find("GameMain").SendMessage("CallGambling");
		GamblingUI.instance.ConWordsShow();
		GamblingUI.instance.SetConText("很遗憾","无法获取网络时间，请保持您的设备时间是联网的",0,0);
//		ForceMarkThisTime();
	}
	
	
        public string[] TodayWord = new string[4]{"本日充值有礼!赠送抽奖点数（u币）\n                       =充值费用（元） X 1!","本日充值有礼!赠送抽奖点数（u币）\n                        =充值费用（元） X 1 !","本日充值有礼!赠送金币数\n                        =充值费用（元） X 150!","本日充值有礼!赠送金币数\n                        =充值费用（元） X 150!"};
	    
	    
	void OpenInNewDay(bool isconti)
	{
        int todayGift =(int) Random.Range(1,5);
		GameObject.Find("GameMain").SendMessage("CallGambling");
		
		
//		LuckyBoxes.instance.ResetBoxesState();
		 bool[] UpperBoxOpenB = new bool[4];
		  bool[] DownBoxOpenB = new bool[4];
		  for(int i=0;i<=3;i++)
			{
				UpperBoxOpenB[i]=false;
				DownBoxOpenB[i]=false;
			}
		  PlayerPrefsX.SetBoolArray("UBBoxes",UpperBoxOpenB);
		  PlayerPrefsX.SetBoolArray("CoinBBoxes",DownBoxOpenB);
//		LuckyBoxes.instance.ResetBoxesState();
		
		
		
//		switch(todayGift)
//		{
//		    case 1:_GamblingUI.GetComponent<GamblingUI>().SetTodayText(TodayWord[0]);PlayerPrefs.SetInt("TodayGift",1);break;
//		    case 2:_GamblingUI.GetComponent<GamblingUI>().SetTodayText(TodayWord[1]);PlayerPrefs.SetInt("TodayGift",2);break;
//		    case 3:_GamblingUI.GetComponent<GamblingUI>().SetTodayText(TodayWord[2]);PlayerPrefs.SetInt("TodayGift",3);break;
//			case 4:_GamblingUI.GetComponent<GamblingUI>().SetTodayText(TodayWord[3]);PlayerPrefs.SetInt("TodayGift",4);break;
//		}
		
		MarkThisTime();
		
		if(DailyGift.isFirstWeekGiftTime())
		{
			PlayerPrefs.SetString("TodayActCoin","Got");
			AutoCtrl.instance.AddUCoin("3");
			GamblingUI.instance.UpdateCoin();
		}
		
		
		PlayerPrefs.SetInt("GBCost",500);
		
		if(!PlayerPrefs.HasKey("Lianxu"))
		{
			PlayerPrefs.SetInt("Lianxu",1);
			lianxutianshu=1;
			AutoCtrl.instance.Save();
			GamblingUI.instance.UpdateCoin();
		}
		else if(isconti)
		{
			lianxutianshu=PlayerPrefs.GetInt("Lianxu")+1;
			PlayerPrefs.SetInt("Lianxu",lianxutianshu);
			switch(lianxutianshu)
			{
			  case 1:AutoCtrl.instance.SetUCoin = AutoCtrl.instance.GetUCoin+1;AutoCtrl.instance.SetBag=AutoCtrl.instance.GetBag+2;break;
			  case 2:AutoCtrl.instance.SetUCoin = AutoCtrl.instance.GetUCoin+2;AutoCtrl.instance.SetBag=AutoCtrl.instance.GetBag+2;break;
			  case 3:AutoCtrl.instance.SetUCoin = AutoCtrl.instance.GetUCoin+3;AutoCtrl.instance.SetBag=AutoCtrl.instance.GetBag+2;break;
			  case 4:AutoCtrl.instance.SetCoin = AutoCtrl.instance.GetCoin+5000; break;
			  case 5:AutoCtrl.instance.SetCoin = AutoCtrl.instance.GetCoin+6000;break;	
			 default:break;
			}
			
			if(lianxutianshu>=6)
			{
				AutoCtrl.instance.SetUCoin=AutoCtrl.instance.GetUCoin+5;
			}
			AutoCtrl.instance.Save();
			GamblingUI.instance.UpdateCoin();
		}
		
		else
		{
			PlayerPrefs.SetInt("Lianxu",1);
			lianxutianshu=1;
			AutoCtrl.instance.SetUCoin=AutoCtrl.instance.GetUCoin+1;
			AutoCtrl.instance.SetBag=AutoCtrl.instance.GetBag+2;
			AutoCtrl.instance.Save();
			GamblingUI.instance.UpdateCoin();
		}
		Debug.Log("连续:"+lianxutianshu);
		
		
		if(PlayerPrefs.GetInt("FirstTime")!=1)
		{
			GamblingUI.instance.ConWordsShow();
			GamblingUI.instance.SetConText("太好了！","首次进入游戏赠送5U币，可抽奖5次。",2,5);
			Debug.Log("FirstTime");
			PlayerPrefs.SetInt("FirstTime",1);
			AutoCtrl.instance.SetUCoin=5;
			AutoCtrl.instance.Save();
			GamblingUI.instance.UpdateCoin();
		}
		else
		{
			GamblingUI.instance.PrizeWordsShow();
			GamblingUI.instance.PosRCheck();
		}
		
	}
	
	void OpenInToday()
	{
		Debug.Log("OpenInToday");
		switch(PlayerPrefs.GetInt("TodayGift"))
		{
		    case 1:_GamblingUI.GetComponent<GamblingUI>().SetTodayText(TodayWord[0]);break;
		    case 2:_GamblingUI.GetComponent<GamblingUI>().SetTodayText(TodayWord[1]);break;
		    case 3:_GamblingUI.GetComponent<GamblingUI>().SetTodayText(TodayWord[2]);break;
			case 4:_GamblingUI.GetComponent<GamblingUI>().SetTodayText(TodayWord[3]);break;
		    default:break;
		}
		if(DailyGift.isFirstWeekGiftTime() && PlayerPrefs.GetString("TodayActCoin")!="Got")
		{
			PlayerPrefs.SetString("TodayActCoin","Got");
			AutoCtrl.instance.AddUCoin("3");
			GamblingUI.instance.UpdateCoin();
		}
		MarkThisTime();
		GamblingUI.instance.ShowOfToday();
	}
	
	
	
	void OnGUI()
	{
#if UNITY_EDITOR	
		 if(GUILayout.Button("CreateFakeDayYYYYYerday"))
		{
			PlayerPrefs.SetString("LastTime",ThisTime.Year.ToString() + "-" + ThisTime.Month.ToString() + "-" + (ThisTime.Day-2).ToString() + " " + ThisTime.Hour.ToString() + ":" + ThisTime.Minute.ToString() + ":" +  ThisTime.Second.ToString());
		}
	    if(GUILayout.Button("CreateFakeDayYesterday"))
		{
			PlayerPrefs.SetString("LastTime",ThisTime.Year.ToString() + "-" + ThisTime.Month.ToString() + "-" + (ThisTime.Day-1).ToString() + " " + ThisTime.Hour.ToString() + ":" + ThisTime.Minute.ToString() + ":" +  ThisTime.Second.ToString());
		}
		if(GUILayout.Button("CreateFakeDayTomorrow"))
		{
			PlayerPrefs.SetString("LastTime",ThisTime.Year.ToString() + "-" + ThisTime.Month.ToString() + "-" + (ThisTime.Day+1).ToString() + " " + ThisTime.Hour.ToString() + ":" + ThisTime.Minute.ToString() + ":" +  ThisTime.Second.ToString());
		}
		if(GUILayout.Button("GetTimeAndCompare"))
		{
		}
		
		if(GUILayout.Button("SaveTime"))
		{
			MarkThisTime();
		}
		
		if(GUILayout.Button("GetTime"))
		{
			Awake();
		}
		if(GUILayout.Button("ADD Money"))
		{
			AutoCtrl.instance.OnBillingResult("001");
		}
		if(GUILayout.Button("Delete"))
		{
			
			PlayerPrefs.DeleteAll();
			PlayerPrefs.DeleteKey("HasUnlockAll");
			PlayerPrefs.DeleteKey("Coin");
			PlayerPrefs.DeleteKey("Lianxu");
			PlayerPrefs.DeleteKey("MissionsUnlocked");
			PlayerPrefs.DeleteKey("MissionsUnlocked"+0);
			PlayerPrefs.DeleteKey("MissionsUnlocked"+1);
			PlayerPrefs.DeleteKey("MissionsUnlocked"+2);
			PlayerPrefs.DeleteKey("HealingBags");
			PlayerPrefs.DeleteKey("E_WeaponType.Shotgun");
			PlayerPrefs.DeleteKey("E_WeaponType.GrenadeLauncher");
			PlayerPrefs.DeleteKey("E_WeaponType.RocketLauncher");
			PlayerPrefs.DeleteKey("E_WeaponType.PlasmaRifle");
			PlayerPrefs.SetString("LastTime","1992-4-13 00:00:00");
			PlayerPrefs.DeleteKey("FirstTime");
			PlayerPrefs.DeleteKey("ShangJin1");
			PlayerPrefs.DeleteKey("ShangJin2");
			PlayerPrefs.DeleteKey("ShangJin3");
			PlayerPrefs.DeleteKey("ShangJin1Star");
			PlayerPrefs.DeleteKey("ShangJin2Star");
			PlayerPrefs.DeleteKey("ShangJin3Star");
			LuckyBoxes.instance.ResetBoxesState();
		}
		if(GUILayout.Button("SetAll"))
		{
			
		}
		if(GUILayout.Button("CallGambling"))
		{
			GameObject.Find("GameMain").SendMessage("CallGambling");
			Gambs.SetActiveRecursively(true);
		}
		
		if(GUILayout.Button("OverGambling"))
		{
			GameObject.Find("GameMain").SendMessage("GamblingOver");
			Gambs.SetActiveRecursively(false);
		}
		
		
		if(GUILayout.Button("ShowPup"))
		{
//			AutoCtrl.instance.InitPopWord("a","a");
		}
#endif		
	}
	

	
	public void OverGambling()
	{
		    GameObject.Find("GameMain").SendMessage("GamblingOver");
			Gambs.SetActiveRecursively(false);
//		    GambUI.SetActiveRecursively(true);	
	}
	
	public void ConCauseHide()
	{
		Gambs.SetActiveRecursively(false);
	}
	
	
	public void CallGambling()
	{
		if(!Gambs.active)
		{
		 GameObject.Find("GameMain").SendMessage("Btn_BackToMenu");
		 Gambs.SetActiveRecursively(true);
		 GameObject.Find("GameMain").SendMessage("CallGambling");
		 GamblingUI.instance.UpdateCoin();
//		 GambUI.SetActiveRecursively(false);	
	     GamblingUI.ShowActivity=false;
		}
	}
	
	IEnumerator GetTime() 
	{
		    Debug.Log("GET Time");
		    WWW Timeget=new WWW("http://www.beijing-time.org/time.asp");
		    float timestart=Time.time;
		
			while(Time.time<timestart+3)
			{
			  if(Timeget.isDone && Timeget.error==null && Once)
			{
					ThisTime=readtext(Timeget.text);
				
					Debug.Log("break:"+ThisTime);
					Invoke("CompareLastTime",.5f);
				    Once=false;
					break;
			}
            yield return null;
			
			if(Time.time>=timestart+3 && Once)
			{
				Once=false;
//				ThisTime= System.DateTime.Now;
				Debug.Log("WrongTime:"+ThisTime);
				WrongSystemTime();
				break;
			}
		   }
		
    }
	
	System.DateTime readtext(string time)
	{
		System.DateTime returndate=new System.DateTime();
		
		        string[] tempArray = time.Split(';');  
                for (int i = 0; i < tempArray.Length; i++)  
                {  
                    tempArray[i] = tempArray[i].Replace("\r\n", "");  
                }  
		
		        string year = tempArray[1].Split('=')[1];  
                string month = tempArray[2].Split('=')[1];  
                string day = tempArray[3].Split('=')[1];  
                string hour = tempArray[5].Split('=')[1];  
                string minite = tempArray[6].Split('=')[1];  
                string second = tempArray[7].Split('=')[1];  
		
		        returndate=System.DateTime.Parse(year + "-" + month + "-" + day + " " + hour + ":" + minite + ":" + second);  
		return returndate;
	}
	
	
}
