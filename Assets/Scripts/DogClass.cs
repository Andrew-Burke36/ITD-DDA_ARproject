// Created by Andrew Burke and Vonce Chew to provide classes for Dog and Player data structures.

using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
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

public class PlayerClass
{
    public string Username;
    public string Email;
    public ObjectiveTypes CurrentObjective;
    public int CurrentObjectiveProgress;
    public int CurrentObjectiveIndex;

    // Objective parts
    public int Score; 
    public List<string> CompletedTasks = new List<string>();
    public List<string> ScannedPictures;
    public List<string> AdoptedDogs = new List<string>();

    public PlayerClass(string email, string username, ObjectiveTypes currentObjective, int currentObjectiveProgress, int currentObjectiveIndex)
    {
        this.Email = email;
        this.Username = username;
        this.CurrentObjective = currentObjective;
        this.CurrentObjectiveProgress = currentObjectiveProgress;
        this.CurrentObjectiveIndex = currentObjectiveIndex;
    
        this.Score = 0;
        this.CompletedTasks = new List<string>();
        this.ScannedPictures = new List<string>();
    }

    public PlayerClass() {} // Deserializer constructor
}