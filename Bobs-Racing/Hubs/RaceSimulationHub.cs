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
        public double Acceleration { get; set; } // In m/s²
        public double ReactionTime { get; set; } // In seconds
    }

    public class RaceSimulationHub : Hub
    {
        public async Task SendRaceUpdate(List<Runner> runners)
        {
            await Clients.All.SendAsync("ReceiveRaceUpdate", runners);
        }
    }

}