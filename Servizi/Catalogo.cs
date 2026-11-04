using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// Il catalogo contiene definizioni lette da file esterni, non lo stato del giocatore.
class Catalogo
{
    public List<DefinizioneNemico> Nemici = new List<DefinizioneNemico>();
    public List<DefinizioneMissione> Missioni = new List<DefinizioneMissione>();
    public List<DefinizioneNPC> NPC = new List<DefinizioneNPC>();
    public List<DefinizioneDialogo> Dialoghi = new List<DefinizioneDialogo>();
    public List<DefinizioneOggetto> Oggetti = new List<DefinizioneOggetto>();
    public List<DefinizioneNegozio> Negozi = new List<DefinizioneNegozio>();
    public List<DefinizioneAbilita> Abilita = new List<DefinizioneAbilita>();
    public List<DefinizioneOggettoIniziale> InventarioIniziale = new List<DefinizioneOggettoIniziale>();

    private static List<T> Leggi<T>(string cartella, string nome)
    {
        string file = Path.Combine(cartella, nome + ".json");
        if (!File.Exists(file)) throw new InvalidDataException("File dati mancante: " + file);
        List<T>? dati = JsonSerializer.Deserialize<List<T>>(File.ReadAllText(file), JsonSupport.Opzioni());
        if (dati == null) throw new InvalidDataException("Lista dati non valida: " + nome);
        return dati;
    }

    public static Catalogo Carica()
    {
        string root = Path.Combine(AppContext.BaseDirectory, "Dati");
        Catalogo c = new Catalogo();
        c.Nemici = Leggi<DefinizioneNemico>(root, "nemici");
        c.Missioni = Leggi<DefinizioneMissione>(root, "missioni");
        c.NPC = Leggi<DefinizioneNPC>(root, "npc");
        c.Dialoghi = Leggi<DefinizioneDialogo>(root, "dialoghi");
        c.Oggetti = Leggi<DefinizioneOggetto>(root, "oggetti");
        c.Negozi = Leggi<DefinizioneNegozio>(root, "negozi");
        c.Abilita = Leggi<DefinizioneAbilita>(root, "abilita");
        c.InventarioIniziale = Leggi<DefinizioneOggettoIniziale>(root, "inventario_iniziale");
        c.Valida();
        return c;
    }

    private static void ControllaId(List<string> ids, string sezione)
    {
        HashSet<string> visti = new HashSet<string>();
        foreach (string id in ids)
        {
            if (string.IsNullOrWhiteSpace(id) || !visti.Add(id))
                throw new InvalidDataException("ID vuoto o duplicato in " + sezione + ": " + id);
        }
    }

    public DefinizioneNemico TrovaNemico(string id)
    {
        foreach (DefinizioneNemico n in Nemici) if (n.Id == id) return n;
        throw new InvalidDataException("Nemico inesistente: " + id);
    }
    public DefinizioneNPC TrovaNPC(string id)
    {
        foreach (DefinizioneNPC n in NPC) if (n.Id == id) return n;
        throw new InvalidDataException("NPC inesistente: " + id);
    }
    public DefinizioneMissione TrovaMissione(string id)
    {
        foreach (DefinizioneMissione m in Missioni) if (m.Id == id) return m;
        throw new InvalidDataException("Missione inesistente: " + id);
    }
    public DefinizioneOggetto TrovaOggetto(string id)
    {
        foreach (DefinizioneOggetto o in Oggetti) if (o.Id == id) return o;
        throw new InvalidDataException("Oggetto inesistente: " + id);
    }
    public DefinizioneNegozio TrovaNegozio(string id)
    {
        foreach (DefinizioneNegozio n in Negozi) if (n.Id == id) return n;
        throw new InvalidDataException("Negozio inesistente: " + id);
    }
    public List<string> DialoghiNPC(string npcId, StatoMissione stato)
    {
        List<string> battute = new List<string>();
        foreach (DefinizioneDialogo d in Dialoghi)
            if (d.NPCId == npcId && d.Stato == stato.ToString()) battute.Add(d.Testo);
        if (battute.Count == 0) battute.Add("Che gli antichi spiriti ti guidino.");
        return battute;
    }
    public Oggetto CreaOggetto(string id)
    {
        DefinizioneOggetto d = TrovaOggetto(id);
        if (d.Tipo == TipoOggetto.Arma || d.Tipo == TipoOggetto.Armatura ||
            d.Tipo == TipoOggetto.Accessorio)
            return new Oggetto(d.Nome, d.Tipo, d.BonusAttacco, d.BonusArmatura,
                              d.BonusMagia, d.Prezzo);
        return new Oggetto(d.Nome, d.Tipo, d.Potenza, d.Prezzo);
    }
    public Nemico CreaNemico(string id)
    {
        DefinizioneNemico d = TrovaNemico(id);
        if (d.Boss) return new Boss(d, this);
        return new Nemico(d, this);
    }
    public List<Missione> CreaMissioni()
    {
        List<Missione> result = new List<Missione>();
        foreach (DefinizioneMissione d in Missioni) result.Add(new Missione(d));
        return result;
    }

