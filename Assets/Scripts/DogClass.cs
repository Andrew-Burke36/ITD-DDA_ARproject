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