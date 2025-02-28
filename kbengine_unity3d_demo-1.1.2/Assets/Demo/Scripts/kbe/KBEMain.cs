﻿using UnityEngine;
using System;
using System.Collections;
using KBEngine;

/*
	可以理解为插件的入口模块
*/

public class KBEMain : MonoBehaviour 
{	
	// 在unity3d界面中可见选项	
    static public bool isStartEngine = false;

	void Awake() 
	 {
		DontDestroyOnLoad(transform.gameObject);
	 }
 
	// Use this for initialization
	void Start () 
	{
		MonoBehaviour.print("clientapp::start()");
	}
	
	public virtual void initKBEngine()
	{
        isStartEngine = true;
	//	Dbg.debugLevel = debugLevel;

        KBEngine.Event.registerIn("_closeNetwork", this, "_closeNetwork");
        //在lua中执行初始化引擎
     //   KBELuaUtil.CallMethod("KBEngineLua", "InitEngine");
	}

    public void _closeNetwork(NetworkInterface networkInterface)
    {
        networkInterface.close();
    }	
	void OnDestroy()
	{
        KBEngine.Event.deregisterIn(this);
	}
	
	void FixedUpdate () 
	{
        // 处理外层抛入的事件
        KBEngine.Event.processInEvents();
        KBEngine.Event.processOutEvents();
	}

}
