namespace Bobs_Racing.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    public class Runner
    {
        public string Name { get; set; }
        public double Position { get; set; } // In meters
        public double Speed { get; set; } // In m/s
        public double SlowestTime { get; set; }
        public double FastestTime { get; set; }
        public int FinalPosition { get; set; }
        public int AthleteID { get; set; }
        public int RaceAthleteID { get; set; }
        public double? FinishTime { get; set; }
        
        public Runner() { }


        public Runner(string name, double position, double speed, double slowestTime, double fastestTime, int finalPosition, int athleteID, int raceAthleteID, double? finishTime)
        {
            Name = name;
            Position = position;
            Speed = speed;
            SlowestTime = slowestTime;
            FastestTime = fastestTime;
            FinalPosition = finalPosition;
            AthleteID = athleteID;
            RaceAthleteID = raceAthleteID;
            FinishTime = finishTime;
        }
    }

    public class RaceSimulationHub : Hub
    {
        public async Task SendRaceUpdate(List<Runner> runners)
        {
            await Clients.All.SendAsync("ReceiveRaceUpdate", runners);
        }
    }

}