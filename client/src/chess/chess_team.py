from enum import IntEnum
from typing import Optional


class ChessTeam(IntEnum):
    Viewer = 0
    White = 1
    Black = 2

    @staticmethod
    def from_str(team_str: str) -> Optional['ChessTeam']:
        match team_str:
            case "White":
                return ChessTeam.White
            case "Black":
                return ChessTeam.Black
            case "Viewer":
                return ChessTeam.Viewer
            case _:
                return None
