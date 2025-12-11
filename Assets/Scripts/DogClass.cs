// Created by Andrew Burke and Vonce Chew

public class DogUIInformation
{
    public string Name;
    public string Age;
    public string Breed;
    public string Personality;
    public string Shortbio;

    public DogUIInformation(string Name, string Age, string Breed, string Personality, string ShortBio)
    {
        this.Name = Name;
        this.Age = Age;
        this.Breed = Breed;
        this.Personality = Personality;
        this.Shortbio = ShortBio;
    }
}

public class DogClass
{
    public string DogID;
    public string Name;

    public string Age;

    public string Breed;

    public string Personality;

    public bool IsAdopted;

    // Constructor class for Dog
    public DogClass(string Name, string Age, string Breed, string Personality, bool IsAdopted)
    {
        this.DogID = System.Guid.NewGuid().ToString(); // Generates unique ID for each dog
        
        this.Name = Name;

        this.Age = Age;

        this.Breed = Breed;

        this.Personality = Personality;

        this.IsAdopted = IsAdopted;
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