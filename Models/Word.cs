namespace WordScrambler.Models
{
	public class Word
	{
		public string Origin { get; set; }
		public string Scrambled { get; set; }

		public Word()
		{
			
		}

		public Word(string origin)
		{
			Origin = origin;
		}
	}
}
