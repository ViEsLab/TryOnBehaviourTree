﻿using BehaviourTree.Precondition;
using UnityEngine;

namespace BehaviourTree.Nodes {
    public class BTAction : BTNode {
        private enum BTActionStatus {
            Ready = 1,
            Running = 2,
        }

        private BTActionStatus curStatus = BTActionStatus.Ready;
        // 如果BT由事件驱动，Action是否需要额外的状态？怎么确定Action的优先级？

		public BTAction (string nodeName, BTPrecondition precondition = null) :
            base (nodeName, precondition) {}

        protected virtual void Enter() {
            Debug.Log($"[ViE] 动作{nodeName}开始");
        }

        protected virtual void Exit() {
            Debug.Log($"[ViE] 动作{nodeName}退出");
        }

        protected virtual BTResult Execute() {
            return BTResult.Running;
        }

        public override BTResult Tick () {
            BTResult executeResult = BTResult.Ended;
            if (curStatus == BTActionStatus.Ready) {
                Enter();
                curStatus = BTActionStatus.Running;
            }

            // 即时反应
            if (curStatus == BTActionStatus.Running) {
                executeResult = Execute();
                if (executeResult != BTResult.Running) {
                    Exit();
                    curStatus = BTActionStatus.Ready;
                }
            }
            return executeResult;
        }

        public override void AddChild(BTNode node) {
            Debug.LogWarning("Action不应该被允许添加或移除节点，后续考虑如何排除");
        }

        public override void RemoveChild(BTNode node) {
            Debug.LogWarning("Action不应该被允许添加或移除节点，后续考虑如何排除");
        }

        public override void Clean () {
            // 主动调用
            if (curStatus != BTActionStatus.Ready) {
                Exit();
                curStatus = BTActionStatus.Ready;
            }
        }
    }
}