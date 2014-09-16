using UnityEngine;
using System.Collections;

public class HomePageHint : MonoBehaviour {

    /*
     * 首页提示，用于显示及消失教程气泡框
     * */
	GameObject thisG;
	Transform thisT;
	public UILabel WordLabel;
	public UIButtonTween ScaleBtn;
	public Transform BackImgT;
	public UISprite BackImg;

	// Use this for initialization
	void Awake () 
	{
		thisG = this.gameObject;
		thisT = this.transform;
		thisG.SetActive(false);
	}

	public void SetLabelWord(string Word)
	{
		WordLabel.text = Word;
	}

	public void ScaleUp()
	{
		if(!thisG.activeSelf)
		  thisG.SetActive(true);
		ScaleBtn.Play(true);
	}

	public void ReverseBackScaleX()
	{
		BackImg.enabled = false;
		BackImgT.localScale = new Vector3(-BackImgT.localScale.x,BackImgT.localScale.y,BackImgT.localScale.z);
		BackImg.enabled = true;
	}
}
