namespace LuviKunG.Random
{
    public sealed class Roulette
    {
        private System.Random random;
        private int min;
        private int max;

        public Roulette(int min, int max)
        {
            this.min = min;
            this.max = max;
            random = new System.Random();
        }

        public Roulette(int min, int max, int seed)
        {
            this.min = min;
            this.max = max;
            random = new System.Random(seed);
        }

        public int Spin()
        {
            return random.Next(min, max + 1);
        }
    }
}