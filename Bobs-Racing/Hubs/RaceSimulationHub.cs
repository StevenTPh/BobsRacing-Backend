namespace Bobs_Racing.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Runner
    {
        public string Name { get; set; }
        public double Position { get; set; } // In meters
        public double Speed { get; set; } // In m/s
        public double LowestTime { get; set; } 
        public double FastestTime { get; set; } 
    }

    public class RaceSimulationHub : Hub
    {
        public async Task SendRaceUpdate(List<Runner> runners)
        {
            await Clients.All.SendAsync("ReceiveRaceUpdate", runners);
        }
    }

}