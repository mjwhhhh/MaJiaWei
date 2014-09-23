package com.unitygames.sheephappens;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.text.Format;
import java.text.SimpleDateFormat;
import java.util.Date;


import com.unicom.dcLoader.Utils;
import com.unicom.dcLoader.Utils.UnipayPayResultListener;
import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;
import com.tencent.mm.sdk.openapi.IWXAPI;
import com.tencent.mm.sdk.openapi.SendMessageToWX;
import com.tencent.mm.sdk.openapi.WXAPIFactory;
import com.tencent.mm.sdk.openapi.WXMediaMessage;
import com.tencent.mm.sdk.openapi.WXWebpageObject;

import android.app.AlertDialog;
import android.content.ComponentName;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.ApplicationInfo;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.content.res.AssetManager;
import android.graphics.Bitmap;
import android.graphics.Bitmap.CompressFormat;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.provider.MediaStore;
import android.util.Log;
import android.view.WindowManager;
import android.widget.ImageView;
import android.widget.Toast;
import android.graphics.drawable.BitmapDrawable;
import android.graphics.drawable.Drawable;

public class MainActivity extends UnityPlayerActivity {

	private static final int PHOTO_REQUEST_CAREMA = 1;// ����
	private static final int PHOTO_REQUEST_GALLERY = 2;// �������ѡ��
	private static final int PHOTO_REQUEST_CUT = 3;// ���
	public Context ThisContext;

	private PackageManager mPackageManager;

	private int operatorType = 1;
	//1  �й��ƶ�
	//2  �й���ͨ
	//3  �й�����

	Format format = new SimpleDateFormat("yyyyMMddHHmmss");
	//9013914566820140221150609967400001
	private String[] UniPayCodeId = new String[] {
			"9013914566820140221150609967400001",
			"9013914566820140221150609967400002",
			"9013914566820140221150609967400003",
			"9013914566820140221150609967400004",
			"9013914566820140221150609967400005",
			"9013914566820140221150609967400006",
			"9013914566820140221150609967400007",
			"9013914566820140221150609967400008", };

	private String[] UniPayCodeCode = new String[] { "140221024844",
			"140221024845", "140221024846", "140221024847", "140221024848",
			"140221024849", "140221024850", "140221024851", };

	/* ͷ������ */
	private static final String PHOTO_FILE_NAME = "temp_photo.jpg";
	private File tempFile;

	private static final String APP_ID = "wxb48199bb355d6ad4";
	private IWXAPI api;

	private void regToWx() {
		api = WXAPIFactory.createWXAPI(this, APP_ID, true);
		api.registerApp(APP_ID);

	}

	public int GetOperatorType()
	{
		return operatorType;
	}
	
	 
	 
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
		
		ThisContext = this;
		mPackageManager = ThisContext.getPackageManager();
		this.runOnUiThread(new Runnable() {
			public void run() 
			{
				regToWx();
			}
		});
//		TelephonyManager telManager = (TelephonyManager) getSystemService(Context.TELEPHONY_SERVICE);   
//		String operator = telManager.getSimOperator();  
//		if(operator!=null)
//		{ 
//	    if(operator.equals("46000") || operator.equals("46002")|| operator.equals("46007"))
//	    {
//		//�й��ƶ� 
//	    	GameInterface.initializeApp(this);
//	    	operatorType=1;
//		}
//	    else if(operator.equals("46001"))
//		{  
//	    	Log.d("UniPayCode", "UniPay Init");
//			Utils.getInstances().init(ThisContext, UniAppid, UniDevCode, UniCpid,
//					"�Ϻ��ž���Ϣ�Ƽ����޹�˾", "4008 360 989", "����������", null,
//					new PayResultListener());
//			operatorType=2;
//			
//		//�й���ͨ
//		}
//	    else if(operator.equals("46003")){  
//	    	operatorType=3;
//		//�й����� 
//
//		}
//		}
		// Toast.makeText(ThisContext, "�Բ�������û�а�װ΢�ţ��޷�����" ,
		// Toast.LENGTH_SHORT).show();
		
