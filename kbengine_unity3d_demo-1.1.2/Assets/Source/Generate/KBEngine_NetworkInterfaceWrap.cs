﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class KBEngine_NetworkInterfaceWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(KBEngine.NetworkInterface), typeof(System.Object));
		L.RegFunction("sock", sock);
		L.RegFunction("reset", reset);
		L.RegFunction("close", close);
		L.RegFunction("packetReceiver", packetReceiver);
		L.RegFunction("valid", valid);
		L.RegFunction("_onConnectionState", _onConnectionState);
		L.RegFunction("connectTo", connectTo);
		L.RegFunction("send", send);
		L.RegFunction("process", process);
		L.RegFunction("New", _CreateKBEngine_NetworkInterface);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegConstant("TCP_PACKET_MAX", 1460);
		L.RegFunction("ConnectCallback", KBEngine_NetworkInterface_ConnectCallback);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateKBEngine_NetworkInterface(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				KBEngine.NetworkInterface obj = new KBEngine.NetworkInterface();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: KBEngine.NetworkInterface.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int sock(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			KBEngine.NetworkInterface obj = (KBEngine.NetworkInterface)ToLua.CheckObject<KBEngine.NetworkInterface>(L, 1);
			System.Net.Sockets.Socket o = obj.sock();
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int reset(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			KBEngine.NetworkInterface obj = (KBEngine.NetworkInterface)ToLua.CheckObject<KBEngine.NetworkInterface>(L, 1);
			obj.reset();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int close(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			KBEngine.NetworkInterface obj = (KBEngine.NetworkInterface)ToLua.CheckObject<KBEngine.NetworkInterface>(L, 1);
			obj.close();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int packetReceiver(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			KBEngine.NetworkInterface obj = (KBEngine.NetworkInterface)ToLua.CheckObject<KBEngine.NetworkInterface>(L, 1);
			KBEngine.PacketReceiver o = obj.packetReceiver();
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int valid(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			KBEngine.NetworkInterface obj = (KBEngine.NetworkInterface)ToLua.CheckObject<KBEngine.NetworkInterface>(L, 1);
			bool o = obj.valid();
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _onConnectionState(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			KBEngine.NetworkInterface obj = (KBEngine.NetworkInterface)ToLua.CheckObject<KBEngine.NetworkInterface>(L, 1);
			KBEngine.NetworkInterface.ConnectState arg0 = (KBEngine.NetworkInterface.ConnectState)ToLua.CheckObject<KBEngine.NetworkInterface.ConnectState>(L, 2);
			obj._onConnectionState(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int connectTo(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 5);
			KBEngine.NetworkInterface obj = (KBEngine.NetworkInterface)ToLua.CheckObject<KBEngine.NetworkInterface>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
			KBEngine.NetworkInterface.ConnectCallback arg2 = (KBEngine.NetworkInterface.ConnectCallback)ToLua.CheckDelegate<KBEngine.NetworkInterface.ConnectCallback>(L, 4);
			object arg3 = ToLua.ToVarObject(L, 5);
			obj.connectTo(arg0, arg1, arg2, arg3);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int send(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			KBEngine.NetworkInterface obj = (KBEngine.NetworkInterface)ToLua.CheckObject<KBEngine.NetworkInterface>(L, 1);
			KBEngine.MemoryStream arg0 = (KBEngine.MemoryStream)ToLua.CheckObject<KBEngine.MemoryStream>(L, 2);
			bool o = obj.send(arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int process(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			KBEngine.NetworkInterface obj = (KBEngine.NetworkInterface)ToLua.CheckObject<KBEngine.NetworkInterface>(L, 1);
			obj.process();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int KBEngine_NetworkInterface_ConnectCallback(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);
			LuaFunction func = ToLua.CheckLuaFunction(L, 1);

			if (count == 1)
			{
				Delegate arg1 = DelegateTraits<KBEngine.NetworkInterface.ConnectCallback>.Create(func);
				ToLua.Push(L, arg1);
			}
			else
			{
				LuaTable self = ToLua.CheckLuaTable(L, 2);
				Delegate arg1 = DelegateTraits<KBEngine.NetworkInterface.ConnectCallback>.Create(func, self);
				ToLua.Push(L, arg1);
			}
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

