using System;
using System.Collections.Generic;
using System.Linq;
namespace StratagemHeroGame
{
    public class StratagemGame
    {
        private List<Stratagem> allStratagems;
        private List<Stratagem> activeStratagems;
        private int score;
        private int streak;
        private Stratagem currentStratagem;
        private List<string> currentInput;
        private bool isGameActive;
        private Random random;
        private int currentRound;
        private int stratagemsInRound;
        private int stratagemsCompletedInRound;
        private int roundScore;
        private System.Windows.Forms.Timer stratagemTimer;
        private int timeLeftForCurrentStratagem;
        private int timePerStratagem;
        private string currentDifficulty;
        private int roundBonus;
        private float scoreMultiplier;
        private int timePenalty;
        public event Action<Stratagem> OnStratagemChanged;
        public event Action<int, int> OnScoreChanged;
        public event Action<int, int, int, int> OnGameEnd;
        public event Action OnCorrectInput;
        public event Action OnWrongInput;
        public event Action<int, int> OnInputProcessed;
        public event Action<int, int, int> OnRoundChanged;
        public event Action<int, int> OnTimerUpdate;
        public event Action OnTimeOut;
        public bool IsGameActive => isGameActive;
        public int CurrentRound => currentRound;
        public int StratagemsInRound => stratagemsInRound;
        public int StratagemsCompleted => stratagemsCompletedInRound;
        public int TimeLeft => timeLeftForCurrentStratagem;
        public int TimePerStratagem => timePerStratagem;
        public StratagemGame()
        {
            InitializeAllStratagems();
            random = new Random();
            score = 0;
            streak = 0;
            isGameActive = false;
            activeStratagems = new List<Stratagem>(allStratagems);
            currentRound = 1;
            stratagemsInRound = 3;
            stratagemsCompletedInRound = 0;
            roundScore = 0;
            timeLeftForCurrentStratagem = 10;
            timePerStratagem = 10;
            currentDifficulty = "MEDIUM";
            roundBonus = 50;
            scoreMultiplier = 2f;
            timePenalty = 2;
            stratagemTimer = new System.Windows.Forms.Timer();
            stratagemTimer.Interval = 1000;
            stratagemTimer.Tick += OnTimerTick;
        }
        private void InitializeAllStratagems()
        {
            allStratagems = new List<Stratagem>
            {
                new Stratagem("REINFORCE", new List<string> { "↑", "↓", "←", "→", "↑" }, 100, DifficultyRating.Easy),
                new Stratagem("RESUPPLY", new List<string> { "↓", "↓", "↑", "←" }, 100, DifficultyRating.Easy),
                new Stratagem("EAGLE CLUSTER", new List<string> { "↑", "→", "↓", "↓", "↓" }, 200, DifficultyRating.Medium),
                new Stratagem("ORBITAL LASER", new List<string> { "↓", "↑", "←", "↓", "→", "↓" }, 300, DifficultyRating.Medium),
                new Stratagem("RAILCANNON", new List<string> { "←", "→", "↑", "↑", "↓" }, 300, DifficultyRating.Medium),
                new Stratagem("SHIELD GENERATOR", new List<string> { "↓", "↑", "←", "→", "↑", "↓" }, 400, DifficultyRating.Hard),
                new Stratagem("HELLBOMB", new List<string> { "↓", "↑", "←", "→", "↑", "↑", "↓" }, 500, DifficultyRating.Hard),
                new Stratagem("ORBITAL 380MM", new List<string> { "←", "→", "←", "→", "↓", "↑", "←" }, 600, DifficultyRating.Expert),
                new Stratagem("NAPALM STRIKE", new List<string> { "↓", "↑", "→", "↓", "←", "↑" }, 550, DifficultyRating.Hard),
                new Stratagem("MORTAR SENTRY", new List<string> { "←", "↑", "→", "↓", "→", "↑" }, 350, DifficultyRating.Medium),
                new Stratagem("TESLA TOWER", new List<string> { "↓", "→", "↑", "←", "↓", "→" }, 450, DifficultyRating.Hard),
                new Stratagem("AUTOCANNON", new List<string> { "←", "↓", "←", "↑", "→", "↓" }, 250, DifficultyRating.Medium),
                new Stratagem("FLAMETHROWER", new List<string> { "↑", "→", "↓", "→", "↑", "←" }, 280, DifficultyRating.Medium),
                new Stratagem("JUMP PACK", new List<string> { "↓", "↑", "→", "←", "↓" }, 150, DifficultyRating.Easy),
                new Stratagem("ANTI-MATERIEL", new List<string> { "←", "→", "←", "→", "↑", "↓" }, 500, DifficultyRating.Hard),
                new Stratagem("SUPPLY PACK", new List<string> { "↓", "←", "→", "↑", "↓" }, 180, DifficultyRating.Easy),
                new Stratagem("AT-48", new List<string> { "→", "←", "↑", "↑", "↓", "→" }, 480, DifficultyRating.Hard),
                new Stratagem("STALWART", new List<string> { "←", "↑", "→", "→", "↓", "←" }, 320, DifficultyRating.Medium),
                new Stratagem("MACHINE GUN", new List<string> { "↓", "←", "↑", "→", "↓", "↑" }, 200, DifficultyRating.Easy),
                new Stratagem("LASER CANNON", new List<string> { "↑", "↑", "↓", "↓", "←", "→" }, 520, DifficultyRating.Hard),
                new Stratagem("ARC THROWER", new List<string> { "→", "←", "↑", "↑", "↓", "↓" }, 380, DifficultyRating.Medium),
                new Stratagem("GRENADE LAUNCHER", new List<string> { "←", "↓", "→", "↑", "←", "↓" }, 340, DifficultyRating.Medium),
                new Stratagem("EAGLE STRAFING", new List<string> { "↑", "→", "↑", "→", "↓" }, 280, DifficultyRating.Medium),
                new Stratagem("ORBITAL AIRBURST", new List<string> { "→", "→", "↑", "←", "↓", "→" }, 450, DifficultyRating.Hard),
                new Stratagem("SMOKE SCREEN", new List<string> { "↓", "↑", "↓", "↑", "←" }, 120, DifficultyRating.Easy),
                new Stratagem("EMS STRIKE", new List<string> { "→", "→", "←", "←", "↑", "↓" }, 350, DifficultyRating.Medium),
                new Stratagem("INCENDIARY MINES", new List<string> { "←", "→", "↓", "↑", "←", "→" }, 480, DifficultyRating.Hard),
                new Stratagem("GAS STRIKE", new List<string> { "↓", "→", "↑", "←", "→", "↓" }, 300, DifficultyRating.Medium)
            };
        }
        public List<Stratagem> GetAllStratagems()
        {
            return allStratagems;
        }
        public void SetSelectedStratagems(List<string> selectedNames)
        {
            activeStratagems = allStratagems.Where(s => selectedNames.Contains(s.Name)).ToList();
            if (activeStratagems.Count == 0)
            {
                activeStratagems = allStratagems.Take(10).ToList();
            }
        }
        public void SetDifficulty(string difficulty)
        {
            currentDifficulty = difficulty;
            switch (difficulty)
            {
                case "TRIVIAL":
                    timePerStratagem = 15;
                    roundBonus = 20;
                    scoreMultiplier = 1f;
                    timePenalty = 1;
                    break;
                case "EASY":
                    timePerStratagem = 12;
                    roundBonus = 30;
                    scoreMultiplier = 1f;
                    timePenalty = 1;
                    break;
                case "MEDIUM":
                    timePerStratagem = 10;
                    roundBonus = 50;
                    scoreMultiplier = 2f;
                    timePenalty = 2;
                    break;
                case "HARD":
                    timePerStratagem = 8;
                    roundBonus = 75;
                    scoreMultiplier = 3f;
                    timePenalty = 2;
                    break;
                case "HELLDIVER":
                    timePerStratagem = 6;
                    roundBonus = 100;
                    scoreMultiplier = 4f;
                    timePenalty = 3;
                    break;
                case "JOHN HELLDIVER":
                    timePerStratagem = 6;
                    roundBonus = 150;
                    scoreMultiplier = 5f;
                    timePenalty = 3;
                    break;
                default:
                    timePerStratagem = 10;
                    roundBonus = 50;
                    scoreMultiplier = 2f;
                    timePenalty = 2;
                    break;
            }
        }
        private void OnTimerTick(object sender, EventArgs e)
        {
            if (!isGameActive) return;
            if (timeLeftForCurrentStratagem > 0)
            {
                timeLeftForCurrentStratagem--;
                OnTimerUpdate?.Invoke(timeLeftForCurrentStratagem, timePerStratagem);

                if (timeLeftForCurrentStratagem <= 0)
                {
                    stratagemTimer.Stop();
                    OnTimeOut?.Invoke();
                    HandleTimeOut();
                }
            }
        }
        private void HandleTimeOut()
        {
            if (!isGameActive) return;

            streak = 0;
            UpdateScore();
            OnWrongInput?.Invoke();
            NextStratagem();
        }
        private void StartStratagemTimer()
        {
            if (stratagemTimer.Enabled)
            {
                stratagemTimer.Stop();
            }
            timeLeftForCurrentStratagem = timePerStratagem;
            OnTimerUpdate?.Invoke(timeLeftForCurrentStratagem, timePerStratagem);
            stratagemTimer.Start();
        }
        private void StopStratagemTimer()
        {
            if (stratagemTimer.Enabled)
            {
                stratagemTimer.Stop();
            }
        }
        public void StartNewGame()
        {
            score = 0;
            streak = 0;
            currentRound = 1;
            stratagemsInRound = 3;
            stratagemsCompletedInRound = 0;
            roundScore = 0;
            isGameActive = true;
            UpdateScore();
            UpdateRound();
            NextStratagem();
        }
        private void UpdateRound()
        {
            stratagemsInRound = 3 + (currentRound - 1) / 2;
            if (stratagemsInRound > 8) stratagemsInRound = 8;
            OnRoundChanged?.Invoke(currentRound, stratagemsInRound, stratagemsCompletedInRound);
        }
        private void NextStratagem()
        {
            if (!isGameActive) return;

            if (activeStratagems.Count == 0)
            {
                activeStratagems = allStratagems.Take(10).ToList();
            }
            List<Stratagem> availableStratagems;
            if (currentRound <= 2)
                availableStratagems = activeStratagems.Where(s => s.Difficulty == DifficultyRating.Easy).ToList();
            else if (currentRound <= 4)
                availableStratagems = activeStratagems.Where(s => s.Difficulty <= DifficultyRating.Medium).ToList();
            else if (currentRound <= 6)
                availableStratagems = activeStratagems.Where(s => s.Difficulty <= DifficultyRating.Hard).ToList();
            else
                availableStratagems = activeStratagems.ToList();
            if (availableStratagems.Count == 0)
                availableStratagems = activeStratagems;
            int index = random.Next(availableStratagems.Count);
            currentStratagem = availableStratagems[index];
            currentInput = new List<string>();
            StartStratagemTimer();
            OnStratagemChanged?.Invoke(currentStratagem);
            OnInputProcessed?.Invoke(0, currentStratagem.Code.Count);
        }
        public void ProcessInput(string direction)
        {
            if (!isGameActive) return;
            currentInput.Add(direction);
            OnInputProcessed?.Invoke(currentInput.Count, currentStratagem.Code.Count);
            for (int i = 0; i < currentInput.Count; i++)
            {
                if (i >= currentStratagem.Code.Count || currentInput[i] != currentStratagem.Code[i])
                {
                    streak = 0;
                    UpdateScore();
                    OnWrongInput?.Invoke();
                    StopStratagemTimer();
                    timeLeftForCurrentStratagem = Math.Max(0, timeLeftForCurrentStratagem - timePenalty);
                    OnTimerUpdate?.Invoke(timeLeftForCurrentStratagem, timePerStratagem);
                    NextStratagem();
                    return;
                }
            }
            if (currentInput.Count == currentStratagem.Code.Count)
            {
                StopStratagemTimer();
                int timeBonus = timeLeftForCurrentStratagem * 5;
                int pointsEarned = (int)(currentStratagem.Points * scoreMultiplier);
                int roundBonusPoints = currentRound * roundBonus;
                int comboBonus = (streak / 3) * 50;
                pointsEarned += comboBonus + roundBonusPoints + timeBonus;
                score += pointsEarned;
                roundScore += pointsEarned;
                streak++;
                stratagemsCompletedInRound++;
                UpdateScore();
                OnCorrectInput?.Invoke();
                if (stratagemsCompletedInRound >= stratagemsInRound)
                {
                    NextRound();
                }
                else
                {
                    NextStratagem();
                }
            }
        }
        private void NextRound()
        {
            currentRound++;
            stratagemsCompletedInRound = 0;
            roundScore = 0;

            UpdateRound();
            NextStratagem();
        }
        public void ResetCurrentStratagem()
        {
            if (!isGameActive) return;
            streak = 0;
            UpdateScore();
            StopStratagemTimer();
            NextStratagem();
        }
        public void ForceEndGame()
        {
            if (!isGameActive) return;
            EndGame();
        }
        private void UpdateScore()
        {
            OnScoreChanged?.Invoke(score, streak);
        }
        public void EndGame()
        {
            isGameActive = false;
            StopStratagemTimer();
            OnGameEnd?.Invoke(score, streak, currentRound, stratagemsCompletedInRound);
        }
    }
    public class Stratagem
    {
        public string Name { get; set; }
        public List<string> Code { get; set; }
        public int Points { get; set; }
        public DifficultyRating Difficulty { get; set; }
        public Stratagem(string name, List<string> code, int points, DifficultyRating difficulty)
        {
            Name = name;
            Code = code;
            Points = points;
            Difficulty = difficulty;
        }
    }
    public enum DifficultyRating
    {
        Easy,
        Medium,
        Hard,
        Expert
    }
}
