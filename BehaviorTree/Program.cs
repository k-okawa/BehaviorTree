using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BehaviorTree {
    class Program {
        static Random rand = new Random();
        
        static async Task Main(string[] args) {
            await Kadai();
        }

        static async Task Kadai() {
            Console.WriteLine("skill1の発動確率を入力してください　例)50%の場合→50");
            int skill1Prop = int.Parse(Console.ReadLine());
            int ownHp = 120;
            int enemyHp = 180;
            
            BTSequenceNode root = new BTSequenceNode(null);
            
            root.AddNode(new BTActionNode(root, () => {
                Console.WriteLine("出発");
                return ActionResult.Success;
            }));
            
            
            BTConditionNode ownHpCondition = new BTConditionNode(root, () => {
                return ownHp >= 100;
                
            });
            ownHpCondition.AppendNode(new BTActionNode(ownHpCondition, () => {
                Console.WriteLine("敵に寄った");
                return ActionResult.Success;
            }));
            root.AddNode(ownHpCondition);
            
            
            BTParallelNode parallelNode = new BTParallelNode(root);
            parallelNode.AddNode(new BTActionNode(parallelNode, () => {
                Console.WriteLine("友達Aを呼んだ");
                return ActionResult.Success;
            }));
            parallelNode.AddNode(new BTActionNode(parallelNode, () => {
                Console.WriteLine("友達Bを呼んだ");
                return ActionResult.Success;
            }));
            root.AddNode(parallelNode);
            
            
            BTRepeaterNode rep = new BTRepeaterNode(root,3);
            BTSelectorNode sel1 = new BTSelectorNode(rep);
            sel1.AddNode(new BTActionNode(sel1, () => {
                if (rand.Next(0, 101) < skill1Prop) {
                    int prevHp = enemyHp;
                    enemyHp -= 50;
                    Console.WriteLine(string.Format("skill1発動　敵の体力{0}→{1}",prevHp,enemyHp));
                    return ActionResult.Success;
                }
                return ActionResult.Failure;
            }));
            sel1.AddNode(new BTActionNode(sel1, () => {
                int prevHp = enemyHp;
                enemyHp -= 60;
                Console.WriteLine(string.Format("skill2発動　敵の体力{0}→{1}", prevHp, enemyHp));
                return ActionResult.Success;
            }));
            rep.AppendNode(sel1);
            root.AddNode(rep);
            
            BTSelectorNode sel2 = new BTSelectorNode(root);
            BTConditionNode enemyDeadCondition = new BTConditionNode(sel2, () => {
                return enemyHp <= 0;
            });
            enemyDeadCondition.AppendNode(new BTActionNode(enemyDeadCondition, () => {
                Console.WriteLine("End1");
                return ActionResult.Success;
            }));
            sel2.AddNode(enemyDeadCondition);
            
            BTConditionNode enemyAliveCondition = new BTConditionNode(sel2, () => {
                return enemyHp > 0;
            });
            enemyAliveCondition.AppendNode(new BTActionNode(enemyAliveCondition, () => {
                Console.WriteLine("End2");
                return ActionResult.Success;
            }));
            sel2.AddNode(enemyAliveCondition);
            
            root.AddNode(sel2);
            
            // 実行
            root.Execute();
            Console.WriteLine("-----------------------------------");

            int counter = 0;
            while (counter < 2) {
                root.Update();
                if (root.status != Status.Running) {
                    root.ResetStatus();
                    root.Execute();
                    Console.WriteLine("-----------------------------------");
                    counter++;
                    enemyHp = 180;
                }

                await Task.Delay(1000);
            }
        }

        static async Task SequenceActionTest() {
            BTSequenceNode root = new BTSequenceNode(null);
            
            BTSequenceNode seq1 = new BTSequenceNode(root);
            seq1.AddNode(new BTActionNode(seq1, () => {
                Console.WriteLine("seq1 action1");
                return ActionResult.Success;
            }));
            seq1.AddNode(new BTActionNode(seq1, () => {
                Console.WriteLine("seq1 action2");
                if (rand.Next(0, 100) < 50) {
                    return ActionResult.Failure;
                }
                return ActionResult.Success;
            }));
            seq1.AddNode(new BTActionNode(seq1, () => {
                Console.WriteLine("seq1 action3");
                return ActionResult.Success;
            }));
            root.AddNode(seq1);
            
            BTSequenceNode seq2 = new BTSequenceNode(root);
            seq2.AddNode(new BTActionNode(seq2, () => {
                Console.WriteLine("seq2 action1");
                return ActionResult.Success;
            }));
            seq2.AddNode(new BTActionNode(seq2, () => {
                Console.WriteLine("seq2 action2");
                if (rand.Next(0, 100) < 50) {
                    return ActionResult.Failure;
                }
                return ActionResult.Success;
            }));
            seq2.AddNode(new BTActionNode(seq2, () => {
                Console.WriteLine("seq2 action3");
                return ActionResult.Success;
            }));
            root.AddNode(seq2);
            
            root.Execute();

            int counter = 0;
            while (counter < 2) {
                root.Update();
                if (root.status != Status.Running) {
                    root.ResetStatus();
                    root.Execute();
                    Console.WriteLine(counter);
                    counter++;
                }

                await Task.Delay(1000);
            }
        }
        
        static async Task SelectorActionTest() {
            BTSelectorNode root = new BTSelectorNode(null);
            
            BTSequenceNode seq1 = new BTSequenceNode(root);
            seq1.AddNode(new BTActionNode(seq1, () => {
                Console.WriteLine("seq1 action1");
                return ActionResult.Success;
            }));
            seq1.AddNode(new BTActionNode(seq1, () => {
                Console.WriteLine("seq1 action2");
                if (rand.Next(0, 100) < 50) {
                    return ActionResult.Failure;
                }
                return ActionResult.Success;
            }));
            seq1.AddNode(new BTActionNode(seq1, () => {
                Console.WriteLine("seq1 action3");
                return ActionResult.Success;
            }));
            root.AddNode(seq1);
            
            BTSequenceNode seq2 = new BTSequenceNode(root);
            seq2.AddNode(new BTActionNode(seq2, () => {
                Console.WriteLine("seq2 action1");
                return ActionResult.Success;
            }));
            seq2.AddNode(new BTActionNode(seq2, () => {
                Console.WriteLine("seq2 action2");
                if (rand.Next(0, 100) < 50) {
                    return ActionResult.Failure;
                }
                return ActionResult.Success;
            }));
            seq2.AddNode(new BTActionNode(seq2, () => {
                Console.WriteLine("seq2 action3");
                return ActionResult.Success;
            }));
            root.AddNode(seq2);
            
            root.Execute();

            int counter = 0;
            while (counter < 2) {
                root.Update();
                if (root.status != Status.Running) {
                    Console.WriteLine(counter);
                    counter++;
                    root.ResetStatus();
                    root.Execute();
                }

                await Task.Delay(1000);
            }
        }
        
        static async Task ParallelActionTest() {
            BTParallelNode root = new BTParallelNode(null);
            root.AddNode(new BTActionNode(root, () => {
                Console.WriteLine("para action1");
                return ActionResult.Success;
            }));
            int writeCount = 0;
            root.AddNode(new BTActionNode(root, () => {
                Console.WriteLine("para action2");
                writeCount++;
                if (writeCount > 3) {
                    return ActionResult.Success;
                }
                return ActionResult.Running;
            }));
            root.AddNode(new BTActionNode(root, () => {
                Console.WriteLine("para action3");
                return ActionResult.Success;
            }));
            
            root.Execute();
            
            int counter = 0;
            while (counter < 2) {
                root.Update();
                if (root.status != Status.Running) {
                    Console.WriteLine(counter);
                    counter++;
                    root.ResetStatus();
                    root.Execute();
                    writeCount = 0;
                }

                await Task.Delay(1000);
            }
        }
        
        static async Task ConditionNodeTest() {
            int hp = 110;
            
            BTSelectorNode root = new BTSelectorNode(null);
            BTConditionNode condition = new BTConditionNode(root, () => {
                return hp > 100;
            });
            condition.AppendNode(new BTActionNode(condition, () => {
                Console.WriteLine("condition through");
                return ActionResult.Success;
                
            }));
            root.AddNode(condition);
            root.AddNode(new BTActionNode(root, () => {
                Console.WriteLine("action2");
                return ActionResult.Success;
            }));
            
            root.Execute();

            int counter = 0;
            while (counter < 2) {
                root.Update();
                if (root.status != Status.Running) {
                    root.ResetStatus();
                    root.Execute();
                    Console.WriteLine(counter);
                    counter++;
                }

                await Task.Delay(1000);
            }
        }
        
        static async Task RepeaterTest() {
            BTRepeaterNode root = new BTRepeaterNode(null,3);
            BTSelectorNode selec = new BTSelectorNode(root);
            selec.AddNode(new BTActionNode(selec, () => {
                Console.WriteLine("action1");
                return ActionResult.Failure;
            }));
            selec.AddNode(new BTActionNode(selec, () => {
                Console.WriteLine("action2");
                return ActionResult.Success;
            }));
            root.AppendNode(selec);

            root.Execute();

            int counter = 0;
            while (counter < 1) {
                root.Update();
                if (root.status != Status.Running) {
                    root.ResetStatus();
                    root.Execute();
                    Console.WriteLine(counter);
                    counter++;
                }

                await Task.Delay(1000);
            }
        }
    }
}