    public void Valida()
    {
        List<string> ids = new List<string>();
        foreach (DefinizioneNemico n in Nemici) ids.Add(n.Id);
        ControllaId(ids, "nemici"); ids.Clear();
        foreach (DefinizioneMissione m in Missioni) ids.Add(m.Id);
        ControllaId(ids, "missioni"); ids.Clear();
        foreach (DefinizioneNPC n in NPC) ids.Add(n.Id);
        ControllaId(ids, "npc"); ids.Clear();
        foreach (DefinizioneOggetto o in Oggetti) ids.Add(o.Id);
        ControllaId(ids, "oggetti"); ids.Clear();
        foreach (DefinizioneNegozio n in Negozi) ids.Add(n.Id);
        ControllaId(ids, "negozi"); ids.Clear();
        foreach (DefinizioneAbilita a in Abilita) ids.Add(a.Id);
        ControllaId(ids, "abilita");
        if (Nemici.Count == 0 || Missioni.Count == 0 || Negozi.Count == 0)
            throw new InvalidDataException("Sono necessari nemici, missioni e un negozio.");

        foreach (DefinizioneNemico n in Nemici)
        {
            if (string.IsNullOrWhiteSpace(n.Nome) || n.HP <= 0 || n.MP < 0 ||
                n.Attacco < 0 || n.Difesa < 0 || n.Armatura < 0 || n.Magia < 0 ||
                n.Agilita < 0 || n.Monete < 0 || n.XP < 0 ||
                n.ProbabilitaDrop < 0 || n.ProbabilitaDrop > 100)
                throw new InvalidDataException("Statistiche non valide per nemico: " + n.Id);
            if (n.DropOggettoId != "") TrovaOggetto(n.DropOggettoId);
        }
        foreach (DefinizioneOggetto o in Oggetti)
        {
            if (string.IsNullOrWhiteSpace(o.Nome) || !Enum.IsDefined(typeof(TipoOggetto), o.Tipo) ||
                o.Potenza < 0 || o.BonusAttacco < 0 || o.BonusArmatura < 0 ||
                o.BonusMagia < 0 || o.Prezzo < 0)
                throw new InvalidDataException("Oggetto non valido: " + o.Id);
        }
        foreach (DefinizioneMissione m in Missioni)
        {
            TrovaNemico(m.NemicoId); TrovaNPC(m.NPCId);
            if (string.IsNullOrWhiteSpace(m.Titolo) || m.Quantita < 1 ||
                m.PremioMonete < 0 || m.PremioXP < 0 ||
                (m.Categoria != "Principale" && m.Categoria != "Secondaria" &&
                 m.Categoria != "Introduzione"))
                throw new InvalidDataException("Missione non valida: " + m.Id);
            if (m.PrerequisitoId != "") TrovaMissione(m.PrerequisitoId);
            HashSet<string> catena = new HashSet<string>();
            string nodo = m.Id;
            while (nodo != "")
            {
                if (!catena.Add(nodo)) throw new InvalidDataException("Ciclo di prerequisiti: " + m.Id);
                nodo = TrovaMissione(nodo).PrerequisitoId;
            }
        }
        foreach (DefinizioneNegozio n in Negozi)
        {
            if (string.IsNullOrWhiteSpace(n.Nome) || n.Oggetti == null)
                throw new InvalidDataException("Negozio non valido: " + n.Id);
            foreach (string id in n.Oggetti) TrovaOggetto(id);
        }
        foreach (DefinizioneDialogo d in Dialoghi)
        {
            TrovaNPC(d.NPCId);
            if (string.IsNullOrWhiteSpace(d.Testo) || !Enum.TryParse<StatoMissione>(d.Stato, out _))
                throw new InvalidDataException("Stato dialogo errato: " + d.Stato);
        }
        foreach (DefinizioneOggettoIniziale d in InventarioIniziale)
        {
            TrovaOggetto(d.OggettoId);
            if (d.Quantita < 1 || d.Quantita > 50)
                throw new InvalidDataException("Quantita iniziale non valida: " + d.OggettoId);
        }
        foreach (DefinizioneAbilita a in Abilita)
        {
            if (!Enum.TryParse<TipoClasse>(a.Classe, out _) ||
                a.CostoMana < 0 || a.LivelloRichiesto < 1 ||
                a.BonusDanno < 0 || a.DurataEffetto < 0 ||
                a.ProbabilitaEffetto < 0 || a.ProbabilitaEffetto > 100)
                throw new InvalidDataException("Abilita non valida: " + a.Id);
        }
        foreach (TipoClasse classe in Enum.GetValues<TipoClasse>())
        {
            int count = 0;
            foreach (DefinizioneAbilita a in Abilita)
                if (a.Classe == classe.ToString()) count++;
            if (count != 2) throw new InvalidDataException("Sono richieste due abilita per " + classe);
        }
    }
}
