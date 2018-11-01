namespace HomeExercises
{
	public class Person
	{
		private static int idCounter = 0;
		public int Age { get; }
		public int Height { get; }
		public int Weight { get; }
		public string Name { get; }
		public Person Parent { get; }
		public int Id { get; }

		public Person(string name, int age, int height, int weight, Person parent)
		{
			Id = idCounter++;
			Name = name;
			Age = age;
			Height = height;
			Weight = weight;
			Parent = parent;
		}
	}
}