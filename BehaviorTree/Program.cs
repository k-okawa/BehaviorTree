using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BehaviorTree {
    class Program {
        static Random rand = new Random();
        
        static async Task Main(string[] args) {
            await ConditionNodeTest();
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
    }
}