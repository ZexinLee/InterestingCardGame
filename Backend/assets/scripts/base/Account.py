# -*- coding: utf-8 -*-
import KBEngine
import random
from KBEDebug import *

class Account(KBEngine.Proxy):
	def __init__(self):
		KBEngine.Proxy.__init__(self)

		if self.Data["rank"]<1000:
			self.Data["rank"] = 1000
			
		self.role = 0
			
		if not self.CardList:
			self.CardList.extend([{"cardID":0,"value":1}, {"cardID":1,"value":1},{"cardID":2,"value":1}, {"cardID":3,"value":1},{"cardID":4,"value":1}])
			
		self.CardNum = 53
		
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

	def reqName(self):
		INFO_MSG("reqName")
		self.Data = self.Data
		self.client.onName(self.Data['name'])
		

	def reqCreateName(self,nameset):
		DEBUG_MSG("ResetName old:%s new:%s"% (self.Data['name'], nameset))
		self.Data['name']=nameset
		self.Data = self.Data
		self.writeToDB()

	def reqChooseRole(self,role):
		DEBUG_MSG("reqChooseRole old:%s new:%s"% (self.Data['role'], role))
		self.Data['role']=role
		self.Data = self.Data

	def reqBuyKaBao(self, sum):
		DEBUG_MSG("reqBuyKaBao sum:%s" % (sum))
		result = 0
		if self.Data['money'] >= sum * 100:
			self.Data['kabao'] += sum
			self.Data['money'] -= sum * 100
			result = 1
		self.Data = self.Data
		self.client.onBuyKaBaoResult(result)

	def reqOpenKaBao(self):
		DEBUG_MSG("Account[%i].reqOpenKaBao:" % self.id)
		self.Data['kabao']=self.Data['kabao']-1

		data =  []

		for i in range(5):
			k = random.randint(0,self.CardNum)
			hasCard = False
			for i in range(len(self.CardList)):
				if self.CardList[i]["cardID"] == k:
					self.CardList[i]["value"] += 1
					hasCard = True
			if not hasCard:
				self.CardList.append({"cardID":k,"value":1})

			data.append(k)
		self.Data = self.Data
		self.CardList = self.CardList
		self.client.onOpenKaBaoResult(data)

	def reqStartMarch(self, role):
		self.role = role
		DEBUG_MSG("Account[%i].reqStartMarch:" % self.id)
		KBEngine.globalData["Halls"].reqAddMarcher(self)

	def reqStopMarch(self):
		DEBUG_MSG("Account[%i].reqStopMarch" % self.id)
		KBEngine.globalData["Halls"].reqDelMarcher(self)

	def OnEnterBattelField(self,battlefiled,_playerID):
		DEBUG_MSG("Account[%i].OnEnterBattelField" % self.id)
		self.BattleField = battlefiled
		self.playerID = _playerID
		self.BattleField.AccountReady(self.playerID)

	def BattleFailed(self):  # 战斗匹配失败
		DEBUG_MSG("Account[%i].BattleInitFailed" % self.id)

		self.reqStartMarch(self.role)

	def creatAvatar(self,battleFiled):
		'''
		'''
		DEBUG_MSG("Account[%i].creatAvatar" %(self.id))
		self.initRole = [[25, 9, 2], [35, 10, 1], [30, 12, 1], [30, 10, 1]]
		avatar = list()
		self.Playing_Avatar_List = list()
		for i in range(30):
			avatar.append(random.sample(list(self.CardList), 1))
		# self.Playing_Avatar_List = list(map(lambda x: x[0]['cardID'], avatar))
		# self.Playing_Avatar_List = [1,2,3,4,5,6,7,8,9]
		for i in range(30):
			self.Playing_Avatar_List.append(random.randint(0,53))
		self.onPlayingBattlefiled = battleFiled
		self.BattleData['selfenergy'] = 0
		self.BattleData['selfstress'] = 0
		self.BattleData['oppenergy'] = 0
		self.BattleData['oppstress'] = 0
		self.BattleData['selfhandcardnum'] = 0
		self.BattleData['selfcardnum'] = 30
		self.BattleData['opphandcardnum'] = 0
		self.BattleData['oppcardnum'] = 30

		self.BattleHandCardList = []



	def OnClientMsg(self,msg):
		if self.client == None:
			return
		self.client.onServerMsg(msg)
		
	def reqChat(self, msg):
		DEBUG_MSG("Account[%i].reqChat" % (self.id))
		self.onPlayingBattlefiled.reqChat(self.another(self.playerID), msg)

	def recvMsg(self, msg):
		DEBUG_MSG("Account[%i].recvMsg" % (self.id))
		self.client.recChatMsg(msg)
		
	def onInitBattleField(self, name):
		self.synBattleData()
		self.client.onInitBattleField(self.Data['role'], name)