		// Utils.getInstances().init(ThisContext,"90234616120120921431100","902346161",
		// "86000504","�Ƽ��Ƽ�","400 600 999","��Ϸ��Ϸ","", new PayResultListener());
	}
	
	public void SMSPayForItem(String PayCode)
	{
		if(operatorType==1)
		{
		    switch(PayCode.charAt(2))
		    {
		       case '1':UnityPlayer.UnitySendMessage("TweensAndMethods", "Billing",	"001");break;
		       case '2':UnityPlayer.UnitySendMessage("TweensAndMethods", "Billing",	"002");break;
		       case '3':UnityPlayer.UnitySendMessage("TweensAndMethods", "Billing",	"003");break;
		       case '4':UnityPlayer.UnitySendMessage("TweensAndMethods", "Billing",	"004");break;
		       case '5':UnityPlayer.UnitySendMessage("UI Root (2D)", "ApayForGold",	"005");break;
		       case '6':UnityPlayer.UnitySendMessage("UI Root (2D)", "ApayForGold",	"006");break;
		       case '7':UnityPlayer.UnitySendMessage("UI Root (2D)", "ApayForGold",	"007");break;
		       case '8':UnityPlayer.UnitySendMessage("UI Root (2D)", "ApayForGold",	"008");break;
		       default:break;
		    }
		}
		else if(operatorType==2)
		{
			switch(PayCode.charAt(2))
			{
			   case '1':UniPaySMS("001");break;
			   case '2':UniPaySMS("002");break;
			   case '3':UniPaySMS("003");break;
			   case '4':UniPaySMS("004");break;
			   case '5':UniPaySMS("005");break;
			   case '6':UniPaySMS("006");break;
			   case '7':UniPaySMS("007");break;
			   case '8':UniPaySMS("008");break;
			}
		}
		else if(operatorType==3)
		{
			this.runOnUiThread(new Runnable() {
				public void run() 
				{
					 Toast.makeText(ThisContext, "�Բ��𣬵����ֻ��ݲ�֧�ֶ��Ÿ��ѣ���ѡ������֧����ʽ��" ,Toast.LENGTH_SHORT).show();
				}
			});
		}		
	}

	
	
	
	
	public void UniPaySMS(String Item) {
		final char UIchar = Item.charAt(2);
		this.runOnUiThread(new Runnable() {
			public void run() {
				// Toast.makeText(ThisContext, "�Բ�������û�а�װ΢�ţ��޷�����" ,
				// Toast.LENGTH_SHORT).show();
				UnityPaySmS(UIchar);
			}
		});
	}

	public void UnityPaySmS(char x) {
		// Utils.getInstances().setBaseInfo(ThisContext, true, false,
		// "http://uniview.wostore.cn/log-app/test");

		Log.e("24Bit",format.format(new Date()));
		
		switch (x) {
		case '1':
			Utils.getInstances().setBaseInfo(ThisContext, false, true, "http://uniview.wostore.cn/log-app/test");
			Utils.getInstances().pay(ThisContext, UniPayCodeCode[0],
					UniPayCodeId[0], "2500���", "3", format.format(new Date()),
					new PayResultListener());
			break;
		case '2':
			Utils.getInstances().setBaseInfo(ThisContext, false, true, "http://uniview.wostore.cn/log-app/test");
			Utils.getInstances().pay(ThisContext, UniPayCodeCode[1],
					UniPayCodeId[1], "5000���", "6", format.format(new Date()),
					new PayResultListener());
			break;
		case '3':
			Utils.getInstances().setBaseInfo(ThisContext, false, true, "http://uniview.wostore.cn/log-app/test");
			Utils.getInstances().pay(ThisContext, UniPayCodeCode[2],
					UniPayCodeId[2], "10000���", "12",
					format.format(new Date()), new PayResultListener());
			break;
		case '4':
			Utils.getInstances().setBaseInfo(ThisContext, false, true, "http://uniview.wostore.cn/log-app/test");
			Utils.getInstances().pay(ThisContext, UniPayCodeCode[3],
					UniPayCodeId[3], "26250���", "30",
					format.format(new Date()), new PayResultListener());
			break;                                                                                                                 
		 case '5':
			 Utils.getInstances().setBaseInfo(ThisContext, false, true, "http://uniview.wostore.cn/log-app/test");
			 Utils.getInstances().pay(ThisContext, UniPayCodeCode[4],
				 UniPayCodeId[4], "55000���", "68", format.format(new Date()),
				 new PayResultListener());break;
		 case '6':
			 Utils.getInstances().setBaseInfo(ThisContext, false, true, "http://uniview.wostore.cn/log-app/test");
			 Utils.getInstances().pay(ThisContext, UniPayCodeCode[5],
				 UniPayCodeId[5], "90000���", "98", format.format(new Date()),
				 new PayResultListener());break;
		 case '7':
			 Utils.getInstances().setBaseInfo(ThisContext, false, true, "http://uniview.wostore.cn/log-app/test");
			 Utils.getInstances().pay(ThisContext, UniPayCodeCode[6],
				 UniPayCodeId[6], "135000���", "128", format.format(new Date()),
				 new PayResultListener());break;
		 case '8':
			 Utils.getInstances().setBaseInfo(ThisContext, false, true, "http://uniview.wostore.cn/log-app/test");
			 Utils.getInstances().pay(ThisContext, UniPayCodeCode[7],
				 UniPayCodeId[7], "255000���", "198", format.format(new Date()),
				 new PayResultListener());break;
		}

	}

	public class PayResultListener implements UnipayPayResultListener {
		@Override
		public void PayResult(String paycode, int flag, String desc) {
			Log.e("UniPayCode", "paycode:"+paycode);
			if (flag == Utils.SUCCESS_SMS) 
			{
				if(paycode.contains(UniPayCodeCode[0]) || paycode.contains(UniPayCodeId[0]))
				{
				Toast.makeText(MainActivity.this, "����֧���ɹ�", 1000)
						.show();
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","001|1");
				}
				if(paycode.contains(UniPayCodeCode[1]) || paycode.contains(UniPayCodeId[1]))
				{
				Toast.makeText(MainActivity.this, "����֧���ɹ�", 1000)
						.show();
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","002|1");
				}
				if(paycode.contains(UniPayCodeCode[2]) || paycode.contains(UniPayCodeId[2]))
				{
				Toast.makeText(MainActivity.this, "����֧���ɹ�", 1000)
						.show();
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","003|1");
				}
				if(paycode.contains(UniPayCodeCode[3]) || paycode.contains(UniPayCodeId[3]))
				{
				Toast.makeText(MainActivity.this, "����֧���ɹ�", 1000)
						.show();
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","004|1");
				}
                if(paycode.contains(UniPayCodeCode[4]) || paycode.contains(UniPayCodeId[4]))
				{
				Toast.makeText(MainActivity.this, "����֧���ɹ�", 1000)
						.show();
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","005|1");
				}
				if(paycode.contains(UniPayCodeCode[5]) || paycode.contains(UniPayCodeId[5]))
				{
				Toast.makeText(MainActivity.this, "����֧���ɹ�", 1000)
						.show();
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","006|1");
				}
				if(paycode.contains(UniPayCodeCode[6]) || paycode.contains(UniPayCodeId[6]))
				{
				Toast.makeText(MainActivity.this, "����֧���ɹ�", 1000)
						.show();
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","007|1");
				}
				if(paycode.contains(UniPayCodeCode[7]) || paycode.contains(UniPayCodeId[7]))
				{
				Toast.makeText(MainActivity.this, "����֧���ɹ�", 1000)
						.show();
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","008|1");
				}
			}			
			else if (flag == Utils.SUCCESS_3RDPAY)
			{ // SDKʹ�õ�����֧�����سɹ�
				if(paycode.contains(UniPayCodeCode[0]) || paycode.contains(UniPayCodeId[0]))
				{
				Toast.makeText(MainActivity.this, "������֧���ɹ�", 1000)
						.show();
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","001|1");
				}
				if(paycode.contains(UniPayCodeCode[1])|| paycode.contains(UniPayCodeId[1]))
				{
				Toast.makeText(MainActivity.this, "������֧���ɹ�", 1000)
						.show();
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","002|1");
				}
				if(paycode.contains(UniPayCodeCode[2])|| paycode.contains(UniPayCodeId[2]))
				{
				Toast.makeText(MainActivity.this, "������֧���ɹ�", 1000)
						.show();
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","003|1");
				}
				if(paycode.contains(UniPayCodeCode[3])|| paycode.contains(UniPayCodeId[3]))
				{
				Toast.makeText(MainActivity.this, "������֧���ɹ�", 1000)
						.show();
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","004|1");
				}
				if(paycode.contains(UniPayCodeCode[4]) || paycode.contains(UniPayCodeId[4]))
				{
				Toast.makeText(MainActivity.this, "������֧���ɹ�", 1000)
						.show();
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","005|1");
				}
				if(paycode.contains(UniPayCodeCode[5])|| paycode.contains(UniPayCodeId[5]))
				{
				Toast.makeText(MainActivity.this, "������֧���ɹ�", 1000)
						.show();
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","006|1");
				}
				if(paycode.contains(UniPayCodeCode[6])|| paycode.contains(UniPayCodeId[6]))
				{
				Toast.makeText(MainActivity.this, "������֧���ɹ�", 1000)
						.show();
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","007|1");
				}
				if(paycode.contains(UniPayCodeCode[7])|| paycode.contains(UniPayCodeId[7]))
				{
				Toast.makeText(MainActivity.this, "������֧���ɹ�", 1000)
						.show();
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","008|1");
				}
			} 
			else if (flag == Utils.FAILED) 
			{
				Toast.makeText(MainActivity.this, "֧��ʧ��", 1000).show();
				if(paycode.contains(UniPayCodeCode[0]) || paycode.contains(UniPayCodeId[0]))
				{
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","001|0");
				}
				if(paycode.contains(UniPayCodeCode[1]) || paycode.contains(UniPayCodeId[1]))
				{
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","002|0");
				}
				if(paycode.contains(UniPayCodeCode[2]) || paycode.contains(UniPayCodeId[2]))
				{
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","003|0");
				}
				if(paycode.contains(UniPayCodeCode[3]) || paycode.contains(UniPayCodeId[3]))
				{
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","004|0");
				}
                if(paycode.contains(UniPayCodeCode[4]) || paycode.contains(UniPayCodeId[4]))
				{
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","005|0");
				}
				if(paycode.contains(UniPayCodeCode[5]) || paycode.contains(UniPayCodeId[5]))
				{
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","006|0");
				}
				if(paycode.contains(UniPayCodeCode[6]) || paycode.contains(UniPayCodeId[6]))
				{
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","007|0");
				}
				if(paycode.contains(UniPayCodeCode[7]) || paycode.contains(UniPayCodeId[7]))
				{
				UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","008|0");
				}
			} 
			else if (flag == Utils.CANCEL) 
			{
		    if(paycode.contains(UniPayCodeCode[0]) || paycode.contains(UniPayCodeId[0]))
			{
			Toast.makeText(MainActivity.this, "֧��ȡ��", 1000)
					.show();
			UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","001|0");
			}
			if(paycode.contains(UniPayCodeCode[1]) || paycode.contains(UniPayCodeId[1]))
			{
			Toast.makeText(MainActivity.this, "֧��ȡ��", 1000)
					.show();
			UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","002|0");
			}
			if(paycode.contains(UniPayCodeCode[2]) || paycode.contains(UniPayCodeId[2]))
			{
			Toast.makeText(MainActivity.this, "֧��ȡ��", 1000)
					.show();
			UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","003|0");
			}
			if(paycode.contains(UniPayCodeCode[3]) || paycode.contains(UniPayCodeId[3]))
			{
			Toast.makeText(MainActivity.this, "֧��ȡ��", 1000)
					.show();
			UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","004|0");
			}
            if(paycode.contains(UniPayCodeCode[4]) || paycode.contains(UniPayCodeId[4]))
			{
			Toast.makeText(MainActivity.this, "֧��ȡ��", 1000)
					.show();
			UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","005|0");
			}
			if(paycode.contains(UniPayCodeCode[5]) || paycode.contains(UniPayCodeId[5]))
			{
			Toast.makeText(MainActivity.this, "֧��ȡ��", 1000)
					.show();
			UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","006|0");
			}
			if(paycode.contains(UniPayCodeCode[6]) || paycode.contains(UniPayCodeId[6]))
			{
			Toast.makeText(MainActivity.this, "֧��ȡ��", 1000)
					.show();
			UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","007|0");
			}
			if(paycode.contains(UniPayCodeCode[7]) || paycode.contains(UniPayCodeId[7]))
			{
			Toast.makeText(MainActivity.this, "֧��ȡ��", 1000)
					.show();
			UnityPlayer.UnitySendMessage("TweensAndMethods", "OnBillingResultChinaUnicom","008|0");
			}
			}
			else if (flag == Utils.OTHERPAY) 
			{
				Toast.makeText(MainActivity.this, "�ڶ��ֵ���������", 1000).show();
				// ����д������֮ͨ�������������֧����ʽ�Ĵ����߼�����
			}

		}
	}

	
	public void ShareWeiChat(String ShareTitle, String ShareText, String Url) throws IOException {
		if (isInstallApplication(ThisContext, "com.tencent.mm")) 
		{
			WXWebpageObject webpage = new WXWebpageObject();
			webpage.webpageUrl =Url;
			WXMediaMessage msg = new WXMediaMessage(webpage);
			msg.mediaObject = webpage;
			msg.title = ShareTitle;
			msg.description = ShareText;
			

			AssetManager asm=getAssets();

			InputStream is=asm.open("smallicon.png");
			
			Drawable d =Drawable.createFromStream(is, null);
			msg.thumbData = Util.bmpToByteArray(
					((BitmapDrawable) d).getBitmap(), true);

			SendMessageToWX.Req req = new SendMessageToWX.Req();
			req.transaction = buildTransaction("webpage");
			req.message = msg;
			req.scene = api.getWXAppSupportAPI() >= 0x21020001 ? SendMessageToWX.Req.WXSceneTimeline
					: SendMessageToWX.Req.WXSceneSession;
			api.sendReq(req);

			// Log.e("Path",
			// Environment.getExternalStorageDirectory()+":::ByMJW");
		} else {
			this.runOnUiThread(new Runnable() {
				public void run() {
					Toast.makeText(ThisContext, "�Բ�������û�а�װ΢�ţ��޷�����",
							Toast.LENGTH_SHORT).show();
				}
			});
		}
	}

	private void ShareWeiBo(String Title, String Text, String Name) {
		if (isInstallApplication(ThisContext, "com.sina.weibo")) {
			ComponentName comp = new ComponentName("com.sina.weibo",
					"com.sina.weibo.EditActivity");
			Intent intent = new Intent(Intent.ACTION_SEND);
			intent.setType("image/*");
			intent.putExtra(Intent.EXTRA_TEXT, Text);
			intent.putExtra(Intent.EXTRA_TITLE, Title);
			// Log.e("Path",
			// Environment.getExternalStorageDirectory()+"/Android/data/com.unitygames.sheephappens/files");
			File MyPicFile = new File(Environment.getExternalStorageDirectory()
					+ "/Android/data/com.unitygames.sheephappens/files", Name);
			Uri uri = Uri.fromFile(MyPicFile);
			intent.putExtra(Intent.EXTRA_STREAM, uri);
			intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
			intent.setComponent(comp);
			startActivity(intent);
			// intent.setFlags(0x3000001);
		} else if (isInstallApplication(ThisContext, "com.sina.weibog3")) {
			ComponentName comp = new ComponentName("com.sina.weibog3",
					"com.sina.weibog3.EditActivity");
			Intent intent = new Intent(Intent.ACTION_SEND);
			intent.setType("image/*");
			intent.putExtra(Intent.EXTRA_TEXT, Text);
			intent.putExtra(Intent.EXTRA_TITLE, Title);
			// Log.e("Path",
			// Environment.getExternalStorageDirectory()+"/Android/data/com.unitygames.sheephappens/files");
			File MyPicFile = new File(Environment.getExternalStorageDirectory()
					+ "/Android/data/com.unitygames.sheephappens/files", Name);
			Uri uri = Uri.fromFile(MyPicFile);
			intent.putExtra(Intent.EXTRA_STREAM, uri);
			intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
			intent.setComponent(comp);
			startActivity(intent);
		} else {
			this.runOnUiThread(new Runnable() {
				public void run() {
					Toast.makeText(ThisContext, "�Բ�������û�а�װ����΢�����޷�����",
							Toast.LENGTH_SHORT).show();
				}
			});

		}
	}

	public void ShareTest(String Name, String title, String text) {
		// File SharePic = new
		// File(Environment.getDataDirectory()+"/Android/data/com.jerry.crop/files","UserFace.png");
		// Uri uri = Uri.fromFile(SharePic);

		File MyPicFile = new File(Environment.getExternalStorageDirectory()
				+ "/Android/data/com.unitygames.sheephappens/files", Name);
		Uri uri = Uri.fromFile(MyPicFile);
        
		Intent i = new Intent(Intent.ACTION_SEND);
		i.putExtra(Intent.EXTRA_STREAM, uri);// ���������ͼƬ��uri
		//i.putExtra(Intent.EXTRA_SUBJECT, title);
		i.putExtra(Intent.EXTRA_TITLE, title);
		i.putExtra(Intent.EXTRA_TEXT, text);
		i.putExtra("Kdescription", text);
		i.setType("image/*");
		 //i.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);

		Log.e("Unity","BeforLog");

		
		
		
		startActivityForResult(Intent.createChooser(i, "ѡ�������"),199204);
		//Log.e("Unity",choseI.getPackage());
	}

	PackageManager pm;

	/*
	 * ��ȡ���� ͼ��
	 */
	public Drawable getAppIcon(String packname) {
		try {
			pm = ThisContext.getPackageManager();
			ApplicationInfo info = pm.getApplicationInfo(packname, 0);
			return info.loadIcon(pm);
		} catch (NameNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
	}

	private String buildTransaction(final String type) {
		return (type == null) ? String.valueOf(System.currentTimeMillis())
				: type + System.currentTimeMillis();
	}

	public void ShowPhotoAsk(final String aaa) {
		this.runOnUiThread(new Runnable() {
			public void run() {
				ShowAsk(aaa);
			}
		});
	}

	public void ShowAsk(String aaa) {

		new AlertDialog.Builder(this)
				.setMessage("��ѡ���ȡͷ��ķ�ʽ")
				.setNegativeButton("����ȡ",
						new DialogInterface.OnClickListener() {
							@Override
							public void onClick(DialogInterface dialog,
									int which) {
								if (hasSdcard()) {
									gallery("aa");
								} else {
									Toast.makeText(ThisContext,
											"�Բ������ֻ���sd�����޻򲻿��ã��޷�ѡȡͷ��",
											Toast.LENGTH_SHORT).show();
								}
							}
						})
				.setPositiveButton("ֱ������",
						new DialogInterface.OnClickListener() {
							public void onClick(DialogInterface dialog,
									int whichButton) {
								if (hasSdcard()) {
									camera("aa");
								} else {
									Toast.makeText(ThisContext,
											"�Բ������ֻ���sd�����޻򲻿��ã��޷���ȡͷ��",
											Toast.LENGTH_SHORT).show();
								}
							}
						}).show();
	}

	/*
	 * ������ȡ
	 */
	public void gallery(String aaa) {

		// Log.e("Where is SD",Environment.getExternalStorageDirectory().toString());
		// ����ϵͳͼ�⣬ѡ��һ��ͼƬ
		Intent intent = new Intent(Intent.ACTION_PICK);
		intent.setType("image/*");
		// ����һ�����з���ֵ��Activity��������ΪPHOTO_REQUEST_GALLERY
		startActivityForResult(intent, PHOTO_REQUEST_GALLERY);
	}

	/*
	 * �������ȡ
	 */
	public void camera(String aaa) {
		// �������

		Intent intent = new Intent("android.media.action.IMAGE_CAPTURE");
		// int cameraId = android.hardware.Camera.getNumberOfCameras();
		//
		// //Log.e("cameras", String.valueOf(cameraId));
		//
		// if(cameraId>1)
		// {
		// intent.putExtra("camerasensortype", 2);
		// }
		intent.putExtra("autofocus", true);
		// �жϴ洢���Ƿ�����ã����ý��д洢
		if (hasSdcard()) {
			tempFile = new File(Environment.getExternalStorageDirectory(),
					PHOTO_FILE_NAME);
			// ���ļ��д���uri
			Uri uri = Uri.fromFile(tempFile);
			intent.putExtra(MediaStore.EXTRA_OUTPUT, uri);
		}

		// ����һ�����з���ֵ��Activity��������ΪPHOTO_REQUEST_CAREMA
		startActivityForResult(intent, PHOTO_REQUEST_CAREMA);
	}

	/*
	 * ����ͼƬ
	 */
	private void crop(Uri uri) {
		// �ü�ͼƬ��ͼ
		Intent intent = new Intent("com.android.camera.action.CROP");
		intent.setDataAndType(uri, "image/*");
		intent.putExtra("crop", "true");
		// �ü���ı�����1��1
		intent.putExtra("aspectX", 1);
		intent.putExtra("aspectY", 1);
		// �ü������ͼƬ�ĳߴ��С
		intent.putExtra("outputX", 250);
		intent.putExtra("outputY", 250);

		intent.putExtra("outputFormat", "PNG");// ͼƬ��ʽ
		intent.putExtra("noFaceDetection", true);// ȡ������ʶ��
		intent.putExtra("return-data", true);
		// ����һ�����з���ֵ��Activity��������ΪPHOTO_REQUEST_CUT
		startActivityForResult(intent, PHOTO_REQUEST_CUT);

	}

	/*
	 * �ж�sdcard�Ƿ񱻹���
	 */
	private boolean hasSdcard() {
		if (Environment.getExternalStorageState().equals(
				Environment.MEDIA_MOUNTED)) {
			return true;
		} else {
			return false;
		}
	}

	boolean isInstallApplication(Context context, String packageName) {
		try {
			mPackageManager.getApplicationInfo(packageName,
					PackageManager.GET_UNINSTALLED_PACKAGES);
			return true;
		} catch (NameNotFoundException e) {
			return false;
		}
	}

	// boolean isInstallAction(String ActionName)
	// {
	// Intent intent = new Intent(ActionName);
	// List resolveInfo = mPackageManager.queryIntentActivities(intent,
	// PackageManager.GET_INTENT_FILTERS);
	// if(resolveInfo.size() == 0){
	// return false;
	// }
	// else
	// {
	// return true;
	// }
	//
	// }

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		if (requestCode == PHOTO_REQUEST_GALLERY) {
			// ����᷵�ص�����
			if (data != null) {
				// �õ�ͼƬ��ȫ·��

				Uri uri = data.getData();
				// Log.e("PicUri", data.getDataString());
				crop(uri);
			}
			super.onActivityResult(requestCode, resultCode, data);

		} else if (requestCode == PHOTO_REQUEST_CAREMA) {

			crop(Uri.fromFile(tempFile));
			super.onActivityResult(requestCode, resultCode, data);
		} else if (requestCode == PHOTO_REQUEST_CUT) {
			// �Ӽ���ͼƬ���ص�����
			if (data != null) {

				Bitmap bitmap = data.getParcelableExtra("data");
				save(bitmap);
			}
			super.onActivityResult(requestCode, resultCode, data);
		}
		else if (requestCode == 199204)
		{
			Log.e("Unity","I'm Here");
		}

		
	}

	public void save(Bitmap baseBitmap) {
		try {
			File file = new File(Environment.getExternalStorageDirectory()
					+ "/Android/data/com.unitygames.sheephappens/files");
			if (!file.exists()) 
			{
				file.mkdirs();
			}
			File myFaceFile = new File(
					Environment.getExternalStorageDirectory()
							+ "/Android/data/com.unitygames.sheephappens/files/UserFace.png");
			OutputStream stream = new FileOutputStream(myFaceFile);
			baseBitmap.compress(CompressFormat.PNG, 100, stream);
			stream.close();
			// ģ��һ���㲥��֪ͨϵͳsdcard������
			Intent intent = new Intent();
			intent.setAction(Intent.ACTION_MEDIA_MOUNTED);
			intent.setData(Uri.fromFile(Environment
					.getExternalStorageDirectory()));
			sendBroadcast(intent);
			this.runOnUiThread(new Runnable() {
				public void run() {
					Toast.makeText(ThisContext, "����ͼƬ�ɹ�", Toast.LENGTH_SHORT)
							.show();
				}
			});
			UnityPlayer.UnitySendMessage("1PanelOfUserAccount", "ChangeFace",
					"UserFace.png");
		} catch (Exception e) {

			this.runOnUiThread(new Runnable() {
				public void run() {
					Toast.makeText(ThisContext, "����ͼƬʧ��", Toast.LENGTH_SHORT)
							.show();
				}
			});
			e.printStackTrace();
		}
		try {
			// ����ʱ�ļ�ɾ��
			tempFile.delete();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
}
