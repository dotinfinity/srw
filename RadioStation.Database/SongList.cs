using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RadioStation.Database.Entities;

namespace RadioStation.Database
{
    public class SongList
    {
        public static List<Song> Get()
        {
            using (var context = new RadioStationContext())
            {
                return context.Songs.ToList();
            }
        }

        public void AddSong(string fullPath)
        {
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Cannot find file.", fullPath);

            var fileName = Path.GetFileName(fullPath);

            using (var context = new RadioStationContext())
            {
                if (context.Songs.Any(s => s.Name == fileName))
                    throw new InvalidOperationException("File has already been added");

                var newSong = new Song
                {
                    Name = fileName,
                    Folder = Path.GetDirectoryName(fullPath),
                    Extension = Path.GetExtension(fullPath)
                };
                context.Songs.Add(newSong);
                context.SaveChanges();
            }
        }
    }
}
