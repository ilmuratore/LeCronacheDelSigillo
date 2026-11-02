using System;

class Nemico : Personaggio
{
    public Nemico(string nome, int vitaMassima, int manaMassimo, int attacco, int difesa, 
                    int armatura, int magia, int agilita) 
        : base(nome, vitaMassima, manaMassimo, attacco, difesa, armatura, magia, agilita)
    {
        vita = vitaMassima;
        mana = manaMassimo;
    }
                                                                                                                                            

    public void Presentati()
    {
        Console.WriteLine($"\n Un nemico appare: {this.nome} !");
        Console.WriteLine("Si prepara a combattere");
    }


}