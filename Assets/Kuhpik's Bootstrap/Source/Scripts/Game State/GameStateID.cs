namespace Kuhpik
{
    public enum GameStateID
    {
        // Don't change int values in the middle of development.
        // Otherwise all of your settings in inspector can be messed up.

        Menu = 0,
        Game = 1,
        Finishing = 2,
        Win = 3,
        Lose = 4,

        // Extend just like that
        //
        // Revive = 100,
        // QTE = 200
    }
}