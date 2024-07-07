using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public enum EndReason
    {
        Checkmate,
        Statemate,
        FiftyMoveRule,
        InsufficientMaterial,
        ThreefoldRepetition
    }
}
