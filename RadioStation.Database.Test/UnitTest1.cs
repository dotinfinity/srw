using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RadioStation.Database.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestBasics()
        {
            var songs = RadioStation.Database.SongList.Get();
            Assert.IsNotNull(songs);
        }

        [TestMethod]
        public void AddSongTest()
        {
            var fileList = Directory.EnumerateFiles(@"E:\音乐\Top 300").ToList();
            var nextFilePath = fileList[new Random().Next(fileList.Count)];

            var songList = new RadioStation.Database.SongList();
            songList.AddSong(nextFilePath);
        }
    }
}
