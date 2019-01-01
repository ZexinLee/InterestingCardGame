import KBEngine
import random
import time
from KBEDebug import *


class Hall(KBEngine.Base):
    """
    大厅管理器实体
    该实体管理该服务组上所有的大厅
    """

    def __init__(self):
        DEBUG_MSG("Hall init")
        KBEngine.Base.__init__(self)

        # 储存大厅
        KBEngine.globalData["Halls"] = self

        # 存放所有正在匹配玩家mailbox
        self.OnMarchingPlayer = []

        # 检测是否正在匹配
        self.OnMarch = 0

        # 匹配程序
        self.addTimer(5, 2, 1)


    def onTimer(self, id, userArg):

        # DEBUG_MSG(id, userArg)

        if userArg == 1:
            self.March()

    def March(self):
        # 正在匹配则返回
        if self.OnMarch == 1:
            return

        self.OnMarch = 1
        # DEBUG_MSG("Server Start A March")
        for i in range(len(self.OnMarchingPlayer)):
            IsSuccessed = 0
            for j in range(len(self.OnMarchingPlayer)):
                if i == j:
                    continue
                # if self.OnMarchingPlayer[j].Data["rank"] > self.OnMarchingPlayer[i].Data["rank"] * (
                #         1 - 0.01 * self.OnMarchingPlayer[i].HaveMarchSum) - self.OnMarchingPlayer[i].HaveMarchSum and \
                #         self.OnMarchingPlayer[j].Data["rank"] < self.OnMarchingPlayer[i].Data["rank"] * (
                #         1 + 0.01 * self.OnMarchingPlayer[i].HaveMarchSum) + self.OnMarchingPlayer[i].HaveMarchSum:
                DEBUG_MSG("March Successed player1:%i player2:%i" % (
                self.OnMarchingPlayer[i].id, self.OnMarchingPlayer[j].id))
                self.CreatBattleField(self.OnMarchingPlayer[i], self.OnMarchingPlayer[j])
                IsSuccessed = 1
            # self.CreatBattleField(self.OnMarchingPlayer[i], self.OnMarchingPlayer[i])
            # IsSuccessed = 1
            if IsSuccessed == 0:
                self.OnMarchingPlayer[i].HaveMarchSum += 1
            else:
                break
        # 匹配成功后跳出匹配 防止再次匹配时再弄到J那个
        # 通过添加定时器调用此函数

        self.OnMarch = 0

    def CreatBattleField(self, player1, player2):
        # 成功匹配完成后再调用一次匹配 增加匹配的效率
        DEBUG_MSG("Start a battle player1:%i player2:%i" % (player1.id, player2.id))
        if player1.isDestroyed or player2.isDestroyed:
            if player1.isDestroyed:
                self.OnMarchingPlayer.remove(player1)
            if player2.isDestroyed:
                self.OnMarchingPlayer.remove(player2)
            DEBUG_MSG("March Fail because One is destroyed")
            return
        DEBUG_MSG("Battle March Successed player1:%i player2:%i" % (player1.id, player2.id))

        prarm = {
            "player0": player1,
            "player1": player2
        }

        BattleField = KBEngine.createBaseAnywhere("BattleField", prarm)
        if player1 in self.OnMarchingPlayer:
            self.OnMarchingPlayer.remove(player1)
        if player2 in self.OnMarchingPlayer:
            self.OnMarchingPlayer.remove(player2)
        self.March()

    def reqAddMarcher(self, player):
        # 此函数添加匹配玩家入列表
        DEBUG_MSG("Account[%i].reqAddMarcher:" % player.id)

        if player in self.OnMarchingPlayer:
            return

        player.HaveMarchSum = 0

        self.OnMarchingPlayer.append(player)

    def reqDelMarcher(self, player):

        # 此函数删除匹配玩家从列表
        DEBUG_MSG("Account[%i].reqDelMarcher:" % player.id)
        if player not in self.OnMarchingPlayer:
            return

        self.OnMarchingPlayer.remove(player)

