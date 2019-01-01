# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
from card import card_cost
import random


class BattleField(KBEngine.Base):
	def __init__(self):
		KBEngine.Base.__init__(self)
		
		DEBUG_MSG("battle filed base init ok::Battle March Successed player0:%i player1:%i" % (
			self.player0.id, self.player1.id))
		
		self.player = [self.player0, self.player1]
		self.Round = 1
		
		self.currentProcess = 0  # 0 刚开始 1 base实体存在确认完成 2 Avatar创建完成
		
		self.AccountReadyList = [0, 0]
		
		self.nextTimeID = self.addTimer(1, 1, 1)
		
		self.afterTime = 0
		self.roundTime = 120
		self.Round = 1
		self.CurrentPlayer = 0
		
		self.onlineCheckTimeID = 0
		
		self.field = [[], []]
		
		self.initRole = [[25,9,2],[35,10,1],[30,12,1],[30,10,1]]
		
		self.data = [{'energy': self.initRole[self.player0.role][1],
		              'stress': 0, 'limit':self.initRole[self.player0.role][0],
		              'energylimit':self.initRole[self.player0.role][1]},
		             
		             {'energy': self.initRole[self.player1.role][1],
		              'stress': 0, 'limit':self.initRole[self.player1.role][0],
		              'energylimit':self.initRole[self.player1.role][1]}]
		
		self.drawnum = [self.initRole[self.player0.role][2],self.initRole[self.player1.role][2]]
		self.outlineTimes = [0,0]
		self.tiredcount = [0,0]
		self.nextRoundExe = []
		self.everyRoundStartExe = []
		self.everyRoundEndExe = []
		
		self.actlist = [[],[]]
	
	def onTimer(self, id, userArg):
		
		# DEBUG_MSG(id, userArg)
		
		if userArg == 1:
			self.Process()
		elif userArg == 2:
			self.onDestroyTime()
		elif userArg == 3:
			self.startFirstRound()
		elif userArg == 5:
			self.onlineCheck()
		
		elif userArg == 10:
			self.TimeTick()
	
	def Process(self):
		DEBUG_MSG('BattleFiled Process  BFID:{%s]  currentProcess:[%s]' % (self.id, self.currentProcess))
		if self.currentProcess == 0:
			self.player0.OnEnterBattelField(self, 0)
			self.player1.OnEnterBattelField(self, 1)
		elif self.currentProcess == 1:
			if not self.AllReady():
				self.BattleFailed()
		elif self.currentProcess == 2:
			self.player0.creatAvatar(self)
			self.player1.creatAvatar(self)
		elif self.currentProcess == 3:
			
			self.MsgToClient_March("匹配成功 正在初始化战场",2)
			
			self.giveCard(0, 5)
			self.giveCard(1, 5)
			
			self.player0.onInitBattleField(self.player[1].Data['name'])
			self.player1.onInitBattleField(self.player[0].Data['name'])
			self.delTimer(self.nextTimeID)
			self.initBattle()
		
		self.currentProcess += 1
	
	def AccountReady(self, playerID):
		DEBUG_MSG('AccountReady playerID:[%s]' % playerID)
		if self.AccountReadyList[int(playerID)] == 1:
			WARNING_MSG("AccountReady.accont registe repeatly BFid:[%s] playerID:[%s]" % (self.id, playerID))
		self.AccountReadyList[int(playerID)] = 1
		if self.AccountReadyList == [1, 1]:
			if self.currentProcess == 3:
				self.Process()
	
	def BattleFailed(self):
		DEBUG_MSG("BattleFailed BattleField currentProcessID:[%s] BFid:[%s]" % (self.currentProcess, self.id))
		self.addTimer(1, 1, 2)
		
		if self.cell is not None:
			self.destroyCellEntity()
			return
		self.player0.BattleFailed()
		self.player1.BattleFailed()
	
	def onDestroyTime(self):
		DEBUG_MSG("battlefiled::onDestroyTimer: %i" % (self.id))
		if self.cell is not None:
			# 销毁cell实体
			self.destroyCellEntity()
			return
		self.destroy()
	
	def AllReady(self):
		DEBUG_MSG('AccountAllReady ')
		if self.AccountReadyList[0] == 1 and self.AccountReadyList[1] == 1:
			self.AccountReadyList = [0, 0]
			return True
		else:
			return False
	
	def checkDestroyed(self):
		if not self.player0.isDestroyed and not self.player1.isDestroyed:
			return True
		return False
	
	def MsgToClient_March(self, msg, id):
		DEBUG_MSG('MsgToClient_March ')
		if self.checkDestroyed():
			if id > 1:
				self.player0.OnClientMsg(msg)
				self.player1.OnClientMsg(msg)
			else:
				self.player[id].OnClientMsg(msg)
			
	def reqChat(self, playerID, msg):
		DEBUG_MSG('reqChat ')
		if not self.player[playerID].isDestroyed:
			self.player[playerID].recvMsg(msg)
	# ----------------------------------------Start Battle---------------------------------------
	
	def initBattle(self):  # 游戏开始入口
		self.onlineCheckTimeID = self.addTimer(10, 1, 5)  # ONLINE CHECK
		
		self.addTimer(3, 0, 3)
		
	def startFirstRound(self):
		DEBUG_MSG("startFirstRound:[%i]" % (self.id))
		
		self.Round = 1
		self.CurrentPlayer = 0
		self.data[0]['energy'] = self.getEnergy(0)
		self.data[1]['energy'] = self.getEnergy(1)
		DEBUG_MSG("player0",self.data[0]['energy'],"player1",self.data[1]['energy'])
		self.player[self.CurrentPlayer].onNextRound_Your(self.Round)
		self.player[self.another(self.CurrentPlayer)].onNextRound_Opps(self.Round)
		self.addTimer(0, 1, 10)
		
		
		
	
	def EndBattle(self, successPlayerID):
		DEBUG_MSG("EndBattle successPlayerID:[%i]  BattleField:[%i]" % (successPlayerID, self.id))
		if successPlayerID == 0:
			if not self.player[0].isDestroyed:
				self.player0.BattleEndResult(1)
			if not self.player[1].isDestroyed:
				self.player1.BattleEndResult(0)
		else:
			if not self.player[0].isDestroyed:
				self.player0.BattleEndResult(0)
			if not self.player[1].isDestroyed:
				self.player1.BattleEndResult(1)
		self.addTimer(2,1,2)
	
	def TimeTick(self):
		self.afterTime += 1
		if self.afterTime > self.roundTime:
			self.reqNextRound(self.CurrentPlayer)
			
	def getEnergy(self,playerID):
		# result = 1
		# if self.Round < 10:
		# 	result = 10
		# else:
		# 	result = 10
		# if result > self.data[playerID]['energylimit']:
		# 	result = self.data[playerID]['energylimit']
		# return result
		return self.data[playerID]['energylimit']
	
	# def EnergyLimit(self):
	# 	limit = 1
	# 	if self.Round < 10:
	# 		# limit = self.Round
	# 		# 测试
	# 		limit = 10
	# 	else:
	# 		limit = 10
	# 	self.data[0]['energylimit'] = limit
	# 	self.data[1]['energylimit'] = limit
	
	def reqNextRound(self, playerID):
		DEBUG_MSG("battlefiled nextRound")
		if playerID == self.CurrentPlayer:
			if self.everyRoundEndExe:
				for i in self.everyRoundEndExe:
					args = i[1:]
					kwargs = {}
					eval('self.'+i[0])(*args, **kwargs)
					
			self.Round += 1
			self.CurrentPlayer = (self.Round + 1) % 2
			self.afterTime = 0
			
			# self.EnergyLimit()
			
			if self.nextRoundExe:
				for i in self.nextRoundExe:
					args = i[1:]
					kwargs = {}
					eval('self.'+i[0])(*args, **kwargs)
			self.nextRoundExe.clear()
			
			self.checkFollowDead(0)
			self.checkFollowDead(1)
			
			if self.everyRoundStartExe:
				for i in self.everyRoundStartExe:
					args = i[1:]
					kwargs = {}
					eval('self.'+i[0])(*args, **kwargs)
			
			self.checkFollowDead(0)
			self.checkFollowDead(1)
			
			DEBUG_MSG(self.field)
			self.data[self.CurrentPlayer]['energy'] = self.getEnergy(self.CurrentPlayer)
			self.data[self.another(self.CurrentPlayer)]['energy'] = self.getEnergy(self.another(self.CurrentPlayer))
			
			self.player[self.CurrentPlayer].onNextRound_Your(self.Round)
			self.player[self.another(self.CurrentPlayer)].onNextRound_Opps(self.Round)
			
			# 抽卡数限制
			self.drawnum = [self.initRole[self.player0.role][2],self.initRole[self.player1.role][2]]
	
	# self.avatarList[self.CurrentPlayer].setSituation(1)
	# self.avatarList[self.another(self.CurrentPlayer)].setSituation(0)
	
	def playerUseCard(self, playerID, position, target):
		DEBUG_MSG("battlefiled playerUseCard playerID, cardID, targe", playerID, position, target)
		if playerID == self.CurrentPlayer and self.checkDestroyed():
			cardID = self.player[playerID].BattleHandCardList[position]
			self.position = 9
			
			# self.data[playerID]['energy'] = 100
			
			if (self.data[playerID]['energy'] >= card_cost[cardID][0]):
				del self.player[playerID].BattleHandCardList[position]
				eval('self.card'+str(cardID))(True, target)
				self.checkFollowDead(playerID)
				self.checkFollowDead(self.another(playerID))
				
				self.player[playerID].onUserCard(cardID = cardID)
				self.player[self.another(playerID)].onUserCard(cardID = cardID)
				self.player[playerID].onRemoveHandCard()
			
			
	def checkFollowDead(self,playerID):
		i = 0
		while True:
			if i >= len(self.field[playerID]):
				break
			if self.field[playerID][i]['hp'] <= 0:
				del self.field[playerID][i]
				i = 0
			else:
				i += 1
		self.player[playerID].onUserCard()
		self.player[self.another(playerID)].onUserCard()
	
	def playerRetinueAction(self, playerID, position, target):
		DEBUG_MSG("battlefiled playerUseCard playerID, cardID, targe", playerID, position, target)
		if playerID == self.CurrentPlayer and self.checkDestroyed():
			cardID = self.field[self.CurrentPlayer][position]['id']
			self.position = position
			
			# self.data[playerID]['energy'] = 100
			
			if (self.data[playerID]['energy'] >= card_cost[cardID][0]):
				eval('self.card' + str(cardID))(False, target)
				self.checkFollowDead(playerID)
				self.checkFollowDead(self.another(playerID))
				self.player[playerID].onRetinueAction()
				self.player[self.another(playerID)].onRetinueAction()
	
	def reqGiveUp(self, playerID):
		
		DEBUG_MSG("GiveUp playerID:[%i] BattleField:[%i]" % (playerID, self.id))
		if playerID == 1:
			successPlayerID = 0
		else:
			successPlayerID = 1
		self.EndBattle(successPlayerID)
		
	def onlineCheck(self):
		# DEBUG_MSG("battlefiled onlineCheck ")
		for i in range(2):
			if self.player[i] != None and not self.player[i].isDestroyed:
				self.outlineTimes[i] = 0
			else:
				self.outlineTimes[i] += 1
				if self.outlineTimes[i] > 5:
					self.MsgToClient_March('您的对手已经被你吓的掉线了，您已经获胜', self.another(i))
					self.EndBattle(self.another(i))
		if self.player[0].isDestroyed and self.player[1].isDestroyed:
			self.addTimer(0,0,2)
		if self.player[0].isDestroyed and not self.player[1].isDestroyed:
			self.MsgToClient_March('您的对手已经被你吓的掉线了，您已经获胜', 1)
			self.EndBattle(1)
		if self.player[1].isDestroyed and not self.player[0].isDestroyed:
			self.MsgToClient_March('您的对手已经被你吓的掉线了，您已经获胜', 0)
			self.EndBattle(0)
	
	def giveCard(self, playerID, cardSum):
		
		DEBUG_MSG("giveCard  playerID:[%s]  cardSum:[%s]" % (playerID, cardSum))
		self.player[playerID].creatHandCard(cardSum)
	
	def another(self, playID):
		if playID == 0:
			return 1
		return 0
	
	def costEnergy(self, cost, playerID):
		self.data[playerID]['energy'] -= cost
		self.data[playerID]['energy'] = max(0,self.data[playerID]['energy'])
		self.data[playerID]['energy'] = min(self.data[playerID]['energylimit'], self.data[playerID]['energy'])
		
	def playerDeadCheck(self, playerID):
		# 	检测死亡
		if self.data[playerID]['stress'] >= self.data[playerID]['limit']:
			self.EndBattle(self.another(playerID))
		
	def tired(self, playerID):
		
		self.tiredcount[playerID] += 1
		self.data[playerID]['stress'] += self.tiredcount[playerID]
		self.playerDeadCheck(playerID)
	
	def addFollower(self, playerID, id, att, hp, group,cost):
		if len(self.field[playerID]) < 6:
			self.field[playerID].append({'id': id, 'att': att, 'hp': hp, 'cost': cost, 'group':group, 'maxhp':hp})
	
	def att(self, playerID, target, value):
		if target == 7:
			self.data[playerID]['stress'] += value
			self.playerDeadCheck(playerID)
			self.data[playerID]['stress'] = max(0,self.data[playerID]['stress'])
			if value < 0:
				if self.CurrentPlayer == playerID:
					self.actlist[self.CurrentPlayer].append(['cure', playerID, self.position + 1, target + 1, value])
					self.actlist[self.another(self.CurrentPlayer)].append(
						['cure', playerID + 1, -self.position - 1, -target - 1, value])
				else:
					self.actlist[self.CurrentPlayer].append(['cure', playerID, self.position + 1, -target - 1, value])
					self.actlist[self.another(self.CurrentPlayer)].append(
						['cure', playerID + 1, -self.position - 1, target + 1, value])
		else:
			if len(self.field[playerID]) > target:
				self.field[playerID][target]['hp'] -= value
				self.field[playerID][target]['hp'] = min(self.field[playerID][target]['hp'],self.field[playerID][target]['maxhp'])
		if value > 0 and (target == 7 or len(self.field[playerID]) > target):
			if self.CurrentPlayer == playerID:
				self.actlist[self.CurrentPlayer].append(['att', playerID, self.position+1, target+1, value])
				self.actlist[self.another(self.CurrentPlayer)].append(['att', playerID+1, -self.position-1, -target - 1, value])
			else:
				self.actlist[self.CurrentPlayer].append(['att',playerID,self.position+1,-target-1,value])
				self.actlist[self.another(self.CurrentPlayer)].append(['att',playerID+1, -self.position-1, target+1, value])
		# 	检测死亡

		
		
	def setLimit(self, playerID, value):
		self.data[playerID]['limit'] += value
		# if playerID == self.CurrentPlayer:
		# 	self.actlist[self.CurrentPlayer].append(['limit',playerID,self.position,8,value])
		# 	self.actlist[self.another(self.CurrentPlayer)].append(['limit', playerID, self.position, -8, value])
		# else:
		# 	self.actlist[self.CurrentPlayer].append(['limit', playerID, self.position, -8, value])
		# 	self.actlist[self.another(self.CurrentPlayer)].append(['limit', playerID, self.position, 8, value])
		
	def setEnergyLimit(self, playerID, value):
		self.data[playerID]['energylimit'] += value
		self.data[playerID]['energy'] = min(self.data[playerID]['energylimit'], self.data[playerID]['energy'])
		# if playerID == self.CurrentPlayer:
		# 	self.actlist[self.CurrentPlayer].append(['energylimit',playerID,self.position,8,value])
		# 	self.actlist[self.another(self.CurrentPlayer)].append(['limit', playerID, self.position, -8, value])
		# else:
		# 	self.actlist[self.CurrentPlayer].append(['energylimit', playerID, self.position, -8, value])
		# 	self.actlist[self.another(self.CurrentPlayer)].append(['limit', playerID, self.position, 8, value])
		
	def powerFollower(self,playerID,target,att,hp):
		self.field[playerID][target]['att'] += att
		self.field[playerID][target]['hp'] += hp
		
		# if self.CurrentPlayer == playerID:
		# 	self.actlist[self.CurrentPlayer].append(['power', playerID, self.position + 1, target + 1, att, hp])
		# 	self.actlist[self.another(self.CurrentPlayer)].append(
		# 		['att', playerID + 1, -self.position - 1, -target -1, att, hp])
		# else:
		# 	self.actlist[self.CurrentPlayer].append(['power', playerID, self.position + 1, -target - 1, att, hp])
		# 	self.actlist[self.another(self.CurrentPlayer)].append(
		# 		['att', playerID + 1, -self.position - 1, target + 1, att, hp])
	
		# self.actlist[0].append(['power',playerID,self.position,target,att,hp])
		# self.actlist[1].append(['power', playerID, self.position, target, att, hp])
		
	def project(self):
		for i in range(len(self.field[0])):
			self.att(0, i, 2)
		for i in range(len(self.field[1])):
			self.att(1, i, 2)
		self.att(0, 7, 3)
		self.att(1, 7, 3)
		
	def addCard(self,playerID):
		if playerID == self.CurrentPlayer:
			self.giveCard(playerID,1)
			
	def exam(self,round, value):
		if round + 5 > self.Round:
			self.att(0, 7, value)
			self.att(1, 7, value)
		else:
			self.everyRoundStartExe.remove(['exam',round])
			
	def addEnergy(self,playerID, value):
		self.data[playerID]['energy'] += value
		self.data[playerID]['energy'] = min(self.data[playerID]['energy'], self.data[playerID]['energylimit'])
		
		if playerID == self.CurrentPlayer:
			self.actlist[self.CurrentPlayer].append(['addenergy',playerID,self.position,8,value])
			self.actlist[self.another(self.CurrentPlayer)].append(['addenergy', playerID, self.position, -8, value])
		else:
			self.actlist[self.CurrentPlayer].append(['addenergy', playerID, self.position, -8, value])
			self.actlist[self.another(self.CurrentPlayer)].append(['addenergy', playerID, self.position, 8, value])
		
		
	def randomPower(self, playerID, att):
		k = len(self.field[playerID])
		if k > 0:
			self.powerFollower(playerID,random.randint(0,k-1),att,0)
			
	def powerAll(self, playerID, att, hp):
		for i in range(len(self.field[playerID])):
			self.powerFollower(playerID,i,att,hp)
			
	def allatt(self):
		for i in range(len(self.field[0])):
			self.att(0, i, 8)
		for i in range(len(self.field[1])):
			self.att(1, i, 8)
	# 	------------------------------------------------------card skill------------------------------------
	
	def card0(self, first, target):
		DEBUG_MSG("Use Card0",first,target)
		if self.data[self.CurrentPlayer]['energy'] >= 2:
			self.costEnergy(2, self.CurrentPlayer)
			if first:
				self.addFollower(self.CurrentPlayer, 0, 3, 4, 0, 2)
				return
			attvalue = card_cost[0][2]
			if self.position < len(self.field[self.CurrentPlayer]):
				attvalue = self.field[self.CurrentPlayer][self.position]['att']
			self.att(self.another(self.CurrentPlayer), target, attvalue)
	
	def card1(self, first, target):
		DEBUG_MSG("Use Card1",first,target)
		if self.data[self.CurrentPlayer]['energy'] >= 2:
			self.costEnergy(2, self.CurrentPlayer)
			if first:
				self.addFollower(self.CurrentPlayer, 1, 4, 4, 0, 2)
				self.nextRoundExe.append(["setLimit",self.another(self.CurrentPlayer),6])
				return
			attvalue = card_cost[1][2]
			if self.position < len(self.field[self.CurrentPlayer]):
				attvalue = self.field[self.CurrentPlayer][self.position]['att']
			self.att(self.another(self.CurrentPlayer), target, attvalue)
		
	def card2(self, first, target):
		DEBUG_MSG("Use Card2",first,target)
		if self.data[self.CurrentPlayer]['energy'] >= 5:
			self.costEnergy(5, self.CurrentPlayer)
			if first:
				self.addFollower(self.CurrentPlayer, 2, 3, 3, 0, 5)
				for i in range(len(self.field[0])):
					if self.field[0][i]['group'] != 0:
						self.att(0,i,3)
				for i in range(len(self.field[1])):
					if self.field[1][i]['group'] != 0:
						self.att(1,i,3)
				self.att(0,7,3)
				self.att(1, 7, 3)
				return
			attvalue = card_cost[2][2]
			if self.position < len(self.field[self.CurrentPlayer]):
				attvalue = self.field[self.CurrentPlayer][self.position]['att']
			self.att(self.another(self.CurrentPlayer), target, attvalue)
		
		
	def card3(self, first, target):
		DEBUG_MSG("Use Card3",first,target)
		self.costEnergy(4, self.CurrentPlayer)
		if first:
			self.addFollower(self.CurrentPlayer, 3, 3, 4, 1, 4)
			self.everyRoundEndExe.append(['att',0,7,-3])
			self.everyRoundEndExe.append(['att', 1, 7, -3])
			return
		attvalue = card_cost[3][2]
		if self.position < len(self.field[self.CurrentPlayer]):
			attvalue = self.field[self.CurrentPlayer][self.position]['att']
		self.att(self.another(self.CurrentPlayer), target, attvalue)
		
	def card4(self, first, target):
		DEBUG_MSG("Use Card4",first,target)
		if self.data[self.CurrentPlayer]['energy'] >= 5:
			self.costEnergy(5, self.CurrentPlayer)
			if first:
				if len(self.field[self.CurrentPlayer]) > 0:
					target = random.randint(0,len(self.field[self.CurrentPlayer])-1)
					self.powerFollower(self.CurrentPlayer,target,1,1)
				self.addFollower(self.CurrentPlayer, 4, 6, 5, 2, 5)
				return
			attvalue = card_cost[4][2]
			if self.position < len(self.field[self.CurrentPlayer]):
				attvalue = self.field[self.CurrentPlayer][self.position]['att']
			self.att(self.another(self.CurrentPlayer), target, attvalue)
		
	def card5(self, first, target):
		DEBUG_MSG("Use Card5",first,target)
		if self.data[self.CurrentPlayer]['energy'] >= 6:
			self.costEnergy(6, self.CurrentPlayer)
			if first:
				
				for i in range(len(self.field[self.CurrentPlayer])):
					self.powerFollower(self.CurrentPlayer,i,1,1)
				self.addFollower(self.CurrentPlayer, 5, 6, 6, 2, 5)
				return
			attvalue = card_cost[5][2]
			if self.position < len(self.field[self.CurrentPlayer]):
				attvalue = self.field[self.CurrentPlayer][self.position]['att']
			self.att(self.another(self.CurrentPlayer), target, attvalue)
	
	def card6(self, first, target):
		DEBUG_MSG("Use Card6",first,target)
		if self.data[self.CurrentPlayer]['energy'] >= 3:
			self.costEnergy(3, self.CurrentPlayer)
			if first:
				self.addFollower(self.CurrentPlayer, 6, 3, 2, 3, 3)
				if len(self.field[self.CurrentPlayer]) < 6:
					self.card53(first, target,call=1)
				if len(self.field[self.CurrentPlayer]) < 6:
					self.card53(first, target,call=1)
				return
			attvalue = card_cost[6][2]
			if self.position < len(self.field[self.CurrentPlayer]):
				attvalue = self.field[self.CurrentPlayer][self.position]['att']
			self.att(self.another(self.CurrentPlayer), target, attvalue)
		
		
	def card7(self, first, target):
		DEBUG_MSG("Use Card7",first,target)
		if self.data[self.CurrentPlayer]['energy'] >= 4:
			self.costEnergy(4, self.CurrentPlayer)
			if first:
				self.addFollower(self.CurrentPlayer, 7, 4, 5, 4, 4)
				return
			attvalue = card_cost[7][2]
			if self.position < len(self.field[self.CurrentPlayer]):
				attvalue = self.field[self.CurrentPlayer][self.position]['att']
			self.att(self.another(self.CurrentPlayer), target, attvalue)
		
		
	def card8(self, first, target):
		DEBUG_MSG("Use Card8",first,target)
		if self.data[self.CurrentPlayer]['energy'] >=4:
			self.costEnergy(4, self.CurrentPlayer)
			if first:
				self.addFollower(self.CurrentPlayer, 8, 6, 5, 4, 4)
				self.att(0, 7, -4)
				self.att(1, 7, -4)
				return
			attvalue = card_cost[8][2]
			if self.position < len(self.field[self.CurrentPlayer]):
				attvalue = self.field[self.CurrentPlayer][self.position]['att']
			self.att(self.another(self.CurrentPlayer), target, attvalue)
		
	def card9(self, first, target):
		DEBUG_MSG("Use Card9",first,target)
		if self.data[self.CurrentPlayer]['energy'] >= 4:
			self.costEnergy(4, self.CurrentPlayer)
			if first:
				self.addFollower(self.CurrentPlayer, 9, 4, 4, 4, 4)
				self.setEnergyLimit(self.CurrentPlayer,-3)
				self.setEnergyLimit(self.another(self.CurrentPlayer), -3)
				return
			attvalue = card_cost[9][2]
			if self.position < len(self.field[self.CurrentPlayer]):
				attvalue = self.field[self.CurrentPlayer][self.position]['att']
			self.att(self.another(self.CurrentPlayer), target, attvalue)
		
	def card10(self, first, target):
		DEBUG_MSG("Use Card10",first,target)
		if self.data[self.CurrentPlayer]['energy'] >= 1:
			self.costEnergy(1, self.CurrentPlayer)
			if first:
				self.att(0, 7, 4)
				self.att(1, 7, 4)
				return
		
	def card11(self, first, target):
		DEBUG_MSG("Use Card11",first,target)
		if self.data[self.CurrentPlayer]['energy'] >= 3:
			self.costEnergy(3, self.CurrentPlayer)
			if first:
				self.setEnergyLimit(self.CurrentPlayer,-1)
				self.player[self.CurrentPlayer].creatHandCard(1)
				for i in range(len(self.field[0])):
					self.att(0,i,3)
				for i in range(len(self.field[1])):
					self.att(1,i,3)
				return
		
	def card12(self, first, target):
		DEBUG_MSG("Use Card12",first,target)
		if self.data[self.CurrentPlayer]['energy'] >= 4:
			self.costEnergy(4, self.CurrentPlayer)
			if first:
				for i in range(len(self.field[0])):
					if self.field[0][i]['group'] == 1:
						self.att(0,i,1)
				for i in range(len(self.field[1])):
					if self.field[1][i]['group'] == 1:
						self.att(1,i,1)
				self.drawnum[self.another(self.CurrentPlayer)] -= 1
				return
		
	def card13(self, first, target):
		DEBUG_MSG("Use Card13",first,target)
		if self.data[self.CurrentPlayer]['energy'] >= 8:
			self.costEnergy(8, self.CurrentPlayer)
			if first:
				self.addFollower(self.CurrentPlayer, 13, 6, 10, 0, 8)
				return
			attvalue = card_cost[13][2]
			if self.position < len(self.field[self.CurrentPlayer]):
				attvalue = self.field[self.CurrentPlayer][self.position]['att']
			self.att(self.another(self.CurrentPlayer), target, attvalue)
		
	def card14(self, first, target):
		DEBUG_MSG("Use Card14",first,target)
		if self.data[self.CurrentPlayer]['energy'] >= 2:
			self.costEnergy(2, self.CurrentPlayer)
			if first:
				for i in range(len(self.field[self.CurrentPlayer])):
					self.att(self.CurrentPlayer,i,-4)
				return

	def card15(self, first, target):
		DEBUG_MSG("Use Card15",first,target)
		if self.data[self.CurrentPlayer]['energy'] >= 2:
			self.costEnergy(2, self.CurrentPlayer)
			if first:
				self.att(self.CurrentPlayer, 7, -5)
				if self.field[self.CurrentPlayer]:
					target = random.randint(0, len(self.field[self.CurrentPlayer])-1)
					self.field[self.CurrentPlayer][target]["hp"] = self.field[self.CurrentPlayer][target]["maxhp"]
				return
		
	def card16(self, first, target):
		DEBUG_MSG("Use Card16",first,target)
		if self.data[self.CurrentPlayer]['energy'] >= 3:
			self.costEnergy(3, self.CurrentPlayer)
			if first:
				self.data[self.CurrentPlayer]['limit'] += 5
				self.player[self.CurrentPlayer].creatHandCard(1)
				return

	def card17(self, first, target):
		DEBUG_MSG("Use Card17",first,target)
		if self.data[self.CurrentPlayer]['energy'] >= 10:
			self.costEnergy(10, self.CurrentPlayer)
			if first:
				for i in range(len(self.field[self.CurrentPlayer])):
					self.powerFollower(self.CurrentPlayer,i,3,3)
				return
		
	def card18(self, first, target):
		DEBUG_MSG("Use Card18",first,target)
		if self.data[self.CurrentPlayer]['energy'] >= 3:
			self.costEnergy(3, self.CurrentPlayer)
			self.costEnergy(3, self.another(self.CurrentPlayer))
			if first:
				self.att(0, 7, 10)
				self.att(1, 7, 10)
				return
		
	def card19(self, first, target):
		DEBUG_MSG("Use Card19",first,target)
		if self.data[self.CurrentPlayer]['energy'] >= 0:
			self.costEnergy(0, self.CurrentPlayer)
			if first:
				self.att(self.CurrentPlayer,7,2)
				if self.field[self.CurrentPlayer]:
					target = random.randint(0,len(self.field[self.CurrentPlayer])-1)
					self.field[self.CurrentPlayer][target]["hp"] = self.field[self.CurrentPlayer][target]["maxhp"]
				return
		
	def card20(self, first, target):
		DEBUG_MSG("Use Card20")
		if self.data[self.CurrentPlayer]['energy'] >= 5:
			self.costEnergy(5, self.CurrentPlayer)
			if first:
				self.att(self.CurrentPlayer,7,-10)
				return
		
	def card21(self, first, target):
		DEBUG_MSG("Use Card21",first,target)
		if self.data[self.CurrentPlayer]['energy'] >= 2:
			self.costEnergy(2, self.CurrentPlayer)
			if first:
				self.powerFollower(self.CurrentPlayer,target,3,3)
				return
		
	def card22(self, first, target):
		DEBUG_MSG("Use Card22")
		if self.data[self.CurrentPlayer]['energy'] >= 2:
			self.costEnergy(2, self.CurrentPlayer)
			if first:
				self.data[self.CurrentPlayer]['energylimit'] += 1
				return
		
	def card23(self, first, target):
		DEBUG_MSG("Use Card23")
		if self.data[self.CurrentPlayer]['energy'] >= 0:
			if first:
				self.addEnergy(self.CurrentPlayer,2)
				return

	def card24(self, first, target):
		DEBUG_MSG("Use Card24")
		if self.data[self.CurrentPlayer]['energy'] >= 2:
			self.costEnergy(2, self.CurrentPlayer)
			if first:
				for i in range(len(self.field[self.CurrentPlayer])):
					self.powerFollower(self.CurrentPlayer,i,1,1)
				return
	
	def card25(self, first, target):
		DEBUG_MSG("Use Card25")
		if self.data[self.CurrentPlayer]['energy'] >= 3:
			self.costEnergy(3, self.CurrentPlayer)
			if first:
				self.card53(first,target,call=1)
				self.card53(first, target,call=1)
				self.card53(first, target,call=1)
				return
		
	def card26(self, first, target):
		DEBUG_MSG("Use Card26")
		if self.data[self.CurrentPlayer]['energy'] >= 1:
			self.costEnergy(1, self.CurrentPlayer)
			if first:
				self.att(self.CurrentPlayer,7,-5)
				self.setLimit(self.CurrentPlayer,-1)
				return
		
	def card27(self, first, target):
		DEBUG_MSG("Use Card27")
		if self.data[self.CurrentPlayer]['energy'] >= 0:
			self.costEnergy(0, self.CurrentPlayer)
			if first:
				for i in range(len(self.field[0])):
					self.powerFollower(0,i,1,1)
				for i in range(len(self.field[1])):
					self.powerFollower(1,i,1,1)
				return
	
	
		
	def card28(self, first, target):
		DEBUG_MSG("Use Card28")
		self.costEnergy(-5, self.CurrentPlayer)
		if first:
			if self.player[self.CurrentPlayer].BattleHandCardList:
				l = random.randint(0,len(self.player[self.CurrentPlayer].BattleHandCardList)-1)
				del self.player[self.CurrentPlayer].BattleHandCardList[l]
			
			return
	
	def card29(self, first, target):
		DEBUG_MSG("Use Card29")
		if self.data[self.CurrentPlayer]['energy'] >= 3:
			self.costEnergy(3, self.CurrentPlayer)
			if first:
				for i in range(2):
					self.giveCard(self.CurrentPlayer,2)
				
				return
	
	def card30(self, first, target):
		DEBUG_MSG("Use Card30")
		if self.data[self.CurrentPlayer]['energy'] >= 3:
			self.costEnergy(3, self.CurrentPlayer)
			if first:
				self.data[self.CurrentPlayer]['limit'] += 1
				for i in range(len(self.field[self.CurrentPlayer])):
					self.powerFollower(self.CurrentPlayer,i,-1,-1)
				
				return
	
	def card31(self, first, target):
		DEBUG_MSG("Use Card31")
		if self.data[self.CurrentPlayer]['energy'] >= 10:
			self.costEnergy(10, self.CurrentPlayer)
			if first:
				self.everyRoundEndExe.append(['project'])
				
				return
	
	def card32(self, first, target):
		DEBUG_MSG("Use Card32")
		if self.data[self.CurrentPlayer]['energy'] >= 10:
			self.costEnergy(10, self.CurrentPlayer)
			if first:
				self.everyRoundEndExe.append(['att',self.CurrentPlayer,7,-10])
				
				return
	
	def card33(self, first, target):
		DEBUG_MSG("Use Card33")
		if self.data[self.CurrentPlayer]['energy'] >= 10:
			self.costEnergy(10, self.CurrentPlayer)
			if first:
				self.everyRoundStartExe.append(['addCard',self.CurrentPlayer])
				
				return
	
	def card34(self, first, target):
		DEBUG_MSG("Use Card34", first, target)
		if self.data[self.CurrentPlayer]['energy'] >= 10:
			self.costEnergy(10, self.CurrentPlayer)
			if first:
				for i in range(6):
					if len(self.field[self.CurrentPlayer]) < 6:
						self.card53(first,target,call=1)
				return
		
	def card35(self, first, target):
		DEBUG_MSG("Use Card35")
		if self.data[self.CurrentPlayer]['energy'] >= 2:
			self.costEnergy(8, self.CurrentPlayer)
			if first:
				self.everyRoundStartExe.append(['exam', self.Round, 5])
				return
		
	def card36(self, first, target):
		DEBUG_MSG("Use Card36")
		if self.data[self.CurrentPlayer]['energy'] >= 5:
			self.costEnergy(5, self.CurrentPlayer)
			if first:
				self.nextRoundExe.append(['setEnergyLimit', self.another(self.CurrentPlayer),-5])
				self.setEnergyLimit(self.CurrentPlayer,-5)
				return
		
	def card37(self, first, target):
		DEBUG_MSG("Use Card37")
		if self.data[self.CurrentPlayer]['energy'] >= 5:
			self.costEnergy(5, self.CurrentPlayer)
			if first:
				self.everyRoundStartExe.append(['exam', self.Round, -5])
				return
		
	def card38(self, first, target):
		# "造成三点压力，随机分配到所有敌人身上",
		DEBUG_MSG("Use Card38 ")
		if self.data[self.CurrentPlayer]['energy'] >= 1:
			self.costEnergy(1, self.CurrentPlayer)
			if first:
				for i in range(3):
					if len(self.field[self.another(self.CurrentPlayer)]) > 0:
						t = random.randint(0,len(self.field[self.another(self.CurrentPlayer)])-1)
						self.att(self.another(self.CurrentPlayer),t,1)
					else:
						self.att(self.another(self.CurrentPlayer), 7, 1)
				return
		
	def card39(self, first, target):
		# "抽两张牌",
		DEBUG_MSG("Use Card39 ")
		if self.data[self.CurrentPlayer]['energy'] >= 3:
			self.costEnergy(3, self.CurrentPlayer)
			if first:
				for i in range(2):
					self.giveCard(self.CurrentPlayer,1)
				return
		
	def card40(self, first, target):
		# "造成十点压力",
		DEBUG_MSG("Use Card40 ")
		if self.data[self.CurrentPlayer]['energy'] >= 10:
			self.costEnergy(10, self.CurrentPlayer)
			if first:
				self.att(self.another(self.CurrentPlayer),7,10)
				return
		
	def card41(self, first, target):
		# "造成六点压力"",
		DEBUG_MSG("Use Card41 ")
		if self.data[self.CurrentPlayer]['energy'] >= 2:
			self.costEnergy(4, self.CurrentPlayer)
			if first:
				self.att(self.another(self.CurrentPlayer),7,6)
				return
		
	def card42(self, first, target):
		# "8/8/6, 下一回合开始时所有随从压力加八"
		DEBUG_MSG("Use Card2")
		if self.data[self.CurrentPlayer]['energy'] >= 8:
			self.costEnergy(8, self.CurrentPlayer)
			if first:
				self.addFollower(self.CurrentPlayer, 42, 8, 6, 0, 6)
				
				self.nextRoundExe.append(['allatt'])
				return
			attvalue = card_cost[42][2]
			if self.position < len(self.field[self.CurrentPlayer]):
				attvalue = self.field[self.CurrentPlayer][self.position]['att']
			self.att(self.another(self.CurrentPlayer), target, attvalue)
		
	def card43(self, first, target):
		# "2/1/3, 每个回合结束时，选择一个随从攻击力+1"
		DEBUG_MSG("Use Card43")
		if self.data[self.CurrentPlayer]['energy'] >= 2:
			self.costEnergy(2, self.CurrentPlayer)
			if first:
				self.addFollower(self.CurrentPlayer, 43, 1, 3, 0, 3)
				self.everyRoundEndExe.append(['randomPower',self.CurrentPlayer,1])
				return
			attvalue = card_cost[43][2]
			if self.position < len(self.field[self.CurrentPlayer]):
				attvalue = self.field[self.CurrentPlayer][self.position]['att']
			self.att(self.another(self.CurrentPlayer), target, attvalue)
		
	def card44(self, first, target):
		# " "对对方所有随从造成(1)点伤害"
		DEBUG_MSG("Use Card44 ")
		if self.data[self.CurrentPlayer]['energy'] >= 2:
			self.costEnergy(2, self.CurrentPlayer)
			if first:
				for i in range(len(self.field[self.another(self.CurrentPlayer)])):
					self.att(self.another(self.CurrentPlayer),i,1)
				return
		
	def card45(self, first, target):
		# "增加自己一点压力，抽两张牌"
		DEBUG_MSG("Use Card45 ")
		if self.data[self.CurrentPlayer]['energy'] >= 2:
			self.costEnergy(2, self.CurrentPlayer)
			if first:
				self.att(self.CurrentPlayer,7,1)
				for i in range(2):
					self.giveCard(self.CurrentPlayer,1)
				return
		
	def card46(self, first, target):
		# "增加五点护盾，抽一张卡"
		DEBUG_MSG("Use Card46 ")
		if self.data[self.CurrentPlayer]['energy'] >= 3:
			self.costEnergy(3, self.CurrentPlayer)
			if first:
				self.setLimit(self.CurrentPlayer,5)
				self.giveCard(self.CurrentPlayer,1)
				return
		
	def card47(self, first, target):
		# "对方随即两个随从收到(2)点伤害"
		DEBUG_MSG("Use Card47 ")
		if self.data[self.CurrentPlayer]['energy'] >= 2:
			self.costEnergy(2, self.CurrentPlayer)
			if first:
				if len(self.field[self.another(self.CurrentPlayer)]) > 0:
					for i in range(2):
						if len(self.field[self.another(self.CurrentPlayer)]) > 0:
							t = random.randint(0,len(self.field[self.another(self.CurrentPlayer)])-1)
							self.att(self.another(self.CurrentPlayer),t,2)
				return
		
	def card48(self, first, target):
		# "4/3/3，战吼：给予对方三点压力"
		DEBUG_MSG("Use Card48")
		if self.data[self.CurrentPlayer]['energy'] >= 4:
			self.costEnergy(4, self.CurrentPlayer)
			if first:
				self.addFollower(self.CurrentPlayer, 48, 3, 3, 1, 3)
				self.att(self.another(self.CurrentPlayer),7,3)
				return
			attvalue = card_cost[48][2]
			if self.position < len(self.field[self.CurrentPlayer]):
				attvalue = self.field[self.CurrentPlayer][self.position]['att']
			self.att(self.another(self.CurrentPlayer), target, attvalue)
		
	def card49(self, first, target):
		DEBUG_MSG("Use Card49")
		if self.data[self.CurrentPlayer]['energy'] >= 1:
			self.costEnergy(1, self.CurrentPlayer)
			if first:
				self.addFollower(self.CurrentPlayer, 49, 1, 2, 1, 2)
				return
			attvalue = card_cost[49][2]
			if self.position < len(self.field[self.CurrentPlayer]):
				attvalue = self.field[self.CurrentPlayer][self.position]['att']
			self.att(self.another(self.CurrentPlayer), target, attvalue)
		
	def card50(self, first, target):
		DEBUG_MSG("Use Card50")
		if self.data[self.CurrentPlayer]['energy'] >= 5:
			self.costEnergy(5, self.CurrentPlayer)
			if first:
				self.addFollower(self.CurrentPlayer, 50, 4, 4, 1, 4)
				return
			attvalue = card_cost[50][2]
			if self.position < len(self.field[self.CurrentPlayer]):
				attvalue = self.field[self.CurrentPlayer][self.position]['att']
			self.att(self.another(self.CurrentPlayer), target, attvalue)
			
		
	def card51(self, first, target):
		# "7/4/6，风怒：每个回合结束时我方所有随从攻击力加(1)"
		DEBUG_MSG("Use Card51")
		if self.data[self.CurrentPlayer]['energy'] >= 7:
			self.costEnergy(7, self.CurrentPlayer)
			if first:
				self.addFollower(self.CurrentPlayer, 51, 4, 6, 2, 6)
				self.everyRoundEndExe.append(['powerAll',self.CurrentPlayer,1,0])
				return
			attvalue = card_cost[51][2]
			if self.position < len(self.field[self.CurrentPlayer]):
				attvalue = self.field[self.CurrentPlayer][self.position]['att']
			self.att(self.another(self.CurrentPlayer), target, attvalue)
		
	def card52(self,first, target):
		DEBUG_MSG("Use Card52")
		if self.data[self.CurrentPlayer]['energy'] >= 1:
			self.costEnergy(1, self.CurrentPlayer)
			if first:
				self.addFollower(self.CurrentPlayer, 52, 3, 3, 4, 2)
				return
			attvalue = card_cost[52][2]
			if self.position < len(self.field[self.CurrentPlayer]):
				attvalue = self.field[self.CurrentPlayer][self.position]['att']
			self.att(self.another(self.CurrentPlayer), target, attvalue)
		
	def card53(self,first, target, call = 0):
		DEBUG_MSG("Use Card53")
		if self.data[self.CurrentPlayer]['energy'] >= 1 or call == 1:
			self.costEnergy(1, self.CurrentPlayer)
			if first:
				self.addFollower(self.CurrentPlayer, 53, 1, 1, 4, 2)
				return
			attvalue = card_cost[53][2]
			if self.position < len(self.field[self.CurrentPlayer]):
				attvalue = self.field[self.CurrentPlayer][self.position]['att']
			self.att(self.another(self.CurrentPlayer), target, attvalue)