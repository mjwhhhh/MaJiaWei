using UnityEngine;
using System.Collections;

public class TutorialPanel : MonoBehaviour {

    //教程主要控制类，用于阴影、提示、步骤的总控

	public Material tintMaterial;
	private TintMeshController _midMeshController;
	public UISprite _mid;
	public UIRoot _root;
	public Camera _uiCamera;

	public float posx;
	public float posy;
	public float sizex;
	public float sizey;

	public Vector2[] TeachPos;
	public Vector2[] TeachSize;
	public Vector2 OriMidPos = new Vector2(10,10);
	public Vector2 OriMidSize = new Vector2(100,100);
	public int TeachPoint = 0;
	public int MaxPoint  = 8;

	public HomePageTeach TeachFinger;
	public HomePageHint TeachWord;
	public GameObject LogMsgP;

	//public LogMsgPanel x;
	public void Awake()
	{
//		_mid = GameObject.Find("TutorialMid").GetComponent<UISprite>();
		OriMidSize = new Vector2(1000,1000);
		OriMidPos = TeachPos[0];
		_midMeshController = new TintMeshController(transform, _mid, tintMaterial, _uiCamera, _root);
		_midMeshController.SetMesh();
        _midMeshController.SetAlpha(_mid.color.a);
	}

#if UNITY_EDITOR
	void OnGUI()
	{
		if(GUILayout.Button("Reload"))
		{
		   Application.LoadLevel( "HomeScene" );
		}
		if(GUILayout.Button("SetTeaching"))
		{
			SetTeaching();
		}

		if(GUILayout.Button("SetNormal"))
		{
			SetNormal();
		}

		if(GUILayout.Button("ShowHint"))
		{
			TeachWord.ScaleUp();
		}

		if(GUILayout.Button("ShowFinger"))
		{
			TeachFinger.StartShine();
		}

		if(GUILayout.Button("CloseFinger"))
		{
			TeachFinger.EndShine();
		}

		if(GUILayout.Button("NextPoint"))
		{
			TeachPoint ++;
			if(TeachPoint > MaxPoint)
			{
				TeachPoint = 0;
			}
		}

		if(GUILayout.Button("PrePoint"))
		{
			TeachPoint -=2;
			if(TeachPoint < 1)
			{
				TeachPoint = MaxPoint;
			}
		}

		if(GUILayout.Button("StartPoint"))
		{
			TeachPoint = 0;
		}

        if (GUILayout.Button("DebugPos"))
        {
            TeachPoint = 100;
        }

	}
#endif

