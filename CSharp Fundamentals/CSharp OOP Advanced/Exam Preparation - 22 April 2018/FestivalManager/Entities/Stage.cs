namespace FestivalManager.Entities
{
	using System.Collections.Generic;
    using System.Linq;
    using Contracts;

	public class Stage : IStage
	{
        public Stage()
        {
            this.sets = new List<ISet>();
            this.songs = new List<ISong>();
            this.performers = new List<IPerformer>();
        }

		private readonly List<ISet> sets;
		private readonly List<ISong> songs;
		private readonly List<IPerformer> performers;
        

        public IReadOnlyCollection<ISet> Sets => sets.AsReadOnly();
        public IReadOnlyCollection<ISong> Songs => songs.AsReadOnly();
        public IReadOnlyCollection<IPerformer> Performers => performers.AsReadOnly();

        public void AddPerformer(IPerformer performer)
        {
            this.performers.Add(performer);
        }

        public void AddSet(ISet set)
        {
            this.sets.Add(set);
        }

        public void AddSong(ISong song)
        {
            this.songs.Add(song);
        }

        public IPerformer GetPerformer(string name)
        {
            return this.performers.FirstOrDefault(x => x.Name == name);
        }

        public ISet GetSet(string name)
        {
            return this.sets.FirstOrDefault(x => x.Name == name);
        }

        public ISong GetSong(string name)
        {
            return this.songs.FirstOrDefault(x => x.Name == name);
        }

        public bool HasPerformer(string name)
        {
            return this.performers.Any(x => x.Name == name);
        }

        public bool HasSet(string name)
        {
            return sets.Any(x => x.Name == name);
        }

        public bool HasSong(string name)
        {
            return this.songs.Any(x => x.Name == name);
        }
    }
}
