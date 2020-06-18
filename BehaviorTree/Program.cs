using System;
using System.Threading.Tasks;

namespace BehaviorTree {
    class Program {
        static Random rand = new Random();
        
        static async Task Main(string[] args) {
            await SelectorActionTest();
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
                    counter++;
                    root.ResetStatus();
                    root.Execute();
                }

                await Task.Delay(1000);
            }
        }
    }
}