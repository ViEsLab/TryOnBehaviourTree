﻿using BehaviourTree.Precondition;
using UnityEngine;

namespace BehaviourTree.Nodes {
    public class BTSequence : BTNode {
        private BTNode curActiveChild;

        public BTSequence(string nodeName, BTPrecondition precondition = null) :
            base(nodeName, precondition) {}

        protected override bool DoEvaluate() {
            if (curActiveChild != null) {
                bool result = curActiveChild.Evaluate();
                if (!result) {
                    // 任意一个阶段的检查不通过，整个队列中止
                    curActiveChild.Clean();
                    curActiveChild = null;
                }

                return result;
            } else if (children.Count <= 0) {
                return false;
            } else {
                return children[0].Evaluate();
            }
        }

        public override BTResult Tick() {
            curActiveChild ??= children[0];

            BTResult result = curActiveChild.Tick();
            if (result == BTResult.Ended) {
                int nextIndex = children.IndexOf(curActiveChild) + 1;
                curActiveChild.Clean();
                if (nextIndex < children.Count) {
                    curActiveChild = children[nextIndex];
                    result = BTResult.Running;
                } else {
                    result = BTResult.Ended;
                }
            }

            return result;
        }

        public override void Clean() {
            if (curActiveChild != null) {
                curActiveChild.Clean();
                curActiveChild = null;
            }

            for (int i = 0, count = children.Count; i < count; i++) {
                children[i].Clean();
            }
        }
    }
}