	public void Update()
	{
		switch(TeachPoint)
		{  
		   case 0:SetTeaching();if(TeachWord.gameObject.activeSelf)TeachWord.ScaleUp(); TeachPoint = 1; StartCoroutine(ShowHint()); break;
		   case 1:
			    OriMidSize = Vector2.Lerp(OriMidSize,TeachSize[0],Time.deltaTime * 5);
                OriMidPos = Vector2.Lerp(OriMidPos, TeachPos[0], Time.deltaTime * 10);
			    _midMeshController.SetTintPosition(
                new Vector3(OriMidPos.x, OriMidPos.y, _mid.cachedTransform.localPosition.z),
				new Vector3(OriMidSize.x, OriMidSize.y, 1f));break;
		   
		   case 2:
			TeachPoint = 3;TeachWord.ScaleUp();StopCoroutine("ShowHint"); StartCoroutine(ShowHint(1));break;
			    
		   case 3:
			    OriMidSize = Vector2.Lerp(OriMidSize,TeachSize[1],Time.deltaTime * 5);
			    OriMidPos = Vector2.Lerp(OriMidPos,TeachPos[1],Time.deltaTime * 10);
			    _midMeshController.SetTintPosition(
				new Vector3(OriMidPos.x,OriMidPos.y, _mid.cachedTransform.localPosition.z),
				new Vector3(OriMidSize.x, OriMidSize.y, 1f));break;
		    case 4:
			TeachPoint = 5;TeachWord.ScaleUp();StopCoroutine("ShowHint");StartCoroutine(ShowHint(2));break;
		    case 5:
			OriMidSize = Vector2.Lerp(OriMidSize,TeachSize[2],Time.deltaTime * 5);
			OriMidPos = Vector2.Lerp(OriMidPos,TeachPos[2],Time.deltaTime * 10);
			_midMeshController.SetTintPosition(
				new Vector3(OriMidPos.x,OriMidPos.y, _mid.cachedTransform.localPosition.z),
				new Vector3(OriMidSize.x, OriMidSize.y, 1f));break;

		case 6:TeachPoint = 7;TeachWord.ScaleUp();StopCoroutine("ShowHint");StartCoroutine(ShowHint(3));break;
            case 7:
               OriMidSize = Vector2.Lerp(OriMidSize, TeachSize[3], Time.deltaTime * 5);
               OriMidPos = Vector2.Lerp(OriMidPos, TeachPos[3], Time.deltaTime * 10);
               _midMeshController.SetTintPosition(
                   new Vector3(OriMidPos.x, OriMidPos.y, _mid.cachedTransform.localPosition.z),
                   new Vector3(OriMidSize.x, OriMidSize.y, 1f)); break;
		case 8:TeachPoint = 9;TeachWord.ScaleUp(); StopCoroutine("ShowHint");StartCoroutine(ShowHint(4));break;
		    case 9:
		    OriMidSize = Vector2.Lerp(OriMidSize,TeachSize[4],Time.deltaTime * 5);
			OriMidPos = Vector2.Lerp(OriMidPos,TeachPos[4],Time.deltaTime * 10);
			_midMeshController.SetTintPosition(
				new Vector3(OriMidPos.x,OriMidPos.y, _mid.cachedTransform.localPosition.z),
				new Vector3(OriMidSize.x, OriMidSize.y, 1f));break;
		case 10:TeachPoint = 11;TeachWord.ScaleUp();StopCoroutine("ShowHint");StartCoroutine(ShowHint(5));break;
		    case 11:
			OriMidSize = Vector2.Lerp(OriMidSize,TeachSize[5],Time.deltaTime * 5);
			OriMidPos = Vector2.Lerp(OriMidPos,TeachPos[5],Time.deltaTime * 10);
			_midMeshController.SetTintPosition(
				new Vector3(OriMidPos.x,OriMidPos.y, _mid.cachedTransform.localPosition.z),
				new Vector3(OriMidSize.x, OriMidSize.y, 1f));break;
		case 12:TeachPoint = 13;TeachWord.ScaleUp();StopCoroutine("ShowHint");StartCoroutine(ShowHint(6));break;
		   case 13:
			OriMidSize = Vector2.Lerp(OriMidSize,TeachSize[6],Time.deltaTime * 5);
			OriMidPos = Vector2.Lerp(OriMidPos,TeachPos[6],Time.deltaTime * 10);
			_midMeshController.SetTintPosition(
				new Vector3(OriMidPos.x,OriMidPos.y, _mid.cachedTransform.localPosition.z),
				new Vector3(OriMidSize.x, OriMidSize.y, 1f));

			//在无点击事件进行下一步
			if(Input.GetMouseButtonDown(0))
			{
				TeachPoint = 14;
			}
			break;

			    

		case 14:TeachPoint = 15;TeachWord.ScaleUp();StopCoroutine("ShowHint");StartCoroutine(ShowHint(7));break;
		   case 15:
			OriMidSize = Vector2.Lerp(OriMidSize,TeachSize[7],Time.deltaTime * 5);
			OriMidPos = Vector2.Lerp(OriMidPos,TeachPos[7],Time.deltaTime * 10);
			_midMeshController.SetTintPosition(
				new Vector3(OriMidPos.x,OriMidPos.y, _mid.cachedTransform.localPosition.z),
				new Vector3(OriMidSize.x, OriMidSize.y, 1f));break;
		case 16:TeachPoint = 17;TeachWord.ScaleUp();StopCoroutine("ShowHint");StartCoroutine(ShowHint(8));break;
		   case 17:
			OriMidSize = Vector2.Lerp(OriMidSize,TeachSize[8],Time.deltaTime * 5);
			OriMidPos = Vector2.Lerp(OriMidPos,TeachPos[8],Time.deltaTime * 10);
			_midMeshController.SetTintPosition(
				new Vector3(OriMidPos.x,OriMidPos.y, _mid.cachedTransform.localPosition.z),
				new Vector3(OriMidSize.x, OriMidSize.y, 1f));break;
			/*
			 * Teach1 End
			 * 宠物购买及装备引导完成
			 * 人物介绍点击完成
			*/
		   case 18:SetNormal();TeachWord.ScaleUp(); TeachPoint = 19; PlayerPrefs.SetInt("NeedTeach",2);break;
		   
		   case 19:break;
		case 20:SetTeaching();TeachPoint = 21;StopCoroutine("ShowHint"); StartCoroutine(ShowHint(9)); break;
		   case 21:
			OriMidSize = Vector2.Lerp(OriMidSize,TeachSize[9],Time.deltaTime * 5);
			OriMidPos = Vector2.Lerp(OriMidPos,TeachPos[9],Time.deltaTime * 10);
			_midMeshController.SetTintPosition(
				new Vector3(OriMidPos.x,OriMidPos.y, _mid.cachedTransform.localPosition.z),
				new Vector3(OriMidSize.x, OriMidSize.y, 1f));break;
		case 22:TeachPoint = 23;TeachWord.ScaleUp();StopCoroutine("ShowHint");StartCoroutine(ShowHint(10));break;
		   case 23:
			OriMidSize = Vector2.Lerp(OriMidSize,TeachSize[10],Time.deltaTime * 5);
			OriMidPos = Vector2.Lerp(OriMidPos,TeachPos[10],Time.deltaTime * 10);
			_midMeshController.SetTintPosition(
				new Vector3(OriMidPos.x,OriMidPos.y, _mid.cachedTransform.localPosition.z),
				new Vector3(OriMidSize.x, OriMidSize.y, 1f));break;
		case 24:TeachPoint = 25;TeachWord.ScaleUp();StopCoroutine("ShowHint");StartCoroutine(ShowHint(11));break;
		   case 25:
			OriMidSize = Vector2.Lerp(OriMidSize,TeachSize[11],Time.deltaTime * 5);
			OriMidPos = Vector2.Lerp(OriMidPos,TeachPos[11],Time.deltaTime * 10);
			_midMeshController.SetTintPosition(
				new Vector3(OriMidPos.x,OriMidPos.y, _mid.cachedTransform.localPosition.z),
				new Vector3(OriMidSize.x, OriMidSize.y, 1f));break;
		   case 26:SetNormal();TeachWord.ScaleUp(); TeachPoint = 27; PlayerPrefs.SetInt("NeedTeach",0);break;
		   case 27:break;
           case 100://Debug调试使用模式，实际无法跳入该循环
                _midMeshController.SetTintPosition(
                new Vector3(posx, posy, _mid.cachedTransform.localPosition.z),
				new Vector3(sizex, sizex, 1f));break;
		   default: break;
		}
	}

