// Created by Andrew Burke and Vonce Chew

public class DogClass
{
    public string Name;

    public string Age;

    public string Breed;

    public string Personality;

    public string Shortbio;

    // Constructor class for Dog
    public DogClass(string Name, string Age, string Breed, string Personality, string Shortbio)
    {
        this.Name = Name;

        this.Age = Age;

        this.Breed = Breed;

        this.Personality = Personality;

        this.Shortbio = Shortbio;
    }

    public DogClass()
    {
        
    }
}

public class Player
{
    public string Username;
    public string Email;
    public int CurrentObjective;

    public Player(string email, string username, int currentObjective)
    {
        this.Email = email;

        this.Username = username;

        this.CurrentObjective = currentObjective;
    }
}