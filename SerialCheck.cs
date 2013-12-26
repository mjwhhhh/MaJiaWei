using UnityEngine;
using System.Collections;

public class SerialCheck : MonoBehaviour {
	
	public UILabel NumLabel;
	System.DateTime ThisTime;
	public GameObject Cube;
		
   public void SubmitNum()
	{
		Debug.Log("Submitting wiht Num:" + NumLabel.text);
		StartCoroutine(GetResultFromServer(NumLabel.text));
	}
	
	
	
   IEnumerator GetResultFromServer(string SerialNum)
	{
		Debug.Log("GET Time");
		    
		    WWW Timeget=new WWW("http://www.beijing-time.org/time.asp");
		    float timestart=Time.time;
		
			while(Time.time<timestart+3)
			{
			  if(Timeget.isDone && Timeget.error==null)
			{
					ThisTime=readtext(Timeget.text);
				
					Debug.Log("break:"+ThisTime);
					Invoke("CompareServerCode",.5f);
					break;
			}
            yield return null;
			
			
			
			if(Time.time>=timestart+3)
			{
//				ThisTime= System.DateTime.Now;
				Debug.Log("WrongTime:"+ThisTime);
				BadInternetenvironment();
				break;
			}
			
			
		   }

	}
	
	
	public void CompareServerCode()
	{
		if(ThisTime.Day.ToString() == NumLabel.text)
		{
			Debug.Log("Code Checked Success!");
			Success("MoveCube");
		}
		else
		{
			Debug.Log("No Such Num At Server");
		}
	}
	
	void Success(string Type)
	{
		switch(Type)
		{
		case "MoveCube":Cube.animation.Play(); break;
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
	
	
	
	
	void BadInternetenvironment()
	{
	   Debug.Log("Bad Net Service,Please Recheck your Internet Service.");
	}
}
