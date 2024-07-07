using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public enum Player
    {
        //clasa so ke pretstavuvat koj player e na red i koj pobedil, kako i bojata na ches s delovite
        None,  //this is in case of a draw
        White,
        Black
    }

    public static class PlayerExtensions
    {
        //metod so go vrakat protivnikot na igracot so e prosleden
        public  static Player Opponent(this Player player)
        {

            return player switch
            {
                Player.White => Player.Black,
                Player.Black => Player.White,
                _ => Player.None,
            };
        }

    }
    
}
