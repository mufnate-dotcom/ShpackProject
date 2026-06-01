using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace StratagemHeroGame
{
    public class GameForm : Form
    {
        private StratagemGame game;
        private Label lblScore;
        private Label lblStreak;
        private Label lblStratagemName;
        private Label lblPoints;
        private Label lblStatus;
        private Label lblBestScore;
        private Label lblTimerValue;
        private Label lblRoundBonus;
        private Panel panelArrows;
        private Button btnNewGame;
        private Button btnAbortGame;
        private ComboBox cmbDifficulty;
        private CheckedListBox chkStratagems;
        private Button btnSelectAll;
        private Button btnDeselectAll;
        private Panel selectionPanel;
        private int bestScore;
        private Timer neonTimer;
        private int neonPhase;
        private Label lblTitle;
        private List<Button> currentSequenceButtons;
        private Label lblRound;
        private Label lblRoundProgress;
        private Label lblTimeBonus;
        public GameForm()
        {
            game = new StratagemGame();
            InitializeComponent();
            SetupGameEvents();
            bestScore = 0;
            UpdateBestScoreDisplay();
            StartNeonEffect();
            currentSequenceButtons = new List<Button>();

            lblTimerValue.Text = "TIME: --";
            cmbDifficulty.SelectedIndexChanged += cmbDifficulty_SelectedIndexChanged;
            cmbDifficulty_SelectedIndexChanged(null, null);
        }
        private void InitializeComponent()
        {
            this.Text = "STRATAGEM HERO";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.KeyPreview = true;
            this.KeyDown += GameForm_KeyDown;
            CreateHeaderPanel();
            CreateSelectionPanel();
            CreateStatsPanel();
            CreateStratagemPanel();
            CreateGamePanel();
            CreateControlButtons();
        }
        private void StartNeonEffect()
        {
            neonTimer = new Timer();
            neonTimer.Interval = 150;
            neonTimer.Tick += (s, e) =>
            {
                neonPhase++;
                if (neonPhase % 2 == 0)
                {
                    lblTitle.ForeColor = Color.Yellow;
                }
                else
                {
                    lblTitle.ForeColor = Color.Gold;
                }
            };
            neonTimer.Start();
        }
        private void CreateHeaderPanel()
        {
            Panel headerPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(1200, 70),
                BackColor = Color.Black
            };
            this.Controls.Add(headerPanel);
            lblTitle = new Label
            {
                Text = "STRATAGEM HERO",
                Font = new Font("Arial", 28, FontStyle.Bold),
                ForeColor = Color.Yellow,
                BackColor = Color.Black,
                Location = new Point(350, 15),
                Size = new Size(500, 45),
                TextAlign = ContentAlignment.MiddleCenter
            };
            headerPanel.Controls.Add(lblTitle);
        }
        private void CreateSelectionPanel()
        {
            selectionPanel = new Panel
            {
                Location = new Point(20, 90),
                Size = new Size(260, 580),
                BackColor = Color.FromArgb(20, 20, 20),
                BorderStyle = BorderStyle.FixedSingle
            };
            selectionPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.Yellow, 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, selectionPanel.Width - 1, selectionPanel.Height - 1);
                }
            };
            this.Controls.Add(selectionPanel);

            Label lblSelectTitle = new Label
            {
                Text = "STRATAGEM LIST",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Yellow,
                BackColor = Color.FromArgb(20, 20, 20),
                Location = new Point(10, 10),
                Size = new Size(240, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };
            selectionPanel.Controls.Add(lblSelectTitle);
            chkStratagems = new CheckedListBox
            {
                Location = new Point(10, 45),
                Size = new Size(240, 350),
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.Yellow,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Arial", 9, FontStyle.Bold),
                CheckOnClick = true
            };
            var allStratagems = game.GetAllStratagems();
            foreach (var stratagem in allStratagems)
            {
                chkStratagems.Items.Add(stratagem.Name, true);
            }
            selectionPanel.Controls.Add(chkStratagems);
            btnSelectAll = new Button
            {
                Text = "SELECT ALL",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Size = new Size(115, 30),
                Location = new Point(10, 405),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.Yellow,
                FlatStyle = FlatStyle.Flat
            };
            btnSelectAll.FlatAppearance.BorderColor = Color.Yellow;
            btnSelectAll.FlatAppearance.BorderSize = 1;
            btnSelectAll.Click += (s, e) => { for (int i = 0; i < chkStratagems.Items.Count; i++) chkStratagems.SetItemChecked(i, true); };
            selectionPanel.Controls.Add(btnSelectAll);
            btnDeselectAll = new Button
            {
                Text = "DESELECT ALL",
                Font = new Font("Arial", 9, FontStyle.Bold),
                Size = new Size(115, 30),
                Location = new Point(135, 405),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.Yellow,
                FlatStyle = FlatStyle.Flat
            };
            btnDeselectAll.FlatAppearance.BorderColor = Color.Yellow;
            btnDeselectAll.FlatAppearance.BorderSize = 1;
            btnDeselectAll.Click += (s, e) => { for (int i = 0; i < chkStratagems.Items.Count; i++) chkStratagems.SetItemChecked(i, false); };
            selectionPanel.Controls.Add(btnDeselectAll);
            Label lblDifficultySelect = new Label
            {
                Text = "DIFFICULTY",
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Yellow,
                BackColor = Color.FromArgb(20, 20, 20),
                Location = new Point(10, 450),
                Size = new Size(240, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            selectionPanel.Controls.Add(lblDifficultySelect);
            cmbDifficulty = new ComboBox
            {
                Location = new Point(10, 475),
                Size = new Size(240, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.Yellow,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            cmbDifficulty.Items.AddRange(new string[] { "TRIVIAL", "EASY", "MEDIUM", "HARD", "HELLDIVER", "JOHN HELLDIVER" });
            cmbDifficulty.SelectedIndex = 2;
            selectionPanel.Controls.Add(cmbDifficulty);
            Label lblInfo = new Label
            {
                Text = "PRESS NEW GAME",
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Yellow,
                BackColor = Color.FromArgb(20, 20, 20),
                Location = new Point(10, 515),
                Size = new Size(240, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };
            selectionPanel.Controls.Add(lblInfo);
        }
        private void CreateStatsPanel()
        {
            Panel statsPanel = new Panel
            {
                Location = new Point(300, 90),
                Size = new Size(280, 200),
                BackColor = Color.FromArgb(20, 20, 20)
            };
            statsPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.Yellow, 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, statsPanel.Width - 1, statsPanel.Height - 1);
                }
            };
            this.Controls.Add(statsPanel);
            Label lblStatsTitle = new Label
            {
                Text = "STATS",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Yellow,
                BackColor = Color.FromArgb(20, 20, 20),
                Location = new Point(90, 5),
                Size = new Size(100, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            statsPanel.Controls.Add(lblStatsTitle);
            lblScore = new Label
            {
                Text = "SCORE: 0",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.Yellow,
                BackColor = Color.FromArgb(20, 20, 20),
                Location = new Point(10, 30),
                Size = new Size(260, 35),
                TextAlign = ContentAlignment.MiddleCenter
            };
            statsPanel.Controls.Add(lblScore);
            lblStreak = new Label
            {
                Text = "STREAK: 0",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.Gold,
                BackColor = Color.FromArgb(20, 20, 20),
                Location = new Point(10, 70),
                Size = new Size(260, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            statsPanel.Controls.Add(lblStreak);
            lblRound = new Label
            {
                Text = "ROUND: 1",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.Cyan,
                BackColor = Color.FromArgb(20, 20, 20),
                Location = new Point(10, 105),
                Size = new Size(260, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };
            statsPanel.Controls.Add(lblRound);
            lblRoundProgress = new Label
            {
                Text = "PROGRESS: 0/3",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Orange,
                BackColor = Color.FromArgb(20, 20, 20),
                Location = new Point(10, 130),
                Size = new Size(260, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };
            statsPanel.Controls.Add(lblRoundProgress);
            lblTimerValue = new Label
            {
                Text = "TIME: --",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.Yellow,
                BackColor = Color.FromArgb(20, 20, 20),
                Location = new Point(10, 160),
                Size = new Size(260, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };
            statsPanel.Controls.Add(lblTimerValue);
        }
        private void CreateStratagemPanel()
        {
            Panel stratagemPanel = new Panel
            {
                Location = new Point(600, 90),
                Size = new Size(580, 200),
                BackColor = Color.FromArgb(20, 20, 20)
            };
            stratagemPanel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.Yellow, 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, stratagemPanel.Width - 1, stratagemPanel.Height - 1);
                }
            };
            this.Controls.Add(stratagemPanel);
            Label lblStratTitle = new Label
            {
                Text = "CURRENT STRATAGEM",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Yellow,
                BackColor = Color.FromArgb(20, 20, 20),
                Location = new Point(180, 5),
                Size = new Size(220, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            stratagemPanel.Controls.Add(lblStratTitle);
            lblStratagemName = new Label
            {
                Text = "READY",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.Yellow,
                BackColor = Color.FromArgb(20, 20, 20),
                Location = new Point(10, 35),
                Size = new Size(420, 35),
                TextAlign = ContentAlignment.MiddleLeft
            };
            stratagemPanel.Controls.Add(lblStratagemName);
            lblPoints = new Label
            {
                Text = "0 PTS",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.Gold,
                BackColor = Color.FromArgb(20, 20, 20),
                Location = new Point(440, 35),
                Size = new Size(130, 35),
                TextAlign = ContentAlignment.MiddleRight
            };
            stratagemPanel.Controls.Add(lblPoints);
            lblBestScore = new Label
            {
                Text = "BEST SCORE: 0",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Yellow,
                BackColor = Color.FromArgb(20, 20, 20),
                Location = new Point(10, 80),
                Size = new Size(260, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };
            stratagemPanel.Controls.Add(lblBestScore);
            lblRoundBonus = new Label
            {
                Text = "ROUND BONUS: +50 per round | x2 SCORE",
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.LightGreen,
                BackColor = Color.FromArgb(20, 20, 20),
                Location = new Point(10, 115),
                Size = new Size(350, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };
            stratagemPanel.Controls.Add(lblRoundBonus);

            lblTimeBonus = new Label
            {
                Text = "TIME BONUS: +5 per remaining second",
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Cyan,
                BackColor = Color.FromArgb(20, 20, 20),
                Location = new Point(10, 150),
                Size = new Size(300, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };
            stratagemPanel.Controls.Add(lblTimeBonus);
        }
        private void CreateGamePanel()
        {
            panelArrows = new Panel
            {
                Location = new Point(300, 310),
                Size = new Size(880, 180),
                BackColor = Color.FromArgb(10, 10, 10)
            };
            panelArrows.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.Yellow, 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, panelArrows.Width - 1, panelArrows.Height - 1);
                }
            };
            this.Controls.Add(panelArrows);

            lblStatus = new Label
            {
                Text = "SELECT STRATAGEMS AND PRESS NEW GAME",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Yellow,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(300, 510),
                Size = new Size(880, 50),
                BackColor = Color.FromArgb(20, 20, 20),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(lblStatus);
        }
        private void CreateControlButtons()
        {
            btnAbortGame = new Button
            {
                Text = "ABORT GAME",
                Font = new Font("Arial", 11, FontStyle.Bold),
                Size = new Size(120, 45),
                Location = new Point(420, 600),
                BackColor = Color.FromArgb(80, 40, 40),
                ForeColor = Color.Yellow,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            btnAbortGame.FlatAppearance.BorderColor = Color.Red;
            btnAbortGame.FlatAppearance.BorderSize = 2;
            btnAbortGame.Click += BtnAbortGame_Click;
            this.Controls.Add(btnAbortGame);

            btnNewGame = new Button
            {
                Text = "NEW GAME",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Size = new Size(120, 45),
                Location = new Point(580, 600),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.Yellow,
                FlatStyle = FlatStyle.Flat
            };
            btnNewGame.FlatAppearance.BorderColor = Color.Yellow;
            btnNewGame.FlatAppearance.BorderSize = 2;
            btnNewGame.Click += BtnNewGame_Click;
            this.Controls.Add(btnNewGame);
            Label lblControls = new Label
            {
                Text = "╔══════════════════════════════════════════════════════════════════════════════════╗\n                         CONTROLS:  ▲ / W    ▼ / S    ◀ / A    ▶ / D                         \n╚══════════════════════════════════════════════════════════════════════════════════╝",
                Font = new Font("Consolas", 11, FontStyle.Bold),
                ForeColor = Color.Yellow,
                BackColor = Color.Black,
                Location = new Point(200, 700),
                Size = new Size(800, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblControls);
            Label lblSkipHint = new Label
            {
                Text = "PRESS ESC TO SKIP CURRENT STRATAGEM",
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Orange,
                BackColor = Color.Black,
                Location = new Point(350, 660),
                Size = new Size(500, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblSkipHint);
        }
        private void cmbDifficulty_SelectedIndexChanged(object sender, EventArgs e)
        {
            string difficulty = cmbDifficulty.SelectedItem.ToString();
            switch (difficulty)
            {
                case "TRIVIAL":
                    lblRoundBonus.Text = "ROUND BONUS: +20 per round | x1 SCORE | 15 sec";
                    lblRoundBonus.ForeColor = Color.LightGreen;
                    break;
                case "EASY":
                    lblRoundBonus.Text = "ROUND BONUS: +30 per round | x1 SCORE | 12 sec";
                    lblRoundBonus.ForeColor = Color.LightGreen;
                    break;
                case "MEDIUM":
                    lblRoundBonus.Text = "ROUND BONUS: +50 per round | x2 SCORE | 10 sec";
                    lblRoundBonus.ForeColor = Color.Yellow;
                    break;
                case "HARD":
                    lblRoundBonus.Text = "ROUND BONUS: +75 per round | x3 SCORE | 8 sec";
                    lblRoundBonus.ForeColor = Color.Orange;
                    break;
                case "HELLDIVER":
                    lblRoundBonus.Text = "ROUND BONUS: +100 per round | x4 SCORE | 6 sec";
                    lblRoundBonus.ForeColor = Color.OrangeRed;
                    break;
                case "JOHN HELLDIVER":
                    lblRoundBonus.Text = "ROUND BONUS: +150 per round | x5 SCORE | 5 sec";
                    lblRoundBonus.ForeColor = Color.Red;
                    break;
            }
        }
        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!game.IsGameActive) return;

            string direction = null;
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.W:
                    direction = "↑";
                    break;
                case Keys.Down:
                case Keys.S:
                    direction = "↓";
                    break;
                case Keys.Left:
                case Keys.A:
                    direction = "←";
                    break;
                case Keys.Right:
                case Keys.D:
                    direction = "→";
                    break;
                case Keys.Escape:
                    if (game.IsGameActive)
                        BtnReset();
                    e.Handled = true;
                    return;
            }
            if (direction != null)
            {
                game.ProcessInput(direction);
                e.Handled = true;
            }
        }
        private void BtnReset()
        {
            if (!game.IsGameActive) return;
            game.ResetCurrentStratagem();
            lblStatus.Text = "STRATAGEM SKIPPED";
            lblStatus.ForeColor = Color.Yellow;
            lblStatus.BackColor = Color.FromArgb(20, 20, 20);

            Task.Delay(600).ContinueWith(_ =>
            {
                this.Invoke(new Action(() =>
                {
                    lblStatus.Text = "ENTER CODE";
                    lblStatus.ForeColor = Color.Yellow;
                }));
            });
        }
        private void UpdateBestScoreDisplay()
        {
            lblBestScore.Text = $"BEST SCORE: {bestScore}";
        }

        private void SaveBestScore(int currentScore)
        {
            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                UpdateBestScoreDisplay();
                lblBestScore.ForeColor = Color.Gold;
                Task.Delay(2000).ContinueWith(_ =>
                {
                    this.Invoke(new Action(() =>
                    {
                        lblBestScore.ForeColor = Color.Yellow;
                    }));
                });
            }
        }
        private void SetupGameEvents()
        {
            game.OnStratagemChanged += UpdateStratagemDisplay;
            game.OnScoreChanged += UpdateScoreDisplay;
            game.OnGameEnd += OnGameEnd;
            game.OnCorrectInput += OnCorrectInput;
            game.OnWrongInput += OnWrongInput;
            game.OnInputProcessed += OnInputProcessed;
            game.OnRoundChanged += OnRoundChanged;
            game.OnTimerUpdate += OnTimerUpdate;
            game.OnTimeOut += OnTimeOut;
        }
        private void OnTimeOut()
        {
            lblStatus.Text = "  TIME'S UP!  ";
            lblStatus.ForeColor = Color.Red;
            lblStatus.BackColor = Color.FromArgb(40, 0, 0);

            Task.Delay(1000).ContinueWith(_ =>
            {
                this.Invoke(new Action(() =>
                {
                    lblStatus.Text = "ENTER CODE";
                    lblStatus.ForeColor = Color.Yellow;
                    lblStatus.BackColor = Color.FromArgb(20, 20, 20);
                }));
            });
        }
        private void OnTimerUpdate(int timeLeft, int maxTime)
        {
            if (timeLeft >= 0 && timeLeft <= 20)
            {
                lblTimerValue.Text = $"TIME: {timeLeft}";

                if (timeLeft <= 3)
                {
                    lblTimerValue.ForeColor = Color.Red;
                }
                else if (timeLeft <= 6)
                {
                    lblTimerValue.ForeColor = Color.Orange;
                }
                else
                {
                    lblTimerValue.ForeColor = Color.Yellow;
                }
            }
        }
        private void OnRoundChanged(int round, int totalInRound, int completed)
        {
            lblRound.Text = $"ROUND: {round}";
            lblRoundProgress.Text = $"PROGRESS: {completed}/{totalInRound}";
            if (round > 1)
            {
                lblStatus.Text = $"ROUND {round} START!";
                lblStatus.ForeColor = Color.Gold;
                Task.Delay(1000).ContinueWith(_ =>
                {
                    this.Invoke(new Action(() =>
                    {
                        lblStatus.Text = "ENTER CODE";
                        lblStatus.ForeColor = Color.Yellow;
                    }));
                });
            }
        }
        private void OnInputProcessed(int currentLength, int totalLength)
        {
            for (int i = 0; i < currentSequenceButtons.Count; i++)
            {
                if (i < currentLength)
                {
                    currentSequenceButtons[i].BackColor = Color.FromArgb(80, 80, 0);
                    currentSequenceButtons[i].ForeColor = Color.DarkGray;
                }
                else
                {
                    currentSequenceButtons[i].BackColor = Color.Yellow;
                    currentSequenceButtons[i].ForeColor = Color.Black;
                }
            }
        }
        private void UpdateStratagemDisplay(Stratagem stratagem)
        {
            lblStratagemName.Text = stratagem.Name;
            lblPoints.Text = $"{stratagem.Points} PTS";
            panelArrows.Controls.Clear();
            currentSequenceButtons.Clear();
            int totalWidth = stratagem.Code.Count * 90;
            int startX = (880 - totalWidth) / 2;

            for (int i = 0; i < stratagem.Code.Count; i++)
            {
                Button arrowButton = new Button
                {
                    Text = GetArrowSymbol(stratagem.Code[i]),
                    Font = new Font("Arial", 32, FontStyle.Bold),
                    Size = new Size(80, 110),
                    Location = new Point(startX + i * 90, 35),
                    BackColor = Color.Yellow,
                    ForeColor = Color.Black,
                    FlatStyle = FlatStyle.Flat,
                    Enabled = false,
                    Tag = i
                };
                arrowButton.FlatAppearance.BorderSize = 3;
                arrowButton.FlatAppearance.BorderColor = Color.DarkGoldenrod;
                panelArrows.Controls.Add(arrowButton);
                currentSequenceButtons.Add(arrowButton);
            }
        }
        private string GetArrowSymbol(string direction)
        {
            switch (direction)
            {
                case "↑": return "▲";
                case "↓": return "▼";
                case "←": return "◀";
                case "→": return "▶";
                default: return direction;
            }
        }
        private void UpdateScoreDisplay(int score, int streak)
        {
            lblScore.Text = $"SCORE: {score}";
            lblStreak.Text = $"STREAK: {streak}";
        }
        private void OnCorrectInput()
        {
            lblStatus.Text = "  CORRECT!  ";
            lblStatus.ForeColor = Color.Yellow;
            lblStatus.BackColor = Color.FromArgb(30, 30, 0);
            Task.Delay(600).ContinueWith(_ =>
            {
                this.Invoke(new Action(() =>
                {
                    lblStatus.Text = "ENTER CODE";
                    lblStatus.ForeColor = Color.Yellow;
                    lblStatus.BackColor = Color.FromArgb(20, 20, 20);
                }));
            });
        }
        private void OnWrongInput()
        {
            foreach (var btn in currentSequenceButtons)
            {
                btn.BackColor = Color.Red;
                btn.ForeColor = Color.Black;
            }
            Task.Delay(300).ContinueWith(_ =>
            {
                this.Invoke(new Action(() =>
                {
                    foreach (var btn in currentSequenceButtons)
                    {
                        btn.BackColor = Color.Yellow;
                        btn.ForeColor = Color.Black;
                    }
                }));
            });
            lblStatus.Text = "  WRONG!  ";
            lblStatus.ForeColor = Color.Yellow;
            lblStatus.BackColor = Color.FromArgb(40, 0, 0);

            Task.Delay(800).ContinueWith(_ =>
            {
                this.Invoke(new Action(() =>
                {
                    lblStatus.Text = "TRY AGAIN";
                    lblStatus.ForeColor = Color.Yellow;
                    lblStatus.BackColor = Color.FromArgb(20, 20, 20);
                }));
            });
        }
        private void OnGameEnd(int finalScore, int maxStreak, int finalRound, int completedInRound)
        {
            SaveBestScore(finalScore);

            lblTimerValue.Text = "TIME: --";
            lblTimerValue.ForeColor = Color.Yellow;

            DialogResult result = MessageBox.Show(
                $"            GAME OVER               \n" +
                $"  FINAL SCORE: {finalScore,-14} \n" +
                $"  BEST STREAK: {maxStreak,-14} \n" +
                $"  REACHED ROUND: {finalRound,-12} \n" +
                $"  BEST SCORE:  {bestScore,-14} \n" +
                $"  PLAY AGAIN?",
                "   MISSION COMPLETE",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                BtnNewGame_Click(null, null);
            }
            else
            {
                btnNewGame.Enabled = true;
                btnAbortGame.Enabled = false;
            }
        }
        private void BtnAbortGame_Click(object sender, EventArgs e)
        {
            if (!game.IsGameActive) return;

            DialogResult result = MessageBox.Show(
                "ABORT CURRENT GAME?\n\nYour progress will be lost!",
                "Abort Game",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                game.EndGame();
            }
        }
        private void BtnNewGame_Click(object sender, EventArgs e)
        {
            List<string> selectedStratagems = new List<string>();
            for (int i = 0; i < chkStratagems.Items.Count; i++)
            {
                if (chkStratagems.GetItemChecked(i))
                {
                    selectedStratagems.Add(chkStratagems.Items[i].ToString());
                }
            }
            if (selectedStratagems.Count == 0)
            {
                for (int i = 0; i < chkStratagems.Items.Count && i < 10; i++)
                {
                    selectedStratagems.Add(chkStratagems.Items[i].ToString());
                }
            }
            game.SetDifficulty(cmbDifficulty.SelectedItem.ToString());
            lblTimerValue.Text = "TIME: --";
            lblTimerValue.ForeColor = Color.Yellow;
            game.SetSelectedStratagems(selectedStratagems);
            game.StartNewGame();
            btnNewGame.Enabled = false;
            btnAbortGame.Enabled = true;
            string difficulty = cmbDifficulty.SelectedItem.ToString();
            switch (difficulty)
            {
                case "TRIVIAL":
                    lblStatus.Text = "TRIVIAL MISSION START!";
                    break;
                case "EASY":
                    lblStatus.Text = "EASY MISSION START!";
                    break;
                case "MEDIUM":
                    lblStatus.Text = "MEDIUM MISSION START!";
                    break;
                case "HARD":
                    lblStatus.Text = "HARD MISSION START!";
                    break;
                case "HELLDIVER":
                    lblStatus.Text = "HELLDIVER MISSION START!";
                    break;
                case "JOHN HELLDIVER":
                    lblStatus.Text = "JOHN HELLDIVER MISSION START!";
                    break;
            }
            lblStatus.ForeColor = Color.Yellow;
            lblStatus.BackColor = Color.FromArgb(0, 40, 0);
            Task.Delay(1500).ContinueWith(_ =>
            {
                this.Invoke(new Action(() =>
                {
                    lblStatus.Text = "ENTER CODE";
                    lblStatus.ForeColor = Color.Yellow;
                    lblStatus.BackColor = Color.FromArgb(20, 20, 20);
                }));
            });
        }
    }
}