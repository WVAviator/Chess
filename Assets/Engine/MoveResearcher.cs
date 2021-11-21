using System.Collections.Generic;

namespace Chess.Engine
{
    public class MoveResearcher
    {

        public List<Move> GetAllLegalMoves(ChessBoard board)
        {
            List<Move> moves = new List<Move>();
            
            byte turn = board.Turn;
            int opponent = turn ^ 24;

            for (int square = 0; square < 64; square++)
            {
                if ((byte)(board[square] & turn) != turn) continue;
                
                //Pawn
                if ((byte)(board[square] ^ turn) == 1)
                {
                    int pawnDirection = turn == 16 ? 1 : -1;
                    int pawnStartRow = turn == 16 ? 1 : 6;
                    
                    int pawnForward = square + pawnDirection * 8;
                    int pawnDoubleForward = square + pawnDirection * 16;
                    
                    if (board[pawnForward] == 0)
                    {
                        moves.Add(new Move(board, square, pawnForward));
                        if (square >> 3 == pawnStartRow && board[pawnDoubleForward] == 0)
                        {
                            moves.Add(new Move(board,square, pawnDoubleForward));
                        }
                    }

                    int diagonalLeft = square + pawnDirection * turn == 16 ? 7 : 11;
                    int diagonalRight = square + pawnDirection * 9;
                    
                    if (board[diagonalLeft] >> 3 == board[square] >> 3 + pawnDirection && (byte)(board[diagonalLeft] & opponent) == opponent)
                    {
                        moves.Add(new Move(board, square, diagonalLeft));
                    }
                    if (board[diagonalRight] >> 3 == board[square] >> 3 + pawnDirection && (byte)(board[diagonalRight] & opponent) == opponent)
                    {
                        moves.Add(new Move(board, square, diagonalRight));
                    }

                    
                    
                    
                }





            }


            return moves;

        }
        
    }
}