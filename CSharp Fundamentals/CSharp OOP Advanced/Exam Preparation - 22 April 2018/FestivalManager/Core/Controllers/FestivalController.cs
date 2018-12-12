namespace FestivalManager.Core.Controllers
{
	using System;
	using System.Globalization;
	using System.Linq;
    using System.Reflection;
    using System.Text;
	using Contracts;
	using Entities.Contracts;
    using FestivalManager.Entities.Factories;
    using FestivalManager.Entities.Factories.Contracts;

    public class FestivalController : IFestivalController
	{
		private const string TimeFormat = "mm\\:ss";
		private const string TimeFormatLong = "{0:2D}:{1:2D}";
		private const string TimeFormatThreeDimensional = "{0:3D}:{1:3D}";

		private readonly IStage stage;
        private  ISetController setController;
        
        private ISetFactory setFactory;
        private IInstrumentFactory instrumentFactory;
        private IPerformerFactory performerFactory;
        private ISongFactory songFactory;

		public FestivalController(IStage stage)
		{
			this.stage = stage;
             this.instrumentFactory = new InstrumentFactory();
            this.performerFactory = new PerformerFactory();
            this.songFactory = new SongFactory();
            this.setFactory = new SetFactory();
        }
        
        public string ProduceReport()
        {
            var result = string.Empty;

            var totalFestivalLength = new TimeSpan(this.stage.Sets.Sum(s => s.ActualDuration.Ticks));

            result += $"Festival length: {FormatTime(totalFestivalLength)}" + "\n";

            foreach (var set in this.stage.Sets)
            {
                result += $"--{set.Name} ({FormatTime(set.ActualDuration)}):" + "\n";

                var performersOrderedDescendingByAge = set.Performers.OrderByDescending(p => p.Age);
                foreach (var performer in performersOrderedDescendingByAge)
                {
                    var instruments = string.Join(", ", performer.Instruments
                        .OrderByDescending(i => i.Wear));

                    result += $"---{performer.Name} ({instruments})" + Environment.NewLine;
                }

                if (!set.Songs.Any())
                    result += "--No songs played" + Environment.NewLine;
                else
                {
                    result += "--Songs played:" + Environment.NewLine;
                    foreach (var song in set.Songs)
                    {
                        result += $"----{song.Name} ({FormatTime(song.Duration)})" + Environment.NewLine;
                    }
                }
            }

            return result;
        }

        public string RegisterSet(string[] args)
		{
            string name = args.First();
            string type = args[1];

            ISet set = this.setFactory.CreateSet(name, type);

            this.stage.AddSet(set);
            return $"Registered {type} set";
        }

		public string SignUpPerformer(string[] args)
		{
			string name = args[0];
			int age = int.Parse(args[1]);

			IInstrument[] instruments = args.Skip(2).Select(i=>this.instrumentFactory.CreateInstrument(i)).ToArray();
            
			var performer = this.performerFactory.CreatePerformer(name, age);

			foreach (var instrument in instruments)
			{
				performer.AddInstrument(instrument);
			}

			this.stage.AddPerformer(performer);

			return $"Registered performer {performer.Name}";
		}

		public string RegisterSong(string[] args)
		{
            string name = args.First();
            TimeSpan duration = TimeSpan.Parse("00:"+args[1]);
            ISong song = this.songFactory.CreateSong(name, duration);

            this.stage.AddSong(song);
			return $"Registered song {song.Name} ({duration.ToString(TimeFormat)})";
		}

        public string AddSongToSet(string[] args)
        {
            string songName = args.First();
            string setName = args[1];

            if (!this.stage.HasSet(setName))
            {
                throw new InvalidOperationException("Invalid set provided");
            }
            if (!this.stage.HasSong(songName))
            {
                throw new InvalidOperationException("Invalid song provided");
            }

            ISet set = this.stage.GetSet(setName);
            ISong song = this.stage.GetSong(songName);

            set.AddSong(song);
            return $"Added {songName} ({song.Duration.ToString(TimeFormat)}) to {set.Name}";
        }

        

        public string AddPerformerToSet(string[] args)
        {
            return PerformerRegistration(args);
        }

		public string RepairInstruments(string[] args)
		{
			var instrumentsToRepair = this.stage.Performers
				.SelectMany(p => p.Instruments)
				.Where(i => i.Wear < 100)
				.ToArray();

			foreach (var instrument in instrumentsToRepair)
			{
				instrument.Repair();
			}

			return $"Repaired {instrumentsToRepair.Length} instruments";
		}

        public string LetsRock()
        {
            this.setController = new SetController(this.stage);

           return this.setController.PerformSets();
        }

        private string PerformerRegistration(string[] args)
        {
            var performerName = args[0];
            var setName = args[1];

            if (!this.stage.HasPerformer(performerName))
            {
                throw new InvalidOperationException("Invalid performer provided");
            }

            if (!this.stage.HasSet(setName))
            {
                throw new InvalidOperationException("Invalid set provided");
            }

            var performer = this.stage.GetPerformer(performerName);
            var set = this.stage.GetSet(setName);

            set.AddPerformer(performer);

            return $"Added {performer.Name} to {set.Name}";
        }

        private string FormatTime(TimeSpan timeSpan)
        {
            int mins = timeSpan.Days * 1440 + timeSpan.Hours * 60 + timeSpan.Minutes;
            int secs = timeSpan.Seconds;

            return $"{mins:d2}:{secs:d2}";
        }
    }
}