	public void SetNormal()
	{
		_midMeshController.enabled = false;
		LogMsgP.SendMessage("SetNormal");
	}

	public void SetTeaching()
	{
		_midMeshController.enabled = true;
		LogMsgP.SendMessage("SetTeaching");
	}

	IEnumerator ShowHint(int Pos = 0)
	{
		yield return new WaitForSeconds(1);

		switch(Pos)
		{
		default:break;
		case 0:TeachWord.SetLabelWord("点击进入宠物购买界面,\n进行宠物购买");TeachWord.transform.localPosition = new Vector3(340,-83,-300); break;
		case 1:TeachWord.SetLabelWord("点击进入宠物介绍");TeachWord.transform.localPosition = new Vector3(-20,96,-300);break;
		case 2:TeachWord.SetLabelWord("点击购买宠物");TeachWord.transform.localPosition = new Vector3(49,-84,-300);break;
		case 3:TeachWord.SetLabelWord("点击确认购买");TeachWord.transform.localPosition = new Vector3(146,-58,-300);break;
		case 4:TeachWord.SetLabelWord("关闭升级界面");TeachWord.transform.localPosition = new Vector3(195,114,-300);break;
		case 5:TeachWord.SetLabelWord("选择上场宠物"); TeachWord.transform.localPosition = new Vector3(-20,96,-300);break;
		case 6:TeachWord.SetLabelWord("小伙伴已经上场咯~"); TeachWord.transform.localPosition = new Vector3(346,0,-300);break;
		case 7:TeachWord.SetLabelWord("看一看角色购买吧~"); TeachWord.transform.localPosition = new Vector3(242,-100,-300);break;
		case 8:TeachWord.SetLabelWord("同样点击介绍可以进入角色购买哦~"); TeachWord.transform.localPosition = new Vector3(-11,-22,-300);break;
		case 9:TeachWord.SetLabelWord("点击打开角色升级界面");TeachWord.ReverseBackScaleX(); TeachWord.transform.localPosition = new Vector3(-44,110,-300);break;
		case 10:TeachWord.SetLabelWord("点击升级角色攻击力");TeachWord.ReverseBackScaleX(); TeachWord.transform.localPosition = new Vector3(132,44,-300);break;
		case 11:TeachWord.SetLabelWord("点击确认升级"); TeachWord.transform.localPosition = new Vector3(134,-55,-300);break;
		}

		TeachWord.ScaleUp();
	}

	public void GotoTutorial(int tutonum = 0)
	{
		TeachPoint = tutonum;
	}
	
}
