namespace FestivalManager.Entities.Factories
{
	using System;
    using System.Linq;
    using System.Reflection;
    using Contracts;
	using Entities.Contracts;

	public class SongFactory : ISongFactory
	{
		public ISong CreateSong(string name, TimeSpan duration)
		{
            Type songType = Assembly.GetCallingAssembly().GetTypes().FirstOrDefault(x => x.Name == "Song");

            ISong song = (ISong)Activator.CreateInstance(songType, new object[] { name,duration});

            return song;
		}
	}
}