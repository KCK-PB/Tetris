using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TetrisMain.Models
{
    public class Scoreboard
    {
        private string highScoreFile;

        public Scoreboard(string highScoreFilePath)
        {
            highScoreFile = highScoreFilePath;
        }

        public void AddToHighScoreFile(DateTime date, string userName, int score)
        {
            File.AppendAllLines(this.highScoreFile, new List<string>
                        {
                            $"[{date.ToShortDateString()}] {userName} => {score}"
                        });
        }

        public List<string> GetHighScoreList()
        {
            return File.ReadAllLines(highScoreFile).ToList();
        }

        public int GetHighScore() {
            var highScore = 0;
            if (File.Exists(this.highScoreFile)) {
                var allScores = File.ReadAllLines(this.highScoreFile);
                foreach (var score in allScores) {
                    var match = Regex.Match(score, @" => (?<score>[0-9]+)");
                    highScore = Math.Max(highScore, int.Parse(match.Groups["score"].Value));
                }
            }

            return highScore;
        }
    }
}
