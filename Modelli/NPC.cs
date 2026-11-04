using System;

// I dialoghi arrivano dal catalogo, non sono scritti nel codice.
class NPC
{
    private string id;
    private string nome;
    public NPC(DefinizioneNPC dati) { id = dati.Id; nome = dati.Nome; }
    public void Parla(Missione missione, Catalogo catalogo)
    {
        foreach (string battuta in catalogo.DialoghiNPC(id, missione.LeggiStato()))
            Console.WriteLine(nome + ": " + battuta);
        missione.Mostra();
    }
}
