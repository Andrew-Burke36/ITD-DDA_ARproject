public class Dog
{
    public int Age;
    public string Breed;
    public string Name;
    public float Weight;
    public bool IsAdopted;

    // Constructor class for Dog
    public Dog(int age, string breed, string name, float weight, bool isAdopted)
    {
        Age = age;
        Breed = breed;
        Name = name;
        Weight = weight;
        IsAdopted = isAdopted;

        // function here
    }
}


public class Player
{
    public string Username;
    public string Email;

    public Player(string email, string username)
    {
        Email = email;
        Username = username;
    }
}