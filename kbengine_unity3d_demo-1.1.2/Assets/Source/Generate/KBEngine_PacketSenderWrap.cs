﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class KBEngine_PacketSenderWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(KBEngine.PacketSender), typeof(System.Object));
		L.RegFunction("networkInterface", networkInterface);
		L.RegFunction("send", send);
		L.RegFunction("New", _CreateKBEngine_PacketSender);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateKBEngine_PacketSender(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				KBEngine.NetworkInterface arg0 = (KBEngine.NetworkInterface)ToLua.CheckObject<KBEngine.NetworkInterface>(L, 1);
				KBEngine.PacketSender obj = new KBEngine.PacketSender(arg0);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: KBEngine.PacketSender.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int networkInterface(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			KBEngine.PacketSender obj = (KBEngine.PacketSender)ToLua.CheckObject<KBEngine.PacketSender>(L, 1);
			KBEngine.NetworkInterface o = obj.networkInterface();
			ToLua.PushObject(L, o);
			return 1;
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
			KBEngine.PacketSender obj = (KBEngine.PacketSender)ToLua.CheckObject<KBEngine.PacketSender>(L, 1);
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
}

