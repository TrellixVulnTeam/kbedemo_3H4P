# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
from AVATAR_INFO import *
class Account(KBEngine.Proxy):
	def __init__(self):
		KBEngine.Proxy.__init__(self)
		self.activeAvatar = None
	def onTimer(self, id, userArg):
		"""
		KBEngine method.
		使用addTimer后， 当时间到达则该接口被调用
		@param id		: addTimer 的返回值ID
		@param userArg	: addTimer 最后一个参数所给入的数据
		"""
		DEBUG_MSG(id, userArg)
		
	def onEntitiesEnabled(self):
		"""
		KBEngine method.
		该entity被正式激活为可使用， 此时entity已经建立了client对应实体， 可以在此创建它的
		cell部分。
		"""
		INFO_MSG("account[%i] entities enable. mailbox:%s" % (self.id, self.client))
			
	def onLogOnAttempt(self, ip, port, password):
		"""
		KBEngine method.
		客户端登陆失败时会回调到这里
		"""
		INFO_MSG(ip, port, password)
		return KBEngine.LOG_ON_ACCEPT
		
	def onClientDeath(self):
		"""
		KBEngine method.
		客户端对应实体已经销毁
		"""
		DEBUG_MSG("Account[%i].onClientDeath:" % self.id)
		self.destroy()

	def reqAvatarList(self):
		"""
		exposed.
		客户端请求查询角色列表
		"""
		DEBUG_MSG("Account[%i].reqAvatarList: size=%i." % (self.id, len(self.characters)))
		self.client.onReqAvatarList(self.characters)

	def reqCreateAvatar(self, name, pro,camp,gender):
		if len(self.characters) >= 3:
			DEBUG_MSG("Account[%i].reqCreateAvatar:%s. character=%s.\n" % (self.id, name, self.characters))		
			self.client.onCreateAvatarResult(3, None)
			return		
	
		props = {
			"name"				: name,
			"pro"				: pro,
			"level"				: 1,
			"mapid"				: 0,
			"dir"				: (0, 0, 0),
			"pos"				: (0,0,0),
			"gender"			:gender,		
			"camp"				:camp,	
			"fashion"			:(0,0,0)
			}		
		avatar = KBEngine.createBaseLocally("Avatar",props)
		if avatar:
			avatar.writeToDB(self._onAvatarSaved)	

	def _onAvatarSaved(self, success, avatar):
		"""
		新建角色写入数据库回调
		"""	
		# 如果此时账号已经销毁， 角色已经无法被记录则我们清除这个角色
		if self.isDestroyed:
			if avatar:
				avatar.destroy(True)				
			return		
		avatarinfo = TAvatarInfo()
		avatarinfo.extend([0, "", 0, 0,0,0,(0,0,0),0,(0,0,0),(0,0,0)])
		if success:		
			avatarinfo[0]=avatar.databaseID
			avatarinfo[1]=avatar.name
			avatarinfo[2] = avatar.pro
			avatarinfo[3] = avatar.camp
			avatarinfo[4] = avatar.gender
			avatarinfo[5] = avatar.level
			avatarinfo[6] = avatar.fashion
			avatarinfo[7] = avatar.mapid
			avatarinfo[8] = avatar.pos
			avatarinfo[9] = avatar.dir
			self.characters[avatar.databaseID] = avatarinfo
			self.writeToDB()
			avatar.destroy()

		if self.client:
			self.client.onCreateAvatarResult(0, avatarinfo)

	def reqRemoveAvatar(self, dbid):

		"""
		exposed.
		客户端请求删除一个角色
		"""
		DEBUG_MSG("Account[%i].reqRemoveAvatar: %s" % (self.id, name))
		found = 0
		for key, info in self.characters.items():
			if info[1] == name:
				del self.characters[key]
				found = key
				break
			
		self.client.onRemoveAvatar(found)

	def selectAvatarGame(self, dbid):
		"""
		exposed.
		客户端选择某个角色进行游戏
		"""
		DEBUG_MSG("Account[%i].selectAvatarGame:%i. self.activeAvatar=%s" % (self.id, dbid, self.activeAvatar))
		# 注意:使用giveClientTo的entity必须是当前baseapp上的entity
		if self.activeAvatar is None:
			if dbid in self.characters:
				#self.lastSelCharacter = dbid
				# 由于需要从数据库加载角色，因此是一个异步过程，加载成功或者失败会调用__onAvatarCreated接口
				# 当角色创建好之后，account会调用giveClientTo将客户端控制权（可理解为网络连接与某个实体的绑定）切换到Avatar身上，
				# 之后客户端各种输入输出都通过服务器上这个Avatar来代理，任何proxy实体获得控制权都会调用onEntitiesEnabled
				# Avatar继承了Teleport，Teleport.onEntitiesEnabled会将玩家创建在具体的场景中
				KBEngine.createBaseFromDBID("Avatar", dbid, self.__onAvatarCreated)
			else:
				ERROR_MSG("Account[%i]::selectAvatarGame: not found dbid(%i)" % (self.id, dbid))
		else:
			self.giveClientTo(self.activeAvatar)

	def __onAvatarCreated(self, baseRef, dbid, wasActive):
		"""
		选择角色进入游戏时被调用
		"""
		if wasActive:
			ERROR_MSG("Account::__onAvatarCreated:(%i): this character is in world now!" % (self.id))
			return
		if baseRef is None:
			ERROR_MSG("Account::__onAvatarCreated:(%i): the character you wanted to created is not exist!" % (self.id))
			return
			
		avatar = KBEngine.entities.get(baseRef.id)
		if avatar is None:
			ERROR_MSG("Account::__onAvatarCreated:(%i): when character was created, it died as well!" % (self.id))
			return
		
		if self.isDestroyed:
			ERROR_MSG("Account::__onAvatarCreated:(%i): i dead, will the destroy of Avatar!" % (self.id))
			avatar.destroy()
			return			
		avatar.accountEntity = self
		self.activeAvatar = avatar
		self.giveClientTo(avatar)		