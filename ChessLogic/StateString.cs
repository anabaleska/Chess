using System.Text;
namespace ChessLogic
{
	public class StateString
	{
		private readonly StringBuilder sb = new StringBuilder();
		public StateString(Player currentPlayer, Board board)
		{
			//podatoci za mestopolozhba
			AddPiecePlacement(board);
			sb.Append(' ');
			//current Player
			AddCurrentPlayer(currentPlayer);
            sb.Append(' ');
			//dozvoli za rokada
			AddCastlingRights(board);
            sb.Append(' ');
			//enpassant podatoci
			AddEnPassant(board, currentPlayer);
        }

		public override string ToString()
		{
			return sb.ToString();
		}

		private static char PieceChar(Piece piece)
		{
			char c = piece.pieceType switch
			{
				PieceType.Pawn => 'p',
				PieceType.Knight => 'n',
				PieceType.Rook => 'r',
				PieceType.Bishop => 'b',
				PieceType.Queen => 'q',
				PieceType.King => 'k',
				_ => ' '
			};

			if (piece.Color == Player.White)
			{
				return char.ToUpper(c);
			}
			return c;
		}

		private void AddRowData(Board  board, int row)
		{
			int empty = 0;
			for(int i=0;i<8; i++)
			{
				if (board[row, i] == null)
				{
					empty++;
					continue;
				}
				if (empty > 0)
				{
					sb.Append(empty);
					empty = 0;
				}

				sb.Append(PieceChar(board[row, i]));
			}

			if (empty > 0)
			{
				sb.Append(empty);
			}
		}
		private void AddPiecePlacement(Board board)
		{
			for(int j = 0; j < 8; j++)
			{
				if (j != 0)
				{
                    sb.Append('/');
                }
				AddRowData(board, j);

			}
		}

		private void AddCurrentPlayer(Player currentPlayer)
		{
			if (currentPlayer == Player.White)  //redot na beliot
			{
                sb.Append('w');
            }
			else   //redot na crniot
			{
                sb.Append('b');
            }
		}

		private void AddCastlingRights(Board board)
		{
			bool castleWKS = board.CastleRightKS(Player.White);
			bool castleWQS = board.CastleRightQS(Player.White);
            bool castleBKS = board.CastleRightKS(Player.Black);
            bool castleBQS = board.CastleRightQS(Player.Black);

			if (!(castleWKS || castleWQS || castleBKS || castleBQS))
			{
				sb.Append('-');
				return;
			}
			if (castleWKS)
			{
				sb.Append('K');
			}
			if (castleWQS)
			{
				sb.Append('Q');
			}
			if (castleBKS)
			{
				sb.Append('k');
			}
			if (castleBQS)
			{
				sb.Append('q');
			}

        }

		private void AddEnPassant(Board board, Player currentPlayer)
		{
			if (!board.CanCaptureEnPassant(currentPlayer))
			{
				sb.Append('-');
				return;
			}

			Position pos = board.GetPawnSkipPosition(currentPlayer.Opponent());
			char file = (char)('a' + pos.Column);
			int rank = 8 - pos.Row;
			sb.Append(file);
			sb.Append(rank);
		}
	}
}
