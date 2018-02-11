using Akka.Actor;
using AkkaWestWorld.Core;
using System;

namespace AkkaWestWorld.CmdLine
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Program().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERR: {ex}");
            }
            Console.WriteLine("done");
            Console.ReadLine();
        }

        private void Run()
        {
            var actorSystem =  ActorSystem.Create("WestWorld");

            var miner = actorSystem.ActorOf(Props.Create(() => new Miner(1, "Craig", new StatusLogger())), "minor");

            while(true)
            {

                var text = Console.ReadLine();
                if(text == "exit")
                {
                    break;
                }
                miner.Tell(new Miner.Tick());
            }

            CoordinatedShutdown.Get(actorSystem).Run().Wait(TimeSpan.FromSeconds(5));
        }
    }
}
