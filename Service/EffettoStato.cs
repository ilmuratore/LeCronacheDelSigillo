class EffettoStato
{
    private TipoEffetto tipo;
    private int turniRimanenti; 

    public EffettoStato(TipoEffetto tipo, int durata)
    {
        this.tipo = tipo;
        turniRimanenti = durata;
    }

    public bool EDiTipo(TipoEffetto altroTipo)
    {
        return tipo == altroTipo;
    }
    public void Rinnova(int durata)
    {
        if(durata > turniRimanenti)
        {
            turniRimanenti = durata;
        }
    }
    public bool EScaduto()
    {
        return turniRimanenti <= 0;
    }
    public void Mostra()
    {
        Console.WriteLine($"Effetto {tipo} ({turniRimanenti} turni");
    }
    public bool ApplicaAllInizioDelTurno(Personaggio bersaglio)
    {
        bool saltaAzione = false;

        switch (tipo)
        {
            case TipoEffetto.Veleno:
                Console.WriteLine("Il veleno infligge 3 danni");
                bersaglio.SubisciDanno(3);
                break;
            case TipoEffetto.Bruciatura:
                Console.WriteLine("La bruciatura infligge 5 danni");
                bersaglio.SubisciDanno(5);
                break;
            case TipoEffetto.Stordimento:
                Console.WriteLine("Il personaggio é stordito.");
                saltaAzione = true;
                break;
        }

        turniRimanenti--;
        return saltaAzione;
    }
}