# --------------------------------------------Battle Start-------------------------------------------

	def getCardnumInfo(self):
		num1 = len(self.BattleHandCardList)
		num2 = len(self.Playing_Avatar_List)
		num3 = len(self.onPlayingBattlefiled.player[(self.playerID+1)%2].BattleHandCardList)
		num4 = len(self.onPlayingBattlefiled.player[(self.playerID + 1) % 2].Playing_Avatar_List)
		self.BattleData['selfhandcardnum'] = num1
		self.BattleData['selfcardnum'] = num2
		self.BattleData['opphandcardnum'] = num3
		self.BattleData['oppcardnum'] = num4
		print('selfcardnum',num1,num2,num3,num4)


	def creatHandCard(self, cardSum):
		DEBUG_MSG("Account[%i].creatHandCard" % (self.id))
		if not self.Playing_Avatar_List:
			self.onPlayingBattlefiled.tired(self.playerID)
		else:
			for i in range(cardSum):
				if (len(self.Playing_Avatar_List) >= 1 and (len(self.BattleHandCardList) + 1 <= 5)):
					self.BattleHandCardList.append(self.Playing_Avatar_List[-1])
					del self.Playing_Avatar_List[-1]
				

		self.getCardnumInfo()
		self.BattleHandCardList = self.BattleHandCardList


	def reqNextRound(self):
		DEBUG_MSG("Account[%i].reqNextRound" % (self.id))
		self.BattleField.reqNextRound(self.playerID)


	def reqGiveUp(self):
		DEBUG_MSG("Account[%i].reqGiveUp" % (self.id))
		self.BattleField.reqGiveUp(self.playerID)

	def reqUseCard(self, position, target):
		DEBUG_MSG("Account[%i].reqUseCard,cardID [%i],target [%i]" % (self.id,position,target))
		# 使用卡
		# 获取index
		# 获取遍历kind
		# 操作Field
		# 操作信息更新
		# 更新信息
		if self.BattleHandCardList:
			self.onPlayingBattlefiled.playerUseCard(self.playerID,position,target)


	def onUserCard(self,cardID = -1):
		DEBUG_MSG("Account[%i].onUserCard" % (self.id))
		
		self.synBattleData()
		self.client.onUseCard(cardID)
		
	def onRemoveHandCard(self):
		self.synBattleData()
		
		self.client.onRemoveHandCard()

	def reqRetinueAction(self, position, target):
		DEBUG_MSG("Account[%i].reqRetinueAction,position [%i],target [%i]" % (self.id, position, target))
		# 获取index
		# 获取遍历kind
		# 操作Field
		# 操作信息更新
		# 更新信息
		self.onPlayingBattlefiled.playerRetinueAction(self.playerID, position, target)


	def onRetinueAction(self):
		DEBUG_MSG("Account[%i].onRetinueAction" % (self.id))

		self.client.onRetinueAction()

	def accountCheck(self):
		i = 0
		while True:
			if i >= len(self.onPlayingBattlefiled.field[0]):
				break
			if self.onPlayingBattlefiled.field[0][i]['hp'] <= 0:
				del self.onPlayingBattlefiled.field[0][i]
				i = 0
			else:
				i += 1
		i = 0
		while True:
			if i >= len(self.onPlayingBattlefiled.field[1]):
				break
			if self.onPlayingBattlefiled.field[1][i]['hp'] <= 0:
				del self.onPlayingBattlefiled.field[1][i]
				i = 0
			else:
				i += 1

	def synBattleData(self):
		DEBUG_MSG("Account[%i].synBattleData" % (self.id))
		data = self.onPlayingBattlefiled.data[self.playerID]
		opps = self.onPlayingBattlefiled.data[self.another(self.playerID)]
		
		self.BattleData['selfenergy'] = data['energy']
		self.BattleData['selfstress'] = data['stress']
		self.BattleData['oppenergy'] = opps['energy']
		self.BattleData['oppstress'] = opps['stress']
		self.BattleData['selfenergylimit'] = data['energylimit']
		self.BattleData['selfstresslimit'] = data['limit']
		self.BattleData['oppenergylimit'] = opps['energylimit']
		self.BattleData['oppstresslimit'] = opps['limit']
		
		DEBUG_MSG("selfenergy",self.BattleData['selfenergy'])
		DEBUG_MSG("oppenergy", self.BattleData['oppenergy'])
		
		self.getCardnumInfo()
		
		self.Field['selffollower'] = []
		self.Field['oppfollower'] = []
		
		self.accountCheck()
		DEBUG_MSG(self.onPlayingBattlefiled.field)
		for i in self.onPlayingBattlefiled.field[self.playerID]:
			self.Field['selffollower'].append({'id':i['id'],'hp':i['hp'],'att':i['att'],'cost':i['cost'],'buff':[]})
		for i in self.onPlayingBattlefiled.field[self.another(self.playerID)]:
			self.Field['oppfollower'].append({'id':i['id'],'hp':i['hp'],'att':i['att'],'cost':i['cost'],'buff':[]})
		
		self.BattleInfoList = []
		for i in self.onPlayingBattlefiled.actlist[self.playerID]:
			if i[0] == 'att':
				self.synAttList(i)
				
			elif i[0] == 'cure':
				self.synCureList(i)
				
			# elif i[0] == 'limit':
			# 	self.synLimitList(i)
			#
			# elif i[0] == 'energylimit':
			# 	self.synEnergyLimitList(i)
			#
			# elif i[0] == 'power':
			# 	self.synPowerList(i)
				
			elif i[0] == 'addenergy':
				self.synAddEnergyList(i)
				
		DEBUG_MSG("selfenergy", self.BattleData['selfenergy'])
		DEBUG_MSG("oppenergy", self.BattleData['oppenergy'])
		bdata = [self.BattleData['selfenergy'],
		         self.BattleData['selfenergylimit'],
		         self.BattleData['selfstress'],
		         self.BattleData['selfstresslimit'],
		         self.BattleData['oppenergy'],
		         self.BattleData['oppenergylimit'],
		         self.BattleData['oppstress'],
		         self.BattleData['oppstresslimit'],
		         self.BattleData['selfhandcardnum'],
		         self.BattleData['selfcardnum'],
		         self.BattleData['opphandcardnum'],
		         self.BattleData['oppcardnum']]
		DEBUG_MSG(self.playerID,"    ",bdata)
		self.client.recv_BattleData(bdata)
		self.BattleInfoList = self.BattleInfoList
		self.BattleData = self.BattleData
		self.BattleHandCardList = self.BattleHandCardList
		self.Field = self.Field
		
		self.BattleInfoList = []
		self.onPlayingBattlefiled.actlist[self.playerID].clear()
	
	def synCureList(self, l):
		DEBUG_MSG("Account[%i].synAttList" % (self.id))
		DEBUG_MSG({'action': 'cure', 'start': l[2], 'end': l[3], 'value': l[4],'valuehp':0})
		self.BattleInfoList.append({'action': 'cure', 'start': l[2], 'end': l[3], 'value': l[4],'valuehp':0})
	
	def synAttList(self, l):
		DEBUG_MSG("Account[%i].synAttList" % (self.id))
		DEBUG_MSG({'action': 'att', 'start': l[2], 'end': l[3], 'value': l[4],'valuehp':0})
		self.BattleInfoList.append({'action': 'att', 'start': l[2], 'end': l[3], 'value': l[4],'valuehp':0})
		
	# def synLimitList(self,l):
	# 	DEBUG_MSG("Account[%i].synLimitList" % (self.id))
	# 	self.BattleInfoList.append({'action': 'limit', 'start': l[2], 'end': l[3], 'value': l[4],'valuehp':0})
	#
	# def synEnergyLimitList(self,l):
	# 	DEBUG_MSG("Account[%i].synEnergyLimitList" % (self.id))
	# 	self.BattleInfoList.append({'action': 'energylimit', 'start': l[2], 'end': l[3], 'value': l[4],'valuehp':0})
			
	def synAddEnergyList(self,l):
		DEBUG_MSG("Account[%i].synAddEnergyList" % (self.id))
		self.BattleInfoList.append({'action': 'addenergy', 'start': l[2], 'end': l[3], 'value': l[4], 'valuehp':0})
			
	# def synPowerList(self,l):
	# 	DEBUG_MSG("Account[%i].synPowerList" % (self.id))
	# 	self.BattleInfoList.append({'action': 'power', 'start': l[2], 'end': l[3], 'value': l[4], 'valuehp':l[5]})

	def onNextRound_Your(self, Round):
		DEBUG_MSG("Account[%i].onNextRound_Your" % (self.id))
		if len(self.BattleHandCardList) < 5:
			self.creatHandCard(self.onPlayingBattlefiled.drawnum[self.playerID])
		self.synBattleData()
		self.client.onNextRound_Your(Round)

	def onNextRound_Opps(self,Round):
		DEBUG_MSG("Account[%i].onNextRound_Opps" % (self.id))
		self.synBattleData()
		self.client.onNextRound_Opps(Round)
		
	def BattleEndResult(self,result):
		DEBUG_MSG("Account[%i].BattleEndResult" % (self.id))
		if result == 1:
			money = random.randint(100,200)
			exp = random.randint(75,100)
			rank = 10
			self.Data['money'] += money
			self.Data['exp'] += exp
			self.Data['rank'] += rank
		else:
			money = random.randint(50, 100)
			exp = random.randint(50, 75)
			rank = -5
			self.Data['money'] += money
			self.Data['exp'] += exp
			self.Data['rank'] += rank
		self.Data = self.Data
		self.client.onBattleEndResult(result,money,exp,rank)
		
	def another(self,playerID):
		if playerID == 0:
			return 1
		return 0


