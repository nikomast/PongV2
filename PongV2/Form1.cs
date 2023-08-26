namespace PongV2
{
    using System.Data.SQLite;
    using System.Text;

    public partial class Form1 : Form
    {
        private bool meneYlos; // merkki siit‰, pit‰isikˆ pelaajan liikkua ylˆsp‰in
        private bool meneAlas; // merkki siit‰, pit‰isikˆ pelaajan liikkua alasp‰in
        private bool meneYlos2; // toisen pelaajan merkki
        private bool meneAlas2; // toisen pelaajan merkki
        private int nopeus = 5; // pallon nopeus
        private int pallox = 5; // pallon x-nopeus
        private int palloy = 5; // pallon y-nopeus
        private int tulos1 = 0; // pelaajan tulos
        private System.Windows.Forms.Timer peliajastin = new System.Windows.Forms.Timer();

        //Tietokanta metodit luonti jos ei ole
        private void CreateDatabase()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=PongScores.db;Version=3;"))
            {
                conn.Open();

                string sql = "CREATE TABLE IF NOT EXISTS HighScores (\r\n    ID INTEGER PRIMARY KEY AUTOINCREMENT,\r\n    PlayerName TEXT,\r\n    Score INTEGER,\r\n    DateAdded DATETIME DEFAULT CURRENT_TIMESTAMP\r\n)";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void SaveScore(string playerName, int score)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=PongScores.db;Version=3;"))
            {
                conn.Open();

                string sql = "INSERT INTO HighScores (PlayerName, Score) VALUES (@PlayerName, @Score)";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PlayerName", playerName);
                    cmd.Parameters.AddWithValue("@Score", score);

                    cmd.ExecuteNonQuery();
                }
            }
        }



        private int GetLowestTopTenScore()
        {
            int lowestScore = int.MaxValue; // Default to a high value

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=PongScores.db;Version=3;"))
            {
                conn.Open();

                string sql = "SELECT MIN(Score) as LowestScore FROM (SELECT Score FROM HighScores ORDER BY Score DESC LIMIT 10)";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // If there are scores, get the lowest
                            if (reader["LowestScore"] != DBNull.Value)
                                lowestScore = int.Parse(reader["LowestScore"].ToString());
                            else
                                lowestScore = 0; // If there are no scores yet
                        }
                    }
                }
            }

            return lowestScore;
        }

        //Tuloksen tallennus jos on
        private void SaveScoreAndUpdateTopTen(string playerName, int score)
        {
            int lowestScore = GetLowestTopTenScore();

            // If the score qualifies for the top ten, or there are less than 10 scores
            if (score > lowestScore || GetHighScores().Count < 10)
            {
                SaveScore(playerName, score);
                RemoveEleventhScore();  // Ensure there are only 10 top scores
            }
        }


        private void RemoveEleventhScore()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=PongScores.db;Version=3;"))
            {
                conn.Open();

                //Varmistetaan ett‰ tietokannassa on vain 10 parasta
                string sql = @"DELETE FROM HighScores WHERE ID IN (
                         SELECT ID FROM HighScores 
                         ORDER BY Score ASC, DateAdded ASC 
                         LIMIT -1 OFFSET 10
                       )";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }




        private List<(string Name, int Score)> GetHighScores()
        {
            List<(string Name, int Score)> scores = new List<(string Name, int Score)>();

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=PongScores.db;Version=3;"))
            {
                conn.Open();

                string sql = "SELECT PlayerName, Score FROM HighScores ORDER BY Score DESC LIMIT 10"; // Gets the top 10 high scores

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            scores.Add((reader["PlayerName"].ToString(), int.Parse(reader["Score"].ToString())));
                        }
                    }
                }
            }

            return scores;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { meneYlos = true; }
            if (e.KeyCode == Keys.S) { meneAlas = true; }
            if (e.KeyCode == Keys.Up) { meneYlos2 = true; }
            if (e.KeyCode == Keys.Down) { meneAlas2 = true; }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { meneYlos = false; }
            if (e.KeyCode == Keys.S) { meneAlas = false; }
            if (e.KeyCode == Keys.Up) { meneYlos2 = false; }
            if (e.KeyCode == Keys.Down) { meneAlas2 = false; }
        }

        private void peliajastinTapahtuma(object sender, EventArgs e)
        {


            //Pisteiden lis‰ys
            if (Pallo.Bounds.IntersectsWith(Ohjain2.Bounds))
            {
                tulos1++;
                Tulos1.Text = tulos1.ToString();

                pallox = (int)(pallox * 1.50);
                palloy = (int)(palloy * 1.50);
                const int MAX_SPEED = 25;

                if (Math.Abs(pallox) > MAX_SPEED)
                {
                    pallox = (pallox < 0) ? -MAX_SPEED : MAX_SPEED;
                }
                if (Math.Abs(palloy) > MAX_SPEED)
                {
                    palloy = (palloy < 0) ? -MAX_SPEED : MAX_SPEED;
                }
            }

            // Liikutetaan ohjainta
            if (meneYlos && Ohjain1.Top > 0) { Ohjain1.Top -= 15; }
            if (meneAlas && Ohjain1.Bottom < this.ClientSize.Height) { Ohjain1.Top += 15; }

            // Liikutetaan palloa
            Pallo.Left += pallox;
            Pallo.Top += palloy;

            // Pallon kimpoaminen yl‰reunasta ja alareunasta
            if (Pallo.Top <= 0 || Pallo.Top + Pallo.Height >= ClientSize.Height)
            {
                palloy = -palloy;
            }

            // Peli p‰‰ttyy kun pallo menee ohi ohjaimen 1
            if (Pallo.Left <= 0)
            {
                peliajastin.Stop();

                // Check if the score qualifies for the top ten or if there are less than 10 scores
                int currentScore = tulos1; // Assuming tulos1 is the current score
                if (currentScore > GetLowestTopTenScore() || GetHighScores().Count < 10)
                {
                    // Prompt the player for their name
                    string playerName = Microsoft.VisualBasic.Interaction.InputBox("Onneksi olkoon! P‰‰sit top 10 tuloksiin! Syˆt‰ nimesi:", "Korkea tulos!");

                    // Check if playerName is not empty, and then save
                    if (!string.IsNullOrWhiteSpace(playerName))
                    {
                        SaveScoreAndUpdateTopTen(playerName, currentScore);
                    }
                }

                // Fetch and format the top scores
                var highScores = GetHighScores();
                StringBuilder highScoreMessage = new StringBuilder("Top 10 Tulokset:\n\n");

                int rank = 1;
                foreach (var score in highScores)
                {
                    highScoreMessage.AppendLine($"{rank}. {score.Name} - {score.Score}");
                    rank++;
                }

                MessageBox.Show(highScoreMessage.ToString(), "Top 10 Tulokset");

                MessageBox.Show("Peli p‰‰ttyi! Yrit‰ uudelleen.");

                // Reset the game, if you want
                ResetoiPeli();
            }


            // Pallo osuu ohjaimeen
            if (Pallo.Bounds.IntersectsWith(Ohjain1.Bounds))
            {
                pallox = -pallox;
            }

            // Pallo osuu oikeaan sein‰‰n
            if (Pallo.Bounds.IntersectsWith(Ohjain2.Bounds))
            {
                pallox = -pallox;
            }
        }

        private void ResetoiPeli()
        {
            Pallo.Left = ClientSize.Width / 2;
            Pallo.Top = ClientSize.Height / 2;
            pallox = 5;
            palloy = 5;
            tulos1 = 0;
            Tulos1.Text = tulos1.ToString();
            peliajastin.Start();
        }



        public Form1()
        {
            InitializeComponent();
            CreateDatabase();
            peliajastin.Interval = 20; // Specifies the timer to tick every 20 milliseconds.
            peliajastin.Tick += peliajastinTapahtuma; // Attach the tick event to your game loop function
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            peliajastin.Start();
        }
    }
}