using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChessLogic;
namespace ChessUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];
        private readonly Image[,] piecceImages = new Image[8, 8];
        private readonly Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();

        private GameState gameState;
        private Position selectedPos = null;
        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();
            gameState = new GameState(Player.White, Board.Initital());
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < 8 ; i++)
            {
                for(int j = 0; j < 8 ; j++)
                {
                    Image image = new Image();
                    piecceImages[i, j] = image;
                    PieceGrid.Children.Add(image);
                    Rectangle highlight = new Rectangle();
                    highlights[i, j] = highlight;
                    HighlightGrid.Children.Add(highlight);
                }
            }
        }

        private void DrawBoard(Board board)
        {
            for( int i = 0;i < 8 ; i++)
            {
                for(int  k = 0;k < 8 ; k++)
                {
                    Piece p = board[i, k];
                    piecceImages[i, k].Source = Images.GetImage(p);
                }
            }
        }

        private void CacheMoves(IEnumerable<Move> moves)
        {
            moveCache.Clear();
            foreach (Move move in moves)
            {
                moveCache[move.ToPos] = move;
            }
        }

        private void ShowHighlights()
        {
            Color color = Color.FromArgb(150, 125, 255, 125);
            foreach(Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Column].Fill = new SolidColorBrush(color);
            }
        }

        private void HideHighlights()
        {
            foreach(Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Column].Fill = Brushes.Transparent;
            }
        }

        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsMenuOnScreen())
            {
                return;
            }

            Point point = e.GetPosition(BoardGrid);
            Position pos = ToSquarePosition(point);
            if (selectedPos == null)
            {
                OnFromPositionSelected(pos);
            }
            else
            {
                OnToPostionSelected(pos);
            }
        }

        private void OnToPostionSelected(Position pos)
        {
            selectedPos = null;
            HideHighlights();

            if(moveCache.TryGetValue(pos, out Move move))
            {
                if(move.Type == MoveType.PawnPromotion)
                {
                    HandlePromotion(move.FromPos, move.ToPos);
                }
                else
                {
                    HandleMove(move);
                }  
            }
        }

        private void HandlePromotion(Position from, Position to)
        {
            piecceImages[to.Row, to.Column].Source = Images.GetImage(gameState.CurrentPlayer, PieceType.Pawn);
            piecceImages[from.Row, to.Column].Source = null;

            PromotionMenu promMenu = new PromotionMenu(gameState.CurrentPlayer);
            MenuContainer.Content = promMenu;

            promMenu.PieceSelected += Type =>
            {
                MenuContainer.Content = null;
                Move promMove = new PawnPromotion(from, to, Type);
                HandleMove(promMove);
            };
        }

        private void HandleMove(Move move)
        {
            gameState.MakeMove(move);
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);

            if (gameState.IsGameOver())
            {
                ShowGameOver();
            }
        }

        private void OnFromPositionSelected(Position pos)
        {
            IEnumerable<Move> moves = gameState.LegalMovesForPieces(pos);

            if(moves.Any())
            {
                selectedPos = pos;
                CacheMoves(moves);
                ShowHighlights();
            }
        }

        private Position ToSquarePosition(Point point)
        {
            double squareSize = BoardGrid.ActualWidth / 8;
            int row = ((int)(point.Y / squareSize));
            int col = ((int)(point.X / squareSize));
            return new Position(row, col);

        }

        private void SetCursor(Player player)
        {
            if(player == Player.White)
            {
                Cursor = ChessCursors.WhiteCursor;
            }
            else
            {
                Cursor = ChessCursors.BlackCursor;
            }
        }

        private bool IsMenuOnScreen()
        {
            return MenuContainer.Content != null;
        }

        private void ShowGameOver()
        {
            GameOverMenu gameOverMenu = new GameOverMenu(gameState);
            MenuContainer.Content = gameOverMenu;

            gameOverMenu.OptionSelected += option =>
            {
                if (option == Option.Restart)
                {
                    MenuContainer.Content = null;
                    RestartGame();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            };
        }

        private void RestartGame()
        {
            selectedPos = null;
            HideHighlights();
            moveCache.Clear();
            gameState = new GameState(Player.White, Board.Initital());
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(!IsMenuOnScreen()&& e.Key == Key.Escape)
            {
                ShowPauseMenu();
            }
        }

        private void ShowPauseMenu()
        {
            PauseMenu pauseMenu = new PauseMenu();
            MenuContainer.Content = pauseMenu;

            pauseMenu.OptionSelected += option =>
            {
                MenuContainer.Content = null;
                if (option == Option.Restart)
                {
                    RestartGame();
                }
            };
        }
    }
}