namespace LuviKunG.Random
{
    public sealed class Dice
    {
        private System.Random random;
        private int maxDice;

        public Dice(DiceType type)
        {
            maxDice = (int)type + 1;
            random = new System.Random();
        }

        public Dice(DiceType type, int seed)
        {
            maxDice = (int)type + 1;
            random = new System.Random(seed);
        }

        public int Roll()
        {
            return random.Next(1, maxDice);
        }
    }

    public enum DiceType : int
    {
        Tetrahedron = 4,
        Cube = 6,
        Octahedron = 8,
        Pentagonal = 10,
        Dodecahedron = 12,
        Icosahedron = 20
    }
}